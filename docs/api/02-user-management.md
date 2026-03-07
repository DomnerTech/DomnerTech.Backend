# User Management API

## Overview
Manages user accounts, including creation, retrieval, and listing with pagination support.

**Base Path:** `/api/v1/user`

---

## Endpoints

### 1. Get Current User

Retrieves the authenticated user's information.

**Endpoint:** `GET /api/v1/user/get-me`

**Authorization:** Required (any authenticated user)

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
  "data": {
    "id": "678cf2a4b3945e0001ac4d3a",
    "companyId": "678cf2a0b3945e0001ac4d30",
    "username": "john.doe@domnertech.com",
    "email": "john.doe@domnertech.com",
    "firstName": "John",
    "lastName": "Doe",
    "phoneNumber": "+1-555-0123",
    "isActive": true,
    "roles": ["User.Read", "User.Write", "Employee.Read"]
  },
  "status": {
    "statusCode": 200
  }
}
```

**Response Schema:**
| Field | Type | Description |
|-------|------|-------------|
| `id` | string | User's unique identifier |
| `companyId` | string | Company/tenant identifier |
| `username` | string | Username (typically email) |
| `email` | string | Email address |
| `firstName` | string | First name |
| `lastName` | string | Last name |
| `phoneNumber` | string | Phone number |
| `isActive` | boolean | Account status |
| `roles` | string[] | Assigned role permissions |

#### Example cURL

```bash
curl -X GET https://api.domnertech.com/api/v1/user/get-me \
  -H "Authorization: Bearer eyJhbGc..." \
  -H "X-Company-Id: 678cf2a0b3945e0001ac4d30" \
  -H "X-Correlation-Id: 550e8400-e29b-41d4-a716-446655440000"
```

---

### 2. Create User

Creates a new user account.

**Endpoint:** `POST /api/v1/user`

**Authorization:** Required - Role: `User.Write`

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
  "username": "jane.smith@domnertech.com",
  "pwd": "SecureP@ssw0rd456"
}
```

**Request Schema:**
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `username` | string | Yes | Username (email format recommended) |
| `pwd` | string | Yes | Password (min 8 chars, must include uppercase, lowercase, number, special char) |

#### Response

**Success Response:** `200 OK`
```json
{
  "data": true,
  "status": {
    "statusCode": 200,
    "message": "User created successfully"
  }
}
```

**Error Responses:**

**Validation Error:** `400 Bad Request`
```json
{
  "data": null,
  "status": {
    "statusCode": 400,
    "message": "Validation failed",
    "errors": [
      {
        "field": "pwd",
        "message": "Password must be at least 8 characters and contain uppercase, lowercase, number, and special character"
      }
    ]
  }
}
```

**Duplicate Username:** `409 Conflict`
```json
{
  "data": null,
  "status": {
    "statusCode": 409,
    "message": "A user with this username already exists",
    "errorCode": "ERR_DUPLICATE_USERNAME"
  }
}
```

**Insufficient Permissions:** `403 Forbidden`
```json
{
  "data": null,
  "status": {
    "statusCode": 403,
    "message": "You do not have permission to access this resource. Required role: User.Write"
  }
}
```

#### Example cURL

```bash
curl -X POST https://api.domnertech.com/api/v1/user \
  -H "Authorization: Bearer eyJhbGc..." \
  -H "X-Company-Id: 678cf2a0b3945e0001ac4d30" \
  -H "X-Correlation-Id: 550e8400-e29b-41d4-a716-446655440000" \
  -H "Content-Type: application/json" \
  -d '{
    "username": "jane.smith@domnertech.com",
    "pwd": "SecureP@ssw0rd456"
  }'
```

---

### 3. Get All Users (Paginated)

Retrieves a paginated list of users with cursor-based navigation.

**Endpoint:** `GET /api/v1/user/all`

**Authorization:** Required - Role: `User.Read`

#### Request

**Headers:**
```http
Authorization: Bearer {your_jwt_token}
X-Company-Id: 678cf2a0b3945e0001ac4d30
X-Correlation-Id: {uuid}
```

**Query Parameters:**
| Parameter | Type | Required | Description | Example |
|-----------|------|----------|-------------|---------|
| `cursor` | string | No | Pagination cursor (null for first page) | `null` or `eyJpZCI6IjY3OGNmMmE0In0=` |
| `page_size` | integer | Yes | Number of items per page (1-100) | `20` |
| `direction` | enum | Yes | Navigation direction | `Forward` or `Backward` |
| `sort_by` | string | Yes | Field to sort by | `username`, `createdAt`, `id` |
| `include_total_count` | boolean | Yes | Include total count in response | `true` or `false` |

#### Response

**Success Response:** `200 OK`
```json
{
  "data": {
    "items": [
      {
        "id": "678cf2a4b3945e0001ac4d3a",
        "companyId": "678cf2a0b3945e0001ac4d30",
        "username": "john.doe@domnertech.com",
        "email": "john.doe@domnertech.com",
        "firstName": "John",
        "lastName": "Doe",
        "isActive": true
      },
      {
        "id": "678cf2a4b3945e0001ac4d3b",
        "companyId": "678cf2a0b3945e0001ac4d30",
        "username": "jane.smith@domnertech.com",
        "email": "jane.smith@domnertech.com",
        "firstName": "Jane",
        "lastName": "Smith",
        "isActive": true
      },
      {
        "id": "678cf2a4b3945e0001ac4d3c",
        "companyId": "678cf2a0b3945e0001ac4d30",
        "username": "michael.chen@domnertech.com",
        "email": "michael.chen@domnertech.com",
        "firstName": "Michael",
        "lastName": "Chen",
        "isActive": true
      }
    ],
    "nextCursor": "eyJpZCI6IjY3OGNmMmE0YjM5NDVlMDAwMWFjNGQzYyIsInVzZXJuYW1lIjoibWljaGFlbC5jaGVuQGRvbW5lcnRlY2guY29tIn0=",
    "previousCursor": null,
    "hasPrevious": false,
    "hasNext": true,
    "totalCount": 156
  },
  "status": {
    "statusCode": 200
  }
}
```

**Response Schema:**
| Field | Type | Description |
|-------|------|-------------|
| `items` | array | Array of user objects |
| `items[].id` | string | User's unique identifier |
| `items[].companyId` | string | Company/tenant identifier |
| `items[].username` | string | Username |
| `items[].email` | string | Email address |
| `items[].firstName` | string | First name |
| `items[].lastName` | string | Last name |
| `items[].isActive` | boolean | Account status |
| `nextCursor` | string | Cursor for next page (null if no next page) |
| `previousCursor` | string | Cursor for previous page (null if no previous page) |
| `hasNext` | boolean | Whether there is a next page |
| `hasPrevious` | boolean | Whether there is a previous page |
| `totalCount` | integer | Total number of users (if requested) |

#### Example Request URL

```
GET /api/v1/user/all?cursor=null&page_size=20&direction=Forward&sort_by=username&include_total_count=true
```

#### Example cURL

```bash
curl -X GET "https://api.domnertech.com/api/v1/user/all?cursor=null&page_size=20&direction=Forward&sort_by=username&include_total_count=true" \
  -H "Authorization: Bearer eyJhbGc..." \
  -H "X-Company-Id: 678cf2a0b3945e0001ac4d30" \
  -H "X-Correlation-Id: 550e8400-e29b-41d4-a716-446655440000"
```

#### Pagination Example

**First Page:**
```
GET /user/all?cursor=null&page_size=20&direction=Forward&sort_by=username&include_total_count=true
```

**Next Page (using nextCursor from previous response):**
```
GET /user/all?cursor=eyJpZCI6IjY3OGNmMmE0YjM5NDVlMDAwMWFjNGQzYyJ9&page_size=20&direction=Forward&sort_by=username&include_total_count=false
```

**Previous Page (using previousCursor):**
```
GET /user/all?cursor=eyJpZCI6IjY3OGNmMmE0YjM5NDVlMDAwMWFjNGQzYSJ9&page_size=20&direction=Backward&sort_by=username&include_total_count=false
```

#### Notes
- Set `cursor` to `null` (as a string) for the first page
- Use `nextCursor` from the response to get the next page
- Use `previousCursor` to navigate backward
- `totalCount` is only included when `include_total_count=true` to optimize performance
- Maximum `page_size` is 100

#### Example JavaScript (Axios)

```javascript
const axios = require('axios');

async function getAllUsers(cursor = null, pageSize = 20) {
  const params = {
    cursor: cursor,
    page_size: pageSize,
    direction: 'Forward',
    sort_by: 'username',
    include_total_count: cursor === null // Only on first page
  };

  const config = {
    headers: {
      'Authorization': `Bearer ${getAuthToken()}`,
      'X-Company-Id': '678cf2a0b3945e0001ac4d30',
      'X-Correlation-Id': generateUUID()
    },
    params: params
  };

  try {
    const response = await axios.get(
      'https://api.domnertech.com/api/v1/user/all',
      config
    );
    
    const data = response.data.data;
    console.log(`Retrieved ${data.items.length} users`);
    console.log(`Total users: ${data.totalCount}`);
    console.log(`Has next page: ${data.hasNext}`);
    
    return data;
  } catch (error) {
    console.error('Error fetching users:', error.response.data);
    throw error;
  }
}

// Usage
getAllUsers()
  .then(data => {
    // Display first page
    console.log('Users:', data.items);
    
    // Get next page if available
    if (data.hasNext) {
      return getAllUsers(data.nextCursor, 20);
    }
  });
```

---

## User Object Reference

### Complete User Object
```json
{
  "id": "678cf2a4b3945e0001ac4d3a",
  "companyId": "678cf2a0b3945e0001ac4d30",
  "username": "john.doe@domnertech.com",
  "email": "john.doe@domnertech.com",
  "firstName": "John",
  "lastName": "Doe",
  "phoneNumber": "+1-555-0123",
  "isActive": true,
  "roles": ["User.Read", "User.Write", "Employee.Read"],
  "createdAt": "2024-01-15T10:00:00Z",
  "updatedAt": "2025-01-20T14:30:00Z"
}
```

### Field Descriptions
| Field | Type | Description | Constraints |
|-------|------|-------------|-------------|
| `id` | string | MongoDB ObjectId | 24 hex characters |
| `companyId` | string | Tenant identifier | 24 hex characters |
| `username` | string | Unique username | Email format, max 255 chars |
| `email` | string | Email address | Valid email, max 255 chars |
| `firstName` | string | First name | Max 100 chars |
| `lastName` | string | Last name | Max 100 chars |
| `phoneNumber` | string | Phone number | E.164 format recommended |
| `isActive` | boolean | Account status | true/false |
| `roles` | string[] | Permission roles | Array of role strings |
| `createdAt` | datetime | Creation timestamp | ISO 8601 UTC |
| `updatedAt` | datetime | Last update timestamp | ISO 8601 UTC |

---

## Business Rules

### Username Rules
- Must be unique within the company/tenant
- Email format recommended but not required
- Case-insensitive for uniqueness check
- Cannot be changed after creation

### Password Requirements
- Minimum 8 characters
- Must contain:
  - At least one uppercase letter (A-Z)
  - At least one lowercase letter (a-z)
  - At least one number (0-9)
  - At least one special character (!@#$%^&*()_+-=[]{}|;:,.<>?)

### Account Status
- New users are active by default
- Inactive users cannot login
- Only users with `User.Write` role can change status

---

## Related Endpoints
- [Authentication](./01-authentication.md) - Login with user credentials
- [Role Management](./04-role-management.md) - Assign roles to users
- [Employee Management](./03-employee-management.md) - Link users to employee records

---

[? Back to Documentation Home](./README.md)
