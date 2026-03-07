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
X-Correlation-Id: {uuid}
Content-Type: application/json
```

**Body:**

```json
{
  "first_name": "Sarah",
  "last_name": "Johnson",
  "email": "sarah.johnson@domnertech.com",
  "phone_number": "+1-555-0156",
  "date_of_birth": "1990-05-15T00:00:00Z",
  "hire_date": "2024-01-15T00:00:00Z",
  "department": "Engineering",
  "job_title": "Senior Software Engineer",
  "address": {
    "street": "123 Tech Street",
    "city": "San Francisco",
    "state": "CA",
    "postal_code": "94105",
    "country": "USA"
  }
}
```

**Request Schema:**
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `first_name` | string | Yes | Employee's first name (max 100 chars) |
| `last_name` | string | Yes | Employee's last name (max 100 chars) |
| `email` | string | Yes | Employee's email address (must be unique) |
| `phone_number` | string | Yes | Phone number (E.164 format recommended) |
| `date_of_birth` | datetime | Yes | Date of birth (ISO 8601 format) |
| `hire_date` | datetime | Yes | Hire/joining date (ISO 8601 format) |
| `department` | string | Yes | Department name (max 100 chars) |
| `job_title` | string | Yes | Job title/position (max 150 chars) |
| `address` | object | No | Employee's address |
| `address.street` | string | No | Street address |
| `address.city` | string | No | City name |
| `address.state` | string | No | State/province |
| `address.postal_code` | string | No | ZIP/postal code |
| `address.country` | string | No | Country name |

#### Response

**Success Response:** `200 OK`

```json
{
  "data": true,
  "status": {
    "status_code": 200,
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
    "status_code": 400,
    "message": "Validation failed",
    "errors": [
      {
        "field": "email",
        "message": "Invalid email format"
      },
      {
        "field": "date_of_birth",
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
    "status_code": 409,
    "message": "An employee with this email already exists",
    "error_code": "ERR_DUPLICATE_EMAIL"
  }
}
```

**Insufficient Permissions:** `403 Forbidden`

```json
{
  "data": null,
  "status": {
    "status_code": 403,
    "message": "You do not have permission to access this resource. Required role: Employee.Write"
  }
}
```

#### Example cURL

```bash
curl -X POST https://api.domnertech.com/api/v1/employee \
  -H "Authorization: Bearer eyJhbGc..." \
  -H "X-Correlation-Id: 550e8400-e29b-41d4-a716-446655440000" \
  -H "Content-Type: application/json" \
  -d '{
    "first_name": "Sarah",
    "last_name": "Johnson",
    "email": "sarah.johnson@domnertech.com",
    "phone_number": "+1-555-0156",
    "date_of_birth": "1990-05-15T00:00:00Z",
    "hire_date": "2024-01-15T00:00:00Z",
    "department": "Engineering",
    "job_title": "Senior Software Engineer",
    "address": {
      "street": "123 Tech Street",
      "city": "San Francisco",
      "state": "CA",
      "postal_code": "94105",
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
X-Correlation-Id: {uuid}
Content-Type: application/json
```

**Body:**

```json
{
  "id": "678cf2a4b3945e0001ac4d40",
  "first_name": "Sarah",
  "last_name": "Johnson",
  "email": "sarah.johnson@domnertech.com",
  "phone_number": "+1-555-0156",
  "date_of_birth": "1990-05-15T00:00:00Z",
  "department": "Engineering",
  "job_title": "Lead Software Engineer",
  "is_active": true,
  "address": {
    "street": "123 Tech Street",
    "city": "San Francisco",
    "state": "CA",
    "postal_code": "94105",
    "country": "USA"
  }
}
```

**Request Schema:**
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `id` | string | Yes | Employee's unique identifier |
| `first_name` | string | Yes | Employee's first name |
| `last_name` | string | Yes | Employee's last name |
| `email` | string | Yes | Employee's email address |
| `phone_number` | string | Yes | Phone number |
| `date_of_birth` | datetime | Yes | Date of birth |
| `department` | string | Yes | Department name |
| `job_title` | string | Yes | Job title/position |
| `is_active` | boolean | Yes | Employment status |
| `address` | object | No | Employee's address |

#### Response

**Success Response:** `200 OK`

```json
{
  "data": true,
  "status": {
    "status_code": 200,
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
    "status_code": 404,
    "message": "Employee with ID '678cf2a4b3945e0001ac4d40' not found",
    "error_code": "ERR_EMPLOYEE_NOT_FOUND"
  }
}
```

**Immutable Field Update:** `422 Unprocessable Entity`

```json
{
  "data": null,
  "status": {
    "status_code": 422,
    "message": "Employee number and hire date cannot be modified after creation",
    "error_code": "ERR_IMMUTABLE_FIELD"
  }
}
```

#### Example cURL

```bash
curl -X PUT https://api.domnertech.com/api/v1/employee \
  -H "Authorization: Bearer eyJhbGc..." \
  -H "X-Correlation-Id: 550e8400-e29b-41d4-a716-446655440000" \
  -H "Content-Type: application/json" \
  -d '{
    "id": "678cf2a4b3945e0001ac4d40",
    "first_name": "Sarah",
    "last_name": "Johnson",
    "email": "sarah.johnson@domnertech.com",
    "phone_number": "+1-555-0156",
    "date_of_birth": "1990-05-15T00:00:00Z",
    "department": "Engineering",
    "job_title": "Lead Software Engineer",
    "is_active": true
  }'
```

#### Notes

- Employee number and hire date are immutable after creation
- Setting `is_active` to false marks the employee as terminated
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
X-Correlation-Id: {uuid}
```

**Query Parameters:**
| Parameter | Type | Required | Description | Example |
|-----------|------|----------|-------------|---------|
| `cursor` | string | No | Pagination cursor (null for first page) | `null` |
| `page_size` | integer | Yes | Number of items per page (1-100) | `20` |
| `direction` | enum | Yes | Navigation direction | `Forward` or `Backward` |
| `sort_by` | string | Yes | Field to sort by | `first_name`, `hire_date`, `department`, `id` |
| `include_total_count` | boolean | Yes | Include total count in response | `true` |

#### Response

**Success Response:** `200 OK`

```json
{
  "data": {
    "items": [
      {
        "id": "678cf2a4b3945e0001ac4d40",
        "company_id": "678cf2a0b3945e0001ac4d30",
        "first_name": "Sarah",
        "last_name": "Johnson",
        "email": "sarah.johnson@domnertech.com",
        "phone_number": "+1-555-0156",
        "date_of_birth": "1990-05-15T00:00:00Z",
        "hire_date": "2024-01-15T00:00:00Z",
        "is_active": true,
        "department": "Engineering",
        "job_title": "Lead Software Engineer",
        "employee_number": "EMP-2024-001",
        "address": {
          "street": "123 Tech Street",
          "city": "San Francisco",
          "state": "CA",
          "postal_code": "94105",
          "country": "USA"
        }
      },
      {
        "id": "678cf2a4b3945e0001ac4d41",
        "company_id": "678cf2a0b3945e0001ac4d30",
        "first_name": "Michael",
        "last_name": "Chen",
        "email": "michael.chen@domnertech.com",
        "phone_number": "+1-555-0157",
        "date_of_birth": "1988-08-22T00:00:00Z",
        "hire_date": "2023-06-01T00:00:00Z",
        "is_active": true,
        "department": "Engineering",
        "job_title": "Staff Software Engineer",
        "employee_number": "EMP-2023-042",
        "address": {
          "street": "456 Innovation Ave",
          "city": "San Francisco",
          "state": "CA",
          "postal_code": "94110",
          "country": "USA"
        }
      },
      {
        "id": "678cf2a4b3945e0001ac4d42",
        "company_id": "678cf2a0b3945e0001ac4d30",
        "first_name": "Emily",
        "last_name": "Davis",
        "email": "emily.davis@domnertech.com",
        "phone_number": "+1-555-0158",
        "date_of_birth": "1992-03-10T00:00:00Z",
        "hire_date": "2024-02-01T00:00:00Z",
        "is_active": true,
        "department": "Marketing",
        "job_title": "Marketing Manager",
        "employee_number": "EMP-2024-008",
        "address": null
      }
    ],
    "next_cursor": "eyJpZCI6IjY3OGNmMmE0YjM5NDVlMDAwMWFjNGQ0MiJ9",
    "previous_cursor": null,
    "has_previous": false,
    "has_next": true,
    "total_count": 342
  },
  "status": {
    "status_code": 200
  }
}
```

**Response Schema:**
| Field | Type | Description |
|-------|------|-------------|
| `items` | array | Array of employee objects |
| `items[].id` | string | Employee's unique identifier |
| `items[].company_id` | string | Company/tenant identifier |
| `items[].first_name` | string | First name |
| `items[].last_name` | string | Last name |
| `items[].email` | string | Email address |
| `items[].phone_number` | string | Phone number |
| `items[].date_of_birth` | datetime | Date of birth |
| `items[].hire_date` | datetime | Hire date |
| `items[].is_active` | boolean | Employment status |
| `items[].department` | string | Department name |
| `items[].job_title` | string | Job title/position |
| `items[].employee_number` | string | Unique employee number |
| `items[].address` | object | Address (nullable) |
| `next_cursor` | string | Cursor for next page |
| `previous_cursor` | string | Cursor for previous page |
| `has_next` | boolean | Whether there is a next page |
| `has_previous` | boolean | Whether there is a previous page |
| `total_count` | integer | Total number of employees |

#### Example Request URL

```
GET /api/v1/employee?cursor=null&page_size=20&direction=Forward&sort_by=first_name&include_total_count=true
```

#### Example cURL

```bash
curl -X GET "https://api.domnertech.com/api/v1/employee?cursor=null&page_size=20&direction=Forward&sort_by=first_name&include_total_count=true" \
  -H "Authorization: Bearer eyJhbGc..." \
  -H "X-Correlation-Id: 550e8400-e29b-41d4-a716-446655440000"
```

#### Sort Options

You can sort by any of these fields:

- `first_name` - First name (alphabetical)
- `last_name` - Last name (alphabetical)
- `email` - Email address (alphabetical)
- `hire_date` - Hire date (chronological)
- `department` - Department name (alphabetical)
- `employee_number` - Employee number (numerical)
- `id` - Employee ID (chronological by creation)

#### Example JavaScript (Axios)

```javascript
const axios = require("axios");

async function getEmployees(
  cursor = null,
  pageSize = 20,
  sortBy = "first_name",
) {
  const params = {
    cursor: cursor,
    page_size: pageSize,
    direction: "Forward",
    sort_by: sortBy,
    include_total_count: cursor === null,
  };

  const config = {
    headers: {
      Authorization: `Bearer ${getAuthToken()}`,
      "X-Correlation-Id": generateUUID(),
    },
    params: params,
  };

  try {
    const response = await axios.get(
      "https://api.domnertech.com/api/v1/employee",
      config,
    );

    const data = response.data.data;
    console.log(`Retrieved ${data.items.length} employees`);
    console.log(`Total employees: ${data.total_count}`);

    return data;
  } catch (error) {
    console.error("Error fetching employees:", error.response.data);
    throw error;
  }
}

// Usage - Get employees by department
async function getEmployeesByDepartment(department) {
  let allEmployees = [];
  let cursor = null;

  do {
    const data = await getEmployees(cursor, 50, "first_name");
    const departmentEmployees = data.items.filter(
      (emp) => emp.department === department,
    );
    allEmployees = allEmployees.concat(departmentEmployees);
    cursor = data.has_next ? data.next_cursor : null;
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
  "company_id": "678cf2a0b3945e0001ac4d30",
  "first_name": "Sarah",
  "last_name": "Johnson",
  "email": "sarah.johnson@domnertech.com",
  "phone_number": "+1-555-0156",
  "date_of_birth": "1990-05-15T00:00:00Z",
  "hire_date": "2024-01-15T00:00:00Z",
  "is_active": true,
  "department": "Engineering",
  "job_title": "Lead Software Engineer",
  "employee_number": "EMP-2024-001",
  "address": {
    "street": "123 Tech Street",
    "city": "San Francisco",
    "state": "CA",
    "postal_code": "94105",
    "country": "USA"
  },
  "created_at": "2024-01-15T09:00:00Z",
  "updated_at": "2025-01-20T14:30:00Z"
}
```

### Field Constraints

| Field             | Type     | Constraints                                       |
| ----------------- | -------- | ------------------------------------------------- |
| `id`              | string   | MongoDB ObjectId (24 hex chars)                   |
| `company_id`      | string   | MongoDB ObjectId (24 hex chars)                   |
| `first_name`      | string   | Max 100 characters, required                      |
| `last_name`       | string   | Max 100 characters, required                      |
| `email`           | string   | Valid email, max 255 chars, unique per company    |
| `phone_number`    | string   | Max 20 characters, E.164 format recommended       |
| `date_of_birth`   | datetime | Employee must be 18+ years old                    |
| `hire_date`       | datetime | Cannot be in the future, immutable after creation |
| `is_active`       | boolean  | true = active, false = terminated                 |
| `department`      | string   | Max 100 characters                                |
| `job_title`       | string   | Max 150 characters                                |
| `employee_number` | string   | Auto-generated, format: EMP-YYYY-###, immutable   |
| `address`         | object   | Optional, can be null                             |

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
3. **Deactivation:** Setting `is_active` to false marks employee as terminated
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
  first_name: "John",
  last_name: "Doe",
  email: "john.doe@domnertech.com",
  phone_number: "+1-555-0100",
  date_of_birth: "1985-01-15T00:00:00Z",
  hire_date: "2025-02-01T00:00:00Z",
  department: "Sales",
  job_title: "Sales Representative",
});

// 2. Create user account (see User Management API)
const user = await createUser({
  username: "john.doe@domnertech.com",
  pwd: "SecurePassword123!",
});

// 3. Initialize leave balances (see Leave Balance API)
await initializeLeaveBalance({
  employeeId: employee.id,
  leaveTypeId: annualLeaveTypeId,
  year: 2025,
  totalAllowance: 20,
});
```

### Use Case 2: Promote Employee

```javascript
// Update job title and potentially department
await updateEmployee({
  id: employeeId,
  ...existingEmployeeData,
  job_title: "Senior Sales Manager",
  department: "Sales Management",
});
```

### Use Case 3: Terminate Employee

```javascript
// Mark as inactive
await updateEmployee({
  id: employeeId,
  ...existingEmployeeData,
  is_active: false,
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
      (emp) => emp.department.toLowerCase() === department.toLowerCase(),
    );
    employees = employees.concat(filtered);

    cursor = result.next_cursor;
    hasMore = result.has_next;
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
