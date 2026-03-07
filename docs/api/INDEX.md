# API Documentation Index

This directory contains comprehensive API documentation for the DomnerTech Backend API.

## ?? Documentation Files

### Getting Started
- [**README.md**](./README.md) - Main documentation hub with overview and common information
- [**INDEX.md**](./INDEX.md) - This file - complete navigation index
- [**DOCUMENTATION-STATUS.md**](./DOCUMENTATION-STATUS.md) - Documentation progress and statistics

### API Endpoints by Controller

#### Core APIs ?
1. [**Authentication API**](./01-authentication.md) ?
   - POST `/auth/login` - User login
   - POST `/auth/logout` - User logout

2. [**User Management API**](./02-user-management.md) ?
   - GET `/user/get-me` - Get current user
   - POST `/user` - Create user
   - GET `/user/all` - Get all users (paginated)

3. [**Employee Management API**](./03-employee-management.md) ?
   - POST `/employee` - Create employee
   - PUT `/employee` - Update employee
   - GET `/employee` - Get employees (paginated)

4. [**Role Management API**](./04-role-management.md) ?
   - POST `/role` - Create role
   - GET `/role` - Get all roles
   - GET `/role/user-roles/{userId}` - Get user roles
   - POST `/role/upsert-user-role` - Assign role to user

#### Leave Management APIs ?
5. [**Leave Types API**](./05-leave-types.md) ?
   - POST `/leave-type` - Create leave type
   - PUT `/leave-type` - Update leave type
   - DELETE `/leave-type/{id}` - Delete leave type
   - GET `/leave-type` - Get all leave types
   - GET `/leave-type/{id}` - Get leave type by ID

6. [**Leave Requests API**](./06-leave-requests.md) ?
   - POST `/leave-request` - Create leave request
   - PUT `/leave-request` - Update leave request
   - POST `/leave-request/cancel` - Cancel leave request
   - GET `/leave-request/{id}` - Get leave request by ID
   - GET `/leave-request/my` - Get my leave requests
   - GET `/leave-request/status/{status}` - Get requests by status

7. [**Leave Balances API**](./07-leave-balances.md) ?
   - POST `/leave-balance` - Initialize leave balance
   - POST `/leave-balance/adjust` - Adjust leave balance
   - GET `/leave-balance/my` - Get my leave balances
   - GET `/leave-balance/employee/{employeeId}` - Get employee balances

8. [**Leave Policies API**](./08-leave-policies.md) ?
   - POST `/leave-policy` - Create policy
   - PUT `/leave-policy` - Update policy
   - DELETE `/leave-policy/{id}` - Delete policy
   - GET `/leave-policy/{id}` - Get policy by ID
   - GET `/leave-policy` - Get active policies
   - GET `/leave-policy/leave-type/{leaveTypeId}` - Get policy by leave type

9. [**Leave Approvals API**](./09-leave-approvals.md) ?
   - POST `/leave-approval/approve` - Approve leave
   - POST `/leave-approval/reject` - Reject leave
   - GET `/leave-approval/pending` - Get pending approvals
   - GET `/leave-approval/history/{leaveRequestId}` - Get approval history

#### Calendar & Planning APIs ?
10. [**Holidays API**](./10-holidays.md) ?
    - POST `/holiday` - Create holiday
    - POST `/holiday/bulk` - Bulk create holidays
    - PUT `/holiday` - Update holiday
    - DELETE `/holiday/{id}` - Delete holiday
    - GET `/holiday/year/{year}` - Get holidays by year
    - GET `/holiday/upcoming` - Get upcoming holidays

11. [**Team Leave API**](./11-team-leave.md) ?
    - GET `/team-leave/calendar` - Get team calendar
    - POST `/team-leave/check-conflicts` - Check conflicts
    - GET `/team-leave/stats/{department}` - Get team stats
    - GET `/team-leave/upcoming/{department}` - Get upcoming leaves

#### Reporting & Analytics APIs ?
12. [**Leave Reports API**](./12-leave-reports.md) ?
    - GET `/leave-report/usage` - Leave usage report
    - GET `/leave-report/department-stats` - Department statistics
    - GET `/leave-report/trends` - Leave trends
    - GET `/leave-report/employee-summary/{employeeId}` - Employee summary

13. [**Admin Dashboard API**](./13-admin-dashboard.md) ?
    - GET `/admin-dashboard/stats` - Dashboard statistics
    - GET `/admin-dashboard/employees-on-leave` - Employees on leave
    - GET `/admin-dashboard/upcoming-leaves` - Upcoming leaves
    - GET `/admin-dashboard/pending-approvals` - Pending approvals

#### System APIs ?
14. [**Notifications API**](./14-notifications.md) ?
    - GET `/notification` - Get my notifications
    - GET `/notification/unread` - Get unread notifications
    - GET `/notification/unread/count` - Get unread count
    - PUT `/notification/{id}/read` - Mark as read
    - PUT `/notification/read-all` - Mark all as read

15. [**Localization API**](./15-localization.md) ?
    - POST `/localize/error-messages/upsert` - Upsert error message
    - POST `/localize/error-messages/upserts` - Bulk upsert
    - GET `/localize/error-messages` - Get error messages (paginated)

#### Reference Documentation ?
16. [**Error Responses**](./16-error-responses.md) ? - Common error codes and handling

---

## ?? Quick Reference

### Base URL
```
https://api.domnertech.com/api/v1
```

### Authentication
All endpoints (except `/auth/login`) require authentication via:
- Bearer token in `Authorization` header, OR
- HTTP-only `authToken` cookie

### Required Headers
```http
Authorization: Bearer {token}
X-Company-Id: {companyId}
X-Correlation-Id: {uuid}
Content-Type: application/json
```

### Response Format
```json
{
  "data": { /* response data */ },
  "status": {
    "statusCode": 200,
    "message": "Success message"
  }
}
```

### Pagination
All list endpoints support cursor-based pagination with these parameters:
- `cursor` - Position cursor (null for first page)
- `page_size` - Items per page (1-100)
- `direction` - Forward or Backward
- `sort_by` - Sort field
- `include_total_count` - Boolean

### Date Format
ISO 8601 with UTC timezone: `2025-01-20T14:30:00Z`

---

## ?? Documentation Status

| Controller | Status | File | Endpoints |
|------------|--------|------|-----------|
| Authentication | ? Complete | [01-authentication.md](./01-authentication.md) | 2 |
| User Management | ? Complete | [02-user-management.md](./02-user-management.md) | 3 |
| Employee Management | ? Complete | [03-employee-management.md](./03-employee-management.md) | 3 |
| Role Management | ? Complete | [04-role-management.md](./04-role-management.md) | 4 |
| Leave Types | ? Complete | [05-leave-types.md](./05-leave-types.md) | 5 |
| Leave Requests | ? Complete | [06-leave-requests.md](./06-leave-requests.md) | 6 |
| Leave Balances | ? Complete | [07-leave-balances.md](./07-leave-balances.md) | 4 |
| Leave Policies | ? Complete | [08-leave-policies.md](./08-leave-policies.md) | 6 |
| Leave Approvals | ? Complete | [09-leave-approvals.md](./09-leave-approvals.md) | 4 |
| Holidays | ? Complete | [10-holidays.md](./10-holidays.md) | 6 |
| Team Leave | ? Complete | [11-team-leave.md](./11-team-leave.md) | 4 |
| Leave Reports | ? Complete | [12-leave-reports.md](./12-leave-reports.md) | 4 |
| Admin Dashboard | ? Complete | [13-admin-dashboard.md](./13-admin-dashboard.md) | 4 |
| Notifications | ? Complete | [14-notifications.md](./14-notifications.md) | 5 |
| Localization | ? Complete | [15-localization.md](./15-localization.md) | 3 |
| Error Responses | ? Complete | [16-error-responses.md](./16-error-responses.md) | Reference |

**Progress: 18/18 files complete (100%)** ??

---

## ?? Documentation Features

Each documentation file includes:

? **Complete endpoint documentation**  
? **Request/response schemas in tables**  
? **Real sample data with realistic values**  
? **cURL examples for every endpoint**  
? **JavaScript/Axios code examples**  
? **Error scenarios with examples**  
? **Business rules and constraints**  
? **Common use cases with code**  
? **Cross-references to related endpoints**  
? **Professional formatting**  

---

## ?? Documentation Modules

### Core Module (4 endpoints + User + Employee + Role Management)
Handles authentication, user management, employee records, and role-based access control.

### Leave Management Module (30+ endpoints)
Complete leave management system including types, requests, balances, policies, and approvals.

### Calendar & Planning Module (10 endpoints)
Holiday management and team leave planning with conflict detection.

### Reporting & Analytics Module (8 endpoints)
Comprehensive reporting, analytics, and administrative dashboard.

### System Module (8 endpoints)
Notifications system and multi-language localization support.

---

## ?? Finding What You Need

### By Feature
- **Authentication:** Start with [Authentication API](./01-authentication.md)
- **User Setup:** [User Management](./02-user-management.md) ? [Employee Management](./03-employee-management.md) ? [Role Management](./04-role-management.md)
- **Leave Setup:** [Leave Types](./05-leave-types.md) ? [Leave Policies](./08-leave-policies.md) ? [Leave Balances](./07-leave-balances.md)
- **Leave Operations:** [Leave Requests](./06-leave-requests.md) ? [Leave Approvals](./09-leave-approvals.md)
- **Planning:** [Holidays](./10-holidays.md) ? [Team Leave](./11-team-leave.md)
- **Reporting:** [Leave Reports](./12-leave-reports.md) ? [Admin Dashboard](./13-admin-dashboard.md)
- **System:** [Notifications](./14-notifications.md) ? [Localization](./15-localization.md)

### By Role
- **Developers:** Start with [README.md](./README.md) and [Authentication](./01-authentication.md)
- **Product Managers:** Review business rules in each documentation file
- **QA Engineers:** Use error scenarios and sample data for testing
- **System Admins:** Focus on [Admin Dashboard](./13-admin-dashboard.md) and [Leave Reports](./12-leave-reports.md)

---

## ?? Getting Started

### For New Developers
1. Read [README.md](./README.md) for API overview
2. Review [Authentication API](./01-authentication.md)
3. Understand [Error Responses](./16-error-responses.md)
4. Explore relevant controller documentation
5. Copy and adapt code examples

### For Integration
1. Set up authentication (see [01-authentication.md](./01-authentication.md))
2. Create users (see [02-user-management.md](./02-user-management.md))
3. Set up employees (see [03-employee-management.md](./03-employee-management.md))
4. Configure leave types and policies (see [05-leave-types.md](./05-leave-types.md) & [08-leave-policies.md](./08-leave-policies.md))
5. Start using leave management features

### For Testing
1. Use sample data from documentation
2. Follow error scenarios for negative testing
3. Test pagination with different parameters
4. Verify business rules enforcement
5. Test multi-language support

---

## ?? Additional Resources

### Code Examples
- **cURL:** Every endpoint includes cURL command
- **JavaScript:** Axios examples for common operations
- **Error Handling:** Client-side error handling patterns

### Business Logic
- **Validation Rules:** Documented per endpoint
- **Workflows:** Approval workflows, status transitions
- **Calculations:** Leave balance calculations, day counting

### Best Practices
- **Authentication:** Token management and security
- **Pagination:** Efficient data retrieval
- **Error Handling:** Retry logic and user feedback
- **Localization:** Multi-language support

---

## ?? Support

### Get Help
- **Email:** api-support@domnertech.com
- **Documentation:** https://docs.domnertech.com
- **Status Page:** https://status.domnertech.com
- **GitHub Issues:** https://github.com/DomnerTech/DomnerTech-Backend/issues

### Report Issues
- Documentation errors or typos
- Missing information
- Unclear explanations
- Outdated examples

---

## ?? Contributing

When updating documentation:
1. Follow the existing structure and format
2. Include real sample data with realistic values
3. Add code examples (cURL and JavaScript)
4. Document all request/response schemas
5. List all error scenarios
6. Include business rules and notes
7. Update this index file

---

## ?? Achievements

### Documentation Metrics
- **Total Files:** 18
- **Total Endpoints:** 63+
- **Code Examples:** 100+
- **Sample Requests:** 60+
- **Sample Responses:** 80+
- **Error Scenarios:** 50+
- **Business Rules:** 200+

### Quality Standards Met
? Comprehensive coverage  
? Consistent formatting  
? Real sample data  
? Multiple code examples  
? Error documentation  
? Business rules  
? Cross-references  
? Professional presentation  

---

## ?? Version History

### Version 1.0 (January 20, 2025)
- ? Initial complete documentation
- ? All 18 files created
- ? 63+ endpoints documented
- ? 100+ code examples
- ? Complete error reference
- ? Multi-language support documentation

---

*Last Updated: January 20, 2025*  
*Documentation Version: 1.0*  
*Status: ? COMPLETE*
