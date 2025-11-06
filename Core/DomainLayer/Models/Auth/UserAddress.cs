namespace DomainLayer.Models.Auth
{
    /// <summary>
    ///     Represents the address details associated with an application user.
    ///     Each user can have one or more addresses stored in the system.
    /// </summary>
    public class UserAddress
    {
        /// <summary>
        ///     Primary key for the UserAddress entity.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     The first name of the user associated with this address.
        /// </summary>
        public string FirstName { get; set; } = null!;

        /// <summary>
        ///     The last name of the user associated with this address.
        /// </summary>
        public string LastName { get; set; } = null!;

        /// <summary>
        ///     The street name and number for this address.
        /// </summary>
        public string Street { get; set; } = null!;

        /// <summary>
        ///     The city where the user resides.
        /// </summary>
        public string City { get; set; } = null!;

        /// <summary>
        ///     The country where the user resides.
        /// </summary>
        public string Country { get; set; } = null!;

        /// <summary>
        ///     Navigation property that links this address to a specific application user.
        /// </summary>
        public ApplicationUser ApplicationUser { get; set; } = null!;

        /// <summary>
        ///     Foreign key that references the associated ApplicationUser.
        /// </summary>
        public string UserId { get; set; } = null!;
    }
}