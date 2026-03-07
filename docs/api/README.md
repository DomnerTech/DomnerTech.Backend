# DomnerTech Backend API Documentation

## Overview

Welcome to the DomnerTech Backend API documentation. This is a comprehensive RESTful API for managing an advanced POS and business management system with a focus on employee leave management, HR operations, and multi-tenant support.

## Base URL
```
https://api.domnertech.com/api/v1
```

## API Version
**Version:** 1.0  
**Last Updated:** January 20, 2025

---

## ?? Documentation Structure

The API documentation is organized by controller/module:

### Core APIs
- [**Authentication**](./01-authentication.md) - Login and authentication
- [**User Management**](./02-user-management.md) - User CRUD operations
- [**Employee Management**](./03-employee-management.md) - Employee records and information
- [**Role Management**](./04-role-management.md) - Role-based access control

### Leave Management APIs
- [**Leave Types**](./05-leave-types.md) - Leave type configuration
- [**Leave Requests**](./06-leave-requests.md) - Submit and manage leave requests
- [**Leave Balances**](./07-leave-balances.md) - Employee leave balance tracking
- [**Leave Policies**](./08-leave-policies.md) - Leave policy management
- [**Leave Approvals**](./09-leave-approvals.md) - Approve/reject leave requests

### Calendar & Planning APIs
- [**Holidays**](./10-holidays.md) - Public holiday management
- [**Team Leave**](./11-team-leave.md) - Team calendar and conflict detection

### Reporting & Analytics APIs
- [**Leave Reports**](./12-leave-reports.md) - Usage reports and analytics
- [**Admin Dashboard**](./13-admin-dashboard.md) - Dashboard statistics

### System APIs
- [**Notifications**](./14-notifications.md) - User notifications
- [**Localization**](./15-localization.md) - Multi-language support

---

## ?? Authentication

All API endpoints (except login) require authentication. Include the JWT token in requests:

### Header-based Authentication
```http
Authorization: Bearer {your_jwt_token}
```

### Cookie-based Authentication
The API automatically sets an HTTP-only cookie named `authToken` upon successful login.

---

## ?? Required Headers

All authenticated requests require these headers:

| Header | Required | Description | Example |
|--------|----------|-------------|---------|
| `Authorization` | Yes* | Bearer token | `Bearer eyJhbGc...` |
| `X-Company-Id` | Yes | Tenant/Company ID | `678cf2a0b3945e0001ac4d30` |
| `X-Correlation-Id` | Yes | Request tracking ID | `uuid-v4-string` |
| `Content-Type` | Yes | Request content type | `application/json` |
| `lang` | No | Preferred language | `en`, `es`, `fr` |
| `platform` | No | Preferred platform | `chrome`, `safari`, `brave` |

*Not required if using cookie-based authentication

---

## ?? Common Response Format

All API responses follow this structure:

### Success Response
```json
{
  "data": {
    // Response data
  },
  "status": {
    "statusCode": 200,
    "message": "Success message"
  }
}
```

### Error Response
```json
{
  "data": null,
  "status": {
    "statusCode": 400,
    "message": "Error message",
    "errorCode": "ERR_CODE",
    "errors": [
      {
        "field": "fieldName",
        "message": "Validation error"
      }
    ]
  }
}
```

---

## ?? Pagination

The API uses **cursor-based pagination** for efficient data retrieval.

### Query Parameters
- `cursor` (string, optional): Position cursor, use `null` for first page
- `page_size` (integer, required): Items per page (1-100)
- `direction` (enum, required): `Forward` or `Backward`
- `sort_by` (string, required): Field to sort by
- `include_total_count` (boolean, required): Include total count

### Paginated Response Structure
```json
{
  "data": {
    "items": [...],
    "nextCursor": "base64EncodedCursor",
    "previousCursor": "base64EncodedCursor",
    "hasNext": true,
    "hasPrevious": false,
    "totalCount": 156
  },
  "status": {
    "statusCode": 200
  }
}
```

### Example Request
```http
GET /api/v1/employee?cursor=null&page_size=20&direction=Forward&sort_by=firstName&include_total_count=true
```

---

## ?? Date/Time Format

All dates and times use **ISO 8601 format** with UTC timezone:

```
2025-01-20T14:30:00Z
```

**Important:** Always send dates in UTC. The API handles timezone conversions internally.

---

## ?? Common HTTP Status Codes

| Code | Description |
|------|-------------|
| `200` | Success |
| `201` | Created |
| `400` | Bad Request - Validation error |
| `401` | Unauthorized - Authentication required |
| `403` | Forbidden - Insufficient permissions |
| `404` | Not Found - Resource doesn't exist |
| `409` | Conflict - Resource conflict |
| `422` | Unprocessable Entity - Business logic error |
| `429` | Too Many Requests - Rate limit exceeded |
| `500` | Internal Server Error |

For detailed error examples, see [Error Responses](./16-error-responses.md).

---

## ?? Rate Limiting

API requests are rate-limited to ensure fair usage:

| User Type | Limit |
|-----------|-------|
| Standard Users | 100 requests/minute |
| Admin Users | 200 requests/minute |
| Bulk Operations | 10 requests/minute |

### Rate Limit Headers
```http
X-RateLimit-Limit: 100
X-RateLimit-Remaining: 95
X-RateLimit-Reset: 1737385200
```

---

## ?? Role-Based Access Control (RBAC)

The API uses role-based permissions. Each endpoint specifies required roles:

### Common Roles
- `User.Read` / `User.Write` - User management
- `Employee.Read` / `Employee.Write` - Employee management
- `Role.Read` / `Role.Write` - Role management
- `LeaveType.Read` / `LeaveType.Write` - Leave type management
- `LeaveRequest.Read` / `LeaveRequest.Write` - Leave request management
- `LeaveRequest.Admin` - Administrative leave operations
- `LeaveBalance.Read` / `LeaveBalance.Write` - Balance management
- `LeavePolicy.Read` / `LeavePolicy.Write` - Policy management
- `LeaveApproval.Write` - Approve/reject leaves
- `Holiday.Read` / `Holiday.Write` - Holiday management
- `Localize.Read` / `Localize.Write` - Localization management

---

## ?? Testing

### Postman Collection
Import our Postman collection for easy testing: [Download Collection](./postman/DomnerTech-API.postman_collection.json)

### Sample Credentials
```json
{
  "username": "demo@domnertech.com",
  "password": "Demo123!@#"
}
```

---

## ?? Multi-Language Support

The API supports multiple languages for error messages and notifications:

**Supported Languages:**
- English (`en`)
- Spanish (`es`)
- French (`fr`)
- German (`de`)

Set the `Accept-Language` header to your preferred language.

---

## ?? Webhook Events (Coming Soon)

Future support for webhook subscriptions:
- `leave_request.created`
- `leave_request.approved`
- `leave_request.rejected`
- `leave_request.cancelled`
- `leave_balance.adjusted`
- `employee.created`
- `employee.updated`

---

## ?? Support

### Get Help
- **Email:** api-support@domnertech.com
- **Documentation:** https://docs.domnertech.com
- **Status Page:** https://status.domnertech.com
- **GitHub Issues:** https://github.com/DomnerTech/DomnerTech-Backend/issues

### Quick Links
- [Getting Started Guide](./getting-started.md)
- [Authentication Guide](./guides/authentication-guide.md)
- [Error Handling Guide](./guides/error-handling-guide.md)
- [Best Practices](./guides/best-practices.md)
- [Changelog](./CHANGELOG.md)

---

## ?? License

This API is proprietary to DomnerTech. Unauthorized use is prohibited.

---

*Last Updated: January 20, 2025*  
*API Version: 1.0*
