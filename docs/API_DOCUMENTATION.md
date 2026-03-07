# DomnerTech Backend API Documentation

## Base URL
```
https://api.domnertech.com/api
```

## Headers Required
All authenticated requests require the following headers:
- `Authorization: Bearer {token}` (or cookie-based authentication)
- `X-Company-Id: {companyId}` - Your company/tenant identifier
- `X-Correlation-Id: {correlationId}` - Unique request tracking ID
- `Content-Type: application/json`

---

## 1. Authentication

### 1.1 Login
**Endpoint:** `POST /auth/login`

**Authorization:** Public (No authentication required)

**Description:** Authenticates a user and returns a JWT token.

**Request Body:**
```json
{
  "username": "john.doe@domnertech.com",
  "pwd": "SecureP@ssw0rd123"
}
```

**Response:** `200 OK`
```json
{
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI2NzhjZjJhNGIzOTQ1ZTAwMDFhYzRkM2EiLCJlbWFpbCI6ImpvaG4uZG9lQGRvbW5lcnRlY2guY29tIiwibmFtZSI6IkpvaG4gRG9lIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjpbIlVzZXIuUmVhZCIsIlVzZXIuV3JpdGUiLCJFbXBsb3llZS5SZWFkIl0sImV4cCI6MTczNzM4NTIwMCwiaXNzIjoiRG9tbmVyVGVjaCIsImF1ZCI6IkRvbW5lclRlY2hBUEkifQ.xYz123abc456def789ghi",
    "user": {
      "id": "678cf2a4b3945e0001ac4d3a",
      "companyId": "678cf2a0b3945e0001ac4d30",
      "username": "john.doe@domnertech.com",
      "email": "john.doe@domnertech.com",
      "firstName": "John",
      "lastName": "Doe",
      "isActive": true
    }
  },
  "status": {
    "statusCode": 200,
    "message": "Login successful"
  }
}
```

**Notes:**
- Token is also set as an HTTP-only cookie named `authToken`
- Token expires in 24 hours

---

### 1.2 Logout
**Endpoint:** `POST /auth/logout`

**Authorization:** Required (any authenticated user)

**Request Body:** None

**Response:** `200 OK`
```json
{
  "data": true,
  "status": {
    "statusCode": 200,
    "message": "Logged out successfully"
  }
}
```

**Notes:**
- Removes the authentication cookie
- Client should also remove stored tokens

---

## 2. User Management

### 2.1 Get Current User
**Endpoint:** `GET /user/get-me`

**Authorization:** Required (any authenticated user)

**Response:** `200 OK`
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

---

### 2.2 Create User
**Endpoint:** `POST /user`

**Authorization:** Required - Role: `User.Write`

**Request Body:**
```json
{
  "username": "jane.smith@domnertech.com",
  "pwd": "SecureP@ssw0rd456"
}
```

**Response:** `200 OK`
```json
{
  "data": true,
  "status": {
    "statusCode": 200,
    "message": "User created successfully"
  }
}
```

---

### 2.3 Get All Users (Paginated)
**Endpoint:** `GET /user/all`

**Authorization:** Required - Role: `User.Read`

**Query Parameters:**
- `cursor` (string, optional): Pagination cursor (use null for first page)
- `page_size` (integer, required): Number of items per page (e.g., 20)
- `direction` (enum, required): `Forward` or `Backward`
- `sort_by` (string, required): Field to sort by (e.g., `username`, `createdAt`, `id`)
- `include_total_count` (boolean, required): Whether to include total count

**Example Request:**
```
GET /user/all?cursor=null&page_size=20&direction=Forward&sort_by=username&include_total_count=true
```

**Response:** `200 OK`
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
      }
    ],
    "nextCursor": "eyJpZCI6IjY3OGNmMmE0YjM5NDVlMDAwMWFjNGQzYiJ9",
    "hasPrevious": false,
    "hasNext": true,
    "totalCount": 156
  },
  "status": {
    "statusCode": 200
  }
}
```

---

## 3. Employee Management

### 3.1 Create Employee
**Endpoint:** `POST /employee`

**Authorization:** Required - Role: `Employee.Write`

**Request Body:**
```json
{
  "firstName": "Sarah",
  "lastName": "Johnson",
  "email": "sarah.johnson@domnertech.com",
  "phoneNumber": "+1-555-0156",
  "dateOfBirth": "1990-05-15T00:00:00Z",
  "hireDate": "2024-01-15T00:00:00Z",
  "department": "Engineering",
  "jobTitle": "Senior Software Engineer",
  "address": {
    "street": "123 Tech Street",
    "city": "San Francisco",
    "state": "CA",
    "postalCode": "94105",
    "country": "USA"
  }
}
```

**Response:** `200 OK`
```json
{
  "data": true,
  "status": {
    "statusCode": 200,
    "message": "Employee created successfully"
  }
}
```

---

### 3.2 Update Employee
**Endpoint:** `PUT /employee`

**Authorization:** Required - Role: `Employee.Write`

**Request Body:**
```json
{
  "id": "678cf2a4b3945e0001ac4d40",
  "firstName": "Sarah",
  "lastName": "Johnson",
  "email": "sarah.johnson@domnertech.com",
  "phoneNumber": "+1-555-0156",
  "dateOfBirth": "1990-05-15T00:00:00Z",
  "department": "Engineering",
  "jobTitle": "Lead Software Engineer",
  "isActive": true,
  "address": {
    "street": "123 Tech Street",
    "city": "San Francisco",
    "state": "CA",
    "postalCode": "94105",
    "country": "USA"
  }
}
```

**Response:** `200 OK`
```json
{
  "data": true,
  "status": {
    "statusCode": 200,
    "message": "Employee updated successfully"
  }
}
```

---

### 3.3 Get Employees (Paginated)
**Endpoint:** `GET /employee`

**Authorization:** Required - Role: `Employee.Read`

**Query Parameters:**
- `cursor` (string, optional): Pagination cursor
- `page_size` (integer, required): Number of items per page
- `direction` (enum, required): `Forward` or `Backward`
- `sort_by` (string, required): Field to sort by (e.g., `firstName`, `hireDate`, `department`)
- `include_total_count` (boolean, required): Whether to include total count

**Example Request:**
```
GET /employee?cursor=null&page_size=20&direction=Forward&sort_by=firstName&include_total_count=true
```

**Response:** `200 OK`
```json
{
  "data": {
    "items": [
      {
        "id": "678cf2a4b3945e0001ac4d40",
        "companyId": "678cf2a0b3945e0001ac4d30",
        "firstName": "Sarah",
        "lastName": "Johnson",
        "email": "sarah.johnson@domnertech.com",
        "phoneNumber": "+1-555-0156",
        "dateOfBirth": "1990-05-15T00:00:00Z",
        "hireDate": "2024-01-15T00:00:00Z",
        "isActive": true,
        "department": "Engineering",
        "jobTitle": "Lead Software Engineer",
        "employeeNumber": "EMP-2024-001",
        "address": {
          "street": "123 Tech Street",
          "city": "San Francisco",
          "state": "CA",
          "postalCode": "94105",
          "country": "USA"
        }
      }
    ],
    "nextCursor": "eyJpZCI6IjY3OGNmMmE0YjM5NDVlMDAwMWFjNGQ0MCJ9",
    "hasPrevious": false,
    "hasNext": true,
    "totalCount": 342
  },
  "status": {
    "statusCode": 200
  }
}
```

---

## 4. Role Management

### 4.1 Create Role
**Endpoint:** `POST /role`

**Authorization:** Required - Role: `Role.Write`

**Request Body:**
```json
{
  "name": "Employee.Manager",
  "desc": "Can manage employee records and approve leave requests"
}
```

**Response:** `200 OK`
```json
{
  "data": true,
  "status": {
    "statusCode": 200,
    "message": "Role created successfully"
  }
}
```

---

### 4.2 Get All Roles
**Endpoint:** `GET /role`

**Authorization:** Required - Role: `Role.Read`

**Response:** `200 OK`
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
    }
  ],
  "status": {
    "statusCode": 200
  }
}
```

---

### 4.3 Get User Roles
**Endpoint:** `GET /role/user-roles/{userId}`

**Authorization:** Required - Role: `Role.Read`

**Response:** `200 OK`
```json
{
  "data": [
    {
      "id": "678cf2a4b3945e0001ac4d50",
      "name": "User.Read",
      "description": "Can view user information"
    },
    {
      "id": "678cf2a4b3945e0001ac4d52",
      "name": "Employee.Read",
      "description": "Can view employee information"
    }
  ],
  "status": {
    "statusCode": 200
  }
}
```

---

### 4.4 Upsert User Role
**Endpoint:** `POST /role/upsert-user-role`

**Authorization:** Required - Role: `Role.Write`

**Request Body:**
```json
{
  "userId": "678cf2a4b3945e0001ac4d3a",
  "roleName": "LeaveRequest.Admin"
}
```

**Response:** `200 OK`
```json
{
  "data": true,
  "status": {
    "statusCode": 200,
    "message": "User role updated successfully"
  }
}
```

---

## 5. Leave Type Management

### 5.1 Create Leave Type
**Endpoint:** `POST /leave-type`

**Authorization:** Required - Role: `LeaveType.Write`

**Request Body:**
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

**Response:** `200 OK`
```json
{
  "data": "678cf2a4b3945e0001ac4d60",
  "status": {
    "statusCode": 200,
    "message": "Leave type created successfully"
  }
}
```

---

### 5.2 Update Leave Type
**Endpoint:** `PUT /leave-type`

**Authorization:** Required - Role: `LeaveType.Write`

**Request Body:**
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

**Response:** `200 OK`
```json
{
  "data": true,
  "status": {
    "statusCode": 200,
    "message": "Leave type updated successfully"
  }
}
```

---

### 5.3 Delete Leave Type
**Endpoint:** `DELETE /leave-type/{id}`

**Authorization:** Required - Role: `LeaveType.Write`

**Response:** `200 OK`
```json
{
  "data": true,
  "status": {
    "statusCode": 200,
    "message": "Leave type deleted successfully"
  }
}
```

---

### 5.4 Get All Leave Types
**Endpoint:** `GET /leave-type`

**Authorization:** Required - Role: `LeaveType.Read`

**Response:** `200 OK`
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
    }
  ],
  "status": {
    "statusCode": 200
  }
}
```

---

### 5.5 Get Leave Type by ID
**Endpoint:** `GET /leave-type/{id}`

**Authorization:** Required - Role: `LeaveType.Read`

**Response:** `200 OK`
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
    "isActive": true
  },
  "status": {
    "statusCode": 200
  }
}
```

---

## 6. Leave Request Management

### 6.1 Create Leave Request
**Endpoint:** `POST /leave-request`

**Authorization:** Required - Role: `LeaveRequest.Write`

**Request Body:**
```json
{
  "employeeId": "678cf2a4b3945e0001ac4d40",
  "leaveTypeId": "678cf2a4b3945e0001ac4d60",
  "startDate": "2025-03-15T00:00:00Z",
  "endDate": "2025-03-20T00:00:00Z",
  "reason": "Family vacation to Hawaii",
  "attachmentUrls": [
    "https://storage.domnertech.com/attachments/flight-booking-12345.pdf"
  ]
}
```

**Response:** `200 OK`
```json
{
  "data": "678cf2a4b3945e0001ac4d70",
  "status": {
    "statusCode": 200,
    "message": "Leave request submitted successfully"
  }
}
```

---

### 6.2 Update Leave Request
**Endpoint:** `PUT /leave-request`

**Authorization:** Required - Role: `LeaveRequest.Write`

**Request Body:**
```json
{
  "id": "678cf2a4b3945e0001ac4d70",
  "leaveTypeId": "678cf2a4b3945e0001ac4d60",
  "startDate": "2025-03-16T00:00:00Z",
  "endDate": "2025-03-22T00:00:00Z",
  "reason": "Extended family vacation to Hawaii",
  "attachmentUrls": [
    "https://storage.domnertech.com/attachments/flight-booking-12345.pdf",
    "https://storage.domnertech.com/attachments/hotel-booking-67890.pdf"
  ]
}
```

**Response:** `200 OK`
```json
{
  "data": true,
  "status": {
    "statusCode": 200,
    "message": "Leave request updated successfully"
  }
}
```

---

### 6.3 Cancel Leave Request
**Endpoint:** `POST /leave-request/cancel`

**Authorization:** Required - Role: `LeaveRequest.Write`

**Request Body:**
```json
{
  "id": "678cf2a4b3945e0001ac4d70",
  "cancellationReason": "Plans changed, vacation postponed"
}
```

**Response:** `200 OK`
```json
{
  "data": true,
  "status": {
    "statusCode": 200,
    "message": "Leave request cancelled successfully"
  }
}
```

---

### 6.4 Get Leave Request by ID
**Endpoint:** `GET /leave-request/{id}`

**Authorization:** Required - Role: `LeaveRequest.Read`

**Response:** `200 OK`
```json
{
  "data": {
    "id": "678cf2a4b3945e0001ac4d70",
    "employeeId": "678cf2a4b3945e0001ac4d40",
    "employeeName": "Sarah Johnson",
    "leaveTypeId": "678cf2a4b3945e0001ac4d60",
    "leaveTypeName": "Annual Leave",
    "startDate": "2025-03-16T00:00:00Z",
    "endDate": "2025-03-22T00:00:00Z",
    "totalDays": 5,
    "status": "Pending",
    "reason": "Extended family vacation to Hawaii",
    "attachmentUrls": [
      "https://storage.domnertech.com/attachments/flight-booking-12345.pdf",
      "https://storage.domnertech.com/attachments/hotel-booking-67890.pdf"
    ],
    "submittedAt": "2025-01-20T10:30:00Z"
  },
  "status": {
    "statusCode": 200
  }
}
```

---

### 6.5 Get My Leave Requests
**Endpoint:** `GET /leave-request/my`

**Authorization:** Required - Role: `LeaveRequest.Read`

**Response:** `200 OK`
```json
{
  "data": [
    {
      "id": "678cf2a4b3945e0001ac4d70",
      "leaveTypeId": "678cf2a4b3945e0001ac4d60",
      "leaveTypeName": "Annual Leave",
      "startDate": "2025-03-16T00:00:00Z",
      "endDate": "2025-03-22T00:00:00Z",
      "totalDays": 5,
      "status": "Pending",
      "reason": "Extended family vacation to Hawaii",
      "submittedAt": "2025-01-20T10:30:00Z"
    },
    {
      "id": "678cf2a4b3945e0001ac4d71",
      "leaveTypeId": "678cf2a4b3945e0001ac4d61",
      "leaveTypeName": "Sick Leave",
      "startDate": "2024-12-10T00:00:00Z",
      "endDate": "2024-12-11T00:00:00Z",
      "totalDays": 2,
      "status": "Approved",
      "reason": "Flu and fever",
      "submittedAt": "2024-12-09T08:00:00Z",
      "approvedAt": "2024-12-09T09:15:00Z",
      "approvedBy": "Manager Name"
    }
  ],
  "status": {
    "statusCode": 200
  }
}
```

---

### 6.6 Get Leave Requests by Status
**Endpoint:** `GET /leave-request/status/{status}`

**Authorization:** Required - Role: `LeaveRequest.Admin`

**Valid Status Values:** `Pending`, `Approved`, `Rejected`, `Cancelled`

**Response:** `200 OK`
```json
{
  "data": [
    {
      "id": "678cf2a4b3945e0001ac4d70",
      "employeeId": "678cf2a4b3945e0001ac4d40",
      "employeeName": "Sarah Johnson",
      "department": "Engineering",
      "leaveTypeId": "678cf2a4b3945e0001ac4d60",
      "leaveTypeName": "Annual Leave",
      "startDate": "2025-03-16T00:00:00Z",
      "endDate": "2025-03-22T00:00:00Z",
      "totalDays": 5,
      "status": "Pending",
      "reason": "Extended family vacation to Hawaii",
      "submittedAt": "2025-01-20T10:30:00Z"
    }
  ],
  "status": {
    "statusCode": 200
  }
}
```

---

## 7. Leave Balance Management

### 7.1 Initialize Leave Balance
**Endpoint:** `POST /leave-balance`

**Authorization:** Required - Role: `LeaveBalance.Write`

**Request Body:**
```json
{
  "employeeId": "678cf2a4b3945e0001ac4d40",
  "leaveTypeId": "678cf2a4b3945e0001ac4d60",
  "year": 2025,
  "totalAllowance": 20.0,
  "carryForward": 5.0
}
```

**Response:** `200 OK`
```json
{
  "data": "678cf2a4b3945e0001ac4d80",
  "status": {
    "statusCode": 200,
    "message": "Leave balance initialized successfully"
  }
}
```

---

### 7.2 Adjust Leave Balance
**Endpoint:** `POST /leave-balance/adjust`

**Authorization:** Required - Role: `LeaveBalance.Write`

**Request Body:**
```json
{
  "employeeId": "678cf2a4b3945e0001ac4d40",
  "leaveTypeId": "678cf2a4b3945e0001ac4d60",
  "year": 2025,
  "adjustmentDays": 2.0,
  "reason": "Additional leave granted for exceptional performance"
}
```

**Response:** `200 OK`
```json
{
  "data": true,
  "status": {
    "statusCode": 200,
    "message": "Leave balance adjusted successfully"
  }
}
```

---

### 7.3 Get My Leave Balances
**Endpoint:** `GET /leave-balance/my?year=2025`

**Authorization:** Required - Role: `LeaveBalance.Read`

**Query Parameters:**
- `year` (integer, required): The year to retrieve balances for

**Response:** `200 OK`
```json
{
  "data": [
    {
      "leaveTypeId": "678cf2a4b3945e0001ac4d60",
      "leaveTypeName": "Annual Leave",
      "year": 2025,
      "totalAllowance": 22.0,
      "used": 3.0,
      "remaining": 19.0,
      "carryForward": 5.0,
      "pending": 5.0
    },
    {
      "leaveTypeId": "678cf2a4b3945e0001ac4d61",
      "leaveTypeName": "Sick Leave",
      "year": 2025,
      "totalAllowance": 10.0,
      "used": 2.0,
      "remaining": 8.0,
      "carryForward": 0.0,
      "pending": 0.0
    }
  ],
  "status": {
    "statusCode": 200
  }
}
```

---

### 7.4 Get Employee Leave Balances
**Endpoint:** `GET /leave-balance/employee/{employeeId}?year=2025`

**Authorization:** Required - Role: `LeaveBalance.Write`

**Query Parameters:**
- `year` (integer, required): The year to retrieve balances for

**Response:** `200 OK`
```json
{
  "data": [
    {
      "leaveTypeId": "678cf2a4b3945e0001ac4d60",
      "leaveTypeName": "Annual Leave",
      "year": 2025,
      "totalAllowance": 22.0,
      "used": 3.0,
      "remaining": 19.0,
      "carryForward": 5.0,
      "pending": 5.0
    }
  ],
  "status": {
    "statusCode": 200
  }
}
```

---

## 8. Leave Policy Management

### 8.1 Create Leave Policy
**Endpoint:** `POST /leave-policy`

**Authorization:** Required - Role: `LeavePolicy.Write`

**Request Body:**
```json
{
  "name": "Standard Annual Leave Policy",
  "leaveTypeId": "678cf2a4b3945e0001ac4d60",
  "defaultAllowance": 20.0,
  "accrualType": "Yearly",
  "accrualRate": 1.67,
  "maxCarryForward": 5.0,
  "minServiceMonths": 6,
  "effectiveFrom": "2025-01-01T00:00:00Z"
}
```

**Response:** `200 OK`
```json
{
  "data": "678cf2a4b3945e0001ac4d90",
  "status": {
    "statusCode": 200,
    "message": "Leave policy created successfully"
  }
}
```

---

### 8.2 Update Leave Policy
**Endpoint:** `PUT /leave-policy`

**Authorization:** Required - Role: `LeavePolicy.Write`

**Request Body:**
```json
{
  "id": "678cf2a4b3945e0001ac4d90",
  "name": "Enhanced Annual Leave Policy",
  "leaveTypeId": "678cf2a4b3945e0001ac4d60",
  "defaultAllowance": 22.0,
  "accrualType": "Monthly",
  "accrualRate": 1.83,
  "maxCarryForward": 7.0,
  "minServiceMonths": 3,
  "effectiveFrom": "2025-01-01T00:00:00Z"
}
```

**Response:** `200 OK`
```json
{
  "data": true,
  "status": {
    "statusCode": 200,
    "message": "Leave policy updated successfully"
  }
}
```

---

### 8.3 Delete Leave Policy
**Endpoint:** `DELETE /leave-policy/{id}`

**Authorization:** Required - Role: `LeavePolicy.Write`

**Response:** `200 OK`
```json
{
  "data": true,
  "status": {
    "statusCode": 200,
    "message": "Leave policy deleted successfully"
  }
}
```

---

### 8.4 Get Leave Policy by ID
**Endpoint:** `GET /leave-policy/{id}`

**Authorization:** Required - Role: `LeavePolicy.Read`

**Response:** `200 OK`
```json
{
  "data": {
    "id": "678cf2a4b3945e0001ac4d90",
    "name": "Enhanced Annual Leave Policy",
    "leaveTypeId": "678cf2a4b3945e0001ac4d60",
    "leaveTypeName": "Annual Leave",
    "defaultAllowance": 22.0,
    "accrualType": "Monthly",
    "accrualRate": 1.83,
    "maxCarryForward": 7.0,
    "minServiceMonths": 3,
    "effectiveFrom": "2025-01-01T00:00:00Z",
    "isActive": true
  },
  "status": {
    "statusCode": 200
  }
}
```

---

### 8.5 Get Active Policies
**Endpoint:** `GET /leave-policy`

**Authorization:** Required - Role: `LeavePolicy.Read`

**Response:** `200 OK`
```json
{
  "data": [
    {
      "id": "678cf2a4b3945e0001ac4d90",
      "name": "Enhanced Annual Leave Policy",
      "leaveTypeId": "678cf2a4b3945e0001ac4d60",
      "leaveTypeName": "Annual Leave",
      "defaultAllowance": 22.0,
      "accrualType": "Monthly",
      "accrualRate": 1.83,
      "maxCarryForward": 7.0,
      "minServiceMonths": 3,
      "effectiveFrom": "2025-01-01T00:00:00Z",
      "isActive": true
    }
  ],
  "status": {
    "statusCode": 200
  }
}
```

---

### 8.6 Get Policy by Leave Type
**Endpoint:** `GET /leave-policy/leave-type/{leaveTypeId}`

**Authorization:** Required - Role: `LeavePolicy.Read`

**Response:** `200 OK`
```json
{
  "data": {
    "id": "678cf2a4b3945e0001ac4d90",
    "name": "Enhanced Annual Leave Policy",
    "leaveTypeId": "678cf2a4b3945e0001ac4d60",
    "leaveTypeName": "Annual Leave",
    "defaultAllowance": 22.0,
    "accrualType": "Monthly",
    "accrualRate": 1.83,
    "maxCarryForward": 7.0,
    "minServiceMonths": 3,
    "effectiveFrom": "2025-01-01T00:00:00Z",
    "isActive": true
  },
  "status": {
    "statusCode": 200
  }
}
```

---

## 9. Leave Approval Management

### 9.1 Approve Leave
**Endpoint:** `POST /leave-approval/approve`

**Authorization:** Required - Role: `LeaveApproval.Write`

**Request Body:**
```json
{
  "leaveRequestId": "678cf2a4b3945e0001ac4d70",
  "comments": "Approved. Enjoy your vacation!"
}
```

**Response:** `200 OK`
```json
{
  "data": true,
  "status": {
    "statusCode": 200,
    "message": "Leave request approved successfully"
  }
}
```

---

### 9.2 Reject Leave
**Endpoint:** `POST /leave-approval/reject`

**Authorization:** Required - Role: `LeaveApproval.Write`

**Request Body:**
```json
{
  "leaveRequestId": "678cf2a4b3945e0001ac4d70",
  "rejectionReason": "Critical project deadline during requested period. Please reschedule."
}
```

**Response:** `200 OK`
```json
{
  "data": true,
  "status": {
    "statusCode": 200,
    "message": "Leave request rejected"
  }
}
```

---

### 9.3 Get Pending Approvals
**Endpoint:** `GET /leave-approval/pending`

**Authorization:** Required - Role: `LeaveApproval.Write`

**Response:** `200 OK`
```json
{
  "data": [
    {
      "id": "678cf2a4b3945e0001ac4da0",
      "leaveRequestId": "678cf2a4b3945e0001ac4d70",
      "employeeId": "678cf2a4b3945e0001ac4d40",
      "employeeName": "Sarah Johnson",
      "leaveTypeName": "Annual Leave",
      "startDate": "2025-03-16T00:00:00Z",
      "endDate": "2025-03-22T00:00:00Z",
      "totalDays": 5,
      "reason": "Extended family vacation to Hawaii",
      "submittedAt": "2025-01-20T10:30:00Z",
      "approverLevel": 1,
      "status": "Pending"
    }
  ],
  "status": {
    "statusCode": 200
  }
}
```

---

### 9.4 Get Approval History
**Endpoint:** `GET /leave-approval/history/{leaveRequestId}`

**Authorization:** Required - Role: `LeaveApproval.Write`

**Response:** `200 OK`
```json
{
  "data": [
    {
      "id": "678cf2a4b3945e0001ac4da0",
      "leaveRequestId": "678cf2a4b3945e0001ac4d70",
      "approverId": "678cf2a4b3945e0001ac4d35",
      "approverName": "Michael Chen",
      "approverLevel": 1,
      "status": "Approved",
      "comments": "Approved. Enjoy your vacation!",
      "decidedAt": "2025-01-20T14:00:00Z"
    }
  ],
  "status": {
    "statusCode": 200
  }
}
```

---

## 10. Holiday Management

### 10.1 Create Holiday
**Endpoint:** `POST /holiday`

**Authorization:** Required - Role: `Holiday.Write`

**Request Body:**
```json
{
  "name": "Independence Day",
  "date": "2025-07-04T00:00:00Z",
  "description": "National Independence Day celebration",
  "isRecurring": true,
  "appliesToAllDepartments": true,
  "departments": []
}
```

**Response:** `200 OK`
```json
{
  "data": "678cf2a4b3945e0001ac4db0",
  "status": {
    "statusCode": 200,
    "message": "Holiday created successfully"
  }
}
```

---

### 10.2 Bulk Create Holidays
**Endpoint:** `POST /holiday/bulk`

**Authorization:** Required - Role: `Holiday.Write`

**Request Body:**
```json
{
  "holidays": [
    {
      "name": "New Year's Day",
      "date": "2025-01-01T00:00:00Z",
      "description": "New Year celebration",
      "isRecurring": true,
      "appliesToAllDepartments": true
    },
    {
      "name": "Labor Day",
      "date": "2025-09-01T00:00:00Z",
      "description": "International Workers' Day",
      "isRecurring": true,
      "appliesToAllDepartments": true
    },
    {
      "name": "Christmas Day",
      "date": "2025-12-25T00:00:00Z",
      "description": "Christmas celebration",
      "isRecurring": true,
      "appliesToAllDepartments": true
    }
  ]
}
```

**Response:** `200 OK`
```json
{
  "data": 3,
  "status": {
    "statusCode": 200,
    "message": "3 holidays created successfully"
  }
}
```

---

### 10.3 Update Holiday
**Endpoint:** `PUT /holiday`

**Authorization:** Required - Role: `Holiday.Write`

**Request Body:**
```json
{
  "id": "678cf2a4b3945e0001ac4db0",
  "name": "Independence Day (US)",
  "date": "2025-07-04T00:00:00Z",
  "description": "United States Independence Day celebration",
  "isRecurring": true,
  "appliesToAllDepartments": true,
  "departments": []
}
```

**Response:** `200 OK`
```json
{
  "data": true,
  "status": {
    "statusCode": 200,
    "message": "Holiday updated successfully"
  }
}
```

---

### 10.4 Delete Holiday
**Endpoint:** `DELETE /holiday/{id}`

**Authorization:** Required - Role: `Holiday.Write`

**Response:** `200 OK`
```json
{
  "data": true,
  "status": {
    "statusCode": 200,
    "message": "Holiday deleted successfully"
  }
}
```

---

### 10.5 Get Holidays by Year
**Endpoint:** `GET /holiday/year/{year}`

**Authorization:** Required - Role: `Holiday.Read`

**Response:** `200 OK`
```json
{
  "data": [
    {
      "id": "678cf2a4b3945e0001ac4db1",
      "name": "New Year's Day",
      "date": "2025-01-01T00:00:00Z",
      "description": "New Year celebration",
      "isRecurring": true,
      "appliesToAllDepartments": true,
      "departments": []
    },
    {
      "id": "678cf2a4b3945e0001ac4db0",
      "name": "Independence Day (US)",
      "date": "2025-07-04T00:00:00Z",
      "description": "United States Independence Day celebration",
      "isRecurring": true,
      "appliesToAllDepartments": true,
      "departments": []
    },
    {
      "id": "678cf2a4b3945e0001ac4db2",
      "name": "Labor Day",
      "date": "2025-09-01T00:00:00Z",
      "description": "International Workers' Day",
      "isRecurring": true,
      "appliesToAllDepartments": true,
      "departments": []
    },
    {
      "id": "678cf2a4b3945e0001ac4db3",
      "name": "Christmas Day",
      "date": "2025-12-25T00:00:00Z",
      "description": "Christmas celebration",
      "isRecurring": true,
      "appliesToAllDepartments": true,
      "departments": []
    }
  ],
  "status": {
    "statusCode": 200
  }
}
```

---

### 10.6 Get Upcoming Holidays
**Endpoint:** `GET /holiday/upcoming?count=5`

**Authorization:** Required - Role: `Holiday.Read`

**Query Parameters:**
- `count` (integer, optional): Maximum number of holidays to return (default: 10)

**Response:** `200 OK`
```json
{
  "data": [
    {
      "id": "678cf2a4b3945e0001ac4db0",
      "name": "Independence Day (US)",
      "date": "2025-07-04T00:00:00Z",
      "description": "United States Independence Day celebration",
      "isRecurring": true,
      "appliesToAllDepartments": true,
      "departments": []
    },
    {
      "id": "678cf2a4b3945e0001ac4db2",
      "name": "Labor Day",
      "date": "2025-09-01T00:00:00Z",
      "description": "International Workers' Day",
      "isRecurring": true,
      "appliesToAllDepartments": true,
      "departments": []
    }
  ],
  "status": {
    "statusCode": 200
  }
}
```

---

## 11. Team Leave Management

### 11.1 Get Team Leave Calendar
**Endpoint:** `GET /team-leave/calendar`

**Authorization:** Required - Role: `LeaveRequest.Admin`

**Query Parameters:**
- `department` (string, required): Department name
- `startDate` (datetime, required): Start date for calendar view
- `endDate` (datetime, required): End date for calendar view

**Example Request:**
```
GET /team-leave/calendar?department=Engineering&startDate=2025-03-01T00:00:00Z&endDate=2025-03-31T23:59:59Z
```

**Response:** `200 OK`
```json
{
  "data": [
    {
      "date": "2025-03-16T00:00:00Z",
      "employeesOnLeave": [
        {
          "employeeId": "678cf2a4b3945e0001ac4d40",
          "employeeName": "Sarah Johnson",
          "leaveTypeName": "Annual Leave",
          "leaveTypeColor": "#4CAF50"
        }
      ],
      "totalOnLeave": 1
    },
    {
      "date": "2025-03-17T00:00:00Z",
      "employeesOnLeave": [
        {
          "employeeId": "678cf2a4b3945e0001ac4d40",
          "employeeName": "Sarah Johnson",
          "leaveTypeName": "Annual Leave",
          "leaveTypeColor": "#4CAF50"
        },
        {
          "employeeId": "678cf2a4b3945e0001ac4d42",
          "employeeName": "Michael Chen",
          "leaveTypeName": "Annual Leave",
          "leaveTypeColor": "#4CAF50"
        }
      ],
      "totalOnLeave": 2
    }
  ],
  "status": {
    "statusCode": 200
  }
}
```

---

### 11.2 Check Team Leave Conflicts
**Endpoint:** `POST /team-leave/check-conflicts`

**Authorization:** Required - Role: `LeaveRequest.Admin`

**Request Body:**
```json
{
  "department": "Engineering",
  "startDate": "2025-03-01T00:00:00Z",
  "endDate": "2025-03-31T23:59:59Z",
  "maxEmployeesAllowed": 3
}
```

**Response:** `200 OK`
```json
{
  "data": [
    {
      "date": "2025-03-20T00:00:00Z",
      "conflictCount": 5,
      "maxAllowed": 3,
      "conflictingEmployees": [
        {
          "employeeId": "678cf2a4b3945e0001ac4d40",
          "employeeName": "Sarah Johnson",
          "leaveTypeName": "Annual Leave"
        },
        {
          "employeeId": "678cf2a4b3945e0001ac4d42",
          "employeeName": "Michael Chen",
          "leaveTypeName": "Annual Leave"
        },
        {
          "employeeId": "678cf2a4b3945e0001ac4d43",
          "employeeName": "Emily Davis",
          "leaveTypeName": "Personal Leave"
        },
        {
          "employeeId": "678cf2a4b3945e0001ac4d44",
          "employeeName": "David Wilson",
          "leaveTypeName": "Annual Leave"
        },
        {
          "employeeId": "678cf2a4b3945e0001ac4d45",
          "employeeName": "Lisa Anderson",
          "leaveTypeName": "Annual Leave"
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

### 11.3 Get Team Leave Statistics
**Endpoint:** `GET /team-leave/stats/{department}`

**Authorization:** Required - Role: `LeaveRequest.Admin`

**Response:** `200 OK`
```json
{
  "data": {
    "department": "Engineering",
    "totalEmployees": 25,
    "currentlyOnLeave": 2,
    "pendingRequests": 3,
    "upcomingLeave30Days": 8,
    "averageDaysPerEmployee": 12.5,
    "mostUsedLeaveType": {
      "leaveTypeId": "678cf2a4b3945e0001ac4d60",
      "leaveTypeName": "Annual Leave",
      "totalDaysUsed": 187.5
    }
  },
  "status": {
    "statusCode": 200
  }
}
```

---

### 11.4 Get Upcoming Team Leave
**Endpoint:** `GET /team-leave/upcoming/{department}`

**Authorization:** Required - Role: `LeaveRequest.Admin`

**Response:** `200 OK`
```json
{
  "data": [
    {
      "date": "2025-03-16T00:00:00Z",
      "employeesOnLeave": [
        {
          "employeeId": "678cf2a4b3945e0001ac4d40",
          "employeeName": "Sarah Johnson",
          "leaveTypeName": "Annual Leave",
          "leaveTypeColor": "#4CAF50",
          "startDate": "2025-03-16T00:00:00Z",
          "endDate": "2025-03-22T00:00:00Z"
        }
      ],
      "totalOnLeave": 1
    }
  ],
  "status": {
    "statusCode": 200
  }
}
```

---

## 12. Leave Reports

### 12.1 Get Leave Usage Report
**Endpoint:** `GET /leave-report/usage`

**Authorization:** Required - Role: `LeaveRequest.Admin`

**Query Parameters:**
- `year` (integer, required): Year for the report
- `department` (string, optional): Department filter

**Example Request:**
```
GET /leave-report/usage?year=2025&department=Engineering
```

**Response:** `200 OK`
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

### 12.2 Get Department Statistics
**Endpoint:** `GET /leave-report/department-stats`

**Authorization:** Required - Role: `LeaveRequest.Admin`

**Query Parameters:**
- `department` (string, optional): Department filter

**Example Request:**
```
GET /leave-report/department-stats?department=Engineering
```

**Response:** `200 OK`
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

### 12.3 Get Leave Trend
**Endpoint:** `GET /leave-report/trends?year=2025`

**Authorization:** Required - Role: `LeaveRequest.Admin`

**Query Parameters:**
- `year` (integer, required): Year for trend analysis

**Response:** `200 OK`
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
      "month": 2,
      "monthName": "February",
      "totalDaysTaken": 38.5,
      "averageDaysPerEmployee": 1.5,
      "mostPopularLeaveType": "Annual Leave"
    },
    {
      "month": 3,
      "monthName": "March",
      "totalDaysTaken": 62.0,
      "averageDaysPerEmployee": 2.5,
      "mostPopularLeaveType": "Annual Leave"
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

### 12.4 Get Employee Leave Summary
**Endpoint:** `GET /leave-report/employee-summary/{employeeId}?year=2025`

**Authorization:** Required - Role: `LeaveRequest.Admin`

**Query Parameters:**
- `year` (integer, required): Year for the summary

**Response:** `200 OK`
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
      },
      {
        "leaveTypeId": "678cf2a4b3945e0001ac4d61",
        "leaveTypeName": "Sick Leave",
        "totalAllowance": 10.0,
        "carryForward": 0.0,
        "used": 2.0,
        "pending": 0.0,
        "remaining": 8.0
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
      },
      {
        "id": "678cf2a4b3945e0001ac4d71",
        "leaveTypeName": "Sick Leave",
        "startDate": "2024-12-10T00:00:00Z",
        "endDate": "2024-12-11T00:00:00Z",
        "totalDays": 2.0,
        "status": "Approved"
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

## 13. Admin Dashboard

### 13.1 Get Dashboard Statistics
**Endpoint:** `GET /admin-dashboard/stats`

**Authorization:** Required - Role: `LeaveRequest.Admin`

**Response:** `200 OK`
```json
{
  "data": {
    "totalEmployees": 342,
    "employeesOnLeaveToday": 12,
    "pendingApprovals": 8,
    "upcomingLeave7Days": 23,
    "upcomingLeave30Days": 67,
    "leaveRequestsThisMonth": 45,
    "approvedThisMonth": 38,
    "rejectedThisMonth": 3,
    "averageApprovalTimeHours": 4.5,
    "departmentWithMostLeave": {
      "department": "Sales",
      "employeesOnLeave": 5
    }
  },
  "status": {
    "statusCode": 200
  }
}
```

---

### 13.2 Get Employees on Leave
**Endpoint:** `GET /admin-dashboard/employees-on-leave`

**Authorization:** Required - Role: `LeaveRequest.Admin`

**Response:** `200 OK`
```json
{
  "data": [
    {
      "employeeId": "678cf2a4b3945e0001ac4d40",
      "employeeName": "Sarah Johnson",
      "department": "Engineering",
      "jobTitle": "Lead Software Engineer",
      "leaveTypeName": "Annual Leave",
      "leaveTypeColor": "#4CAF50",
      "startDate": "2025-01-20T00:00:00Z",
      "endDate": "2025-01-22T00:00:00Z",
      "daysRemaining": 2
    },
    {
      "employeeId": "678cf2a4b3945e0001ac4d43",
      "employeeName": "Emily Davis",
      "department": "Marketing",
      "jobTitle": "Marketing Manager",
      "leaveTypeName": "Sick Leave",
      "leaveTypeColor": "#FF9800",
      "startDate": "2025-01-20T00:00:00Z",
      "endDate": "2025-01-20T00:00:00Z",
      "daysRemaining": 0
    }
  ],
  "status": {
    "statusCode": 200
  }
}
```

---

### 13.3 Get Upcoming Leaves
**Endpoint:** `GET /admin-dashboard/upcoming-leaves?days=30`

**Authorization:** Required - Role: `LeaveRequest.Admin`

**Query Parameters:**
- `days` (integer, optional): Number of days to look ahead (default: 30)

**Response:** `200 OK`
```json
{
  "data": [
    {
      "leaveRequestId": "678cf2a4b3945e0001ac4d70",
      "employeeId": "678cf2a4b3945e0001ac4d40",
      "employeeName": "Sarah Johnson",
      "department": "Engineering",
      "leaveTypeName": "Annual Leave",
      "startDate": "2025-03-16T00:00:00Z",
      "endDate": "2025-03-22T00:00:00Z",
      "totalDays": 5.0,
      "daysUntilStart": 55
    },
    {
      "leaveRequestId": "678cf2a4b3945e0001ac4d72",
      "employeeId": "678cf2a4b3945e0001ac4d42",
      "employeeName": "Michael Chen",
      "department": "Engineering",
      "leaveTypeName": "Annual Leave",
      "startDate": "2025-02-10T00:00:00Z",
      "endDate": "2025-02-14T00:00:00Z",
      "totalDays": 5.0,
      "daysUntilStart": 21
    }
  ],
  "status": {
    "statusCode": 200
  }
}
```

---

### 13.4 Get Pending Approvals Summary
**Endpoint:** `GET /admin-dashboard/pending-approvals`

**Authorization:** Required - Role: `LeaveRequest.Admin`

**Response:** `200 OK`
```json
{
  "data": [
    {
      "leaveRequestId": "678cf2a4b3945e0001ac4d75",
      "employeeId": "678cf2a4b3945e0001ac4d45",
      "employeeName": "Lisa Anderson",
      "department": "Finance",
      "leaveTypeName": "Personal Leave",
      "startDate": "2025-02-05T00:00:00Z",
      "endDate": "2025-02-07T00:00:00Z",
      "totalDays": 3.0,
      "submittedAt": "2025-01-19T09:00:00Z",
      "hoursPending": 32,
      "currentApproverName": "David Wilson"
    },
    {
      "leaveRequestId": "678cf2a4b3945e0001ac4d76",
      "employeeId": "678cf2a4b3945e0001ac4d46",
      "employeeName": "Robert Taylor",
      "department": "Sales",
      "leaveTypeName": "Annual Leave",
      "startDate": "2025-02-15T00:00:00Z",
      "endDate": "2025-02-21T00:00:00Z",
      "totalDays": 5.0,
      "submittedAt": "2025-01-20T14:30:00Z",
      "hoursPending": 2,
      "currentApproverName": "Jennifer Martinez"
    }
  ],
  "status": {
    "statusCode": 200
  }
}
```

---

## 14. Notification Management

### 14.1 Get My Notifications
**Endpoint:** `GET /notification?limit=50`

**Authorization:** Required (any authenticated user)

**Query Parameters:**
- `limit` (integer, optional): Maximum number of notifications to return (default: 50)

**Response:** `200 OK`
```json
{
  "data": [
    {
      "id": "678cf2a4b3945e0001ac4de0",
      "userId": "678cf2a4b3945e0001ac4d3a",
      "type": "LeaveRequestApproved",
      "title": "Leave Request Approved",
      "message": "Your leave request from March 16 to March 22 has been approved.",
      "isRead": false,
      "createdAt": "2025-01-20T14:00:00Z",
      "relatedEntityId": "678cf2a4b3945e0001ac4d70",
      "relatedEntityType": "LeaveRequest"
    },
    {
      "id": "678cf2a4b3945e0001ac4de1",
      "userId": "678cf2a4b3945e0001ac4d3a",
      "type": "LeaveBalanceAdjusted",
      "title": "Leave Balance Updated",
      "message": "Your annual leave balance has been adjusted by +2 days.",
      "isRead": true,
      "createdAt": "2025-01-15T10:00:00Z",
      "relatedEntityId": "678cf2a4b3945e0001ac4d80",
      "relatedEntityType": "LeaveBalance"
    }
  ],
  "status": {
    "statusCode": 200
  }
}
```

---

### 14.2 Get Unread Notifications
**Endpoint:** `GET /notification/unread`

**Authorization:** Required (any authenticated user)

**Response:** `200 OK`
```json
{
  "data": [
    {
      "id": "678cf2a4b3945e0001ac4de0",
      "userId": "678cf2a4b3945e0001ac4d3a",
      "type": "LeaveRequestApproved",
      "title": "Leave Request Approved",
      "message": "Your leave request from March 16 to March 22 has been approved.",
      "isRead": false,
      "createdAt": "2025-01-20T14:00:00Z",
      "relatedEntityId": "678cf2a4b3945e0001ac4d70",
      "relatedEntityType": "LeaveRequest"
    }
  ],
  "status": {
    "statusCode": 200
  }
}
```

---

### 14.3 Get Unread Notification Count
**Endpoint:** `GET /notification/unread/count`

**Authorization:** Required (any authenticated user)

**Response:** `200 OK`
```json
{
  "data": 3,
  "status": {
    "statusCode": 200
  }
}
```

---

### 14.4 Mark Notification as Read
**Endpoint:** `PUT /notification/{id}/read`

**Authorization:** Required (any authenticated user)

**Response:** `200 OK`
```json
{
  "data": true,
  "status": {
    "statusCode": 200,
    "message": "Notification marked as read"
  }
}
```

---

### 14.5 Mark All Notifications as Read
**Endpoint:** `PUT /notification/read-all`

**Authorization:** Required (any authenticated user)

**Response:** `200 OK`
```json
{
  "data": true,
  "status": {
    "statusCode": 200,
    "message": "All notifications marked as read"
  }
}
```

---

## 15. Localization Management

### 15.1 Upsert Error Message Localization
**Endpoint:** `POST /localize/error-messages/upsert`

**Authorization:** Required - Role: `Localize.Write`

**Request Body:**
```json
{
  "key": "ERR_INVALID_CREDENTIALS",
  "messages": {
    "en": "Invalid username or password",
    "es": "Usuario o contraseña inválidos",
    "fr": "Nom d'utilisateur ou mot de passe invalide",
    "de": "Ungültiger Benutzername oder Passwort"
  }
}
```

**Response:** `200 OK`
```json
{
  "data": true,
  "status": {
    "statusCode": 200,
    "message": "Error message localization saved successfully"
  }
}
```

---

### 15.2 Bulk Upsert Error Message Localizations
**Endpoint:** `POST /localize/error-messages/upserts`

**Authorization:** Required - Role: `Localize.Write`

**Request Body:**
```json
[
  {
    "key": "ERR_INVALID_CREDENTIALS",
    "messages": {
      "en": "Invalid username or password",
      "es": "Usuario o contraseña inválidos"
    }
  },
  {
    "key": "ERR_LEAVE_BALANCE_INSUFFICIENT",
    "messages": {
      "en": "Insufficient leave balance",
      "es": "Saldo de licencia insuficiente"
    }
  },
  {
    "key": "ERR_UNAUTHORIZED",
    "messages": {
      "en": "You do not have permission to perform this action",
      "es": "No tienes permiso para realizar esta acción"
    }
  }
]
```

**Response:** `200 OK`
```json
{
  "data": true,
  "status": {
    "statusCode": 200,
    "message": "Error message localizations saved successfully"
  }
}
```

---

### 15.3 Get Error Message Localizations (Paginated)
**Endpoint:** `GET /localize/error-messages`

**Authorization:** Required - Role: `Localize.Read`

**Query Parameters:**
- `cursor` (string, optional): Pagination cursor
- `page_size` (integer, required): Number of items per page
- `direction` (enum, required): `Forward` or `Backward`
- `sort_by` (string, required): Field to sort by (e.g., `key`)
- `include_total_count` (boolean, required): Whether to include total count

**Example Request:**
```
GET /localize/error-messages?cursor=null&page_size=20&direction=Forward&sort_by=key&include_total_count=true
```

**Response:** `200 OK`
```json
{
  "data": {
    "items": [
      {
        "id": "678cf2a4b3945e0001ac4df0",
        "key": "ERR_INVALID_CREDENTIALS",
        "messages": {
          "en": "Invalid username or password",
          "es": "Usuario o contraseña inválidos",
          "fr": "Nom d'utilisateur ou mot de passe invalide",
          "de": "Ungültiger Benutzername oder Passwort"
        }
      },
      {
        "id": "678cf2a4b3945e0001ac4df1",
        "key": "ERR_LEAVE_BALANCE_INSUFFICIENT",
        "messages": {
          "en": "Insufficient leave balance",
          "es": "Saldo de licencia insuficiente"
        }
      },
      {
        "id": "678cf2a4b3945e0001ac4df2",
        "key": "ERR_UNAUTHORIZED",
        "messages": {
          "en": "You do not have permission to perform this action",
          "es": "No tienes permiso para realizar esta acción"
        }
      }
    ],
    "nextCursor": "eyJpZCI6IjY3OGNmMmE0YjM5NDVlMDAwMWFjNGRmMiJ9",
    "hasPrevious": false,
    "hasNext": true,
    "totalCount": 87
  },
  "status": {
    "statusCode": 200
  }
}
```

---

## Common Error Responses

### 400 Bad Request
```json
{
  "data": null,
  "status": {
    "statusCode": 400,
    "message": "Validation failed",
    "errors": [
      {
        "field": "email",
        "message": "Invalid email format"
      },
      {
        "field": "startDate",
        "message": "Start date must be in the future"
      }
    ]
  }
}
```

### 401 Unauthorized
```json
{
  "data": null,
  "status": {
    "statusCode": 401,
    "message": "Authentication required. Please login."
  }
}
```

### 403 Forbidden
```json
{
  "data": null,
  "status": {
    "statusCode": 403,
    "message": "You do not have permission to access this resource. Required role: Employee.Write"
  }
}
```

### 404 Not Found
```json
{
  "data": null,
  "status": {
    "statusCode": 404,
    "message": "Resource not found"
  }
}
```

### 409 Conflict
```json
{
  "data": null,
  "status": {
    "statusCode": 409,
    "message": "Leave request conflicts with existing approved leave"
  }
}
```

### 422 Unprocessable Entity
```json
{
  "data": null,
  "status": {
    "statusCode": 422,
    "message": "Insufficient leave balance. You have 3 days remaining but requested 5 days."
  }
}
```

### 500 Internal Server Error
```json
{
  "data": null,
  "status": {
    "statusCode": 500,
    "message": "An unexpected error occurred. Please contact support with correlation ID: 8a3f2c1b-4e5d-6a7b-8c9d-0e1f2a3b4c5d"
  }
}
```

---

## Rate Limiting

The API implements rate limiting to ensure fair usage:
- **Standard users:** 100 requests per minute
- **Admin users:** 200 requests per minute
- **Bulk operations:** 10 requests per minute

Rate limit headers are included in all responses:
```
X-RateLimit-Limit: 100
X-RateLimit-Remaining: 95
X-RateLimit-Reset: 1737385200
```

---

## Pagination

The API uses cursor-based pagination for efficient data retrieval:

**Query Parameters:**
- `cursor`: String representing the position (use `null` for first page)
- `page_size`: Number of items to return (1-100)
- `direction`: `Forward` or `Backward`
- `sort_by`: Field to sort by
- `include_total_count`: Boolean to include total count

**Response Structure:**
```json
{
  "items": [...],
  "nextCursor": "base64EncodedCursor",
  "previousCursor": "base64EncodedCursor",
  "hasNext": true,
  "hasPrevious": false,
  "totalCount": 156
}
```

---

## Date/Time Format

All dates and times are in ISO 8601 format with UTC timezone:
```
2025-01-20T14:30:00Z
```

---

## Webhook Events (Coming Soon)

The API will support webhook subscriptions for the following events:
- `leave_request.created`
- `leave_request.approved`
- `leave_request.rejected`
- `leave_request.cancelled`
- `leave_balance.adjusted`
- `employee.created`
- `employee.updated`

---

## Support

For API support, contact:
- Email: api-support@domnertech.com
- Documentation: https://docs.domnertech.com
- Status Page: https://status.domnertech.com

---

*Last Updated: January 20, 2025*
*API Version: 1.0*
