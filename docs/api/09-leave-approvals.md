# Leave Approvals API

## Overview
Manages the approval workflow for leave requests including approving, rejecting, and tracking approval history.

**Base Path:** `/api/v1/leave-approval`

---

## Endpoints Summary

| Method | Endpoint | Description | Authorization |
|--------|----------|-------------|---------------|
| POST | `/leave-approval/approve` | Approve leave request | `LeaveApproval.Write` |
| POST | `/leave-approval/reject` | Reject leave request | `LeaveApproval.Write` |
| GET | `/leave-approval/pending` | Get pending approvals | `LeaveApproval.Write` |
| GET | `/leave-approval/history/{leaveRequestId}` | Get approval history | `LeaveApproval.Write` |

---

## 1. Approve Leave Request

Approves a pending leave request.

**Endpoint:** `POST /api/v1/leave-approval/approve`

**Authorization:** Required - Role: `LeaveApproval.Write`

### Request

```json
{
  "leaveRequestId": "678cf2a4b3945e0001ac4d70",
  "comments": "Approved. Enjoy your vacation!"
}
```

**Schema:**
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `leaveRequestId` | string | Yes | Leave request ID to approve |
| `comments` | string | No | Approval comments (max 500 chars) |

### Response `200 OK`

```json
{
  "data": true,
  "status": {
    "statusCode": 200,
    "message": "Leave request approved successfully"
  }
}
```

### Business Rules
- Only managers or HR can approve
- Request must be in "Pending" status
- Approver cannot approve their own request
- Leave balance is deducted upon approval
- Employee receives notification

---

## 2. Reject Leave Request

Rejects a pending leave request with a reason.

**Endpoint:** `POST /api/v1/leave-approval/reject`

**Authorization:** Required - Role: `LeaveApproval.Write`

### Request

```json
{
  "leaveRequestId": "678cf2a4b3945e0001ac4d70",
  "rejectionReason": "Critical project deadline during requested period. Please reschedule."
}
```

**Schema:**
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `leaveRequestId` | string | Yes | Leave request ID to reject |
| `rejectionReason` | string | Yes | Reason for rejection (max 500 chars) |

### Response `200 OK`

```json
{
  "data": true,
  "status": {
    "statusCode": 200,
    "message": "Leave request rejected"
  }
}
```

### Business Rules
- Rejection reason is mandatory
- Request status changed to "Rejected"
- Leave balance is not deducted
- Employee receives notification with reason
- Cannot reject already approved/rejected requests

---

## 3. Get Pending Approvals

Retrieves all leave requests pending approval for the current user.

**Endpoint:** `GET /api/v1/leave-approval/pending`

**Authorization:** Required - Role: `LeaveApproval.Write`

### Response `200 OK`

```json
{
  "data": [
    {
      "id": "678cf2a4b3945e0001ac4da0",
      "leaveRequestId": "678cf2a4b3945e0001ac4d70",
      "employeeId": "678cf2a4b3945e0001ac4d40",
      "employeeName": "Sarah Johnson",
      "employeeDepartment": "Engineering",
      "employeeJobTitle": "Lead Software Engineer",
      "leaveTypeId": "678cf2a4b3945e0001ac4d60",
      "leaveTypeName": "Annual Leave",
      "leaveTypeColor": "#4CAF50",
      "startDate": "2025-03-16T00:00:00Z",
      "endDate": "2025-03-22T00:00:00Z",
      "totalDays": 5,
      "reason": "Extended family vacation to Hawaii",
      "attachmentUrls": [
        "https://storage.domnertech.com/attachments/flight-booking-12345.pdf"
      ],
      "submittedAt": "2025-01-20T10:30:00Z",
      "approverLevel": 1,
      "status": "Pending",
      "currentBalance": 19.0,
      "balanceAfterApproval": 14.0
    },
    {
      "id": "678cf2a4b3945e0001ac4da1",
      "leaveRequestId": "678cf2a4b3945e0001ac4d71",
      "employeeId": "678cf2a4b3945e0001ac4d42",
      "employeeName": "Michael Chen",
      "employeeDepartment": "Engineering",
      "employeeJobTitle": "Staff Software Engineer",
      "leaveTypeId": "678cf2a4b3945e0001ac4d61",
      "leaveTypeName": "Sick Leave",
      "leaveTypeColor": "#FF9800",
      "startDate": "2025-01-22T00:00:00Z",
      "endDate": "2025-01-23T00:00:00Z",
      "totalDays": 2,
      "reason": "Medical appointment and recovery",
      "submittedAt": "2025-01-21T08:00:00Z",
      "approverLevel": 1,
      "status": "Pending",
      "currentBalance": 8.0,
      "balanceAfterApproval": 6.0
    }
  ],
  "status": {
    "statusCode": 200
  }
}
```

### Response Includes
- Employee details
- Leave type information
- Request details
- Current balance
- Balance after approval
- Days pending

---

## 4. Get Approval History

Retrieves complete approval history for a specific leave request.

**Endpoint:** `GET /api/v1/leave-approval/history/{leaveRequestId}`

**Authorization:** Required - Role: `LeaveApproval.Write`

### Path Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `leaveRequestId` | string | Yes | Leave request's unique identifier |

### Response `200 OK`

```json
{
  "data": [
    {
      "id": "678cf2a4b3945e0001ac4da0",
      "leaveRequestId": "678cf2a4b3945e0001ac4d70",
      "approverId": "678cf2a4b3945e0001ac4d35",
      "approverName": "Michael Chen",
      "approverRole": "Engineering Manager",
      "approverLevel": 1,
      "action": "Approved",
      "comments": "Approved. Enjoy your vacation!",
      "decidedAt": "2025-01-20T14:00:00Z",
      "status": "Approved"
    }
  ],
  "status": {
    "statusCode": 200
  }
}
```

### Use Cases
- Audit trail for compliance
- Track multi-level approvals
- Review approval decisions
- Investigate disputes

---

## Approval Workflow

### Single-Level Approval
```
Employee ? Submit Request ? Manager ? Approve/Reject
```

### Multi-Level Approval
```
Employee ? Submit ? L1 Manager ? L2 Director ? L3 VP ? Approve
                         ?              ?           ?
                       Reject       Reject      Reject
```

### Approval Levels
| Level | Role | Typical For |
|-------|------|-------------|
| 1 | Direct Manager | Most leave requests |
| 2 | Department Head | Extended leave (>10 days) |
| 3 | VP/C-Level | Long leave (>20 days), special cases |

---

## Business Rules

### Approval Authorization
1. Manager can approve their team's requests
2. HR can approve any request
3. Cannot approve own requests
4. Multi-level approval may be required based on:
   - Duration of leave
   - Leave type
   - Employee level

### Approval Impact
**Upon Approval:**
- Request status ? "Approved"
- Leave balance deducted
- Pending days moved to allocated
- Employee notified
- Calendar updated

**Upon Rejection:**
- Request status ? "Rejected"
- Balance not affected
- Rejection reason recorded
- Employee notified

### Delegation
- Managers can delegate approval authority
- Acting managers can approve during manager absence
- Admin role can bypass approval

---

## Notification Flow

### Employee Notifications
- **Approved:** "Your leave request has been approved"
- **Rejected:** "Your leave request has been rejected: [reason]"
- **Pending:** "Your leave request is pending approval"

### Manager Notifications
- **New Request:** "New leave request requires your approval"
- **Reminder:** "You have X pending leave requests"
- **Escalation:** "Leave request pending for Y days"

---

## Common Use Cases

### Use Case 1: Batch Approval
```javascript
async function batchApproveLeaves(leaveRequestIds) {
  const results = [];
  for (const id of leaveRequestIds) {
    try {
      await approveLeave({
        leaveRequestId: id,
        comments: "Batch approved"
      });
      results.push({ id, status: 'success' });
    } catch (error) {
      results.push({ id, status: 'failed', error: error.message });
    }
  }
  return results;
}
```

### Use Case 2: Approval with Validation
```javascript
async function approveWithValidation(leaveRequestId) {
  // Get request details
  const request = await getLeaveRequestById(leaveRequestId);
  
  // Check team availability
  const conflicts = await checkTeamLeaveConflicts({
    department: request.employeeDepartment,
    startDate: request.startDate,
    endDate: request.endDate,
    maxEmployeesAllowed: 3
  });
  
  if (conflicts.length > 0) {
    throw new Error('Too many team members on leave during this period');
  }
  
  // Approve
  await approveLeave({
    leaveRequestId: leaveRequestId,
    comments: 'Approved after team availability check'
  });
}
```

### Use Case 3: Conditional Approval
```javascript
async function conditionalApprove(leaveRequestId) {
  const request = await getLeaveRequestById(leaveRequestId);
  
  // Auto-approve if <= 2 days
  if (request.totalDays <= 2) {
    await approveLeave({
      leaveRequestId: leaveRequestId,
      comments: 'Auto-approved (?2 days)'
    });
  } else {
    // Requires manual review
    await sendNotificationToManager(request.employeeId, leaveRequestId);
  }
}
```

---

## Error Scenarios

**Cannot Approve Own Request:** `403`
```json
{
  "status": {
    "statusCode": 403,
    "message": "You cannot approve your own leave request",
    "errorCode": "ERR_CANNOT_APPROVE_OWN"
  }
}
```

**Request Already Processed:** `409`
```json
{
  "status": {
    "statusCode": 409,
    "message": "Leave request has already been approved/rejected",
    "errorCode": "ERR_ALREADY_PROCESSED"
  }
}
```

**Insufficient Balance:** `422`
```json
{
  "status": {
    "statusCode": 422,
    "message": "Employee has insufficient leave balance",
    "errorCode": "ERR_INSUFFICIENT_BALANCE"
  }
}
```

---

## Related Endpoints
- [Leave Requests](./06-leave-requests.md) - Submit requests for approval
- [Team Leave](./11-team-leave.md) - Check team availability before approving
- [Notifications](./14-notifications.md) - Approval notifications

---

[? Back to Documentation Home](./README.md)
