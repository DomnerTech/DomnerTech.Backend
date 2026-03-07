# Employee Management API

## Overview
Manages employee records, including creation, updates, and retrieval with pagination support.

**Base Path:** `/api/v1/employee`

---

## Endpoints

### 1. Create Employee

Creates a new employee record in the system.

**Endpoint:** `POST /api/v1/employee`

**Authorization:** Required - Role: `Employee.Write`

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

**Request Schema:**
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `firstName` | string | Yes | Employee's first name (max 100 chars) |
| `lastName` | string | Yes | Employee's last name (max 100 chars) |
| `email` | string | Yes | Employee's email address (must be unique) |
| `phoneNumber` | string | Yes | Phone number (E.164 format recommended) |
| `dateOfBirth` | datetime | Yes | Date of birth (ISO 8601 format) |
| `hireDate` | datetime | Yes | Hire/joining date (ISO 8601 format) |
| `department` | string | Yes | Department name (max 100 chars) |
| `jobTitle` | string | Yes | Job title/position (max 150 chars) |
| `address` | object | No | Employee's address |
| `address.street` | string | No | Street address |
| `address.city` | string | No | City name |
| `address.state` | string | No | State/province |
| `address.postalCode` | string | No | ZIP/postal code |
| `address.country` | string | No | Country name |

#### Response

**Success Response:** `200 OK`
```json
{
  "data": true,
  "status": {
    "statusCode": 200,
    "message": "Employee created successfully"
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
        "field": "email",
        "message": "Invalid email format"
      },
      {
        "field": "dateOfBirth",
        "message": "Employee must be at least 18 years old"
      }
    ]
  }
}
```

**Duplicate Email:** `409 Conflict`
```json
{
  "data": null,
  "status": {
    "statusCode": 409,
    "message": "An employee with this email already exists",
    "errorCode": "ERR_DUPLICATE_EMAIL"
  }
}
```

**Insufficient Permissions:** `403 Forbidden`
```json
{
  "data": null,
  "status": {
    "statusCode": 403,
    "message": "You do not have permission to access this resource. Required role: Employee.Write"
  }
}
```

#### Example cURL

```bash
curl -X POST https://api.domnertech.com/api/v1/employee \
  -H "Authorization: Bearer eyJhbGc..." \
  -H "X-Company-Id: 678cf2a0b3945e0001ac4d30" \
  -H "X-Correlation-Id: 550e8400-e29b-41d4-a716-446655440000" \
  -H "Content-Type: application/json" \
  -d '{
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
  }'
```

#### Notes
- Employee number is auto-generated (format: EMP-YYYY-###)
- Email must be unique within the company
- Date of birth must indicate employee is at least 18 years old
- Hire date cannot be in the future
- New employees are marked as active by default

---

### 2. Update Employee

Updates an existing employee's information.

**Endpoint:** `PUT /api/v1/employee`

**Authorization:** Required - Role: `Employee.Write`

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

**Request Schema:**
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `id` | string | Yes | Employee's unique identifier |
| `firstName` | string | Yes | Employee's first name |
| `lastName` | string | Yes | Employee's last name |
| `email` | string | Yes | Employee's email address |
| `phoneNumber` | string | Yes | Phone number |
| `dateOfBirth` | datetime | Yes | Date of birth |
| `department` | string | Yes | Department name |
| `jobTitle` | string | Yes | Job title/position |
| `isActive` | boolean | Yes | Employment status |
| `address` | object | No | Employee's address |

#### Response

**Success Response:** `200 OK`
```json
{
  "data": true,
  "status": {
    "statusCode": 200,
    "message": "Employee updated successfully"
  }
}
```

**Error Responses:**

**Employee Not Found:** `404 Not Found`
```json
{
  "data": null,
  "status": {
    "statusCode": 404,
    "message": "Employee with ID '678cf2a4b3945e0001ac4d40' not found",
    "errorCode": "ERR_EMPLOYEE_NOT_FOUND"
  }
}
```

**Immutable Field Update:** `422 Unprocessable Entity`
```json
{
  "data": null,
  "status": {
    "statusCode": 422,
    "message": "Employee number and hire date cannot be modified after creation",
    "errorCode": "ERR_IMMUTABLE_FIELD"
  }
}
```

#### Example cURL

```bash
curl -X PUT https://api.domnertech.com/api/v1/employee \
  -H "Authorization: Bearer eyJhbGc..." \
  -H "X-Company-Id: 678cf2a0b3945e0001ac4d30" \
  -H "X-Correlation-Id: 550e8400-e29b-41d4-a716-446655440000" \
  -H "Content-Type: application/json" \
  -d '{
    "id": "678cf2a4b3945e0001ac4d40",
    "firstName": "Sarah",
    "lastName": "Johnson",
    "email": "sarah.johnson@domnertech.com",
    "phoneNumber": "+1-555-0156",
    "dateOfBirth": "1990-05-15T00:00:00Z",
    "department": "Engineering",
    "jobTitle": "Lead Software Engineer",
    "isActive": true
  }'
```

#### Notes
- Employee number and hire date are immutable after creation
- Setting `isActive` to false marks the employee as terminated
- Email uniqueness is validated on update
- All fields except `address` are required

---

### 3. Get Employees (Paginated)

Retrieves a paginated list of employees with cursor-based navigation and sorting options.

**Endpoint:** `GET /api/v1/employee`

**Authorization:** Required - Role: `Employee.Read`

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
| `cursor` | string | No | Pagination cursor (null for first page) | `null` |
| `page_size` | integer | Yes | Number of items per page (1-100) | `20` |
| `direction` | enum | Yes | Navigation direction | `Forward` or `Backward` |
| `sort_by` | string | Yes | Field to sort by | `firstName`, `hireDate`, `department`, `id` |
| `include_total_count` | boolean | Yes | Include total count in response | `true` |

#### Response

**Success Response:** `200 OK`
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
      },
      {
        "id": "678cf2a4b3945e0001ac4d41",
        "companyId": "678cf2a0b3945e0001ac4d30",
        "firstName": "Michael",
        "lastName": "Chen",
        "email": "michael.chen@domnertech.com",
        "phoneNumber": "+1-555-0157",
        "dateOfBirth": "1988-08-22T00:00:00Z",
        "hireDate": "2023-06-01T00:00:00Z",
        "isActive": true,
        "department": "Engineering",
        "jobTitle": "Staff Software Engineer",
        "employeeNumber": "EMP-2023-042",
        "address": {
          "street": "456 Innovation Ave",
          "city": "San Francisco",
          "state": "CA",
          "postalCode": "94110",
          "country": "USA"
        }
      },
      {
        "id": "678cf2a4b3945e0001ac4d42",
        "companyId": "678cf2a0b3945e0001ac4d30",
        "firstName": "Emily",
        "lastName": "Davis",
        "email": "emily.davis@domnertech.com",
        "phoneNumber": "+1-555-0158",
        "dateOfBirth": "1992-03-10T00:00:00Z",
        "hireDate": "2024-02-01T00:00:00Z",
        "isActive": true,
        "department": "Marketing",
        "jobTitle": "Marketing Manager",
        "employeeNumber": "EMP-2024-008",
        "address": null
      }
    ],
    "nextCursor": "eyJpZCI6IjY3OGNmMmE0YjM5NDVlMDAwMWFjNGQ0MiJ9",
    "previousCursor": null,
    "hasPrevious": false,
    "hasNext": true,
    "totalCount": 342
  },
  "status": {
    "statusCode": 200
  }
}
```

**Response Schema:**
| Field | Type | Description |
|-------|------|-------------|
| `items` | array | Array of employee objects |
| `items[].id` | string | Employee's unique identifier |
| `items[].companyId` | string | Company/tenant identifier |
| `items[].firstName` | string | First name |
| `items[].lastName` | string | Last name |
| `items[].email` | string | Email address |
| `items[].phoneNumber` | string | Phone number |
| `items[].dateOfBirth` | datetime | Date of birth |
| `items[].hireDate` | datetime | Hire date |
| `items[].isActive` | boolean | Employment status |
| `items[].department` | string | Department name |
| `items[].jobTitle` | string | Job title/position |
| `items[].employeeNumber` | string | Unique employee number |
| `items[].address` | object | Address (nullable) |
| `nextCursor` | string | Cursor for next page |
| `previousCursor` | string | Cursor for previous page |
| `hasNext` | boolean | Whether there is a next page |
| `hasPrevious` | boolean | Whether there is a previous page |
| `totalCount` | integer | Total number of employees |

#### Example Request URL

```
GET /api/v1/employee?cursor=null&page_size=20&direction=Forward&sort_by=firstName&include_total_count=true
```

#### Example cURL

```bash
curl -X GET "https://api.domnertech.com/api/v1/employee?cursor=null&page_size=20&direction=Forward&sort_by=firstName&include_total_count=true" \
  -H "Authorization: Bearer eyJhbGc..." \
  -H "X-Company-Id: 678cf2a0b3945e0001ac4d30" \
  -H "X-Correlation-Id: 550e8400-e29b-41d4-a716-446655440000"
```

#### Sort Options

You can sort by any of these fields:
- `firstName` - First name (alphabetical)
- `lastName` - Last name (alphabetical)
- `email` - Email address (alphabetical)
- `hireDate` - Hire date (chronological)
- `department` - Department name (alphabetical)
- `employeeNumber` - Employee number (numerical)
- `id` - Employee ID (chronological by creation)

#### Example JavaScript (Axios)

```javascript
const axios = require('axios');

async function getEmployees(cursor = null, pageSize = 20, sortBy = 'firstName') {
  const params = {
    cursor: cursor,
    page_size: pageSize,
    direction: 'Forward',
    sort_by: sortBy,
    include_total_count: cursor === null
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
      'https://api.domnertech.com/api/v1/employee',
      config
    );
    
    const data = response.data.data;
    console.log(`Retrieved ${data.items.length} employees`);
    console.log(`Total employees: ${data.totalCount}`);
    
    return data;
  } catch (error) {
    console.error('Error fetching employees:', error.response.data);
    throw error;
  }
}

// Usage - Get employees by department
async function getEmployeesByDepartment(department) {
  let allEmployees = [];
  let cursor = null;
  
  do {
    const data = await getEmployees(cursor, 50, 'firstName');
    const departmentEmployees = data.items.filter(
      emp => emp.department === department
    );
    allEmployees = allEmployees.concat(departmentEmployees);
    cursor = data.hasNext ? data.nextCursor : null;
  } while (cursor !== null);
  
  return allEmployees;
}
```

---

## Employee Object Reference

### Complete Employee Object
```json
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
  },
  "createdAt": "2024-01-15T09:00:00Z",
  "updatedAt": "2025-01-20T14:30:00Z"
}
```

### Field Constraints
| Field | Type | Constraints |
|-------|------|-------------|
| `id` | string | MongoDB ObjectId (24 hex chars) |
| `companyId` | string | MongoDB ObjectId (24 hex chars) |
| `firstName` | string | Max 100 characters, required |
| `lastName` | string | Max 100 characters, required |
| `email` | string | Valid email, max 255 chars, unique per company |
| `phoneNumber` | string | Max 20 characters, E.164 format recommended |
| `dateOfBirth` | datetime | Employee must be 18+ years old |
| `hireDate` | datetime | Cannot be in the future, immutable after creation |
| `isActive` | boolean | true = active, false = terminated |
| `department` | string | Max 100 characters |
| `jobTitle` | string | Max 150 characters |
| `employeeNumber` | string | Auto-generated, format: EMP-YYYY-###, immutable |
| `address` | object | Optional, can be null |

---

## Business Rules

### Employee Creation
1. **Age Requirement:** Employee must be at least 18 years old
2. **Email Uniqueness:** Email must be unique within the company
3. **Employee Number:** Auto-generated in format EMP-YYYY-### where YYYY is hire year
4. **Default Status:** New employees are active by default
5. **Hire Date:** Cannot be in the future

### Employee Updates
1. **Immutable Fields:** 
   - Employee number cannot be changed
   - Hire date cannot be changed
2. **Email Changes:** Must remain unique within company
3. **Deactivation:** Setting `isActive` to false marks employee as terminated
4. **All Fields Required:** Except address which is optional

### Department & Job Title
- Department names are free-text (no predefined list)
- Job titles are free-text (no predefined list)
- Consider using consistent naming conventions

### Address
- Address is optional
- All address fields are optional if address object is provided
- Address can be set to null

---

## Common Use Cases

### Use Case 1: Onboard New Employee
```javascript
// 1. Create employee record
const employee = await createEmployee({
  firstName: "John",
  lastName: "Doe",
  email: "john.doe@domnertech.com",
  phoneNumber: "+1-555-0100",
  dateOfBirth: "1985-01-15T00:00:00Z",
  hireDate: "2025-02-01T00:00:00Z",
  department: "Sales",
  jobTitle: "Sales Representative"
});

// 2. Create user account (see User Management API)
const user = await createUser({
  username: "john.doe@domnertech.com",
  pwd: "SecurePassword123!"
});

// 3. Initialize leave balances (see Leave Balance API)
await initializeLeaveBalance({
  employeeId: employee.id,
  leaveTypeId: annualLeaveTypeId,
  year: 2025,
  totalAllowance: 20
});
```

### Use Case 2: Promote Employee
```javascript
// Update job title and potentially department
await updateEmployee({
  id: employeeId,
  ...existingEmployeeData,
  jobTitle: "Senior Sales Manager",
  department: "Sales Management"
});
```

### Use Case 3: Terminate Employee
```javascript
// Mark as inactive
await updateEmployee({
  id: employeeId,
  ...existingEmployeeData,
  isActive: false
});

// Also deactivate user account
await deactivateUser(userId);
```

### Use Case 4: Search Employees by Department
```javascript
async function searchEmployeesByDepartment(department) {
  let employees = [];
  let cursor = null;
  let hasMore = true;
  
  while (hasMore) {
    const result = await getEmployees(cursor, 50);
    const filtered = result.items.filter(
      emp => emp.department.toLowerCase() === department.toLowerCase()
    );
    employees = employees.concat(filtered);
    
    cursor = result.nextCursor;
    hasMore = result.hasNext;
  }
  
  return employees;
}
```

---

## Related Endpoints
- [User Management](./02-user-management.md) - Link employees to user accounts
- [Leave Balances](./07-leave-balances.md) - Initialize employee leave balances
- [Leave Requests](./06-leave-requests.md) - Submit leave requests for employees

---

[? Back to Documentation Home](./README.md)
