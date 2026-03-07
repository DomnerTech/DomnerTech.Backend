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
      "employees_on_leave": [
        {
          "employee_id": "678cf2a4b3945e0001ac4d40",
          "employee_name": "Sarah Johnson",
          "leave_type_name": "Annual Leave",
          "leave_type_color": "#4CAF50"
        }
      ],
      "total_on_leave": 1
    },
    {
      "date": "2025-03-17T00:00:00Z",
      "employees_on_leave": [
        {
          "employee_id": "678cf2a4b3945e0001ac4d40",
          "employee_name": "Sarah Johnson",
          "leave_type_name": "Annual Leave",
          "leave_type_color": "#4CAF50"
        },
        {
          "employee_id": "678cf2a4b3945e0001ac4d42",
          "employee_name": "Michael Chen",
          "leave_type_name": "Annual Leave",
          "leave_type_color": "#4CAF50"
        }
      ],
      "total_on_leave": 2
    }
  ],
  "status": {
    "status_code": 200
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
  "start_date": "2025-03-01T00:00:00Z",
  "end_date": "2025-03-31T23:59:59Z",
  "max_employees_allowed": 3
}
```

### Response `200 OK`

```json
{
  "data": [
    {
      "date": "2025-03-20T00:00:00Z",
      "conflict_count": 5,
      "max_allowed": 3,
      "conflicting_employees": [
        {
          "employee_id": "678cf2a4b3945e0001ac4d40",
          "employee_name": "Sarah Johnson",
          "leave_type_name": "Annual Leave"
        },
        {
          "employee_id": "678cf2a4b3945e0001ac4d42",
          "employee_name": "Michael Chen",
          "leave_type_name": "Annual Leave"
        },
        {
          "employee_id": "678cf2a4b3945e0001ac4d43",
          "employee_name": "Emily Davis",
          "leave_type_name": "Personal Leave"
        },
        {
          "employee_id": "678cf2a4b3945e0001ac4d44",
          "employee_name": "David Wilson",
          "leave_type_name": "Annual Leave"
        },
        {
          "employee_id": "678cf2a4b3945e0001ac4d45",
          "employee_name": "Lisa Anderson",
          "leave_type_name": "Annual Leave"
        }
      ]
    }
  ],
  "status": {
    "status_code": 200
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
    "total_employees": 25,
    "currently_on_leave": 2,
    "pending_requests": 3,
    "upcoming_leave30_days": 8,
    "average_days_per_employee": 12.5,
    "most_used_leave_type": {
      "leave_type_id": "678cf2a4b3945e0001ac4d60",
      "leave_type_name": "Annual Leave",
      "total_days_used": 187.5
    }
  },
  "status": {
    "status_code": 200
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
      "employees_on_leave": [
        {
          "employee_id": "678cf2a4b3945e0001ac4d40",
          "employee_name": "Sarah Johnson",
          "leave_type_name": "Annual Leave",
          "leave_type_color": "#4CAF50",
          "start_date": "2025-03-16T00:00:00Z",
          "end_date": "2025-03-22T00:00:00Z"
        }
      ],
      "total_on_leave": 1
    }
  ],
  "status": {
    "status_code": 200
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
