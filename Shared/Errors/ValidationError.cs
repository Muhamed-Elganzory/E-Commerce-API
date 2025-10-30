namespace Shared.Errors;

/// <summary>
///     Represents a detailed validation error for a specific field or property.
///     Each instance contains the field name and a list of validation error messages related to it.
/// </summary>
public class ValidationError
{
    /// <summary>
    ///     Gets or sets the name of the field or property that caused the validation error.
    /// </summary>
    public string Field { get; set; } = null!;

    /// <summary>
    ///     Gets or sets a collection of validation error messages associated with the field.
    ///     Each string in the list describes a specific validation rule that was violated.
    /// </summary>
    public IEnumerable<string> Errors { get; set; } = [];
}
