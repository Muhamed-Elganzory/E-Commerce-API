using Service.Spec.Base;
using Shared.Queries;

namespace Service.Spec.Product;

public class ProductCountSpecification(ProductQueryParams queryParams): BaseSpecification<DomainLayer.Models.Product.Product, int>(p =>
    (!queryParams.BrandId.HasValue || p.ProductBrandId == queryParams.BrandId) &&
    (!queryParams.TypeId.HasValue || p.ProductTypeId == queryParams.TypeId) &&
    (string.IsNullOrWhiteSpace(queryParams.SearchValue) || p.Name.ToLower().Contains(queryParams.SearchValue.ToLower())))
{

}