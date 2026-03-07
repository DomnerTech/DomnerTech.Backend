# Admin Dashboard API

## Overview
Provides high-level statistics and summaries for administrative dashboards and management views.

**Base Path:** `/api/v1/admin-dashboard`

**Authorization:** All endpoints require `LeaveRequest.Admin` role

---

## Endpoints Summary

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/admin-dashboard/stats` | Dashboard statistics |
| GET | `/admin-dashboard/employees-on-leave` | Current employees on leave |
| GET | `/admin-dashboard/upcoming-leaves` | Upcoming approved leaves |
| GET | `/admin-dashboard/pending-approvals` | Pending approval summary |

---

## 1. Get Dashboard Statistics

**Endpoint:** `GET /api/v1/admin-dashboard/stats`

### Response `200 OK`

```json
{
  "data": {
    "total_employees": 250,
    "employees_currently_on_leave": 12,
    "pending_approvals": 8,
    "approved_leave_this_month": 45,
    "total_days_taken_this_month": 187.5,
    "average_days_per_employee": 0.75,
    "upcoming_leave_next7_days": 15,
    "upcoming_leave_next30_days": 52,
    "leave_utilization_rate": 58.3,
    "department_breakdown": [
      {
        "department": "Engineering",
        "total_employees": 75,
        "on_leave_today": 3,
        "pending_approvals": 2
      },
      {
        "department": "Sales",
        "total_employees": 50,
        "on_leave_today": 4,
        "pending_approvals": 3
      },
      {
        "department": "Marketing",
        "total_employees": 35,
        "on_leave_today": 2,
        "pending_approvals": 1
      }
    ],
    "leave_type_usage": [
      {
        "leave_type_name": "Annual Leave",
        "total_days": 312.5,
        "percentage_of_total": 62.5
      },
      {
        "leave_type_name": "Sick Leave",
        "total_days": 125.0,
        "percentage_of_total": 25.0
      },
      {
        "leave_type_name": "Personal Leave",
        "total_days": 62.5,
        "percentage_of_total": 12.5
      }
    ]
  },
  "status": {
    "status_code": 200
  }
}
```

---

## 2. Get Employees Currently On Leave

**Endpoint:** `GET /api/v1/admin-dashboard/employees-on-leave`

### Response `200 OK`

```json
{
  "data": [
    {
      "employee_id": "678cf2a4b3945e0001ac4d40",
      "employee_name": "Sarah Johnson",
      "department": "Engineering",
      "job_title": "Lead Software Engineer",
      "leave_type_id": "678cf2a4b3945e0001ac4d60",
      "leave_type_name": "Annual Leave",
      "leave_type_color": "#4CAF50",
      "start_date": "2025-01-20T00:00:00Z",
      "end_date": "2025-01-22T23:59:59Z",
      "return_date": "2025-01-23T00:00:00Z",
      "days_remaining": 2,
      "reason": "Personal vacation"
    },
    {
      "employee_id": "678cf2a4b3945e0001ac4d42",
      "employee_name": "Michael Chen",
      "department": "Engineering",
      "job_title": "Staff Software Engineer",
      "leave_type_id": "678cf2a4b3945e0001ac4d61",
      "leave_type_name": "Sick Leave",
      "leave_type_color": "#FF9800",
      "start_date": "2025-01-21T00:00:00Z",
      "end_date": "2025-01-21T23:59:59Z",
      "return_date": "2025-01-22T00:00:00Z",
      "days_remaining": 1,
      "reason": "Medical appointment"
    }
  ],
  "status": {
    "status_code": 200
  }
}
```

---

## 3. Get Upcoming Leaves

**Endpoint:** `GET /api/v1/admin-dashboard/upcoming-leaves?days=30`

### Query Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `days` | integer | No | Number of days to look ahead (default: 30) |

### Response `200 OK`

```json
{
  "data": [
    {
      "date": "2025-02-15T00:00:00Z",
      "employees_starting_leave": [
        {
          "employee_id": "678cf2a4b3945e0001ac4d43",
          "employee_name": "Emily Davis",
          "department": "Marketing",
          "leave_type_name": "Annual Leave",
          "leave_type_color": "#4CAF50",
          "end_date": "2025-02-20T23:59:59Z",
          "total_days": 5
        }
      ],
      "total_starting_leave": 1
    },
    {
      "date": "2025-03-16T00:00:00Z",
      "employees_starting_leave": [
        {
          "employee_id": "678cf2a4b3945e0001ac4d40",
          "employee_name": "Sarah Johnson",
          "department": "Engineering",
          "leave_type_name": "Annual Leave",
          "leave_type_color": "#4CAF50",
          "end_date": "2025-03-22T23:59:59Z",
          "total_days": 5
        },
        {
          "employee_id": "678cf2a4b3945e0001ac4d44",
          "employee_name": "David Wilson",
          "department": "Sales",
          "leave_type_name": "Personal Leave",
          "leave_type_color": "#2196F3",
          "end_date": "2025-03-17T23:59:59Z",
          "total_days": 2
        }
      ],
      "total_starting_leave": 2
    }
  ],
  "status": {
    "status_code": 200
  }
}
```

---

## 4. Get Pending Approvals Summary

**Endpoint:** `GET /api/v1/admin-dashboard/pending-approvals`

### Response `200 OK`

```json
{
  "data": {
    "total_pending": 8,
    "urgent_approvals": 2,
    "by_department": [
      {
        "department": "Engineering",
        "pending_count": 3,
        "oldest_request_days": 5
      },
      {
        "department": "Sales",
        "pending_count": 3,
        "oldest_request_days": 3
      },
      {
        "department": "Marketing",
        "pending_count": 2,
        "oldest_request_days": 2
      }
    ],
    "by_leave_type": [
      {
        "leave_type_name": "Annual Leave",
        "pending_count": 5,
        "total_days_requested": 25.0
      },
      {
        "leave_type_name": "Sick Leave",
        "pending_count": 2,
        "total_days_requested": 4.0
      },
      {
        "leave_type_name": "Personal Leave",
        "pending_count": 1,
        "total_days_requested": 3.0
      }
    ],
    "oldest_pending_requests": [
      {
        "leave_request_id": "678cf2a4b3945e0001ac4d70",
        "employee_name": "Sarah Johnson",
        "department": "Engineering",
        "leave_type_name": "Annual Leave",
        "requested_days": 5.0,
        "submitted_at": "2025-01-15T10:00:00Z",
        "days_ago": 5
      },
      {
        "leave_request_id": "678cf2a4b3945e0001ac4d71",
        "employee_name": "Michael Chen",
        "department": "Engineering",
        "leave_type_name": "Sick Leave",
        "requested_days": 2.0,
        "submitted_at": "2025-01-17T08:00:00Z",
        "days_ago": 3
      }
    ]
  },
  "status": {
    "status_code": 200
  }
}
```

---

## Dashboard Metrics

### Key Performance Indicators (KPIs)

**Leave Utilization Rate:**
```
Utilization Rate = (Total Days Taken / Total Days Allocated) � 100
```

**Average Days Per Employee:**
```
Average = Total Days Taken / Total Employees
```

**Approval Efficiency:**
```
Efficiency = Requests Approved within 24hrs / Total Requests
```

---

## Use Cases

### Executive Dashboard
- Quick overview of company-wide leave status
- Identify departments with high utilization
- Track pending approvals requiring attention

### Department Manager View
- Monitor team leave patterns
- Plan resource allocation
- Identify coverage gaps

### HR Administration
- Track policy compliance
- Monitor approval workflows
- Generate compliance reports

---

## Refresh Intervals

Recommended refresh intervals for real-time dashboards:
- **Dashboard Statistics:** Every 5 minutes
- **Employees on Leave:** Every 1 minute
- **Pending Approvals:** Every 30 seconds
- **Upcoming Leaves:** Every 30 minutes

---

## Related Endpoints
- [Leave Reports](./12-leave-reports.md) - Detailed analytics
- [Team Leave](./11-team-leave.md) - Department-specific views
- [Leave Approvals](./09-leave-approvals.md) - Process pending approvals

---

[? Back to Documentation Home](./README.md)
