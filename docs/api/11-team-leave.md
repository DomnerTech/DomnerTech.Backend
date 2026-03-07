# Team Leave API

## Overview
Provides team-level leave visibility, conflict detection, and department leave planning capabilities.

**Base Path:** `/api/v1/team-leave`

---

## Endpoints Summary

| Method | Endpoint | Description | Authorization |
|--------|----------|-------------|---------------|
| GET | `/team-leave/calendar` | Get team calendar | `LeaveRequest.Admin` |
| POST | `/team-leave/check-conflicts` | Check leave conflicts | `LeaveRequest.Admin` |
| GET | `/team-leave/stats/{department}` | Get team statistics | `LeaveRequest.Admin` |
| GET | `/team-leave/upcoming/{department}` | Get upcoming leaves | `LeaveRequest.Admin` |

---

## 1. Get Team Leave Calendar

**Endpoint:** `GET /api/v1/team-leave/calendar`

**Authorization:** Required - Role: `LeaveRequest.Admin`

### Query Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `department` | string | Yes | Department name |
| `startDate` | datetime | Yes | Calendar start date |
| `endDate` | datetime | Yes | Calendar end date |

### Example Request
```
GET /team-leave/calendar?department=Engineering&startDate=2025-03-01T00:00:00Z&endDate=2025-03-31T23:59:59Z
```

### Response `200 OK`

```json
{
  "data": [
    {
      "date": "2025-03-16T00:00:00Z",
      "employeesOnLeave": [
        {
          "employeeId": "678cf2a4b3945e0001ac4d40",
          "employeeName": "Sarah Johnson",
          "leaveTypeName": "Annual Leave",
          "leaveTypeColor": "#4CAF50"
        }
      ],
      "totalOnLeave": 1
    },
    {
      "date": "2025-03-17T00:00:00Z",
      "employeesOnLeave": [
        {
          "employeeId": "678cf2a4b3945e0001ac4d40",
          "employeeName": "Sarah Johnson",
          "leaveTypeName": "Annual Leave",
          "leaveTypeColor": "#4CAF50"
        },
        {
          "employeeId": "678cf2a4b3945e0001ac4d42",
          "employeeName": "Michael Chen",
          "leaveTypeName": "Annual Leave",
          "leaveTypeColor": "#4CAF50"
        }
      ],
      "totalOnLeave": 2
    }
  ],
  "status": {
    "statusCode": 200
  }
}
```

---

## 2. Check Team Leave Conflicts

**Endpoint:** `POST /api/v1/team-leave/check-conflicts`

**Authorization:** Required - Role: `LeaveRequest.Admin`

### Request

```json
{
  "department": "Engineering",
  "startDate": "2025-03-01T00:00:00Z",
  "endDate": "2025-03-31T23:59:59Z",
  "maxEmployeesAllowed": 3
}
```

### Response `200 OK`

```json
{
  "data": [
    {
      "date": "2025-03-20T00:00:00Z",
      "conflictCount": 5,
      "maxAllowed": 3,
      "conflictingEmployees": [
        {
          "employeeId": "678cf2a4b3945e0001ac4d40",
          "employeeName": "Sarah Johnson",
          "leaveTypeName": "Annual Leave"
        },
        {
          "employeeId": "678cf2a4b3945e0001ac4d42",
          "employeeName": "Michael Chen",
          "leaveTypeName": "Annual Leave"
        },
        {
          "employeeId": "678cf2a4b3945e0001ac4d43",
          "employeeName": "Emily Davis",
          "leaveTypeName": "Personal Leave"
        },
        {
          "employeeId": "678cf2a4b3945e0001ac4d44",
          "employeeName": "David Wilson",
          "leaveTypeName": "Annual Leave"
        },
        {
          "employeeId": "678cf2a4b3945e0001ac4d45",
          "employeeName": "Lisa Anderson",
          "leaveTypeName": "Annual Leave"
        }
      ]
    }
  ],
  "status": {
    "statusCode": 200
  }
}
```

---

## 3. Get Team Leave Statistics

**Endpoint:** `GET /api/v1/team-leave/stats/{department}`

**Authorization:** Required - Role: `LeaveRequest.Admin`

### Response `200 OK`

```json
{
  "data": {
    "department": "Engineering",
    "totalEmployees": 25,
    "currentlyOnLeave": 2,
    "pendingRequests": 3,
    "upcomingLeave30Days": 8,
    "averageDaysPerEmployee": 12.5,
    "mostUsedLeaveType": {
      "leaveTypeId": "678cf2a4b3945e0001ac4d60",
      "leaveTypeName": "Annual Leave",
      "totalDaysUsed": 187.5
    }
  },
  "status": {
    "statusCode": 200
  }
}
```

---

## 4. Get Upcoming Team Leave

**Endpoint:** `GET /api/v1/team-leave/upcoming/{department}`

**Authorization:** Required - Role: `LeaveRequest.Admin`

### Response `200 OK`

```json
{
  "data": [
    {
      "date": "2025-03-16T00:00:00Z",
      "employeesOnLeave": [
        {
          "employeeId": "678cf2a4b3945e0001ac4d40",
          "employeeName": "Sarah Johnson",
          "leaveTypeName": "Annual Leave",
          "leaveTypeColor": "#4CAF50",
          "startDate": "2025-03-16T00:00:00Z",
          "endDate": "2025-03-22T00:00:00Z"
        }
      ],
      "totalOnLeave": 1
    }
  ],
  "status": {
    "statusCode": 200
  }
}
```

---

## Use Cases

### Planning Team Coverage
Managers can view team calendar to ensure adequate coverage during busy periods.

### Conflict Prevention
Check conflicts before approving leave to prevent understaffing.

### Resource Planning
View upcoming leaves to plan project assignments and deadlines.

---

## Related Endpoints
- [Leave Approvals](./09-leave-approvals.md) - Use conflict check before approving
- [Leave Requests](./06-leave-requests.md) - View individual requests

---

[? Back to Documentation Home](./README.md)
