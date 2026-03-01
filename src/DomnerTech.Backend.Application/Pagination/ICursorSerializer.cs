namespace DomnerTech.Backend.Application.Pagination;

public interface ICursorSerializer
{
    string Serialize<T>(T payload);
    T Deserialize<T>(string cursor);
}