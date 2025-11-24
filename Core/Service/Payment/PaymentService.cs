using AutoMapper;
using DomainLayer.Contracts.Repository.Basket;
using DomainLayer.Contracts.Unit;
using DomainLayer.Exceptions.Basket;
using DomainLayer.Exceptions.Order;
using DomainLayer.Exceptions.Product;
using DomainLayer.Models.Basket;
using DomainLayer.Models.Order;
using Microsoft.Extensions.Configuration;
using Service.Spec.Order;
using serviceAbstraction.Contracts.Payment;
using Shared.DTO.Basket;
using Stripe;
// TODO: Alias Name
using ProductClass = DomainLayer.Models.Product.Product;

namespace Service.Payment;

/// <summary>
///     Implements payment handling using Stripe, including creating and updating
///     payment intents based on basket content.
/// </summary>
public class PaymentService(IConfiguration configuration, IBasketRepository basketRepository, IUnitOfWork unitOfWork, IMapper mapper) : IPaymentService
{
    /// <summary>
    ///     Provides access to the application configuration settings
    ///     (e.g., Stripe keys, connection strings, API settings).
    /// </summary>
    private readonly IConfiguration _configuration = configuration;

    /// <summary>
    ///     Handles all basket-related operations such as retrieving,
    ///     updating, or creating a basket in the underlying data store.
    /// </summary>
    private readonly IBasketRepository _basketRepository = basketRepository;

    /// <summary>
    ///     Provides access to the unit-of-work pattern, allowing
    ///     coordinated database operations and repository creation.
    /// </summary>
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    /// <summary>
    ///     Responsible for mapping between domain models and DTOs
    ///     to keep API responses and requests clean and structured.
    /// </summary>
    private readonly IMapper _mapper = mapper;

    /// <summary>
    ///     Creates or updates a Stripe Payment Intent for the specified basket.
    /// </summary>
    /// <param name="basketId">The ID of the customer's basket.</param>
    /// <returns>
    ///     Returns the updated <see cref="CustomerBasketDto"/> containing the
    ///     Stripe PaymentIntent ID and ClientSecret.
    /// </returns>
    /// <exception cref="BasketNotFoundException">Thrown if the basket does not exist.</exception>
    /// <exception cref="ProductNotFoundException">Thrown if any product inside the basket doesn't exist.</exception>
    /// <exception cref="DeliveryMethodNotFoundException">Thrown if the delivery method is invalid.</exception>
    public async Task<CustomerBasketDto> CreateOrUpdatePaymentIntentAsync(string basketId)
    {
        // 0) Stripe package should be installed (Stripe.net) before using Stripe APIs

        // 1) Set Stripe API key from appsettings.json to authorize Stripe requests
        StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];

        // 2) Get the customer's basket by basketId from repository or throw exception if not found
        // This basket contains the items user wants to buy and payment info
        var basket = await _basketRepository.GetBasketAsync(basketId) ?? throw new BasketNotFoundException(basketId);

        // Create repository instance to work with orders in the database
        var orderRepo = await _unitOfWork.CreateRepositoryAsync<DomainLayer.Models.Order.Order, Guid>();

        // Build specification to check if an order already uses this PaymentIntentId
        if (basket.PaymentIntentId != null)
        {
            var orderSpec = new OrderPaymentIntentSpecifications(basket.PaymentIntentId);

            // Use specification to find existing order with this PaymentIntentId
            var existOrder = await orderRepo.GetByIdAsync(orderSpec);

            // If an order with this PaymentIntentId already exists (possible duplicate)
            if (existOrder is not null)
            {
                // Delete existing order to avoid duplicate orders for same payment
                // This can happen if user retried payment or updated basket after payment started
                orderRepo.Delete(existOrder);

                // Optionally, throw exception to stop reusing this PaymentIntentId
                // throw new PaymentIntentAlreadyUsedException(basket.PaymentIntentId);
            }
        }

        // 3) Create repository for products to fetch actual prices from the database
        var productRepo = await _unitOfWork.CreateRepositoryAsync<ProductClass, int>();

        // For each item in the basket, get product info from database to sync prices
        // This avoids trusting possibly tampered prices sent from frontend
        foreach (var item in basket.items)
        {
            var product = await productRepo.GetByIdAsync(item.Id) ?? throw new ProductNotFoundException(item.Id);

            // Override item's price with backend price to ensure accuracy
            item.Price = product.Price;
        }

        // 4) Check that the delivery method ID is set, else throw an exception
        ArgumentNullException.ThrowIfNull(basket.DeliveryMethodId);

        // Create repository for delivery methods to validate and get cost
        var deliveryMethodRepo = await _unitOfWork.CreateRepositoryAsync<DeliveryMethod, int>();

        // Retrieve delivery method details from database or throw if invalid
        var deliveryMethod = await deliveryMethodRepo.GetByIdAsync(basket.DeliveryMethodId.Value) ?? throw new DeliveryMethodNotFoundException(basket.DeliveryMethodId.Value);

        // Assign the shipping price to the basket based on the delivery method
        basket.ShippingPrice = deliveryMethod.Cost;

        // 5) Calculate total cost for Stripe payment: items price + shipping
        // Stripe requires amount in the smallest currency unit (e.g., cents)
        var itemsTotal = basket.items.Sum(i => i.Price * i.Quantity);

        // Convert the total (items + shipping) to cents by multiplying by 100
        var basketAmount = (long)((itemsTotal + deliveryMethod.Cost) * 100);

        // 6) Initialize Stripe PaymentIntent service to create or update payment intent
        var paymentService = new PaymentIntentService();

        // If there is no existing PaymentIntentId, create a new payment intent
        if (basket.PaymentIntentId is null)
        {
            // Prepare options with amount, currency, and payment method types allowed
            var options = new PaymentIntentCreateOptions
            {
                Amount = basketAmount,
                Currency = "usd",
                PaymentMethodTypes = new List<string> { "card" }
            };

            // Create payment intent with Stripe API
            var paymentIntent = await paymentService.CreateAsync(options);

            // Store new payment intent's ID and client secret in the basket for client use
            basket.PaymentIntentId = paymentIntent.Id;
            basket.ClientSecret = paymentIntent.ClientSecret;
        }
        else
        {
            // If PaymentIntent already exists, update its amount (in case prices changed)
            var updateOptions = new PaymentIntentUpdateOptions
            {
                Amount = basketAmount
            };

            // Update existing payment intent with new amount via Stripe API
            await paymentService.UpdateAsync(basket.PaymentIntentId, updateOptions);
        }

        // 7) Save the updated basket (with new or updated payment intent info) to Redis or database
        await _basketRepository.CreateOrUpdateBasketAsync(basket);

        // 8) Map the basket domain model to a DTO and return it to the client
        // This DTO includes the PaymentIntentId and ClientSecret for completing payment
        return _mapper.Map<CustomerBasket, CustomerBasketDto>(basket);
    }
}
