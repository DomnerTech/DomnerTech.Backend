namespace DomnerTech.Backend.Domain.Entities;

public interface ISoftDeleteEntity
{
    bool IsDeleted {get; set; }
}