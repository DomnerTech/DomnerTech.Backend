# Leave Policies API

## Overview

Manages leave policy configurations that define rules for leave types including allowances, accrual, and carry-forward rules.

**Base Path:** `/api/v1/leave-policy`

---

## Endpoints Summary

| Method | Endpoint                                 | Description              | Authorization       |
| ------ | ---------------------------------------- | ------------------------ | ------------------- |
| POST   | `/leave-policy`                          | Create policy            | `LeavePolicy.Write` |
| PUT    | `/leave-policy`                          | Update policy            | `LeavePolicy.Write` |
| DELETE | `/leave-policy/{id}`                     | Delete policy            | `LeavePolicy.Write` |
| GET    | `/leave-policy/{id}`                     | Get policy by ID         | `LeavePolicy.Read`  |
| GET    | `/leave-policy`                          | Get active policies      | `LeavePolicy.Read`  |
| GET    | `/leave-policy/leave-type/{leaveTypeId}` | Get policy by leave type | `LeavePolicy.Read`  |

---

## 1. Create Leave Policy

**Endpoint:** `POST /api/v1/leave-policy`

**Authorization:** Required - Role: `LeavePolicy.Write`

### Request

```json
{
  "name": "Standard Annual Leave Policy",
  "leave_type_id": "678cf2a4b3945e0001ac4d60",
  "default_allowance": 20.0,
  "accrual_type": "Yearly",
  "accrual_rate": 1.67,
  "max_carry_forward": 5.0,
  "carry_forward_expiry_months": 3,
  "min_service_months": 6,
  "effective_from": "2025-01-01T00:00:00Z",
  "effective_to": null
}
```

### Response `200 OK`

```json
{
  "data": "678cf2a4b3945e0001ac4d90",
  "status": {
    "status_code": 200,
    "message": "Leave policy created successfully"
  }
}
```

---

## 2. Update Leave Policy

**Endpoint:** `PUT /api/v1/leave-policy`

### Request

```json
{
  "id": "678cf2a4b3945e0001ac4d90",
  "name": "Enhanced Annual Leave Policy",
  "leave_type_id": "678cf2a4b3945e0001ac4d60",
  "default_allowance": 22.0,
  "accrual_type": "Monthly",
  "accrual_rate": 1.83,
  "max_carry_forward": 7.0,
  "carry_forward_expiry_months": 3,
  "min_service_months": 3,
  "effective_from": "2025-01-01T00:00:00Z"
}
```

### Response `200 OK`

```json
{
  "data": true,
  "status": {
    "status_code": 200,
    "message": "Leave policy updated successfully"
  }
}
```

---

## 3. Delete Leave Policy

**Endpoint:** `DELETE /api/v1/leave-policy/{id}`

### Response `200 OK`

```json
{
  "data": true,
  "status": {
    "status_code": 200,
    "message": "Leave policy deleted successfully"
  }
}
```

---

## 4. Get Leave Policy by ID

**Endpoint:** `GET /api/v1/leave-policy/{id}`

### Response `200 OK`

```json
{
  "data": {
    "id": "678cf2a4b3945e0001ac4d90",
    "name": "Enhanced Annual Leave Policy",
    "leave_type_id": "678cf2a4b3945e0001ac4d60",
    "leave_type_name": "Annual Leave",
    "default_allowance": 22.0,
    "accrual_type": "Monthly",
    "accrual_rate": 1.83,
    "max_carry_forward": 7.0,
    "carry_forward_expiry_months": 3,
    "min_service_months": 3,
    "effective_from": "2025-01-01T00:00:00Z",
    "effective_to": null,
    "is_active": true
  },
  "status": {
    "status_code": 200
  }
}
```

---

## 5. Get Active Policies

**Endpoint:** `GET /api/v1/leave-policy`

### Response `200 OK`

```json
{
  "data": [
    {
      "id": "678cf2a4b3945e0001ac4d90",
      "name": "Enhanced Annual Leave Policy",
      "leave_type_id": "678cf2a4b3945e0001ac4d60",
      "leave_type_name": "Annual Leave",
      "default_allowance": 22.0,
      "accrual_type": "Monthly",
      "max_carry_forward": 7.0,
      "is_active": true
    }
  ],
  "status": {
    "status_code": 200
  }
}
```

---

## 6. Get Policy by Leave Type

**Endpoint:** `GET /api/v1/leave-policy/leave-type/{leaveTypeId}`

### Response `200 OK`

```json
{
  "data": {
    "id": "678cf2a4b3945e0001ac4d90",
    "name": "Enhanced Annual Leave Policy",
    "leave_type_id": "678cf2a4b3945e0001ac4d60",
    "leave_type_name": "Annual Leave",
    "default_allowance": 22.0,
    "accrual_type": "Monthly",
    "accrual_rate": 1.83,
    "max_carry_forward": 7.0,
    "min_service_months": 3,
    "effective_from": "2025-01-01T00:00:00Z",
    "is_active": true
  },
  "status": {
    "status_code": 200
  }
}
```

---

## Policy Configuration

### Accrual Types

**Yearly Accrual:**

- Entire allowance granted at start of year
- Simple and straightforward
- Common for most companies

**Monthly Accrual:**

- Days accrue monthly based on rate
- Pro-rated for new employees
- Formula: `accrualRate * monthsWorked`

**Daily Accrual:**

- Days accrue per working day
- Most granular approach
- Formula: `accrualRate * daysWorked`

### Carry Forward Rules

- **maxCarryForward:** Maximum days that can carry to next year
- **carryForwardExpiryMonths:** Months before carried days expire
- **Example:** 5 days carry forward, expires after 3 months

### Minimum Service

- **minServiceMonths:** Months of service required before eligible
- **Probation Period:** Typically 3-6 months
- **New Employees:** May not be eligible immediately

---

## Business Rules

1. **One Policy Per Leave Type:** Only one active policy per leave type at a time
2. **Effective Dates:** `effectiveFrom` determines when policy starts
3. **Policy Changes:** Create new policy with new effective date for changes
4. **Historical Data:** Old policies remain for historical accuracy
5. **Default Allowance:** Used when initializing employee balances

---

## Common Policies

### Standard Annual Leave

```json
{
  "name": "Standard Annual Leave",
  "default_allowance": 20.0,
  "accrual_type": "Yearly",
  "max_carry_forward": 5.0,
  "min_service_months": 6
}
```

### Sick Leave (No Carry Forward)

```json
{
  "name": "Sick Leave Policy",
  "default_allowance": 10.0,
  "accrual_type": "Yearly",
  "max_carry_forward": 0.0,
  "min_service_months": 0
}
```

---

## Related Endpoints

- [Leave Types](./05-leave-types.md) - Define leave types
- [Leave Balances](./07-leave-balances.md) - Policies determine initial balances

---

[? Back to Documentation Home](./README.md)
