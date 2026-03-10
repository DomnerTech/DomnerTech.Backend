# Leave Reports API

## Overview

Provides comprehensive reporting and analytics on leave usage, trends, and statistics.

**Base Path:** `/api/v1/leave-report`

**Authorization:** All endpoints require `LeaveRequest.Admin` role

---

## Endpoints Summary

| Method | Endpoint                                      | Description           |
| ------ | --------------------------------------------- | --------------------- |
| GET    | `/leave-report/usage`                         | Leave usage report    |
| GET    | `/leave-report/department-stats`              | Department statistics |
| GET    | `/leave-report/trends`                        | Leave trends analysis |
| GET    | `/leave-report/employee-summary/{employeeId}` | Employee summary      |

---

## 1. Get Leave Usage Report

**Endpoint:** `GET /api/v1/leave-report/usage`

### Query Parameters

| Parameter    | Type    | Required | Description       |
| ------------ | ------- | -------- | ----------------- |
| `year`       | integer | Yes      | Report year       |
| `department` | string  | No       | Department filter |

### Example Request

```
GET /leave-report/usage?year=2025&department=Engineering
```

### Response `200 OK`

```json
{
  "data": [
    {
      "employee_id": "678cf2a4b3945e0001ac4d40",
      "employee_name": "Sarah Johnson",
      "department": "Engineering",
      "leave_balances": [
        {
          "leave_type_name": "Annual Leave",
          "total_allowance": 22.0,
          "used": 8.0,
          "remaining": 14.0,
          "percentage_used": 36.4
        },
        {
          "leave_type_name": "Sick Leave",
          "total_allowance": 10.0,
          "used": 2.0,
          "remaining": 8.0,
          "percentage_used": 20.0
        }
      ],
      "total_days_used": 10.0,
      "total_days_remaining": 22.0
    }
  ],
  "status": {
    "status_code": 200
  }
}
```

---

## 2. Get Department Statistics

**Endpoint:** `GET /api/v1/leave-report/department-stats`

### Query Parameters

| Parameter    | Type   | Required | Description       |
| ------------ | ------ | -------- | ----------------- |
| `department` | string | No       | Department filter |

### Response `200 OK`

```json
{
  "data": [
    {
      "department": "Engineering",
      "total_employees": 25,
      "total_leave_allowance": 550.0,
      "total_leave_used": 312.5,
      "total_leave_remaining": 237.5,
      "average_days_used_per_employee": 12.5,
      "utilization_rate": 56.8,
      "leave_type_breakdown": [
        {
          "leave_type_name": "Annual Leave",
          "total_used": 187.5,
          "percentage_of_total": 60.0
        },
        {
          "leave_type_name": "Sick Leave",
          "total_used": 75.0,
          "percentage_of_total": 24.0
        },
        {
          "leave_type_name": "Personal Leave",
          "total_used": 50.0,
          "percentage_of_total": 16.0
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

## 3. Get Leave Trend

**Endpoint:** `GET /leave-report/trends?year=2025`

### Query Parameters

| Parameter | Type    | Required | Description             |
| --------- | ------- | -------- | ----------------------- |
| `year`    | integer | Yes      | Year for trend analysis |

### Response `200 OK`

```json
{
  "data": [
    {
      "month": 1,
      "month_name": "January",
      "total_days_taken": 45.0,
      "average_days_per_employee": 1.8,
      "most_popular_leave_type": "Sick Leave"
    },
    {
      "month": 7,
      "month_name": "July",
      "total_days_taken": 95.0,
      "average_days_per_employee": 3.8,
      "most_popular_leave_type": "Annual Leave"
    },
    {
      "month": 12,
      "month_name": "December",
      "total_days_taken": 112.5,
      "average_days_per_employee": 4.5,
      "most_popular_leave_type": "Annual Leave"
    }
  ],
  "status": {
    "status_code": 200
  }
}
```

---

## 4. Get Employee Leave Summary

**Endpoint:** `GET /api/v1/leave-report/employee-summary/{employeeId}?year=2025`

### Path Parameters

| Parameter    | Type   | Required | Description |
| ------------ | ------ | -------- | ----------- |
| `employeeId` | string | Yes      | Employee ID |

### Query Parameters

| Parameter | Type    | Required | Description      |
| --------- | ------- | -------- | ---------------- |
| `year`    | integer | Yes      | Year for summary |

### Response `200 OK`

```json
{
  "data": {
    "employee_id": "678cf2a4b3945e0001ac4d40",
    "employee_name": "Sarah Johnson",
    "department": "Engineering",
    "year": 2025,
    "leave_balances": [
      {
        "leave_type_id": "678cf2a4b3945e0001ac4d60",
        "leave_type_name": "Annual Leave",
        "total_allowance": 22.0,
        "carry_forward": 5.0,
        "used": 8.0,
        "pending": 5.0,
        "remaining": 9.0
      }
    ],
    "recent_requests": [
      {
        "id": "678cf2a4b3945e0001ac4d70",
        "leave_type_name": "Annual Leave",
        "start_date": "2025-03-16T00:00:00Z",
        "end_date": "2025-03-22T00:00:00Z",
        "total_days": 5.0,
        "status": "Pending"
      }
    ],
    "total_days_used": 10.0,
    "total_days_remaining": 17.0
  },
  "status": {
    "status_code": 200
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
