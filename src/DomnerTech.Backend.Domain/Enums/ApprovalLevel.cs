namespace DomnerTech.Backend.Domain.Enums;

/// <summary>
/// Represents the hierarchical level in the leave approval workflow.
/// </summary>
public enum ApprovalLevel
{
    /// <summary>
    /// Team lead approval level.
    /// </summary>
    TeamLead = 0,

    /// <summary>
    /// Manager approval level.
    /// </summary>
    Manager = 1,

    /// <summary>
    /// Human Resources approval level.
    /// </summary>
    Hr = 2,

    /// <summary>
    /// Final approval level (e.g., Director, CEO).
    /// </summary>
    Final = 3
}
