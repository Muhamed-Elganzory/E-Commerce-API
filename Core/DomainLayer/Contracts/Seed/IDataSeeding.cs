namespace DomainLayer.Contracts.Seed;

/// <summary>
///     Defines the contract for database seeding operations.
/// </summary>
/// <remarks>
///     This interface provides methods to populate initial data into both
///     the application database and the Identity database.
///     Implementations of this interface are typically executed during
///     application startup to ensure required data exists.
/// </remarks>
public interface IDataSeeding
{
    /// <summary>
    ///     Seeds initial or default data into the main application database.
    /// </summary>
    /// <returns>
    ///     A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    Task DataSeedAsync();

    /// <summary>
    ///     Seeds default Identity data such as roles and users
    ///     into the Identity database.
    /// </summary>
    /// <returns>
    ///     A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    Task IdentityDataSeedAsync();
}