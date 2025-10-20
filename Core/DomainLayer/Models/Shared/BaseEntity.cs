namespace DomainLayer.Models.Shared;

/// <summary>
///     Base Entity Class To be inherited by other entities
/// </summary>
/// <typeparam name="TKey">Generic Type</typeparam>
public class BaseEntity <TKey>
{
    public TKey Id { get; set; }
}