# Common Error Responses

## Overview

This document describes common error responses across all API endpoints.

---

## Standard Error Response Format

All errors follow this structure:

```json
{
  "data": null,
  "status": {
    "status_code": 400,
    "message": "Human-readable error message",
    "error_code": "ERR_CODE",
    "desc": "Localized error description",
    "errors": [
      {
        "field": "fieldName",
        "message": "Field-specific error message"
      }
    ]
  }
}
```

---

## HTTP Status Codes

### 2xx Success

| Code  | Name       | Description                             |
| ----- | ---------- | --------------------------------------- |
| `200` | OK         | Request succeeded                       |
| `201` | Created    | Resource created successfully           |
| `204` | No Content | Request succeeded, no content to return |

### 4xx Client Errors

| Code  | Name                 | Description                                          |
| ----- | -------------------- | ---------------------------------------------------- |
| `400` | Bad Request          | Invalid request format or validation error           |
| `401` | Unauthorized         | Authentication required or token invalid             |
| `403` | Forbidden            | Authenticated but insufficient permissions           |
| `404` | Not Found            | Resource doesn't exist                               |
| `409` | Conflict             | Resource conflict (duplicate, etc.)                  |
| `422` | Unprocessable Entity | Request valid but business logic prevents processing |
| `429` | Too Many Requests    | Rate limit exceeded                                  |

### 5xx Server Errors

| Code  | Name                  | Description                     |
| ----- | --------------------- | ------------------------------- |
| `500` | Internal Server Error | Unexpected server error         |
| `502` | Bad Gateway           | Upstream service error          |
| `503` | Service Unavailable   | Service temporarily unavailable |
| `504` | Gateway Timeout       | Upstream service timeout        |

---

## Detailed Error Examples

### 400 Bad Request

**Validation Errors:**

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
        "field": "start_date",
        "message": "Start date must be in the future"
      },
      {
        "field": "page_size",
        "message": "Page size must be between 1 and 100"
      }
    ]
  }
}
```

**Invalid JSON:**

```json
{
  "data": null,
  "status": {
    "status_code": 400,
    "message": "Invalid JSON format in request body"
  }
}
```

**Missing Required Field:**

```json
{
  "data": null,
  "status": {
    "status_code": 400,
    "message": "Validation failed",
    "errors": [
      {
        "field": "leave_type_id",
        "message": "Leave type ID is required"
      }
    ]
  }
}
```

---

### 401 Unauthorized

**Missing Authentication:**

```json
{
  "data": null,
  "status": {
    "status_code": 401,
    "message": "Authentication required. Please login.",
    "error_code": "ERR_UNAUTHORIZED"
  }
}
```

**Invalid Token:**

```json
{
  "data": null,
  "status": {
    "status_code": 401,
    "message": "Invalid or expired authentication token",
    "error_code": "ERR_INVALID_TOKEN"
  }
}
```

**Invalid Credentials:**

```json
{
  "data": null,
  "status": {
    "status_code": 500,
    "message": "Invalid username or password",
    "error_code": "ERR_INVALID_CREDENTIALS"
  }
}
```

**Token Expired:**

```json
{
  "data": null,
  "status": {
    "status_code": 503,
    "message": "Your session has expired. Please login again.",
    "error_code": "ERR_TOKEN_EXPIRED"
  }
}
```

---

### 403 Forbidden

**Insufficient Permissions:**

```json
{
  "data": null,
  "status": {
    "status_code": 403,
    "message": "You do not have permission to access this resource. Required role: Employee.Write",
    "error_code": "ERR_FORBIDDEN"
  }
}
```

**Account Inactive:**

```json
{
  "data": null,
  "status": {
    "status_code": 403,
    "message": "Your account has been deactivated. Please contact support.",
    "error_code": "ERR_ACCOUNT_INACTIVE"
  }
}
```

**Company Access Denied:**

```json
{
  "data": null,
  "status": {
    "status_code": 403,
    "message": "You do not have access to this company's data",
    "error_code": "ERR_COMPANY_ACCESS_DENIED"
  }
}
```

---

### 404 Not Found

**Resource Not Found:**

```json
{
  "data": null,
  "status": {
    "status_code": 404,
    "message": "The requested resource was not found",
    "error_code": "ERR_NOT_FOUND"
  }
}
```

**Specific Resource Not Found:**

```json
{
  "data": null,
  "status": {
    "status_code": 404,
    "message": "Employee with ID '678cf2a4b3945e0001ac4d99' not found",
    "error_code": "ERR_EMPLOYEE_NOT_FOUND"
  }
}
```

**Endpoint Not Found:**

```json
{
  "data": null,
  "status": {
    "status_code": 404,
    "message": "Endpoint not found: GET /api/v1/invalid-endpoint"
  }
}
```

---

### 409 Conflict

**Duplicate Resource:**

```json
{
  "data": null,
  "status": {
    "status_code": 409,
    "message": "A user with this username already exists",
    "error_code": "ERR_DUPLICATE_USERNAME"
  }
}
```

**Leave Conflict:**

```json
{
  "data": null,
  "status": {
    "status_code": 409,
    "message": "Leave request conflicts with existing approved leave from 2025-03-18 to 2025-03-20",
    "error_code": "ERR_LEAVE_CONFLICT"
  }
}
```

**State Conflict:**

```json
{
  "data": null,
  "status": {
    "status_code": 409,
    "message": "Leave request has already been approved and cannot be modified",
    "error_code": "ERR_INVALID_STATE"
  }
}
```

---

### 422 Unprocessable Entity

**Business Rule Violation:**

```json
{
  "data": null,
  "status": {
    "status_code": 422,
    "message": "Insufficient leave balance. You have 3 days remaining but requested 5 days.",
    "error_code": "ERR_INSUFFICIENT_BALANCE"
  }
}
```

**Date Range Invalid:**

```json
{
  "data": null,
  "status": {
    "status_code": 422,
    "message": "End date must be after start date",
    "error_code": "ERR_INVALID_DATE_RANGE"
  }
}
```

**Cannot Perform Action:**

```json
{
  "data": null,
  "status": {
    "status_code": 422,
    "message": "Cannot cancel leave request that has already been rejected",
    "error_code": "ERR_CANNOT_CANCEL"
  }
}
```

**Minimum Service Not Met:**

```json
{
  "data": null,
  "status": {
    "status_code": 422,
    "message": "You must complete 6 months of service before requesting this leave type",
    "error_code": "ERR_MIN_SERVICE_NOT_MET"
  }
}
```

---

### 429 Too Many Requests

**Rate Limit Exceeded:**

```json
{
  "data": null,
  "status": {
    "status_code": 429,
    "message": "Rate limit exceeded. Please try again in 42 seconds.",
    "error_code": "ERR_RATE_LIMIT_EXCEEDED"
  }
}
```

**Response Headers:**

```http
X-RateLimit-Limit: 100
X-RateLimit-Remaining: 0
X-RateLimit-Reset: 1737385242
Retry-After: 42
```

---

### 500 Internal Server Error

**Unexpected Error:**

```json
{
  "data": null,
  "status": {
    "status_code": 500,
    "message": "An unexpected error occurred. Please contact support with correlation ID: 8a3f2c1b-4e5d-6a7b-8c9d-0e1f2a3b4c5d",
    "error_code": "ERR_INTERNAL_SERVER_ERROR"
  }
}
```

**Database Error:**

```json
{
  "data": null,
  "status": {
    "status_code": 500,
    "message": "Database connection error. Please try again later.",
    "error_code": "ERR_DATABASE_ERROR"
  }
}
```

---

### 503 Service Unavailable

**Maintenance Mode:**

```json
{
  "data": null,
  "status": {
    "status_code": 503,
    "message": "The API is currently undergoing maintenance. Please try again later.",
    "error_code": "ERR_MAINTENANCE"
  }
}
```

**Response Headers:**

```http
Retry-After: 3600
```

---

## Common Error Codes

### Authentication & Authorization

| Error Code                  | HTTP Status | Description                    |
| --------------------------- | ----------- | ------------------------------ |
| `ERR_UNAUTHORIZED`          | 401         | Authentication required        |
| `ERR_INVALID_TOKEN`         | 401         | Token is invalid or malformed  |
| `ERR_TOKEN_EXPIRED`         | 401         | Token has expired              |
| `ERR_INVALID_CREDENTIALS`   | 401         | Username or password incorrect |
| `ERR_FORBIDDEN`             | 403         | Insufficient permissions       |
| `ERR_ACCOUNT_INACTIVE`      | 403         | User account is deactivated    |
| `ERR_COMPANY_ACCESS_DENIED` | 403         | No access to company data      |

### Validation Errors

| Error Code               | HTTP Status | Description                   |
| ------------------------ | ----------- | ----------------------------- |
| `ERR_VALIDATION_FAILED`  | 400         | One or more validation errors |
| `ERR_INVALID_FORMAT`     | 400         | Invalid data format           |
| `ERR_INVALID_DATE_RANGE` | 422         | End date before start date    |
| `ERR_INVALID_EMAIL`      | 400         | Invalid email format          |

### Resource Errors

| Error Code                      | HTTP Status | Description                    |
| ------------------------------- | ----------- | ------------------------------ |
| `ERR_NOT_FOUND`                 | 404         | Resource not found             |
| `ERR_EMPLOYEE_NOT_FOUND`        | 404         | Employee not found             |
| `ERR_LEAVE_REQUEST_NOT_FOUND`   | 404         | Leave request not found        |
| `ERR_DUPLICATE_USERNAME`        | 409         | Username already exists        |
| `ERR_DUPLICATE_EMPLOYEE_NUMBER` | 409         | Employee number already exists |

### Leave Management Errors

| Error Code                  | HTTP Status | Description                            |
| --------------------------- | ----------- | -------------------------------------- |
| `ERR_INSUFFICIENT_BALANCE`  | 422         | Not enough leave balance               |
| `ERR_LEAVE_CONFLICT`        | 409         | Overlapping leave request              |
| `ERR_INVALID_STATE`         | 409         | Cannot perform action in current state |
| `ERR_CANNOT_CANCEL`         | 422         | Leave cannot be cancelled              |
| `ERR_MIN_SERVICE_NOT_MET`   | 422         | Minimum service period not completed   |
| `ERR_MAX_DAYS_EXCEEDED`     | 422         | Request exceeds maximum allowed days   |
| `ERR_PAST_DATE_NOT_ALLOWED` | 422         | Cannot request leave in the past       |

### System Errors

| Error Code                  | HTTP Status | Description                    |
| --------------------------- | ----------- | ------------------------------ |
| `ERR_INTERNAL_SERVER_ERROR` | 500         | Unexpected server error        |
| `ERR_DATABASE_ERROR`        | 500         | Database operation failed      |
| `ERR_RATE_LIMIT_EXCEEDED`   | 429         | Too many requests              |
| `ERR_MAINTENANCE`           | 503         | System maintenance in progress |

---

## Error Handling Best Practices

### Client-Side Error Handling

```javascript
async function apiRequest(url, options) {
  try {
    const response = await fetch(url, options);
    const data = await response.json();

    if (!response.ok) {
      // Handle different error types
      switch (data.status.status_code) {
        case 401:
          // Redirect to login
          window.location.href = "/login";
          break;
        case 403:
          // Show permission denied message
          showError("You do not have permission to perform this action");
          break;
        case 422:
          // Show business rule violation
          showError(data.status.message);
          break;
        case 429:
          // Handle rate limiting
          const retryAfter = response.headers.get("Retry-After");
          showError(`Too many requests. Try again in ${retryAfter} seconds`);
          break;
        case 500:
          // Show generic error with correlation ID
          showError(
            `An error occurred. Reference: ${data.status.correlation_id || "N/A"}`,
          );
          break;
        default:
          showError(data.status.message);
      }

      throw new Error(data.status.message);
    }

    return data;
  } catch (error) {
    console.error("API request failed:", error);
    throw error;
  }
}
```

### Validation Error Handling

```javascript
function handleValidationErrors(errors) {
  errors.forEach((error) => {
    // Show error next to field
    const fieldElement = document.getElementById(error.field);
    if (fieldElement) {
      const errorElement = document.createElement("div");
      errorElement.className = "error-message";
      errorElement.textContent = error.message;
      fieldElement.parentNode.appendChild(errorElement);
    }
  });
}
```

### Retry Logic for Transient Errors

```javascript
async function apiRequestWithRetry(url, options, maxRetries = 3) {
  for (let attempt = 0; attempt < maxRetries; attempt++) {
    try {
      return await apiRequest(url, options);
    } catch (error) {
      const status_code = error.response?.status;

      // Retry on 429, 500, 502, 503, 504
      const shouldRetry = [429, 500, 502, 503, 504].includes(status_code);

      if (!shouldRetry || attempt === maxRetries - 1) {
        throw error;
      }

      // Exponential backoff
      const delay = Math.pow(2, attempt) * 1000;
      await new Promise((resolve) => setTimeout(resolve, delay));
    }
  }
}
```

---

## Debugging Errors

### Correlation ID

Every request includes a correlation ID that can be used to trace the request through the system:

**Request Header:**

```http
X-Correlation-Id: 8a3f2c1b-4e5d-6a7b-8c9d-0e1f2a3b4c5d
```

**Error Response:**

```json
{
  "data": null,
  "status": {
    "status_code": 500,
    "message": "An unexpected error occurred. Please contact support with correlation ID: 8a3f2c1b-4e5d-6a7b-8c9d-0e1f2a3b4c5d"
  }
}
```

When reporting errors to support, always include the correlation ID.

### Logging

All requests and responses are logged server-side with:

- Correlation ID
- Timestamp
- Request method and path
- Response status code
- Error details
- User ID and company ID

---

## Multi-Language Error Messages

Error messages can be localized based on the `Accept-Language` header:

**Request:**

```http
Accept-Language: es
```

**Response:**

```json
{
  "data": null,
  "status": {
    "status_code": 401,
    "message": "Usuario o contrase�a inv�lidos",
    "error_code": "ERR_INVALID_CREDENTIALS"
  }
}
```

**Supported Languages:**

- English (`en`) - Default
- Spanish (`es`)
- French (`fr`)
- German (`de`)

---

## Support

If you encounter persistent errors:

1. **Check the documentation** for the specific endpoint
2. **Verify your request** format and parameters
3. **Check your permissions** and authentication
4. **Contact support** with:
   - Correlation ID
   - Timestamp
   - Request details
   - Error message

**Support Contacts:**

- Email: api-support@domnertech.com
- Status Page: https://status.domnertech.com

---

[? Back to Documentation Home](./README.md)
