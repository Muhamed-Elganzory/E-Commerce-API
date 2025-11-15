namespace Shared.DTO.Auth
{
    /// <summary>
    ///     Represents a user's physical address information.
    ///     This DTO is used for transferring address data between layers,
    ///     typically in authentication or profile-related operations.
    /// </summary>
    public class AddressDto
    {
        /// <summary>
        ///     Gets or sets the first name of the user associated with this address.
        /// </summary>
        public string FirstName { get; set; } = null!;

        /// <summary>
        ///     Gets or sets the last name of the user associated with this address.
        /// </summary>
        public string LastName { get; set; } = null!;

        /// <summary>
        ///     Gets or sets the street name and number of the address.
        /// </summary>
        public string Street { get; set; } = null!;

        /// <summary>
        ///     Gets or sets the city where the user resides.
        /// </summary>
        public string City { get; set; } = null!;

        /// <summary>
        ///     Gets or sets the country where the user resides.
        /// </summary>
        public string Country { get; set; } = null!;
    }
}