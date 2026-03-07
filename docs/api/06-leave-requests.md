# Leave Requests API

## Overview
Manages employee leave requests including submission, updates, cancellation, and retrieval of leave requests.

**Base Path:** `/api/v1/leave-request`

---

## Endpoints Summary

| Method | Endpoint | Description | Authorization |
|--------|----------|-------------|---------------|
| POST | `/leave-request` | Create leave request | `LeaveRequest.Write` |
| PUT | `/leave-request` | Update leave request | `LeaveRequest.Write` |
| POST | `/leave-request/cancel` | Cancel leave request | `LeaveRequest.Write` |
| GET | `/leave-request/{id}` | Get request by ID | `LeaveRequest.Read` |
| GET | `/leave-request/my` | Get my requests | `LeaveRequest.Read` |
| GET | `/leave-request/status/{status}` | Get requests by status | `LeaveRequest.Admin` |

---

## 1. Create Leave Request

Submit a new leave request.

**Endpoint:** `POST /api/v1/leave-request`

**Authorization:** Required - Role: `LeaveRequest.Write`

### Request

```json
{
  "employeeId": "678cf2a4b3945e0001ac4d40",
  "leaveTypeId": "678cf2a4b3945e0001ac4d60",
  "startDate": "2025-03-15T00:00:00Z",
  "endDate": "2025-03-20T00:00:00Z",
  "reason": "Family vacation to Hawaii",
  "attachmentUrls": [
    "https://storage.domnertech.com/attachments/flight-booking-12345.pdf"
  ]
}
```

### Response `200 OK`

```json
{
  "data": "678cf2a4b3945e0001ac4d70",
  "status": {
    "statusCode": 200,
    "message": "Leave request submitted successfully"
  }
}
```

### Common Errors

**Insufficient Balance:** `422`
```json
{
  "status": {
    "statusCode": 422,
    "message": "Insufficient leave balance. You have 3 days remaining but requested 5 days.",
    "errorCode": "ERR_INSUFFICIENT_BALANCE"
  }
}
```

**Conflicting Leave:** `409`
```json
{
  "status": {
    "statusCode": 409,
    "message": "Leave request conflicts with existing approved leave from 2025-03-18 to 2025-03-20",
    "errorCode": "ERR_LEAVE_CONFLICT"
  }
}
```

---

## 2. Update Leave Request

Update a pending leave request.

**Endpoint:** `PUT /api/v1/leave-request`

**Authorization:** Required - Role: `LeaveRequest.Write`

### Request

```json
{
  "id": "678cf2a4b3945e0001ac4d70",
  "leaveTypeId": "678cf2a4b3945e0001ac4d60",
  "startDate": "2025-03-16T00:00:00Z",
  "endDate": "2025-03-22T00:00:00Z",
  "reason": "Extended family vacation to Hawaii",
  "attachmentUrls": [
    "https://storage.domnertech.com/attachments/flight-booking-12345.pdf",
    "https://storage.domnertech.com/attachments/hotel-booking-67890.pdf"
  ]
}
```

### Response `200 OK`

```json
{
  "data": true,
  "status": {
    "statusCode": 200,
    "message": "Leave request updated successfully"
  }
}
```

### Business Rules
- Only pending requests can be updated
- Cannot modify approved or rejected requests
- Must still pass balance and conflict checks

---

## 3. Cancel Leave Request

Cancel a pending or approved leave request.

**Endpoint:** `POST /api/v1/leave-request/cancel`

**Authorization:** Required - Role: `LeaveRequest.Write`

### Request

```json
{
  "id": "678cf2a4b3945e0001ac4d70",
  "cancellationReason": "Plans changed, vacation postponed"
}
```

### Response `200 OK`

```json
{
  "data": true,
  "status": {
    "statusCode": 200,
    "message": "Leave request cancelled successfully"
  }
}
```

### Business Rules
- Can cancel pending or approved requests
- Cannot cancel rejected or already cancelled requests
- Leave balance is restored upon cancellation
- Cancellation reason is mandatory

---

## 4. Get Leave Request by ID

Retrieve detailed information about a specific leave request.

**Endpoint:** `GET /api/v1/leave-request/{id}`

**Authorization:** Required - Role: `LeaveRequest.Read`

### Response `200 OK`

```json
{
  "data": {
    "id": "678cf2a4b3945e0001ac4d70",
    "employeeId": "678cf2a4b3945e0001ac4d40",
    "employeeName": "Sarah Johnson",
    "leaveTypeId": "678cf2a4b3945e0001ac4d60",
    "leaveTypeName": "Annual Leave",
    "startDate": "2025-03-16T00:00:00Z",
    "endDate": "2025-03-22T00:00:00Z",
    "totalDays": 5,
    "status": "Pending",
    "reason": "Extended family vacation to Hawaii",
    "attachmentUrls": [
      "https://storage.domnertech.com/attachments/flight-booking-12345.pdf",
      "https://storage.domnertech.com/attachments/hotel-booking-67890.pdf"
    ],
    "submittedAt": "2025-01-20T10:30:00Z",
    "approvedAt": null,
    "approvedBy": null,
    "rejectedAt": null,
    "rejectionReason": null
  },
  "status": {
    "statusCode": 200
  }
}
```

---

## 5. Get My Leave Requests

Retrieve all leave requests for the authenticated user.

**Endpoint:** `GET /api/v1/leave-request/my`

**Authorization:** Required - Role: `LeaveRequest.Read`

### Response `200 OK`

```json
{
  "data": [
    {
      "id": "678cf2a4b3945e0001ac4d70",
      "leaveTypeId": "678cf2a4b3945e0001ac4d60",
      "leaveTypeName": "Annual Leave",
      "startDate": "2025-03-16T00:00:00Z",
      "endDate": "2025-03-22T00:00:00Z",
      "totalDays": 5,
      "status": "Pending",
      "reason": "Extended family vacation to Hawaii",
      "submittedAt": "2025-01-20T10:30:00Z"
    },
    {
      "id": "678cf2a4b3945e0001ac4d71",
      "leaveTypeId": "678cf2a4b3945e0001ac4d61",
      "leaveTypeName": "Sick Leave",
      "startDate": "2024-12-10T00:00:00Z",
      "endDate": "2024-12-11T00:00:00Z",
      "totalDays": 2,
      "status": "Approved",
      "reason": "Flu and fever",
      "submittedAt": "2024-12-09T08:00:00Z",
      "approvedAt": "2024-12-09T09:15:00Z",
      "approvedBy": "Manager Name"
    }
  ],
  "status": {
    "statusCode": 200
  }
}
```

---

## 6. Get Leave Requests by Status

Retrieve all leave requests with a specific status (Admin only).

**Endpoint:** `GET /api/v1/leave-request/status/{status}`

**Authorization:** Required - Role: `LeaveRequest.Admin`

**Valid Status Values:** `Pending`, `Approved`, `Rejected`, `Cancelled`

### Response `200 OK`

```json
{
  "data": [
    {
      "id": "678cf2a4b3945e0001ac4d70",
      "employeeId": "678cf2a4b3945e0001ac4d40",
      "employeeName": "Sarah Johnson",
      "department": "Engineering",
      "leaveTypeId": "678cf2a4b3945e0001ac4d60",
      "leaveTypeName": "Annual Leave",
      "startDate": "2025-03-16T00:00:00Z",
      "endDate": "2025-03-22T00:00:00Z",
      "totalDays": 5,
      "status": "Pending",
      "reason": "Extended family vacation to Hawaii",
      "submittedAt": "2025-01-20T10:30:00Z"
    }
  ],
  "status": {
    "statusCode": 200
  }
}
```

---

## Leave Request Status Workflow

```
    Submitted
        ?
    Pending ??? Approved ??? Completed
        ?           ?
    Rejected   Cancelled
```

### Status Definitions

| Status | Description | Can Edit | Can Cancel |
|--------|-------------|----------|------------|
| `Pending` | Awaiting approval | Yes | Yes |
| `Approved` | Approved by manager | No | Yes (before start date) |
| `Rejected` | Rejected by manager | No | No |
| `Cancelled` | Cancelled by employee | No | No |
| `Completed` | Leave period finished | No | No |

---

## Business Rules

### Submission Rules
1. Start date must be in the future
2. End date must be after start date
3. Must have sufficient leave balance (unless negative allowed)
4. Cannot conflict with existing approved leave
5. Must respect max days per request for leave type
6. Must meet minimum service period (if applicable)

### Update Rules
1. Only pending requests can be updated
2. Same validation rules as submission apply
3. Updates trigger re-approval workflow if configured

### Cancellation Rules
1. Can cancel pending or approved requests
2. Cannot cancel rejected or already cancelled requests
3. Can only cancel approved leave before start date
4. Cancellation reason is mandatory
5. Balance is automatically restored

### Date Calculation
- Excludes weekends (Saturday, Sunday)
- Excludes company holidays
- Counts only business days
- Half-day requests count as 0.5 days

---

## Related Endpoints
- [Leave Types](./05-leave-types.md) - Configure leave types
- [Leave Balances](./07-leave-balances.md) - Check available balance
- [Leave Approvals](./09-leave-approvals.md) - Approve/reject requests

---

[? Back to Documentation Home](./README.md)
