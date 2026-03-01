namespace DomnerTech.Backend.Domain;

[AttributeUsage(AttributeTargets.Property)]
public sealed class SortableAttribute(string? alias = null, int order = 0, bool descending = true) : Attribute
{
    public string? Alias { get; } = alias;
    public int Order { get; } = order;
    public bool Descending { get; } = descending;
}