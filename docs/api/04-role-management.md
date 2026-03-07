# Role Management API

## Overview
Manages roles and permissions, including role creation, user role assignments, and role retrieval.

**Base Path:** `/api/v1/role`

---

## Endpoints

### 1. Create Role

Creates a new role with a specific name and description.

**Endpoint:** `POST /api/v1/role`

**Authorization:** Required - Role: `Role.Write`

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
  "name": "Employee.Manager",
  "desc": "Can manage employee records and approve leave requests"
}
```

**Request Schema:**
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `name` | string | Yes | Role name (format: Resource.Permission, max 100 chars) |
| `desc` | string | Yes | Role description (max 500 chars) |

#### Response

**Success Response:** `200 OK`
```json
{
  "data": true,
  "status": {
    "statusCode": 200,
    "message": "Role created successfully"
  }
}
```

**Error Responses:**

**Duplicate Role:** `409 Conflict`
```json
{
  "data": null,
  "status": {
    "statusCode": 409,
    "message": "A role with this name already exists",
    "errorCode": "ERR_DUPLICATE_ROLE"
  }
}
```

**Invalid Role Format:** `400 Bad Request`
```json
{
  "data": null,
  "status": {
    "statusCode": 400,
    "message": "Validation failed",
    "errors": [
      {
        "field": "name",
        "message": "Role name must follow the format: Resource.Permission (e.g., Employee.Read)"
      }
    ]
  }
}
```

#### Example cURL

```bash
curl -X POST https://api.domnertech.com/api/v1/role \
  -H "Authorization: Bearer eyJhbGc..." \
  -H "X-Company-Id: 678cf2a0b3945e0001ac4d30" \
  -H "X-Correlation-Id: 550e8400-e29b-41d4-a716-446655440000" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Employee.Manager",
    "desc": "Can manage employee records and approve leave requests"
  }'
```

#### Notes
- Role names follow the convention: `Resource.Permission`
- Common permissions: `Read`, `Write`, `Admin`, `Manager`
- Role names are case-sensitive
- Role names must be unique within the system

---

### 2. Get All Roles

Retrieves all available roles in the system.

**Endpoint:** `GET /api/v1/role`

**Authorization:** Required - Role: `Role.Read`

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
      "id": "678cf2a4b3945e0001ac4d50",
      "name": "User.Read",
      "description": "Can view user information"
    },
    {
      "id": "678cf2a4b3945e0001ac4d51",
      "name": "User.Write",
      "description": "Can create and modify users"
    },
    {
      "id": "678cf2a4b3945e0001ac4d52",
      "name": "Employee.Read",
      "description": "Can view employee information"
    },
    {
      "id": "678cf2a4b3945e0001ac4d53",
      "name": "Employee.Write",
      "description": "Can create and modify employees"
    },
    {
      "id": "678cf2a4b3945e0001ac4d54",
      "name": "Role.Read",
      "description": "Can view roles and permissions"
    },
    {
      "id": "678cf2a4b3945e0001ac4d55",
      "name": "Role.Write",
      "description": "Can create roles and assign permissions"
    },
    {
      "id": "678cf2a4b3945e0001ac4d56",
      "name": "LeaveType.Read",
      "description": "Can view leave types"
    },
    {
      "id": "678cf2a4b3945e0001ac4d57",
      "name": "LeaveType.Write",
      "description": "Can create and modify leave types"
    },
    {
      "id": "678cf2a4b3945e0001ac4d58",
      "name": "LeaveRequest.Read",
      "description": "Can view leave requests"
    },
    {
      "id": "678cf2a4b3945e0001ac4d59",
      "name": "LeaveRequest.Write",
      "description": "Can create and modify own leave requests"
    },
    {
      "id": "678cf2a4b3945e0001ac4d5a",
      "name": "LeaveRequest.Admin",
      "description": "Can view and manage all leave requests"
    },
    {
      "id": "678cf2a4b3945e0001ac4d5b",
      "name": "LeaveApproval.Write",
      "description": "Can approve or reject leave requests"
    },
    {
      "id": "678cf2a4b3945e0001ac4d5c",
      "name": "LeaveBalance.Read",
      "description": "Can view leave balances"
    },
    {
      "id": "678cf2a4b3945e0001ac4d5d",
      "name": "LeaveBalance.Write",
      "description": "Can adjust leave balances"
    },
    {
      "id": "678cf2a4b3945e0001ac4d5e",
      "name": "LeavePolicy.Read",
      "description": "Can view leave policies"
    },
    {
      "id": "678cf2a4b3945e0001ac4d5f",
      "name": "LeavePolicy.Write",
      "description": "Can create and modify leave policies"
    },
    {
      "id": "678cf2a4b3945e0001ac4d60",
      "name": "Holiday.Read",
      "description": "Can view holidays"
    },
    {
      "id": "678cf2a4b3945e0001ac4d61",
      "name": "Holiday.Write",
      "description": "Can create and modify holidays"
    },
    {
      "id": "678cf2a4b3945e0001ac4d62",
      "name": "Localize.Read",
      "description": "Can view localized messages"
    },
    {
      "id": "678cf2a4b3945e0001ac4d63",
      "name": "Localize.Write",
      "description": "Can create and modify localized messages"
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
| `id` | string | Role's unique identifier |
| `name` | string | Role name (Resource.Permission format) |
| `description` | string | Role description |

#### Example cURL

```bash
curl -X GET https://api.domnertech.com/api/v1/role \
  -H "Authorization: Bearer eyJhbGc..." \
  -H "X-Company-Id: 678cf2a0b3945e0001ac4d30" \
  -H "X-Correlation-Id: 550e8400-e29b-41d4-a716-446655440000"
```

#### Notes
- Returns all system-defined roles
- Roles are not tenant-specific (shared across all companies)
- Results are not paginated as the role list is typically small

---

### 3. Get User Roles

Retrieves all roles assigned to a specific user.

**Endpoint:** `GET /api/v1/role/user-roles/{userId}`

**Authorization:** Required - Role: `Role.Read`

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
| `userId` | string | Yes | User's unique identifier |

#### Response

**Success Response:** `200 OK`
```json
{
  "data": [
    {
      "id": "678cf2a4b3945e0001ac4d52",
      "name": "Employee.Read",
      "description": "Can view employee information"
    },
    {
      "id": "678cf2a4b3945e0001ac4d58",
      "name": "LeaveRequest.Read",
      "description": "Can view leave requests"
    },
    {
      "id": "678cf2a4b3945e0001ac4d59",
      "name": "LeaveRequest.Write",
      "description": "Can create and modify own leave requests"
    },
    {
      "id": "678cf2a4b3945e0001ac4d5c",
      "name": "LeaveBalance.Read",
      "description": "Can view leave balances"
    }
  ],
  "status": {
    "statusCode": 200
  }
}
```

**Error Responses:**

**User Not Found:** `404 Not Found`
```json
{
  "data": null,
  "status": {
    "statusCode": 404,
    "message": "User with ID '678cf2a4b3945e0001ac4d99' not found",
    "errorCode": "ERR_USER_NOT_FOUND"
  }
}
```

#### Example cURL

```bash
curl -X GET https://api.domnertech.com/api/v1/role/user-roles/678cf2a4b3945e0001ac4d3a \
  -H "Authorization: Bearer eyJhbGc..." \
  -H "X-Company-Id: 678cf2a0b3945e0001ac4d30" \
  -H "X-Correlation-Id: 550e8400-e29b-41d4-a716-446655440000"
```

#### Example JavaScript (Axios)

```javascript
async function getUserRoles(userId) {
  const config = {
    headers: {
      'Authorization': `Bearer ${getAuthToken()}`,
      'X-Company-Id': '678cf2a0b3945e0001ac4d30',
      'X-Correlation-Id': generateUUID()
    }
  };

  try {
    const response = await axios.get(
      `https://api.domnertech.com/api/v1/role/user-roles/${userId}`,
      config
    );
    
    const roles = response.data.data;
    console.log(`User has ${roles.length} roles assigned`);
    return roles;
  } catch (error) {
    console.error('Error fetching user roles:', error.response.data);
    throw error;
  }
}
```

---

### 4. Upsert User Role

Assigns a role to a user. If the user already has the role, it updates the assignment.

**Endpoint:** `POST /api/v1/role/upsert-user-role`

**Authorization:** Required - Role: `Role.Write`

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
  "userId": "678cf2a4b3945e0001ac4d3a",
  "roleName": "LeaveRequest.Admin"
}
```

**Request Schema:**
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `userId` | string | Yes | User's unique identifier |
| `roleName` | string | Yes | Role name to assign (must exist) |

#### Response

**Success Response:** `200 OK`
```json
{
  "data": true,
  "status": {
    "statusCode": 200,
    "message": "User role updated successfully"
  }
}
```

**Error Responses:**

**User Not Found:** `404 Not Found`
```json
{
  "data": null,
  "status": {
    "statusCode": 404,
    "message": "User with ID '678cf2a4b3945e0001ac4d99' not found",
    "errorCode": "ERR_USER_NOT_FOUND"
  }
}
```

**Role Not Found:** `404 Not Found`
```json
{
  "data": null,
  "status": {
    "statusCode": 404,
    "message": "Role 'InvalidRole.Name' does not exist",
    "errorCode": "ERR_ROLE_NOT_FOUND"
  }
}
```

#### Example cURL

```bash
curl -X POST https://api.domnertech.com/api/v1/role/upsert-user-role \
  -H "Authorization: Bearer eyJhbGc..." \
  -H "X-Company-Id: 678cf2a0b3945e0001ac4d30" \
  -H "X-Correlation-Id: 550e8400-e29b-41d4-a716-446655440000" \
  -H "Content-Type: application/json" \
  -d '{
    "userId": "678cf2a4b3945e0001ac4d3a",
    "roleName": "LeaveRequest.Admin"
  }'
```

#### Example JavaScript (Axios)

```javascript
async function assignRoleToUser(userId, roleName) {
  const config = {
    headers: {
      'Authorization': `Bearer ${getAuthToken()}`,
      'X-Company-Id': '678cf2a0b3945e0001ac4d30',
      'X-Correlation-Id': generateUUID(),
      'Content-Type': 'application/json'
    }
  };

  const body = {
    userId: userId,
    roleName: roleName
  };

  try {
    const response = await axios.post(
      'https://api.domnertech.com/api/v1/role/upsert-user-role',
      body,
      config
    );
    
    console.log(`Role ${roleName} assigned to user ${userId}`);
    return response.data;
  } catch (error) {
    console.error('Error assigning role:', error.response.data);
    throw error;
  }
}

// Usage: Assign multiple roles to a user
async function setupManagerPermissions(userId) {
  const managerRoles = [
    'Employee.Read',
    'Employee.Write',
    'LeaveRequest.Admin',
    'LeaveApproval.Write',
    'LeaveBalance.Read',
    'LeaveBalance.Write'
  ];

  for (const role of managerRoles) {
    await assignRoleToUser(userId, role);
  }
  
  console.log('Manager permissions configured');
}
```

#### Notes
- This is an "upsert" operation - creates or updates the role assignment
- If the user already has the role, the operation succeeds with no changes
- Role assignments are immediate and effective on the next API request
- Users can have multiple roles assigned simultaneously

---

## Role Hierarchy & Permissions

### Permission Levels

Roles follow a hierarchical permission structure:

#### Read Permissions
- View-only access to resources
- Cannot create, modify, or delete
- Examples: `User.Read`, `Employee.Read`, `LeaveRequest.Read`

#### Write Permissions
- Full CRUD operations on resources
- Typically includes Read permissions
- Examples: `User.Write`, `Employee.Write`, `LeaveType.Write`

#### Admin Permissions
- Advanced administrative functions
- Typically includes Read and Write permissions
- Can manage resources across all users/departments
- Examples: `LeaveRequest.Admin`

### Standard Role Combinations

#### Employee (Standard User)
```json
[
  "Employee.Read",
  "LeaveRequest.Read",
  "LeaveRequest.Write",
  "LeaveBalance.Read",
  "LeaveType.Read",
  "Holiday.Read"
]
```

#### Manager
```json
[
  "Employee.Read",
  "Employee.Write",
  "LeaveRequest.Read",
  "LeaveRequest.Write",
  "LeaveRequest.Admin",
  "LeaveApproval.Write",
  "LeaveBalance.Read",
  "LeaveType.Read",
  "Holiday.Read"
]
```

#### HR Administrator
```json
[
  "User.Read",
  "User.Write",
  "Employee.Read",
  "Employee.Write",
  "Role.Read",
  "Role.Write",
  "LeaveType.Read",
  "LeaveType.Write",
  "LeaveRequest.Admin",
  "LeaveApproval.Write",
  "LeaveBalance.Read",
  "LeaveBalance.Write",
  "LeavePolicy.Read",
  "LeavePolicy.Write",
  "Holiday.Read",
  "Holiday.Write"
]
```

#### System Administrator
```json
[
  "User.Read",
  "User.Write",
  "Employee.Read",
  "Employee.Write",
  "Role.Read",
  "Role.Write",
  "LeaveType.Read",
  "LeaveType.Write",
  "LeaveRequest.Admin",
  "LeaveApproval.Write",
  "LeaveBalance.Read",
  "LeaveBalance.Write",
  "LeavePolicy.Read",
  "LeavePolicy.Write",
  "Holiday.Read",
  "Holiday.Write",
  "Localize.Read",
  "Localize.Write"
]
```

---

## Complete Role Reference

### User Management Roles
| Role | Description | Common Users |
|------|-------------|--------------|
| `User.Read` | View user accounts | HR, Managers |
| `User.Write` | Create and modify user accounts | HR, System Admin |

### Employee Management Roles
| Role | Description | Common Users |
|------|-------------|--------------|
| `Employee.Read` | View employee records | All users, HR, Managers |
| `Employee.Write` | Create and modify employee records | HR, System Admin |

### Role Management Roles
| Role | Description | Common Users |
|------|-------------|--------------|
| `Role.Read` | View roles and permissions | HR, System Admin |
| `Role.Write` | Create roles and assign permissions | System Admin |

### Leave Type Management Roles
| Role | Description | Common Users |
|------|-------------|--------------|
| `LeaveType.Read` | View leave types | All users |
| `LeaveType.Write` | Create and modify leave types | HR |

### Leave Request Roles
| Role | Description | Common Users |
|------|-------------|--------------|
| `LeaveRequest.Read` | View own leave requests | All employees |
| `LeaveRequest.Write` | Create and modify own leave requests | All employees |
| `LeaveRequest.Admin` | View and manage all leave requests | HR, Managers |

### Leave Approval Roles
| Role | Description | Common Users |
|------|-------------|--------------|
| `LeaveApproval.Write` | Approve or reject leave requests | Managers, HR |

### Leave Balance Roles
| Role | Description | Common Users |
|------|-------------|--------------|
| `LeaveBalance.Read` | View leave balances | All employees |
| `LeaveBalance.Write` | Adjust leave balances | HR |

### Leave Policy Roles
| Role | Description | Common Users |
|------|-------------|--------------|
| `LeavePolicy.Read` | View leave policies | All employees |
| `LeavePolicy.Write` | Create and modify leave policies | HR |

### Holiday Roles
| Role | Description | Common Users |
|------|-------------|--------------|
| `Holiday.Read` | View holidays | All employees |
| `Holiday.Write` | Create and modify holidays | HR |

### Localization Roles
| Role | Description | Common Users |
|------|-------------|--------------|
| `Localize.Read` | View localized messages | All users |
| `Localize.Write` | Create and modify localized messages | System Admin |

---

## Business Rules

### Role Assignment
1. A user can have multiple roles assigned
2. Role assignments are cumulative (all permissions are combined)
3. There is no role hierarchy - each role is independent
4. Removing a role immediately revokes its permissions

### Role Naming Convention
- Format: `Resource.Permission`
- Resource: The entity or feature (e.g., User, Employee, LeaveRequest)
- Permission: The access level (e.g., Read, Write, Admin)
- Case-sensitive

### Permission Checking
- API endpoints check for specific role names
- Users must have ALL required roles for an endpoint
- Example: An endpoint requiring `[Employee.Write, LeaveRequest.Admin]` needs both roles

---

## Common Use Cases

### Use Case 1: Onboard Employee with Standard Permissions
```javascript
const employeeRoles = [
  'Employee.Read',
  'LeaveRequest.Read',
  'LeaveRequest.Write',
  'LeaveBalance.Read',
  'LeaveType.Read',
  'Holiday.Read'
];

for (const role of employeeRoles) {
  await assignRoleToUser(newUserId, role);
}
```

### Use Case 2: Promote to Manager
```javascript
const managerRoles = [
  'LeaveRequest.Admin',
  'LeaveApproval.Write',
  'Employee.Write',
  'LeaveBalance.Write'
];

for (const role of managerRoles) {
  await assignRoleToUser(userId, role);
}
```

### Use Case 3: Check User Permissions
```javascript
async function userHasPermission(userId, requiredRole) {
  const userRoles = await getUserRoles(userId);
  return userRoles.some(role => role.name === requiredRole);
}

// Usage
if (await userHasPermission(userId, 'LeaveApproval.Write')) {
  // User can approve leave requests
  showApproveButton();
}
```

---

## Related Endpoints
- [User Management](./02-user-management.md) - Create users to assign roles to
- [Authentication](./01-authentication.md) - Role-based access in JWT tokens

---

[? Back to Documentation Home](./README.md)
