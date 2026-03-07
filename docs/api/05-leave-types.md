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
X-Company-Id: 678cf2a0b3945e0001ac4d30
X-Correlation-Id: {uuid}
Content-Type: application/json
```

**Body:**
```json
{
  "name": "Annual Leave",
  "code": "AL",
  "description": "Standard paid annual leave for all employees",
  "isPaid": true,
  "requiresApproval": true,
  "maxDaysPerRequest": 15,
  "allowNegativeBalance": false,
  "displayOrder": 1,
  "color": "#4CAF50"
}
```

**Request Schema:**
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `name` | string | Yes | Leave type name (max 100 chars) |
| `code` | string | Yes | Short code/abbreviation (2-10 chars, unique) |
| `description` | string | Yes | Detailed description (max 500 chars) |
| `isPaid` | boolean | Yes | Whether leave is paid or unpaid |
| `requiresApproval` | boolean | Yes | Whether approval workflow is required |
| `maxDaysPerRequest` | integer | Yes | Maximum days per single request (1-365) |
| `allowNegativeBalance` | boolean | Yes | Allow negative balance for this type |
| `displayOrder` | integer | Yes | Display order in UI (lower = higher priority) |
| `color` | string | No | Hex color code for UI display (e.g., #4CAF50) |

#### Response

**Success Response:** `200 OK`
```json
{
  "data": "678cf2a4b3945e0001ac4d60",
  "status": {
    "statusCode": 200,
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
    "statusCode": 409,
    "message": "A leave type with code 'AL' already exists",
    "errorCode": "ERR_DUPLICATE_LEAVE_CODE"
  }
}
```

**Validation Error:** `400 Bad Request`
```json
{
  "data": null,
  "status": {
    "statusCode": 400,
    "message": "Validation failed",
    "errors": [
      {
        "field": "maxDaysPerRequest",
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
  -H "X-Company-Id: 678cf2a0b3945e0001ac4d30" \
  -H "X-Correlation-Id: 550e8400-e29b-41d4-a716-446655440000" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Annual Leave",
    "code": "AL",
    "description": "Standard paid annual leave for all employees",
    "isPaid": true,
    "requiresApproval": true,
    "maxDaysPerRequest": 15,
    "allowNegativeBalance": false,
    "displayOrder": 1,
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
X-Company-Id: 678cf2a0b3945e0001ac4d30
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
  "isPaid": true,
  "requiresApproval": true,
  "maxDaysPerRequest": 20,
  "allowNegativeBalance": false,
  "displayOrder": 1,
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
| `isPaid` | boolean | Yes | Whether leave is paid |
| `requiresApproval` | boolean | Yes | Whether approval is required |
| `maxDaysPerRequest` | integer | Yes | Maximum days per request |
| `allowNegativeBalance` | boolean | Yes | Allow negative balance |
| `displayOrder` | integer | Yes | Display order |
| `color` | string | No | Hex color code |

#### Response

**Success Response:** `200 OK`
```json
{
  "data": true,
  "status": {
    "statusCode": 200,
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
    "statusCode": 404,
    "message": "Leave type with ID '678cf2a4b3945e0001ac4d60' not found",
    "errorCode": "ERR_LEAVE_TYPE_NOT_FOUND"
  }
}
```

**Leave Type In Use:** `422 Unprocessable Entity`
```json
{
  "data": null,
  "status": {
    "statusCode": 422,
    "message": "Cannot modify leave type that has existing balances or requests. Consider creating a new leave type instead.",
    "errorCode": "ERR_LEAVE_TYPE_IN_USE"
  }
}
```

#### Example cURL

```bash
curl -X PUT https://api.domnertech.com/api/v1/leave-type \
  -H "Authorization: Bearer eyJhbGc..." \
  -H "X-Company-Id: 678cf2a0b3945e0001ac4d30" \
  -H "X-Correlation-Id: 550e8400-e29b-41d4-a716-446655440000" \
  -H "Content-Type: application/json" \
  -d '{
    "id": "678cf2a4b3945e0001ac4d60",
    "name": "Annual Leave",
    "code": "AL",
    "description": "Standard paid annual leave for all full-time employees",
    "isPaid": true,
    "requiresApproval": true,
    "maxDaysPerRequest": 20,
    "allowNegativeBalance": false,
    "displayOrder": 1,
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
    "statusCode": 200,
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
    "statusCode": 404,
    "message": "Leave type with ID '678cf2a4b3945e0001ac4d60' not found",
    "errorCode": "ERR_LEAVE_TYPE_NOT_FOUND"
  }
}
```

**Has Active Requests:** `422 Unprocessable Entity`
```json
{
  "data": null,
  "status": {
    "statusCode": 422,
    "message": "Cannot delete leave type with active or pending leave requests",
    "errorCode": "ERR_LEAVE_TYPE_HAS_ACTIVE_REQUESTS"
  }
}
```

#### Example cURL

```bash
curl -X DELETE https://api.domnertech.com/api/v1/leave-type/678cf2a4b3945e0001ac4d60 \
  -H "Authorization: Bearer eyJhbGc..." \
  -H "X-Company-Id: 678cf2a0b3945e0001ac4d30" \
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
X-Company-Id: 678cf2a0b3945e0001ac4d30
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
      "isPaid": true,
      "requiresApproval": true,
      "maxDaysPerRequest": 20,
      "allowNegativeBalance": false,
      "displayOrder": 1,
      "color": "#4CAF50",
      "isActive": true
    },
    {
      "id": "678cf2a4b3945e0001ac4d61",
      "name": "Sick Leave",
      "code": "SL",
      "description": "Leave for medical reasons",
      "isPaid": true,
      "requiresApproval": true,
      "maxDaysPerRequest": 10,
      "allowNegativeBalance": true,
      "displayOrder": 2,
      "color": "#FF9800",
      "isActive": true
    },
    {
      "id": "678cf2a4b3945e0001ac4d62",
      "name": "Personal Leave",
      "code": "PL",
      "description": "Personal time off",
      "isPaid": false,
      "requiresApproval": true,
      "maxDaysPerRequest": 5,
      "allowNegativeBalance": false,
      "displayOrder": 3,
      "color": "#2196F3",
      "isActive": true
    },
    {
      "id": "678cf2a4b3945e0001ac4d63",
      "name": "Maternity Leave",
      "code": "ML",
      "description": "Maternity leave for expecting mothers",
      "isPaid": true,
      "requiresApproval": true,
      "maxDaysPerRequest": 90,
      "allowNegativeBalance": false,
      "displayOrder": 4,
      "color": "#E91E63",
      "isActive": true
    },
    {
      "id": "678cf2a4b3945e0001ac4d64",
      "name": "Paternity Leave",
      "code": "PTL",
      "description": "Paternity leave for new fathers",
      "isPaid": true,
      "requiresApproval": true,
      "maxDaysPerRequest": 14,
      "allowNegativeBalance": false,
      "displayOrder": 5,
      "color": "#3F51B5",
      "isActive": true
    },
    {
      "id": "678cf2a4b3945e0001ac4d65",
      "name": "Bereavement Leave",
      "code": "BL",
      "description": "Leave for family bereavement",
      "isPaid": true,
      "requiresApproval": true,
      "maxDaysPerRequest": 5,
      "allowNegativeBalance": true,
      "displayOrder": 6,
      "color": "#607D8B",
      "isActive": true
    },
    {
      "id": "678cf2a4b3945e0001ac4d66",
      "name": "Study Leave",
      "code": "STL",
      "description": "Leave for education and training",
      "isPaid": false,
      "requiresApproval": true,
      "maxDaysPerRequest": 10,
      "allowNegativeBalance": false,
      "displayOrder": 7,
      "color": "#9C27B0",
      "isActive": true
    }
  ],
  "status": {
    "statusCode": 200
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
| `isPaid` | boolean | Whether leave is paid |
| `requiresApproval` | boolean | Whether approval is required |
| `maxDaysPerRequest` | integer | Maximum days per request |
| `allowNegativeBalance` | boolean | Allow negative balance |
| `displayOrder` | integer | Display order (lower first) |
| `color` | string | Hex color code for UI |
| `isActive` | boolean | Whether leave type is active |

#### Example cURL

```bash
curl -X GET https://api.domnertech.com/api/v1/leave-type \
  -H "Authorization: Bearer eyJhbGc..." \
  -H "X-Company-Id: 678cf2a0b3945e0001ac4d30" \
  -H "X-Correlation-Id: 550e8400-e29b-41d4-a716-446655440000"
```

#### Example JavaScript (Axios)

```javascript
async function getAllLeaveTypes() {
  const config = {
    headers: {
      'Authorization': `Bearer ${getAuthToken()}`,
      'X-Company-Id': '678cf2a0b3945e0001ac4d30',
      'X-Correlation-Id': generateUUID()
    }
  };

  try {
    const response = await axios.get(
      'https://api.domnertech.com/api/v1/leave-type',
      config
    );
    
    const leaveTypes = response.data.data;
    console.log(`Retrieved ${leaveTypes.length} leave types`);
    return leaveTypes;
  } catch (error) {
    console.error('Error fetching leave types:', error.response.data);
    throw error;
  }
}

// Usage: Populate dropdown
async function populateLeaveTypeDropdown() {
  const leaveTypes = await getAllLeaveTypes();
  const dropdown = document.getElementById('leaveTypeSelect');
  
  leaveTypes.forEach(type => {
    const option = document.createElement('option');
    option.value = type.id;
    option.textContent = `${type.name} (${type.code})`;
    option.style.color = type.color;
    dropdown.appendChild(option);
  });
}
```

#### Notes
- Returns only active leave types
- Ordered by `displayOrder` (ascending) then `name` (alphabetical)
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
  "data": {
    "id": "678cf2a4b3945e0001ac4d60",
    "name": "Annual Leave",
    "code": "AL",
    "description": "Standard paid annual leave for all full-time employees",
    "isPaid": true,
    "requiresApproval": true,
    "maxDaysPerRequest": 20,
    "allowNegativeBalance": false,
    "displayOrder": 1,
    "color": "#4CAF50",
    "isActive": true,
    "createdAt": "2024-01-15T10:00:00Z",
    "updatedAt": "2025-01-20T14:30:00Z"
  },
  "status": {
    "statusCode": 200
  }
}
```

**Error Responses:**

**Leave Type Not Found:** `404 Not Found`
```json
{
  "data": null,
  "status": {
    "statusCode": 404,
    "message": "Leave type with ID '678cf2a4b3945e0001ac4d60' not found",
    "errorCode": "ERR_LEAVE_TYPE_NOT_FOUND"
  }
}
```

#### Example cURL

```bash
curl -X GET https://api.domnertech.com/api/v1/leave-type/678cf2a4b3945e0001ac4d60 \
  -H "Authorization: Bearer eyJhbGc..." \
  -H "X-Company-Id: 678cf2a0b3945e0001ac4d30" \
  -H "X-Correlation-Id: 550e8400-e29b-41d4-a716-446655440000"
```

---

## Leave Type Object Reference

### Complete Leave Type Object
```json
{
  "id": "678cf2a4b3945e0001ac4d60",
  "companyId": "678cf2a0b3945e0001ac4d30",
  "name": "Annual Leave",
  "code": "AL",
  "description": "Standard paid annual leave for all full-time employees",
  "isPaid": true,
  "requiresApproval": true,
  "maxDaysPerRequest": 20,
  "allowNegativeBalance": false,
  "displayOrder": 1,
  "color": "#4CAF50",
  "isActive": true,
  "createdAt": "2024-01-15T10:00:00Z",
  "updatedAt": "2025-01-20T14:30:00Z"
}
```

### Field Constraints
| Field | Type | Constraints |
|-------|------|-------------|
| `id` | string | MongoDB ObjectId (24 hex chars) |
| `companyId` | string | MongoDB ObjectId (24 hex chars) |
| `name` | string | Max 100 characters, required |
| `code` | string | 2-10 characters, unique per company, uppercase recommended |
| `description` | string | Max 500 characters, required |
| `isPaid` | boolean | true = paid leave, false = unpaid |
| `requiresApproval` | boolean | true = approval workflow required |
| `maxDaysPerRequest` | integer | 1-365, maximum days per single request |
| `allowNegativeBalance` | boolean | Allow negative balance (e.g., for sick leave) |
| `displayOrder` | integer | Positive integer, lower = higher priority |
| `color` | string | Hex color code (#RRGGBB), optional |
| `isActive` | boolean | true = active, false = soft deleted |

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
- `isPaid: true` - Employee receives regular salary during leave
- `isPaid: false` - No salary during leave period

**Negative Balance:**
- `allowNegativeBalance: true` - Useful for sick leave, bereavement
- `allowNegativeBalance: false` - Enforces available balance check

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
  "isPaid": true,
  "requiresApproval": true,
  "maxDaysPerRequest": 20,
  "allowNegativeBalance": false,
  "color": "#4CAF50"
}
```

**Sick Leave:**
```json
{
  "name": "Sick Leave",
  "code": "SL",
  "isPaid": true,
  "requiresApproval": true,
  "maxDaysPerRequest": 10,
  "allowNegativeBalance": true,
  "color": "#FF9800"
}
```

**Personal/Unpaid Leave:**
```json
{
  "name": "Personal Leave",
  "code": "PL",
  "isPaid": false,
  "requiresApproval": true,
  "maxDaysPerRequest": 5,
  "allowNegativeBalance": false,
  "color": "#2196F3"
}
```

**Maternity Leave:**
```json
{
  "name": "Maternity Leave",
  "code": "ML",
  "isPaid": true,
  "requiresApproval": true,
  "maxDaysPerRequest": 90,
  "allowNegativeBalance": false,
  "color": "#E91E63"
}
```

**Bereavement Leave:**
```json
{
  "name": "Bereavement Leave",
  "code": "BL",
  "isPaid": true,
  "requiresApproval": true,
  "maxDaysPerRequest": 5,
  "allowNegativeBalance": true,
  "color": "#607D8B"
}
```

---

## Color Scheme Recommendations

Suggested colors for common leave types:

| Leave Type | Color | Hex Code | Visual Effect |
|------------|-------|----------|---------------|
| Annual/Vacation | Green | `#4CAF50` | Positive, relaxing |
| Sick Leave | Orange | `#FF9800` | Alert, caution |
| Personal Leave | Blue | `#2196F3` | Neutral, calm |
| Maternity | Pink | `#E91E63` | Warm, caring |
| Paternity | Indigo | `#3F51B5` | Professional |
| Bereavement | Gray | `#607D8B` | Somber, respectful |
| Study Leave | Purple | `#9C27B0` | Educational |
| Sabbatical | Teal | `#009688` | Refreshing |

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
      isPaid: true,
      requiresApproval: true,
      maxDaysPerRequest: 20,
      allowNegativeBalance: false,
      displayOrder: 1,
      color: "#4CAF50"
    },
    {
      name: "Sick Leave",
      code: "SL",
      description: "Medical leave",
      isPaid: true,
      requiresApproval: true,
      maxDaysPerRequest: 10,
      allowNegativeBalance: true,
      displayOrder: 2,
      color: "#FF9800"
    }
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
      ${leaveTypes.map(type => `
        <option value="${type.id}" 
                data-max-days="${type.maxDaysPerRequest}"
                data-paid="${type.isPaid}"
                style="color: ${type.color}">
          ${type.name} - Max ${type.maxDaysPerRequest} days
          ${type.isPaid ? '(Paid)' : '(Unpaid)'}
        </option>
      `).join('')}
    </select>
  `;
}
```

### Use Case 3: Validate Request Against Leave Type
```javascript
async function validateLeaveRequest(leaveTypeId, requestedDays) {
  const leaveType = await getLeaveTypeById(leaveTypeId);
  
  if (requestedDays > leaveType.maxDaysPerRequest) {
    throw new Error(
      `Maximum ${leaveType.maxDaysPerRequest} days allowed for ${leaveType.name}`
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
