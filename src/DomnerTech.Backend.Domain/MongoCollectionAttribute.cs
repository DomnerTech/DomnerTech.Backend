namespace DomnerTech.Backend.Domain;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class MongoCollectionAttribute(string collectionName) : Attribute
{
    /// <summary>
    /// Gets the name of the collection associated with this instance.
    /// </summary>
    public string CollectionName { get; } = collectionName;
}