# Leave Balances API

## Overview
Manages employee leave balances including initialization, adjustments, and balance tracking.

**Base Path:** `/api/v1/leave-balance`

---

## Endpoints Summary

| Method | Endpoint | Description | Authorization |
|--------|----------|-------------|---------------|
| POST | `/leave-balance` | Initialize leave balance | `LeaveBalance.Write` |
| POST | `/leave-balance/adjust` | Adjust leave balance | `LeaveBalance.Write` |
| GET | `/leave-balance/my` | Get my balances | `LeaveBalance.Read` |
| GET | `/leave-balance/employee/{employeeId}` | Get employee balances | `LeaveBalance.Write` |

---

## 1. Initialize Leave Balance

Creates initial leave balance for an employee for a specific leave type and year.

**Endpoint:** `POST /api/v1/leave-balance`

**Authorization:** Required - Role: `LeaveBalance.Write`

### Request

```json
{
  "employeeId": "678cf2a4b3945e0001ac4d40",
  "leaveTypeId": "678cf2a4b3945e0001ac4d60",
  "year": 2025,
  "totalAllowance": 20.0,
  "carryForward": 5.0
}
```

**Schema:**
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `employeeId` | string | Yes | Employee ID |
| `leaveTypeId` | string | Yes | Leave type ID |
| `year` | integer | Yes | Year (e.g., 2025) |
| `totalAllowance` | decimal | Yes | Total days allocated |
| `carryForward` | decimal | No | Days carried from previous year (default: 0) |

### Response `200 OK`

```json
{
  "data": "678cf2a4b3945e0001ac4d80",
  "status": {
    "statusCode": 200,
    "message": "Leave balance initialized successfully"
  }
}
```

### Business Rules
- Can only initialize once per employee/leave type/year combination
- Total allowance must be positive
- Carry forward cannot exceed policy limits

---

## 2. Adjust Leave Balance

Manually adjusts an employee's leave balance (add or subtract days).

**Endpoint:** `POST /api/v1/leave-balance/adjust`

**Authorization:** Required - Role: `LeaveBalance.Write`

### Request

```json
{
  "employeeId": "678cf2a4b3945e0001ac4d40",
  "leaveTypeId": "678cf2a4b3945e0001ac4d60",
  "year": 2025,
  "adjustmentDays": 2.0,
  "reason": "Additional leave granted for exceptional performance"
}
```

**Schema:**
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `employeeId` | string | Yes | Employee ID |
| `leaveTypeId` | string | Yes | Leave type ID |
| `year` | integer | Yes | Year |
| `adjustmentDays` | decimal | Yes | Days to add (+) or subtract (-) |
| `reason` | string | Yes | Reason for adjustment (max 500 chars) |

### Response `200 OK`

```json
{
  "data": true,
  "status": {
    "statusCode": 200,
    "message": "Leave balance adjusted successfully"
  }
}
```

### Use Cases for Adjustments
- **Positive Adjustment (+):** Bonus days, compensation, policy changes
- **Negative Adjustment (-):** Corrections, penalties, policy changes
- All adjustments are audited with reason and timestamp

---

## 3. Get My Leave Balances

Retrieves leave balances for the authenticated user for a specific year.

**Endpoint:** `GET /api/v1/leave-balance/my?year=2025`

**Authorization:** Required - Role: `LeaveBalance.Read`

### Query Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `year` | integer | Yes | Year to retrieve balances for |

### Response `200 OK`

```json
{
  "data": [
    {
      "leaveTypeId": "678cf2a4b3945e0001ac4d60",
      "leaveTypeName": "Annual Leave",
      "leaveTypeColor": "#4CAF50",
      "year": 2025,
      "totalAllowance": 22.0,
      "used": 3.0,
      "remaining": 19.0,
      "carryForward": 5.0,
      "pending": 5.0,
      "available": 14.0
    },
    {
      "leaveTypeId": "678cf2a4b3945e0001ac4d61",
      "leaveTypeName": "Sick Leave",
      "leaveTypeColor": "#FF9800",
      "year": 2025,
      "totalAllowance": 10.0,
      "used": 2.0,
      "remaining": 8.0,
      "carryForward": 0.0,
      "pending": 0.0,
      "available": 8.0
    }
  ],
  "status": {
    "statusCode": 200
  }
}
```

### Field Descriptions
- **totalAllowance:** Base allocation + carry forward + adjustments
- **used:** Days already taken (approved and completed)
- **pending:** Days in pending/approved requests (not yet taken)
- **remaining:** totalAllowance - used
- **available:** remaining - pending (actual days available to request)

---

## 4. Get Employee Leave Balances

Retrieves leave balances for a specific employee (HR/Manager access).

**Endpoint:** `GET /api/v1/leave-balance/employee/{employeeId}?year=2025`

**Authorization:** Required - Role: `LeaveBalance.Write`

### Path Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `employeeId` | string | Yes | Employee's unique identifier |

### Query Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `year` | integer | Yes | Year to retrieve balances for |

### Response `200 OK`

```json
{
  "data": [
    {
      "leaveTypeId": "678cf2a4b3945e0001ac4d60",
      "leaveTypeName": "Annual Leave",
      "leaveTypeColor": "#4CAF50",
      "year": 2025,
      "totalAllowance": 22.0,
      "used": 3.0,
      "remaining": 19.0,
      "carryForward": 5.0,
      "pending": 5.0,
      "available": 14.0,
      "adjustmentHistory": [
        {
          "date": "2025-01-15T10:00:00Z",
          "adjustmentDays": 2.0,
          "reason": "Performance bonus",
          "adjustedBy": "HR Manager"
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

## Balance Calculation

### Formula

```
totalAllowance = baseAllowance + carryForward + adjustments
used = sum(approved and completed leave days)
pending = sum(pending and approved but not started leave days)
remaining = totalAllowance - used
available = remaining - pending
```

### Example Calculation

Given:
- Base Allowance: 15 days
- Carry Forward: 5 days
- Adjustment: +2 days (bonus)
- Used: 3 days
- Pending: 5 days

Calculation:
```
totalAllowance = 15 + 5 + 2 = 22 days
used = 3 days
remaining = 22 - 3 = 19 days
available = 19 - 5 = 14 days
```

Result: Employee has **14 days available** to request

---

## Business Rules

### Initialization
1. Can only initialize once per employee/type/year
2. Typically done at start of calendar/fiscal year
3. Carry forward rules depend on leave policy
4. System can auto-initialize based on policy

### Balance Updates
Balances are automatically updated when:
- Leave request is approved (deducts from available)
- Leave is completed (moves pending to used)
- Leave is cancelled (restores balance)
- Leave is rejected (restores balance)

### Negative Balances
- Allowed only if leave type permits (`allowNegativeBalance: true`)
- Typically for sick leave, bereavement leave
- Cannot go negative for leave types that don't allow it

### Carry Forward Rules
- Maximum carry forward defined in leave policy
- Unused days may expire based on policy
- Carry forward typically expires after certain period (e.g., 3 months)

---

## Common Use Cases

### Use Case 1: Annual Balance Initialization
```javascript
async function initializeAnnualBalances(employeeId, year) {
  const leaveTypes = await getAllLeaveTypes();
  const policies = await getActivePolicies();
  
  for (const type of leaveTypes) {
    const policy = policies.find(p => p.leaveTypeId === type.id);
    if (policy) {
      await initializeLeaveBalance({
        employeeId: employeeId,
        leaveTypeId: type.id,
        year: year,
        totalAllowance: policy.defaultAllowance,
        carryForward: await calculateCarryForward(employeeId, type.id, year - 1)
      });
    }
  }
}
```

### Use Case 2: Check Available Balance
```javascript
async function canRequestLeave(employeeId, leaveTypeId, requestedDays) {
  const balances = await getEmployeeLeaveBalances(employeeId, new Date().getFullYear());
  const balance = balances.find(b => b.leaveTypeId === leaveTypeId);
  
  if (!balance) {
    throw new Error('Leave balance not initialized');
  }
  
  if (balance.available < requestedDays) {
    throw new Error(`Insufficient balance. Available: ${balance.available} days`);
  }
  
  return true;
}
```

### Use Case 3: Award Bonus Leave
```javascript
async function awardBonusLeave(employeeId, days, reason) {
  await adjustLeaveBalance({
    employeeId: employeeId,
    leaveTypeId: annualLeaveTypeId,
    year: new Date().getFullYear(),
    adjustmentDays: days,
    reason: reason
  });
  
  // Send notification
  await sendNotification(employeeId, `You have been awarded ${days} bonus leave days!`);
}
```

---

## Related Endpoints
- [Leave Requests](./06-leave-requests.md) - Balances are checked when submitting requests
- [Leave Policies](./08-leave-policies.md) - Define default allowances and carry forward rules
- [Leave Types](./05-leave-types.md) - Configure if negative balance is allowed

---

[? Back to Documentation Home](./README.md)
