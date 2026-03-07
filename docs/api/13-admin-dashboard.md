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
    "totalEmployees": 250,
    "employeesCurrentlyOnLeave": 12,
    "pendingApprovals": 8,
    "approvedLeaveThisMonth": 45,
    "totalDaysTakenThisMonth": 187.5,
    "averageDaysPerEmployee": 0.75,
    "upcomingLeaveNext7Days": 15,
    "upcomingLeaveNext30Days": 52,
    "leaveUtilizationRate": 58.3,
    "departmentBreakdown": [
      {
        "department": "Engineering",
        "totalEmployees": 75,
        "onLeaveToday": 3,
        "pendingApprovals": 2
      },
      {
        "department": "Sales",
        "totalEmployees": 50,
        "onLeaveToday": 4,
        "pendingApprovals": 3
      },
      {
        "department": "Marketing",
        "totalEmployees": 35,
        "onLeaveToday": 2,
        "pendingApprovals": 1
      }
    ],
    "leaveTypeUsage": [
      {
        "leaveTypeName": "Annual Leave",
        "totalDays": 312.5,
        "percentageOfTotal": 62.5
      },
      {
        "leaveTypeName": "Sick Leave",
        "totalDays": 125.0,
        "percentageOfTotal": 25.0
      },
      {
        "leaveTypeName": "Personal Leave",
        "totalDays": 62.5,
        "percentageOfTotal": 12.5
      }
    ]
  },
  "status": {
    "statusCode": 200
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
      "employeeId": "678cf2a4b3945e0001ac4d40",
      "employeeName": "Sarah Johnson",
      "department": "Engineering",
      "jobTitle": "Lead Software Engineer",
      "leaveTypeId": "678cf2a4b3945e0001ac4d60",
      "leaveTypeName": "Annual Leave",
      "leaveTypeColor": "#4CAF50",
      "startDate": "2025-01-20T00:00:00Z",
      "endDate": "2025-01-22T23:59:59Z",
      "returnDate": "2025-01-23T00:00:00Z",
      "daysRemaining": 2,
      "reason": "Personal vacation"
    },
    {
      "employeeId": "678cf2a4b3945e0001ac4d42",
      "employeeName": "Michael Chen",
      "department": "Engineering",
      "jobTitle": "Staff Software Engineer",
      "leaveTypeId": "678cf2a4b3945e0001ac4d61",
      "leaveTypeName": "Sick Leave",
      "leaveTypeColor": "#FF9800",
      "startDate": "2025-01-21T00:00:00Z",
      "endDate": "2025-01-21T23:59:59Z",
      "returnDate": "2025-01-22T00:00:00Z",
      "daysRemaining": 1,
      "reason": "Medical appointment"
    }
  ],
  "status": {
    "statusCode": 200
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
      "employeesStartingLeave": [
        {
          "employeeId": "678cf2a4b3945e0001ac4d43",
          "employeeName": "Emily Davis",
          "department": "Marketing",
          "leaveTypeName": "Annual Leave",
          "leaveTypeColor": "#4CAF50",
          "endDate": "2025-02-20T23:59:59Z",
          "totalDays": 5
        }
      ],
      "totalStartingLeave": 1
    },
    {
      "date": "2025-03-16T00:00:00Z",
      "employeesStartingLeave": [
        {
          "employeeId": "678cf2a4b3945e0001ac4d40",
          "employeeName": "Sarah Johnson",
          "department": "Engineering",
          "leaveTypeName": "Annual Leave",
          "leaveTypeColor": "#4CAF50",
          "endDate": "2025-03-22T23:59:59Z",
          "totalDays": 5
        },
        {
          "employeeId": "678cf2a4b3945e0001ac4d44",
          "employeeName": "David Wilson",
          "department": "Sales",
          "leaveTypeName": "Personal Leave",
          "leaveTypeColor": "#2196F3",
          "endDate": "2025-03-17T23:59:59Z",
          "totalDays": 2
        }
      ],
      "totalStartingLeave": 2
    }
  ],
  "status": {
    "statusCode": 200
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
    "totalPending": 8,
    "urgentApprovals": 2,
    "byDepartment": [
      {
        "department": "Engineering",
        "pendingCount": 3,
        "oldestRequestDays": 5
      },
      {
        "department": "Sales",
        "pendingCount": 3,
        "oldestRequestDays": 3
      },
      {
        "department": "Marketing",
        "pendingCount": 2,
        "oldestRequestDays": 2
      }
    ],
    "byLeaveType": [
      {
        "leaveTypeName": "Annual Leave",
        "pendingCount": 5,
        "totalDaysRequested": 25.0
      },
      {
        "leaveTypeName": "Sick Leave",
        "pendingCount": 2,
        "totalDaysRequested": 4.0
      },
      {
        "leaveTypeName": "Personal Leave",
        "pendingCount": 1,
        "totalDaysRequested": 3.0
      }
    ],
    "oldestPendingRequests": [
      {
        "leaveRequestId": "678cf2a4b3945e0001ac4d70",
        "employeeName": "Sarah Johnson",
        "department": "Engineering",
        "leaveTypeName": "Annual Leave",
        "requestedDays": 5.0,
        "submittedAt": "2025-01-15T10:00:00Z",
        "daysAgo": 5
      },
      {
        "leaveRequestId": "678cf2a4b3945e0001ac4d71",
        "employeeName": "Michael Chen",
        "department": "Engineering",
        "leaveTypeName": "Sick Leave",
        "requestedDays": 2.0,
        "submittedAt": "2025-01-17T08:00:00Z",
        "daysAgo": 3
      }
    ]
  },
  "status": {
    "statusCode": 200
  }
}
```

---

## Dashboard Metrics

### Key Performance Indicators (KPIs)

**Leave Utilization Rate:**
```
Utilization Rate = (Total Days Taken / Total Days Allocated) × 100
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
