# Leave Reports API

## Overview
Provides comprehensive reporting and analytics on leave usage, trends, and statistics.

**Base Path:** `/api/v1/leave-report`

**Authorization:** All endpoints require `LeaveRequest.Admin` role

---

## Endpoints Summary

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/leave-report/usage` | Leave usage report |
| GET | `/leave-report/department-stats` | Department statistics |
| GET | `/leave-report/trends` | Leave trends analysis |
| GET | `/leave-report/employee-summary/{employeeId}` | Employee summary |

---

## 1. Get Leave Usage Report

**Endpoint:** `GET /api/v1/leave-report/usage`

### Query Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `year` | integer | Yes | Report year |
| `department` | string | No | Department filter |

### Example Request
```
GET /leave-report/usage?year=2025&department=Engineering
```

### Response `200 OK`

```json
{
  "data": [
    {
      "employeeId": "678cf2a4b3945e0001ac4d40",
      "employeeName": "Sarah Johnson",
      "department": "Engineering",
      "leaveBalances": [
        {
          "leaveTypeName": "Annual Leave",
          "totalAllowance": 22.0,
          "used": 8.0,
          "remaining": 14.0,
          "percentageUsed": 36.4
        },
        {
          "leaveTypeName": "Sick Leave",
          "totalAllowance": 10.0,
          "used": 2.0,
          "remaining": 8.0,
          "percentageUsed": 20.0
        }
      ],
      "totalDaysUsed": 10.0,
      "totalDaysRemaining": 22.0
    }
  ],
  "status": {
    "statusCode": 200
  }
}
```

---

## 2. Get Department Statistics

**Endpoint:** `GET /api/v1/leave-report/department-stats`

### Query Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `department` | string | No | Department filter |

### Response `200 OK`

```json
{
  "data": [
    {
      "department": "Engineering",
      "totalEmployees": 25,
      "totalLeaveAllowance": 550.0,
      "totalLeaveUsed": 312.5,
      "totalLeaveRemaining": 237.5,
      "averageDaysUsedPerEmployee": 12.5,
      "utilizationRate": 56.8,
      "leaveTypeBreakdown": [
        {
          "leaveTypeName": "Annual Leave",
          "totalUsed": 187.5,
          "percentageOfTotal": 60.0
        },
        {
          "leaveTypeName": "Sick Leave",
          "totalUsed": 75.0,
          "percentageOfTotal": 24.0
        },
        {
          "leaveTypeName": "Personal Leave",
          "totalUsed": 50.0,
          "percentageOfTotal": 16.0
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

## 3. Get Leave Trend

**Endpoint:** `GET /leave-report/trends?year=2025`

### Query Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `year` | integer | Yes | Year for trend analysis |

### Response `200 OK`

```json
{
  "data": [
    {
      "month": 1,
      "monthName": "January",
      "totalDaysTaken": 45.0,
      "averageDaysPerEmployee": 1.8,
      "mostPopularLeaveType": "Sick Leave"
    },
    {
      "month": 7,
      "monthName": "July",
      "totalDaysTaken": 95.0,
      "averageDaysPerEmployee": 3.8,
      "mostPopularLeaveType": "Annual Leave"
    },
    {
      "month": 12,
      "monthName": "December",
      "totalDaysTaken": 112.5,
      "averageDaysPerEmployee": 4.5,
      "mostPopularLeaveType": "Annual Leave"
    }
  ],
  "status": {
    "statusCode": 200
  }
}
```

---

## 4. Get Employee Leave Summary

**Endpoint:** `GET /api/v1/leave-report/employee-summary/{employeeId}?year=2025`

### Path Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `employeeId` | string | Yes | Employee ID |

### Query Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `year` | integer | Yes | Year for summary |

### Response `200 OK`

```json
{
  "data": {
    "employeeId": "678cf2a4b3945e0001ac4d40",
    "employeeName": "Sarah Johnson",
    "department": "Engineering",
    "year": 2025,
    "leaveBalances": [
      {
        "leaveTypeId": "678cf2a4b3945e0001ac4d60",
        "leaveTypeName": "Annual Leave",
        "totalAllowance": 22.0,
        "carryForward": 5.0,
        "used": 8.0,
        "pending": 5.0,
        "remaining": 9.0
      }
    ],
    "recentRequests": [
      {
        "id": "678cf2a4b3945e0001ac4d70",
        "leaveTypeName": "Annual Leave",
        "startDate": "2025-03-16T00:00:00Z",
        "endDate": "2025-03-22T00:00:00Z",
        "totalDays": 5.0,
        "status": "Pending"
      }
    ],
    "totalDaysUsed": 10.0,
    "totalDaysRemaining": 17.0
  },
  "status": {
    "statusCode": 200
  }
}
```

---

## Report Types

### Usage Reports
- Individual employee usage
- Department-wide usage
- Leave type breakdown
- Year-over-year comparison

### Trend Analysis
- Monthly patterns
- Seasonal trends
- Popular leave periods
- Utilization rates

### Compliance Reports
- Audit trails
- Policy compliance
- Balance accuracy
- Approval workflows

---

## Export Formats

Reports can be exported in:
- JSON (default via API)
- CSV (add `?format=csv` parameter)
- Excel (add `?format=xlsx` parameter)
- PDF (add `?format=pdf` parameter)

---

## Related Endpoints
- [Admin Dashboard](./13-admin-dashboard.md) - Quick stats overview
- [Leave Balances](./07-leave-balances.md) - Individual balance details

---

[? Back to Documentation Home](./README.md)
