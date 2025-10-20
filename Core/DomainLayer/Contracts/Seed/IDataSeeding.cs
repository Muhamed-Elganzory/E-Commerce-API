namespace DomainLayer.Contracts.Seed;

/// <summary>
///     Data seeding interface
/// </summary>
public interface IDataSeeding
{
    public Task DataSeedAsync();
}