# Authentication API

## Overview

Handles user authentication, login, and logout operations.

**Base Path:** `/api/v1/auth`

---

## Endpoints

### 1. Login

Authenticates a user and returns a JWT token.

**Endpoint:** `POST /api/v1/auth/login`

**Authorization:** Public (No authentication required)

#### Request

**Headers:**

```http
Content-Type: application/json
```

**Body:**

```json
{
  "username": "john.doe@domnertech.com",
  "pwd": "SecureP@ssw0rd123"
}
```

**Request Schema:**
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `username` | string | Yes | User's email or username |
| `pwd` | string | Yes | User's password |

#### Response

**Success Response:** `200 OK`

```json
{
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI2NzhjZjJhNGIzOTQ1ZTAwMDFhYzRkM2EiLCJlbWFpbCI6ImpvaG4uZG9lQGRvbW5lcnRlY2guY29tIiwibmFtZSI6IkpvaG4gRG9lIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjpbIlVzZXIuUmVhZCIsIlVzZXIuV3JpdGUiLCJFbXBsb3llZS5SZWFkIl0sImV4cCI6MTczNzM4NTIwMCwiaXNzIjoiRG9tbmVyVGVjaCIsImF1ZCI6IkRvbW5lclRlY2hBUEkifQ.xYz123abc456def789ghi",
    "user": {
      "id": "678cf2a4b3945e0001ac4d3a",
      "company_id": "678cf2a0b3945e0001ac4d30",
      "username": "john.doe@domnertech.com",
      "email": "john.doe@domnertech.com",
      "first_name": "John",
      "last_name": "Doe",
      "is_active": true
    }
  },
  "status": {
    "status_code": 200,
    "message": "Login successful"
  }
}
```

**Response Schema:**
| Field | Type | Description |
|-------|------|-------------|
| `data.token` | string | JWT authentication token |
| `data.user.id` | string | User's unique identifier |
| `data.user.company_id` | string | Company/tenant identifier |
| `data.user.username` | string | User's username |
| `data.user.email` | string | User's email address |
| `data.user.first_name` | string | User's first name |
| `data.user.last_name` | string | User's last name |
| `data.user.is_active` | boolean | Whether the user account is active |

**Error Responses:**

**Invalid Credentials:** `401 Unauthorized`

```json
{
  "data": null,
  "status": {
    "status_code": 401,
    "message": "Invalid username or password",
    "error_code": "ERR_INVALID_CREDENTIALS"
  }
}
```

**Account Inactive:** `403 Forbidden`

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

**Validation Error:** `400 Bad Request`

```json
{
  "data": null,
  "status": {
    "status_code": 400,
    "message": "Validation failed",
    "errors": [
      {
        "field": "username",
        "message": "Username is required"
      },
      {
        "field": "pwd",
        "message": "Password is required"
      }
    ]
  }
}
```

#### Notes

- The JWT token is automatically set as an HTTP-only cookie named `authToken`
- Token expires in 24 hours by default
- Include the token in subsequent requests via `Authorization: Bearer {token}` header
- The token contains user claims including roles and permissions

#### Example cURL

```bash
curl -X POST https://api.domnertech.com/api/v1/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "username": "john.doe@domnertech.com",
    "pwd": "SecureP@ssw0rd123"
  }'
```

#### Example JavaScript (Axios)

```javascript
const axios = require("axios");

const loginData = {
  username: "john.doe@domnertech.com",
  pwd: "SecureP@ssw0rd123",
};

axios
  .post("https://api.domnertech.com/api/v1/auth/login", loginData)
  .then((response) => {
    const token = response.data.data.token;
    const user = response.data.data.user;
    console.log("Login successful:", user);
    // Store token for future requests
    localStorage.setItem("authToken", token);
  })
  .catch((error) => {
    console.error("Login failed:", error.response.data);
  });
```

---

### 2. Logout

Logs out the current user and invalidates the authentication token.

**Endpoint:** `POST /api/v1/auth/logout`

**Authorization:** Required (any authenticated user)

#### Request

**Headers:**

```http
Authorization: Bearer {your_jwt_token}
X-Correlation-Id: {uuid}
```

**Body:** None

#### Response

**Success Response:** `200 OK`

```json
{
  "data": true,
  "status": {
    "status_code": 200,
    "message": "Logged out successfully"
  }
}
```

#### Notes

- Removes the `authToken` HTTP-only cookie
- Client applications should also clear stored tokens from local storage
- The token is not blacklisted server-side; it simply expires naturally
- After logout, the token cannot be reused if cookies are cleared

#### Example cURL

```bash
curl -X POST https://api.domnertech.com/api/v1/auth/logout \
  -H "Authorization: Bearer eyJhbGc..." \
  -H "X-Correlation-Id: 550e8400-e29b-41d4-a716-446655440000"
```

#### Example JavaScript (Axios)

```javascript
const axios = require("axios");

const config = {
  headers: {
    Authorization: `Bearer ${localStorage.getItem("authToken")}`,
    "X-Correlation-Id": generateUUID(),
  },
};

axios
  .post("https://api.domnertech.com/api/v1/auth/logout", {}, config)
  .then((response) => {
    console.log("Logout successful");
    // Clear stored token
    localStorage.removeItem("authToken");
    // Redirect to login page
    window.location.href = "/login";
  })
  .catch((error) => {
    console.error("Logout failed:", error.response.data);
  });
```

---

## Authentication Flow

### Standard Authentication Flow

```
????????????                ????????????                ????????????
?  Client  ?                ?   API    ?                ? Database ?
????????????                ????????????                ????????????
     ?                           ?                           ?
     ?  POST /auth/login         ?                           ?
     ?  {username, password}     ?                           ?
     ???????????????????????????>?                           ?
     ?                           ?                           ?
     ?                           ?  Validate credentials     ?
     ?                           ???????????????????????????>?
     ?                           ?                           ?
     ?                           ?  User data + roles        ?
     ?                           ?<???????????????????????????
     ?                           ?                           ?
     ?                           ?  Generate JWT token       ?
     ?                           ?  Set authToken cookie     ?
     ?                           ?                           ?
     ?  200 OK                   ?                           ?
     ?  {token, user}            ?                           ?
     ?<???????????????????????????                           ?
     ?                           ?                           ?
     ?  Store token              ?                           ?
     ?                           ?                           ?
     ?  Subsequent API calls     ?                           ?
     ?  Authorization: Bearer... ?                           ?
     ???????????????????????????>?                           ?
     ?                           ?                           ?
```

### Token Refresh Flow (Coming Soon)

Token refresh functionality will be available in a future release.

---

## Security Best Practices

### Client-Side

1. **Store tokens securely:**
   - Use `httpOnly` cookies when possible
   - If using localStorage, implement XSS protection
   - Never store tokens in session storage

2. **Include tokens in requests:**
   - Use `Authorization: Bearer {token}` header
   - Or rely on automatic cookie transmission

3. **Handle token expiration:**
   - Check for 401 responses
   - Redirect to login when token expires
   - Implement token refresh logic (when available)

### Server-Side Security Features

- Passwords are hashed using bcrypt
- JWT tokens are signed with HS256
- Tokens include expiration claims
- HTTP-only cookies prevent XSS attacks
- HTTPS required for production

---

## Related Endpoints

- [User Management](./02-user-management.md) - Manage user accounts
- [Role Management](./04-role-management.md) - Assign roles and permissions

---

[? Back to Documentation Home](./README.md)
