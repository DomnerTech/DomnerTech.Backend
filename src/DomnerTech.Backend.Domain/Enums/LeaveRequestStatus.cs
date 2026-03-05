namespace DomnerTech.Backend.Domain.Enums;

/// <summary>
/// Represents the status of a leave request throughout its lifecycle.
/// </summary>
public enum LeaveRequestStatus
{
    /// <summary>
    /// Leave request is awaiting approval.
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Leave request has been approved.
    /// </summary>
    Approved = 1,

    /// <summary>
    /// Leave request has been rejected.
    /// </summary>
    Rejected = 2,

    /// <summary>
    /// Leave request has been cancelled by the employee or system.
    /// </summary>
    Cancelled = 3
}
