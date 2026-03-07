# Leave Types API

## Overview

Manages leave type configurations, including creation, updates, deletion, and retrieval of leave types used throughout the system.

**Base Path:** `/api/v1/leave-type`

---

## Endpoints

### 1. Create Leave Type

Creates a new leave type configuration.

**Endpoint:** `POST /api/v1/leave-type`

**Authorization:** Required - Role: `LeaveType.Write`

#### Request

**Headers:**

```http
Authorization: Bearer {your_jwt_token}
X-Correlation-Id: {uuid}
Content-Type: application/json
```

**Body:**

```json
{
  "name": "Annual Leave",
  "code": "AL",
  "description": "Standard paid annual leave for all employees",
  "is_paid": true,
  "requires_approval": true,
  "max_days_per_request": 15,
  "allow_negative_nalance": false,
  "display_order": 1,
  "color": "#4CAF50"
}
```

**Request Schema:**
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `name` | string | Yes | Leave type name (max 100 chars) |
| `code` | string | Yes | Short code/abbreviation (2-10 chars, unique) |
| `description` | string | Yes | Detailed description (max 500 chars) |
| `is_paid` | boolean | Yes | Whether leave is paid or unpaid |
| `requires_approval` | boolean | Yes | Whether approval workflow is required |
| `max_days_per_request` | integer | Yes | Maximum days per single request (1-365) |
| `allow_negative_nalance` | boolean | Yes | Allow negative balance for this type |
| `display_order` | integer | Yes | Display order in UI (lower = higher priority) |
| `color` | string | No | Hex color code for UI display (e.g., #4CAF50) |

#### Response

**Success Response:** `200 OK`

```json
{
  "data": "678cf2a4b3945e0001ac4d60",
  "status": {
    "status_code": 200,
    "message": "Leave type created successfully"
  }
}
```

**Error Responses:**

**Duplicate Code:** `409 Conflict`

```json
{
  "data": null,
  "status": {
    "status_code": 409,
    "message": "A leave type with code 'AL' already exists",
    "error_code": "ERR_DUPLICATE_LEAVE_CODE"
  }
}
```

**Validation Error:** `400 Bad Request`

```json
{
  "data": null,
  "status": {
    "status_code": 400,
    "message": "Validation failed",
    "errors": [
      {
        "field": "max_days_per_request",
        "message": "Maximum days per request must be between 1 and 365"
      },
      {
        "field": "color",
        "message": "Color must be a valid hex code (e.g., #4CAF50)"
      }
    ]
  }
}
```

#### Example cURL

```bash
curl -X POST https://api.domnertech.com/api/v1/leave-type \
  -H "Authorization: Bearer eyJhbGc..." \
  -H "X-Correlation-Id: 550e8400-e29b-41d4-a716-446655440000" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Annual Leave",
    "code": "AL",
    "description": "Standard paid annual leave for all employees",
    "is_paid": true,
    "requires_approval": true,
    "max_days_per_request": 15,
    "allow_negative_nalance": false,
    "display_order": 1,
    "color": "#4CAF50"
  }'
```

#### Notes

- Leave type code must be unique within the company
- Only HR administrators should create leave types
- New leave types are active by default
- Color codes help with calendar visualization

---

### 2. Update Leave Type

Updates an existing leave type configuration.

**Endpoint:** `PUT /api/v1/leave-type`

**Authorization:** Required - Role: `LeaveType.Write`

#### Request

**Headers:**

```http
Authorization: Bearer {your_jwt_token}
X-Correlation-Id: {uuid}
Content-Type: application/json
```

**Body:**

```json
{
  "id": "678cf2a4b3945e0001ac4d60",
  "name": "Annual Leave",
  "code": "AL",
  "description": "Standard paid annual leave for all full-time employees",
  "is_paid": true,
  "requires_approval": true,
  "max_days_per_request": 20,
  "allow_negative_nalance": false,
  "display_order": 1,
  "color": "#4CAF50"
}
```

**Request Schema:**
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `id` | string | Yes | Leave type's unique identifier |
| `name` | string | Yes | Leave type name |
| `code` | string | Yes | Short code/abbreviation |
| `description` | string | Yes | Detailed description |
| `is_paid` | boolean | Yes | Whether leave is paid |
| `requires_approval` | boolean | Yes | Whether approval is required |
| `max_days_per_request` | integer | Yes | Maximum days per request |
| `allow_negative_nalance` | boolean | Yes | Allow negative balance |
| `display_order` | integer | Yes | Display order |
| `color` | string | No | Hex color code |

#### Response

**Success Response:** `200 OK`

```json
{
  "data": true,
  "status": {
    "status_code": 200,
    "message": "Leave type updated successfully"
  }
}
```

**Error Responses:**

**Leave Type Not Found:** `404 Not Found`

```json
{
  "data": null,
  "status": {
    "status_code": 404,
    "message": "Leave type with ID '678cf2a4b3945e0001ac4d60' not found",
    "error_code": "ERR_LEAVE_TYPE_NOT_FOUND"
  }
}
```

**Leave Type In Use:** `422 Unprocessable Entity`

```json
{
  "data": null,
  "status": {
    "status_code": 422,
    "message": "Cannot modify leave type that has existing balances or requests. Consider creating a new leave type instead.",
    "error_code": "ERR_LEAVE_TYPE_IN_USE"
  }
}
```

#### Example cURL

```bash
curl -X PUT https://api.domnertech.com/api/v1/leave-type \
  -H "Authorization: Bearer eyJhbGc..." \
  -H "X-Correlation-Id: 550e8400-e29b-41d4-a716-446655440000" \
  -H "Content-Type: application/json" \
  -d '{
    "id": "678cf2a4b3945e0001ac4d60",
    "name": "Annual Leave",
    "code": "AL",
    "description": "Standard paid annual leave for all full-time employees",
    "is_paid": true,
    "requires_approval": true,
    "max_days_per_request": 20,
    "allow_negative_nalance": false,
    "display_order": 1,
    "color": "#4CAF50"
  }'
```

#### Notes

- Be careful when modifying leave types with existing balances
- Changes affect future leave requests immediately
- Consider the impact on pending requests

---

### 3. Delete Leave Type

Soft deletes a leave type (marks as inactive).

**Endpoint:** `DELETE /api/v1/leave-type/{id}`

**Authorization:** Required - Role: `LeaveType.Write`

#### Request

**Headers:**

```http
Authorization: Bearer {your_jwt_token}
X-Company-Id: 678cf2a0b3945e0001ac4d30
X-Correlation-Id: {uuid}
```

**Path Parameters:**
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `id` | string | Yes | Leave type's unique identifier |

#### Response

**Success Response:** `200 OK`

```json
{
  "data": true,
  "status": {
    "status_code": 200,
    "message": "Leave type deleted successfully"
  }
}
```

**Error Responses:**

**Leave Type Not Found:** `404 Not Found`

```json
{
  "data": null,
  "status": {
    "status_code": 404,
    "message": "Leave type with ID '678cf2a4b3945e0001ac4d60' not found",
    "error_code": "ERR_LEAVE_TYPE_NOT_FOUND"
  }
}
```

**Has Active Requests:** `422 Unprocessable Entity`

```json
{
  "data": null,
  "status": {
    "status_code": 422,
    "message": "Cannot delete leave type with active or pending leave requests",
    "error_code": "ERR_LEAVE_TYPE_HAS_ACTIVE_REQUESTS"
  }
}
```

#### Example cURL

```bash
curl -X DELETE https://api.domnertech.com/api/v1/leave-type/678cf2a4b3945e0001ac4d60 \
  -H "Authorization: Bearer eyJhbGc..." \
  -H "X-Correlation-Id: 550e8400-e29b-41d4-a716-446655440000"
```

#### Notes

- This is a soft delete - records are marked inactive, not removed
- Cannot delete leave types with pending or approved requests
- Historical data remains intact
- Deleted leave types don't appear in dropdowns for new requests

---

### 4. Get All Leave Types

Retrieves all active leave types ordered by display order and name.

**Endpoint:** `GET /api/v1/leave-type`

**Authorization:** Required - Role: `LeaveType.Read`

#### Request

**Headers:**

```http
Authorization: Bearer {your_jwt_token}
X-Correlation-Id: {uuid}
```

#### Response

**Success Response:** `200 OK`

```json
{
  "data": [
    {
      "id": "678cf2a4b3945e0001ac4d60",
      "name": "Annual Leave",
      "code": "AL",
      "description": "Standard paid annual leave for all full-time employees",
      "is_paid": true,
      "requires_approval": true,
      "max_days_per_request": 20,
      "allow_negative_nalance": false,
      "display_order": 1,
      "color": "#4CAF50",
      "is_active": true
    },
    {
      "id": "678cf2a4b3945e0001ac4d61",
      "name": "Sick Leave",
      "code": "SL",
      "description": "Leave for medical reasons",
      "is_paid": true,
      "requires_approval": true,
      "max_days_per_request": 10,
      "allow_negative_nalance": true,
      "display_order": 2,
      "color": "#FF9800",
      "is_active": true
    },
    {
      "id": "678cf2a4b3945e0001ac4d62",
      "name": "Personal Leave",
      "code": "PL",
      "description": "Personal time off",
      "is_paid": false,
      "requires_approval": true,
      "max_days_per_request": 5,
      "allow_negative_nalance": false,
      "display_order": 3,
      "color": "#2196F3",
      "is_active": true
    },
    {
      "id": "678cf2a4b3945e0001ac4d63",
      "name": "Maternity Leave",
      "code": "ML",
      "description": "Maternity leave for expecting mothers",
      "is_paid": true,
      "requires_approval": true,
      "max_days_per_request": 90,
      "allow_negative_nalance": false,
      "display_order": 4,
      "color": "#E91E63",
      "is_active": true
    },
    {
      "id": "678cf2a4b3945e0001ac4d64",
      "name": "Paternity Leave",
      "code": "PTL",
      "description": "Paternity leave for new fathers",
      "is_paid": true,
      "requires_approval": true,
      "max_days_per_request": 14,
      "allow_negative_nalance": false,
      "display_order": 5,
      "color": "#3F51B5",
      "is_active": true
    },
    {
      "id": "678cf2a4b3945e0001ac4d65",
      "name": "Bereavement Leave",
      "code": "BL",
      "description": "Leave for family bereavement",
      "is_paid": true,
      "requires_approval": true,
      "max_days_per_request": 5,
      "allow_negative_nalance": true,
      "display_order": 6,
      "color": "#607D8B",
      "is_active": true
    },
    {
      "id": "678cf2a4b3945e0001ac4d66",
      "name": "Study Leave",
      "code": "STL",
      "description": "Leave for education and training",
      "is_paid": false,
      "requires_approval": true,
      "max_days_per_request": 10,
      "allow_negative_nalance": false,
      "display_order": 7,
      "color": "#9C27B0",
      "is_active": true
    }
  ],
  "status": {
    "status_code": 200
  }
}
```

**Response Schema:**
| Field | Type | Description |
|-------|------|-------------|
| `id` | string | Leave type's unique identifier |
| `name` | string | Leave type name |
| `code` | string | Short code/abbreviation |
| `description` | string | Detailed description |
| `is_paid` | boolean | Whether leave is paid |
| `requires_approval` | boolean | Whether approval is required |
| `max_days_per_request` | integer | Maximum days per request |
| `allow_negative_nalance` | boolean | Allow negative balance |
| `display_order` | integer | Display order (lower first) |
| `color` | string | Hex color code for UI |
| `is_active` | boolean | Whether leave type is active |

#### Example cURL

```bash
curl -X GET https://api.domnertech.com/api/v1/leave-type \
  -H "Authorization: Bearer eyJhbGc..." \
  -H "X-Correlation-Id: 550e8400-e29b-41d4-a716-446655440000"
```

#### Example JavaScript (Axios)

```javascript
async function getAllLeaveTypes() {
  const config = {
    headers: {
      Authorization: `Bearer ${getAuthToken()}`,
      "X-Correlation-Id": generateUUID(),
    },
  };

  try {
    const response = await axios.get(
      "https://api.domnertech.com/api/v1/leave-type",
      config,
    );

    const leaveTypes = response.data.data;
    console.log(`Retrieved ${leaveTypes.length} leave types`);
    return leaveTypes;
  } catch (error) {
    console.error("Error fetching leave types:", error.response.data);
    throw error;
  }
}

// Usage: Populate dropdown
async function populateLeaveTypeDropdown() {
  const leaveTypes = await getAllLeaveTypes();
  const dropdown = document.getElementById("leaveTypeSelect");

  leaveTypes.forEach((type) => {
    const option = document.createElement("option");
    option.value = type.id;
    option.textContent = `${type.name} (${type.code})`;
    option.style.color = type.color;
    dropdown.appendChild(option);
  });
}
```

#### Notes

- Returns only active leave types
- Ordered by `display_order` (ascending) then `name` (alphabetical)
- Use for populating dropdowns in leave request forms
- Color codes useful for calendar visualization

---

### 5. Get Leave Type by ID

Retrieves a specific leave type by its unique identifier.

**Endpoint:** `GET /api/v1/leave-type/{id}`

**Authorization:** Required - Role: `LeaveType.Read`

#### Request

**Headers:**

```http
Authorization: Bearer {your_jwt_token}
X-Correlation-Id: {uuid}
```

**Path Parameters:**
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `id` | string | Yes | Leave type's unique identifier |

#### Response

**Success Response:** `200 OK`

```json
{
  "data": {
    "id": "678cf2a4b3945e0001ac4d60",
    "name": "Annual Leave",
    "code": "AL",
    "description": "Standard paid annual leave for all full-time employees",
    "is_paid": true,
    "requires_approval": true,
    "max_days_per_request": 20,
    "allow_negative_nalance": false,
    "display_order": 1,
    "color": "#4CAF50",
    "is_active": true,
    "created_at": "2024-01-15T10:00:00Z",
    "updated_at": "2025-01-20T14:30:00Z"
  },
  "status": {
    "status_code": 200
  }
}
```

**Error Responses:**

**Leave Type Not Found:** `404 Not Found`

```json
{
  "data": null,
  "status": {
    "status_code": 404,
    "message": "Leave type with ID '678cf2a4b3945e0001ac4d60' not found",
    "error_code": "ERR_LEAVE_TYPE_NOT_FOUND"
  }
}
```

#### Example cURL

```bash
curl -X GET https://api.domnertech.com/api/v1/leave-type/678cf2a4b3945e0001ac4d60 \
  -H "Authorization: Bearer eyJhbGc..." \
  -H "X-Correlation-Id: 550e8400-e29b-41d4-a716-446655440000"
```

---

## Leave Type Object Reference

### Complete Leave Type Object

```json
{
  "id": "678cf2a4b3945e0001ac4d60",
  "company_id": "678cf2a0b3945e0001ac4d30",
  "name": "Annual Leave",
  "code": "AL",
  "description": "Standard paid annual leave for all full-time employees",
  "is_paid": true,
  "requires_approval": true,
  "max_days_per_request": 20,
  "allow_negative_nalance": false,
  "display_order": 1,
  "color": "#4CAF50",
  "is_active": true,
  "created_at": "2024-01-15T10:00:00Z",
  "updated_at": "2025-01-20T14:30:00Z"
}
```

### Field Constraints

| Field                    | Type    | Constraints                                                |
| ------------------------ | ------- | ---------------------------------------------------------- |
| `id`                     | string  | MongoDB ObjectId (24 hex chars)                            |
| `companyId`              | string  | MongoDB ObjectId (24 hex chars)                            |
| `name`                   | string  | Max 100 characters, required                               |
| `code`                   | string  | 2-10 characters, unique per company, uppercase recommended |
| `description`            | string  | Max 500 characters, required                               |
| `is_paid`                | boolean | true = paid leave, false = unpaid                          |
| `requires_approval`      | boolean | true = approval workflow required                          |
| `max_days_per_request`   | integer | 1-365, maximum days per single request                     |
| `allow_negative_nalance` | boolean | Allow negative balance (e.g., for sick leave)              |
| `display_order`          | integer | Positive integer, lower = higher priority                  |
| `color`                  | string  | Hex color code (#RRGGBB), optional                         |
| `is_active`              | boolean | true = active, false = soft deleted                        |

---

## Business Rules

### Leave Type Creation

1. **Code Uniqueness:** Code must be unique within the company
2. **Uppercase Convention:** Codes typically uppercase (e.g., AL, SL, PL)
3. **Default Active:** New leave types are active by default
4. **Display Order:** Controls UI ordering (1 = highest priority)

### Leave Type Updates

1. **Impact on Existing Data:** Changes don't affect historical requests
2. **Future Requests:** New settings apply to all future requests
3. **Pending Requests:** Be cautious - may affect pending approvals

### Leave Type Deletion

1. **Soft Delete:** Records marked inactive, not physically deleted
2. **Cannot Delete:** If active or pending requests exist
3. **Historical Integrity:** Past requests remain linked to deleted types

### Configuration Guidelines

**Paid vs Unpaid:**

- `is_paid: true` - Employee receives regular salary during leave
- `is_paid: false` - No salary during leave period

**Negative Balance:**

- `allow_negative_nalance: true` - Useful for sick leave, bereavement
- `allow_negative_nalance: false` - Enforces available balance check

**Max Days Per Request:**

- Set realistic limits based on leave type
- Annual leave: 15-20 days typical
- Sick leave: 5-10 days typical
- Maternity leave: 60-120 days typical

---

## Common Leave Type Configurations

### Standard Leave Types

**Annual/Vacation Leave:**

```json
{
  "name": "Annual Leave",
  "code": "AL",
  "is_paid": true,
  "requires_approval": true,
  "max_days_per_request": 20,
  "allow_negative_nalance": false,
  "color": "#4CAF50"
}
```

**Sick Leave:**

```json
{
  "name": "Sick Leave",
  "code": "SL",
  "is_paid": true,
  "requires_approval": true,
  "max_days_per_request": 10,
  "allow_negative_nalance": true,
  "color": "#FF9800"
}
```

**Personal/Unpaid Leave:**

```json
{
  "name": "Personal Leave",
  "code": "PL",
  "is_paid": false,
  "requires_approval": true,
  "max_days_per_request": 5,
  "allow_negative_nalance": false,
  "color": "#2196F3"
}
```

**Maternity Leave:**

```json
{
  "name": "Maternity Leave",
  "code": "ML",
  "is_paid": true,
  "requires_approval": true,
  "max_days_per_request": 90,
  "allow_negative_nalance": false,
  "color": "#E91E63"
}
```

**Bereavement Leave:**

```json
{
  "name": "Bereavement Leave",
  "code": "BL",
  "is_paid": true,
  "requires_approval": true,
  "max_days_per_request": 5,
  "allow_negative_nalance": true,
  "color": "#607D8B"
}
```

---

## Color Scheme Recommendations

Suggested colors for common leave types:

| Leave Type      | Color  | Hex Code  | Visual Effect      |
| --------------- | ------ | --------- | ------------------ |
| Annual/Vacation | Green  | `#4CAF50` | Positive, relaxing |
| Sick Leave      | Orange | `#FF9800` | Alert, caution     |
| Personal Leave  | Blue   | `#2196F3` | Neutral, calm      |
| Maternity       | Pink   | `#E91E63` | Warm, caring       |
| Paternity       | Indigo | `#3F51B5` | Professional       |
| Bereavement     | Gray   | `#607D8B` | Somber, respectful |
| Study Leave     | Purple | `#9C27B0` | Educational        |
| Sabbatical      | Teal   | `#009688` | Refreshing         |

---

## Common Use Cases

### Use Case 1: Initial Setup

```javascript
async function setupLeaveTypes() {
  const standardTypes = [
    {
      name: "Annual Leave",
      code: "AL",
      description: "Standard paid vacation",
      is_paid: true,
      requires_approval: true,
      max_days_per_request: 20,
      allow_negative_nalance: false,
      display_order: 1,
      color: "#4CAF50",
    },
    {
      name: "Sick Leave",
      code: "SL",
      description: "Medical leave",
      is_paid: true,
      requires_approval: true,
      max_days_per_request: 10,
      allow_negative_nalance: true,
      display_order: 2,
      color: "#FF9800",
    },
  ];

  for (const type of standardTypes) {
    await createLeaveType(type);
  }
}
```

### Use Case 2: Display Leave Types in Form

```javascript
async function renderLeaveRequestForm() {
  const leaveTypes = await getAllLeaveTypes();

  return `
    <select id="leaveType">
      <option value="">Select Leave Type</option>
      ${leaveTypes
        .map(
          (type) => `
        <option value="${type.id}" 
                data-max-days="${type.max_days_per_request}"
                data-paid="${type.is_paid}"
                style="color: ${type.color}">
          ${type.name} - Max ${type.max_days_per_request} days
          ${type.is_paid ? "(Paid)" : "(Unpaid)"}
        </option>
      `,
        )
        .join("")}
    </select>
  `;
}
```

### Use Case 3: Validate Request Against Leave Type

```javascript
async function validateLeaveRequest(leaveTypeId, requestedDays) {
  const leaveType = await getLeaveTypeById(leaveTypeId);

  if (requestedDays > leaveType.max_days_per_request) {
    throw new Error(
      `Maximum ${leaveType.max_days_per_request} days allowed for ${leaveType.name}`,
    );
  }

  return true;
}
```

---

## Related Endpoints

- [Leave Policies](./08-leave-policies.md) - Configure policies per leave type
- [Leave Balances](./07-leave-balances.md) - Track balances per leave type
- [Leave Requests](./06-leave-requests.md) - Submit requests for specific leave types

---

[? Back to Documentation Home](./README.md)
