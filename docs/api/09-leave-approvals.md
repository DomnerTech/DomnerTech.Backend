# Leave Approvals API

## Overview

Manages the approval workflow for leave requests including approving, rejecting, and tracking approval history.

**Base Path:** `/api/v1/leave-approval`

---

## Endpoints Summary

| Method | Endpoint                                     | Description           | Authorization         |
| ------ | -------------------------------------------- | --------------------- | --------------------- |
| POST   | `/leave-approval/approve`                    | Approve leave request | `LeaveApproval.Write` |
| POST   | `/leave-approval/reject`                     | Reject leave request  | `LeaveApproval.Write` |
| GET    | `/leave-approval/pending`                    | Get pending approvals | `LeaveApproval.Write` |
| GET    | `/leave-approval/history/{leave_request_id}` | Get approval history  | `LeaveApproval.Write` |

---

## 1. Approve Leave Request

Approves a pending leave request.

**Endpoint:** `POST /api/v1/leave-approval/approve`

**Authorization:** Required - Role: `LeaveApproval.Write`

### Request

```json
{
  "leave_request_id": "678cf2a4b3945e0001ac4d70",
  "comments": "Approved. Enjoy your vacation!"
}
```

**Schema:**
| Field | Type | Required | Description |
|---------------------|---------|-----------|-----------------------------------|
| `leave_request_id` | string | Yes | Leave request ID to approve |
| `comments` | string | No | Approval comments (max 500 chars) |

### Response `200 OK`

```json
{
  "data": true,
  "status": {
    "status_code": 200,
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
  "leave_request_id": "678cf2a4b3945e0001ac4d70",
  "rejection_reason": "Critical project deadline during requested period. Please reschedule."
}
```

**Schema:**
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `leave_request_id` | string | Yes | Leave request ID to reject |
| `rejection_reason` | string | Yes | Reason for rejection (max 500 chars) |

### Response `200 OK`

```json
{
  "data": true,
  "status": {
    "status_code": 200,
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
      "leave_request_id": "678cf2a4b3945e0001ac4d70",
      "employee_id": "678cf2a4b3945e0001ac4d40",
      "employee_name": "Sarah Johnson",
      "employee_department": "Engineering",
      "employee_job_title": "Lead Software Engineer",
      "leave_type_id": "678cf2a4b3945e0001ac4d60",
      "leave_type_name": "Annual Leave",
      "leave_type_color": "#4CAF50",
      "start_date": "2025-03-16T00:00:00Z",
      "end_date": "2025-03-22T00:00:00Z",
      "total_days": 5,
      "reason": "Extended family vacation to Hawaii",
      "attachment_urls": [
        "https://storage.domnertech.com/attachments/flight-booking-12345.pdf"
      ],
      "submitted_at": "2025-01-20T10:30:00Z",
      "approver_level": 1,
      "status": "Pending",
      "current_balance": 19.0,
      "balance_after_approval": 14.0
    },
    {
      "id": "678cf2a4b3945e0001ac4da1",
      "leave_request_id": "678cf2a4b3945e0001ac4d71",
      "employee_id": "678cf2a4b3945e0001ac4d42",
      "employee_name": "Michael Chen",
      "employee_department": "Engineering",
      "employee_job_title": "Staff Software Engineer",
      "leave_type_id": "678cf2a4b3945e0001ac4d61",
      "leave_type_name": "Sick Leave",
      "leave_type_color": "#FF9800",
      "start_date": "2025-01-22T00:00:00Z",
      "end_date": "2025-01-23T00:00:00Z",
      "total_days": 2,
      "reason": "Medical appointment and recovery",
      "submitted_at": "2025-01-21T08:00:00Z",
      "approver_level": 1,
      "status": "Pending",
      "current_balance": 8.0,
      "balance_after_approval": 6.0
    }
  ],
  "status": {
    "status_code": 200
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

**Endpoint:** `GET /api/v1/leave-approval/history/{leave_request_id}`

**Authorization:** Required - Role: `LeaveApproval.Write`

### Path Parameters

| Parameter          | Type   | Required | Description                       |
| ------------------ | ------ | -------- | --------------------------------- |
| `leave_request_id` | string | Yes      | Leave request's unique identifier |

### Response `200 OK`

```json
{
  "data": [
    {
      "id": "678cf2a4b3945e0001ac4da0",
      "leave_request_id": "678cf2a4b3945e0001ac4d70",
      "approver_id": "678cf2a4b3945e0001ac4d35",
      "approver_name": "Michael Chen",
      "approver_role": "Engineering Manager",
      "approver_level": 1,
      "action": "Approved",
      "comments": "Approved. Enjoy your vacation!",
      "decided_at": "2025-01-20T14:00:00Z",
      "status": "Approved"
    }
  ],
  "status": {
    "status_code": 200
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

| Level | Role            | Typical For                          |
| ----- | --------------- | ------------------------------------ |
| 1     | Direct Manager  | Most leave requests                  |
| 2     | Department Head | Extended leave (>10 days)            |
| 3     | VP/C-Level      | Long leave (>20 days), special cases |

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
async function batchApproveLeaves(leave_request_ids) {
  const results = [];
  for (const id of leave_request_ids) {
    try {
      await approveLeave({
        leave_request_id: id,
        comments: "Batch approved",
      });
      results.push({ id, status: "success" });
    } catch (error) {
      results.push({ id, status: "failed", error: error.message });
    }
  }
  return results;
}
```

### Use Case 2: Approval with Validation

```javascript
async function approveWithValidation(leave_request_id) {
  // Get request details
  const request = await getLeaveRequestById(leave_request_id);

  // Check team availability
  const conflicts = await checkTeamLeaveConflicts({
    department: request.employeeDepartment,
    startDate: request.startDate,
    endDate: request.endDate,
    maxEmployeesAllowed: 3,
  });

  if (conflicts.length > 0) {
    throw new Error("Too many team members on leave during this period");
  }

  // Approve
  await approveLeave({
    leave_request_id: leave_request_id,
    comments: "Approved after team availability check",
  });
}
```

### Use Case 3: Conditional Approval

```javascript
async function conditionalApprove(leave_request_id) {
  const request = await getLeaveRequestById(leave_request_id);

  // Auto-approve if <= 2 days
  if (request.totalDays <= 2) {
    await approveLeave({
      leave_request_id: leave_request_id,
      comments: "Auto-approved (?2 days)",
    });
  } else {
    // Requires manual review
    await sendNotificationToManager(request.employeeId, leave_request_id);
  }
}
```

---

## Error Scenarios

**Cannot Approve Own Request:** `403`

```json
{
  "status": {
    "status_code": 403,
    "message": "You cannot approve your own leave request",
    "error_code": "ERR_CANNOT_APPROVE_OWN"
  }
}
```

**Request Already Processed:** `409`

```json
{
  "status": {
    "status_code": 409,
    "message": "Leave request has already been approved/rejected",
    "error_code": "ERR_ALREADY_PROCESSED"
  }
}
```

**Insufficient Balance:** `422`

```json
{
  "status": {
    "status_code": 422,
    "message": "Employee has insufficient leave balance",
    "error_code": "ERR_INSUFFICIENT_BALANCE"
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
