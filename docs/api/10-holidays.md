# Holidays API

## Overview

Manages public holidays and company-specific holidays used for leave calculations and calendar planning.

**Base Path:** `/api/v1/holiday`

---

## Endpoints Summary

| Method | Endpoint               | Description           | Authorization   |
| ------ | ---------------------- | --------------------- | --------------- |
| POST   | `/holiday`             | Create holiday        | `Holiday.Write` |
| POST   | `/holiday/bulk`        | Bulk create holidays  | `Holiday.Write` |
| PUT    | `/holiday`             | Update holiday        | `Holiday.Write` |
| DELETE | `/holiday/{id}`        | Delete holiday        | `Holiday.Write` |
| GET    | `/holiday/year/{year}` | Get holidays by year  | `Holiday.Read`  |
| GET    | `/holiday/upcoming`    | Get upcoming holidays | `Holiday.Read`  |

---

## 1. Create Holiday

**Endpoint:** `POST /api/v1/holiday`

**Authorization:** Required - Role: `Holiday.Write`

### Request

```json
{
  "name": "Independence Day",
  "date": "2025-07-04T00:00:00Z",
  "description": "National Independence Day celebration",
  "is_recurring": true,
  "applies_to_all_departments": true,
  "departments": []
}
```

### Response `200 OK`

```json
{
  "data": "678cf2a4b3945e0001ac4db0",
  "status": {
    "status_code": 200,
    "message": "Holiday created successfully"
  }
}
```

---

## 2. Bulk Create Holidays

**Endpoint:** `POST /api/v1/holiday/bulk`

**Authorization:** Required - Role: `Holiday.Write`

### Request

```json
{
  "holidays": [
    {
      "name": "New Year's Day",
      "date": "2025-01-01T00:00:00Z",
      "description": "New Year celebration",
      "is_recurring": true,
      "applies_to_all_departments": true
    },
    {
      "name": "Labor Day",
      "date": "2025-09-01T00:00:00Z",
      "description": "International Workers' Day",
      "is_recurring": true,
      "applies_to_all_departments": true
    },
    {
      "name": "Christmas Day",
      "date": "2025-12-25T00:00:00Z",
      "description": "Christmas celebration",
      "is_recurring": true,
      "applies_to_all_departments": true
    }
  ]
}
```

### Response `200 OK`

```json
{
  "data": 3,
  "status": {
    "status_code": 200,
    "message": "3 holidays created successfully"
  }
}
```

---

## 3. Update Holiday

**Endpoint:** `PUT /api/v1/holiday`

### Request

```json
{
  "id": "678cf2a4b3945e0001ac4db0",
  "name": "Independence Day (US)",
  "date": "2025-07-04T00:00:00Z",
  "description": "United States Independence Day celebration",
  "is_recurring": true,
  "applies_to_all_departments": true,
  "departments": []
}
```

### Response `200 OK`

```json
{
  "data": true,
  "status": {
    "status_code": 200,
    "message": "Holiday updated successfully"
  }
}
```

---

## 4. Delete Holiday

**Endpoint:** `DELETE /api/v1/holiday/{id}`

### Response `200 OK`

```json
{
  "data": true,
  "status": {
    "status_code": 200,
    "message": "Holiday deleted successfully"
  }
}
```

---

## 5. Get Holidays by Year

**Endpoint:** `GET /api/v1/holiday/year/{year}`

**Authorization:** Required - Role: `Holiday.Read`

### Response `200 OK`

```json
{
  "data": [
    {
      "id": "678cf2a4b3945e0001ac4db1",
      "name": "New Year's Day",
      "date": "2025-01-01T00:00:00Z",
      "description": "New Year celebration",
      "is_recurring": true,
      "applies_to_all_departments": true,
      "departments": []
    },
    {
      "id": "678cf2a4b3945e0001ac4db0",
      "name": "Independence Day (US)",
      "date": "2025-07-04T00:00:00Z",
      "description": "United States Independence Day celebration",
      "is_recurring": true,
      "applies_to_all_departments": true,
      "departments": []
    },
    {
      "id": "678cf2a4b3945e0001ac4db2",
      "name": "Labor Day",
      "date": "2025-09-01T00:00:00Z",
      "description": "International Workers' Day",
      "is_recurring": true,
      "applies_to_all_departments": true,
      "departments": []
    },
    {
      "id": "678cf2a4b3945e0001ac4db3",
      "name": "Christmas Day",
      "date": "2025-12-25T00:00:00Z",
      "description": "Christmas celebration",
      "is_recurring": true,
      "applies_to_all_departments": true,
      "departments": []
    }
  ],
  "status": {
    "status_code": 200
  }
}
```

---

## 6. Get Upcoming Holidays

**Endpoint:** `GET /api/v1/holiday/upcoming?count=5`

**Authorization:** Required - Role: `Holiday.Read`

### Query Parameters

| Parameter | Type    | Required | Description                              |
| --------- | ------- | -------- | ---------------------------------------- |
| `count`   | integer | No       | Maximum holidays to return (default: 10) |

### Response `200 OK`

```json
{
  "data": [
    {
      "id": "678cf2a4b3945e0001ac4db0",
      "name": "Independence Day (US)",
      "date": "2025-07-04T00:00:00Z",
      "description": "United States Independence Day celebration",
      "days_until": 165,
      "is_recurring": true
    },
    {
      "id": "678cf2a4b3945e0001ac4db2",
      "name": "Labor Day",
      "date": "2025-09-01T00:00:00Z",
      "description": "International Workers' Day",
      "days_until": 224,
      "is_recurring": true
    }
  ],
  "status": {
    "status_code": 200
  }
}
```

---

## Holiday Configuration

### Recurring Holidays

- `isRecurring: true` - Holiday repeats annually
- System can auto-create for future years
- Examples: New Year, Christmas, Independence Day

### Department-Specific Holidays

- `appliesToAllDepartments: false` - Specify departments
- `departments: ["Sales", "Marketing"]` - List of departments
- Useful for regional offices or specific teams

---

## Common Holiday Calendars

### US Holidays

```json
[
  { "name": "New Year's Day", "date": "01-01", "is_recurring": true },
  {
    "name": "Martin Luther King Jr. Day",
    "date": "01-15",
    "is_recurring": true
  },
  { "name": "Presidents' Day", "date": "02-19", "is_recurring": true },
  { "name": "Memorial Day", "date": "05-27", "is_recurring": true },
  { "name": "Independence Day", "date": "07-04", "is_recurring": true },
  { "name": "Labor Day", "date": "09-02", "is_recurring": true },
  { "name": "Thanksgiving", "date": "11-28", "is_recurring": true },
  { "name": "Christmas Day", "date": "12-25", "is_recurring": true }
]
```

---

## Business Rules

1. **Leave Calculation:** Holidays excluded from leave day count
2. **Weekend Handling:** Holiday on weekend may roll to Monday
3. **Regional Variations:** Different offices can have different holidays
4. **Recurring Logic:** System auto-creates next year's holidays
5. **Calendar Display:** Holidays marked distinctly in calendar views

---

## Related Endpoints

- [Leave Requests](./06-leave-requests.md) - Holidays affect day calculations
- [Team Leave](./11-team-leave.md) - Holidays shown in team calendar

---

[? Back to Documentation Home](./README.md)
