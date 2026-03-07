# Notifications API

## Overview
Manages user notifications for leave-related events including request submissions, approvals, rejections, and reminders.

**Base Path:** `/api/v1/notification`

---

## Endpoints Summary

| Method | Endpoint | Description | Authorization |
|--------|----------|-------------|---------------|
| GET | `/notification` | Get my notifications (paginated) | Any authenticated user |
| GET | `/notification/unread` | Get unread notifications | Any authenticated user |
| GET | `/notification/unread/count` | Get unread count | Any authenticated user |
| PUT | `/notification/{id}/read` | Mark as read | Any authenticated user |
| PUT | `/notification/read-all` | Mark all as read | Any authenticated user |

---

## 1. Get My Notifications (Paginated)

**Endpoint:** `GET /api/v1/notification`

### Query Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `cursor` | string | No | Pagination cursor |
| `page_size` | integer | Yes | Items per page (1-100) |
| `direction` | enum | Yes | `Forward` or `Backward` |
| `sort_by` | string | Yes | `createdAt`, `isRead` |
| `include_total_count` | boolean | Yes | Include total count |

### Response `200 OK`

```json
{
  "data": {
    "items": [
      {
        "id": "678cf2a4b3945e0001ac4dc0",
        "userId": "678cf2a4b3945e0001ac4d3a",
        "type": "LeaveRequestApproved",
        "title": "Leave Request Approved",
        "message": "Your annual leave request from March 16 to March 22 has been approved by Michael Chen",
        "relatedEntityType": "LeaveRequest",
        "relatedEntityId": "678cf2a4b3945e0001ac4d70",
        "isRead": false,
        "createdAt": "2025-01-20T14:30:00Z"
      },
      {
        "id": "678cf2a4b3945e0001ac4dc1",
        "userId": "678cf2a4b3945e0001ac4d3a",
        "type": "LeaveBalanceUpdated",
        "title": "Leave Balance Updated",
        "message": "Your annual leave balance has been adjusted. You now have 22 days available.",
        "relatedEntityType": "LeaveBalance",
        "relatedEntityId": "678cf2a4b3945e0001ac4d80",
        "isRead": true,
        "createdAt": "2025-01-15T10:00:00Z",
        "readAt": "2025-01-15T10:05:00Z"
      }
    ],
    "nextCursor": "eyJpZCI6IjY3OGNmMmE0YjM5NDVlMDAwMWFjNGRjMSJ9",
    "previousCursor": null,
    "hasPrevious": false,
    "hasNext": true,
    "totalCount": 45
  },
  "status": {
    "statusCode": 200
  }
}
```

---

## 2. Get Unread Notifications

**Endpoint:** `GET /api/v1/notification/unread`

### Response `200 OK`

```json
{
  "data": [
    {
      "id": "678cf2a4b3945e0001ac4dc0",
      "type": "LeaveRequestApproved",
      "title": "Leave Request Approved",
      "message": "Your annual leave request from March 16 to March 22 has been approved by Michael Chen",
      "relatedEntityType": "LeaveRequest",
      "relatedEntityId": "678cf2a4b3945e0001ac4d70",
      "createdAt": "2025-01-20T14:30:00Z"
    },
    {
      "id": "678cf2a4b3945e0001ac4dc2",
      "type": "PendingApproval",
      "title": "New Leave Request",
      "message": "Sarah Johnson has submitted a leave request that requires your approval",
      "relatedEntityType": "LeaveRequest",
      "relatedEntityId": "678cf2a4b3945e0001ac4d71",
      "createdAt": "2025-01-20T10:00:00Z"
    }
  ],
  "status": {
    "statusCode": 200
  }
}
```

---

## 3. Get Unread Count

**Endpoint:** `GET /api/v1/notification/unread/count`

### Response `200 OK`

```json
{
  "data": 5,
  "status": {
    "statusCode": 200
  }
}
```

---

## 4. Mark Notification as Read

**Endpoint:** `PUT /api/v1/notification/{id}/read`

### Path Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `id` | string | Yes | Notification ID |

### Response `200 OK`

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

## 5. Mark All as Read

**Endpoint:** `PUT /api/v1/notification/read-all`

### Response `200 OK`

```json
{
  "data": 5,
  "status": {
    "statusCode": 200,
    "message": "5 notifications marked as read"
  }
}
```

---

## Notification Types

### Employee Notifications

**Leave Request Status:**
- `LeaveRequestSubmitted` - Confirmation of submission
- `LeaveRequestApproved` - Request approved
- `LeaveRequestRejected` - Request rejected with reason
- `LeaveRequestCancelled` - Request cancelled

**Leave Balance:**
- `LeaveBalanceInitialized` - Balance set for new year
- `LeaveBalanceUpdated` - Manual adjustment made
- `LeaveBalanceExpiring` - Carry-forward days expiring soon

**Reminders:**
- `LeaveStartingTomorrow` - Leave starts tomorrow reminder
- `ReturnToWorkTomorrow` - Return to work tomorrow reminder

### Manager Notifications

**Approval Requests:**
- `PendingApproval` - New request awaiting approval
- `ApprovalReminder` - Request pending for X days
- `TeamLeaveConflict` - Too many team members on leave

**Team Updates:**
- `TeamMemberOnLeave` - Team member starting leave today
- `TeamMemberReturned` - Team member returned from leave

---

## Notification Delivery Channels

### In-App Notifications
- Delivered via this API
- Badge count on navigation
- Real-time updates via WebSocket

### Email Notifications
- Configurable per user
- Batched for non-urgent notifications
- Immediate for urgent (approvals, rejections)

### Push Notifications (Mobile)
- For mobile apps
- Configurable in user preferences
- Supports iOS and Android

---

## Notification Preferences

Users can configure notification preferences:

```json
{
  "channels": {
    "inApp": true,
    "email": true,
    "push": false
  },
  "types": {
    "leaveApproved": { "inApp": true, "email": true, "push": false },
    "leaveRejected": { "inApp": true, "email": true, "push": false },
    "pendingApproval": { "inApp": true, "email": true, "push": true },
    "leaveReminder": { "inApp": true, "email": false, "push": true }
  }
}
```

---

## Real-Time Updates

### WebSocket Connection

Connect to receive real-time notifications:

```javascript
const ws = new WebSocket('wss://api.domnertech.com/ws/notifications');

ws.onopen = () => {
  // Send authentication token
  ws.send(JSON.stringify({
    type: 'authenticate',
    token: getAuthToken()
  }));
};

ws.onmessage = (event) => {
  const notification = JSON.parse(event.data);
  console.log('New notification:', notification);
  
  // Update UI
  displayNotification(notification);
  incrementBadgeCount();
};
```

### Server-Sent Events (SSE)

Alternative to WebSocket for one-way notifications:

```javascript
const eventSource = new EventSource(
  'https://api.domnertech.com/api/v1/notification/stream',
  {
    headers: {
      'Authorization': `Bearer ${getAuthToken()}`
    }
  }
);

eventSource.onmessage = (event) => {
  const notification = JSON.parse(event.data);
  handleNotification(notification);
};
```

---

## Common Use Cases

### Use Case 1: Notification Bell
```javascript
async function updateNotificationBell() {
  // Get unread count
  const count = await getUnreadCount();
  
  // Update badge
  document.getElementById('notificationBadge').textContent = count;
  document.getElementById('notificationBadge').style.display = 
    count > 0 ? 'block' : 'none';
}

// Poll every 30 seconds
setInterval(updateNotificationBell, 30000);
```

### Use Case 2: Notification Panel
```javascript
async function loadNotifications() {
  const response = await getNotifications({
    cursor: null,
    page_size: 20,
    direction: 'Forward',
    sort_by: 'createdAt',
    include_total_count: true
  });
  
  renderNotifications(response.items);
}

function renderNotifications(notifications) {
  const container = document.getElementById('notificationList');
  container.innerHTML = notifications.map(notif => `
    <div class="notification ${notif.isRead ? 'read' : 'unread'}" 
         data-id="${notif.id}">
      <h4>${notif.title}</h4>
      <p>${notif.message}</p>
      <small>${formatDate(notif.createdAt)}</small>
    </div>
  `).join('');
  
  // Add click handlers
  container.querySelectorAll('.notification').forEach(elem => {
    elem.addEventListener('click', () => {
      markAsRead(elem.dataset.id);
      navigateToRelatedEntity(elem.dataset.id);
    });
  });
}
```

### Use Case 3: Mark as Read on View
```javascript
async function viewNotification(notificationId) {
  // Mark as read
  await markNotificationAsRead(notificationId);
  
  // Update UI
  updateBadgeCount();
  
  // Navigate to related entity
  const notification = await getNotificationById(notificationId);
  if (notification.relatedEntityType === 'LeaveRequest') {
    navigateToLeaveRequest(notification.relatedEntityId);
  }
}
```

---

## Business Rules

1. **Retention Period:** Notifications kept for 90 days
2. **Auto-Read:** Notifications older than 30 days auto-marked as read
3. **Duplicate Prevention:** Same event doesn't create duplicate notifications
4. **Priority Levels:** Critical notifications shown first
5. **Batching:** Non-urgent email notifications batched hourly

---

## Related Endpoints
- [Leave Requests](./06-leave-requests.md) - Events trigger notifications
- [Leave Approvals](./09-leave-approvals.md) - Approval/rejection notifications

---

[? Back to Documentation Home](./README.md)
