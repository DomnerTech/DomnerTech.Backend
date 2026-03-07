# Localization API

## Overview
Manages multi-language error messages and localized content for internationalization support.

**Base Path:** `/api/v1/localize`

---

## Endpoints Summary

| Method | Endpoint | Description | Authorization |
|--------|----------|-------------|---------------|
| POST | `/localize/error-messages/upsert` | Upsert error message | `Localize.Write` |
| POST | `/localize/error-messages/upserts` | Bulk upsert messages | `Localize.Write` |
| GET | `/localize/error-messages` | Get error messages (paginated) | `Localize.Read` |

---

## 1. Upsert Error Message

Creates or updates a localized error message for a specific error code and language.

**Endpoint:** `POST /api/v1/localize/error-messages/upsert`

**Authorization:** Required - Role: `Localize.Write`

### Request

```json
{
  "errorCode": "ERR_INSUFFICIENT_BALANCE",
  "language": "es",
  "message": "Saldo de licencia insuficiente. Tiene {0} días restantes pero solicitó {1} días."
}
```

**Request Schema:**
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `errorCode` | string | Yes | Error code identifier (e.g., ERR_INSUFFICIENT_BALANCE) |
| `language` | string | Yes | ISO 639-1 language code (en, es, fr, de) |
| `message` | string | Yes | Localized error message (max 1000 chars) |

### Response `200 OK`

```json
{
  "data": true,
  "status": {
    "statusCode": 200,
    "message": "Error message localization updated successfully"
  }
}
```

### Notes
- **Upsert Operation:** Creates new or updates existing localization
- **Placeholders:** Use {0}, {1}, {2} for dynamic values
- **Default Language:** English (en) is required for all error codes

---

## 2. Bulk Upsert Error Messages

Creates or updates multiple error message localizations in a single request.

**Endpoint:** `POST /api/v1/localize/error-messages/upserts`

**Authorization:** Required - Role: `Localize.Write`

### Request

```json
{
  "errorMessages": [
    {
      "errorCode": "ERR_INSUFFICIENT_BALANCE",
      "language": "es",
      "message": "Saldo de licencia insuficiente. Tiene {0} días restantes pero solicitó {1} días."
    },
    {
      "errorCode": "ERR_INSUFFICIENT_BALANCE",
      "language": "fr",
      "message": "Solde de congés insuffisant. Vous avez {0} jours restants mais vous avez demandé {1} jours."
    },
    {
      "errorCode": "ERR_INSUFFICIENT_BALANCE",
      "language": "de",
      "message": "Unzureichendes Urlaubsguthaben. Sie haben {0} Tage übrig, haben aber {1} Tage beantragt."
    },
    {
      "errorCode": "ERR_LEAVE_CONFLICT",
      "language": "es",
      "message": "La solicitud de licencia entra en conflicto con la licencia aprobada existente del {0} al {1}"
    },
    {
      "errorCode": "ERR_LEAVE_CONFLICT",
      "language": "fr",
      "message": "La demande de congé entre en conflit avec les congés approuvés existants du {0} au {1}"
    }
  ]
}
```

**Request Schema:**
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `errorMessages` | array | Yes | Array of error message localizations |
| `errorMessages[].errorCode` | string | Yes | Error code |
| `errorMessages[].language` | string | Yes | Language code |
| `errorMessages[].message` | string | Yes | Localized message |

### Response `200 OK`

```json
{
  "data": 5,
  "status": {
    "statusCode": 200,
    "message": "5 error message localizations updated successfully"
  }
}
```

### Use Cases
- Initial system setup
- Adding support for new language
- Bulk updates from translation files
- Importing translations from external sources

---

## 3. Get Error Messages (Paginated)

Retrieves localized error messages with pagination support.

**Endpoint:** `GET /api/v1/localize/error-messages`

**Authorization:** Required - Role: `Localize.Read`

### Query Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `cursor` | string | No | Pagination cursor |
| `page_size` | integer | Yes | Items per page (1-100) |
| `direction` | enum | Yes | `Forward` or `Backward` |
| `sort_by` | string | Yes | `errorCode`, `language`, `createdAt` |
| `include_total_count` | boolean | Yes | Include total count |
| `language` | string | No | Filter by language code |
| `errorCode` | string | No | Filter by error code |

### Response `200 OK`

```json
{
  "data": {
    "items": [
      {
        "id": "678cf2a4b3945e0001ac4dd0",
        "errorCode": "ERR_INSUFFICIENT_BALANCE",
        "language": "en",
        "message": "Insufficient leave balance. You have {0} days remaining but requested {1} days.",
        "createdAt": "2024-01-15T10:00:00Z",
        "updatedAt": "2025-01-20T14:30:00Z"
      },
      {
        "id": "678cf2a4b3945e0001ac4dd1",
        "errorCode": "ERR_INSUFFICIENT_BALANCE",
        "language": "es",
        "message": "Saldo de licencia insuficiente. Tiene {0} días restantes pero solicitó {1} días.",
        "createdAt": "2024-01-15T10:05:00Z",
        "updatedAt": "2025-01-20T14:35:00Z"
      },
      {
        "id": "678cf2a4b3945e0001ac4dd2",
        "errorCode": "ERR_INSUFFICIENT_BALANCE",
        "language": "fr",
        "message": "Solde de congés insuffisant. Vous avez {0} jours restants mais vous avez demandé {1} jours.",
        "createdAt": "2024-01-15T10:10:00Z",
        "updatedAt": "2025-01-20T14:40:00Z"
      }
    ],
    "nextCursor": "eyJpZCI6IjY3OGNmMmE0YjM5NDVlMDAwMWFjNGRkMiJ9",
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

### Example Request URLs

**Get all Spanish translations:**
```
GET /localize/error-messages?language=es&cursor=null&page_size=50&direction=Forward&sort_by=errorCode&include_total_count=true
```

**Get all translations for specific error code:**
```
GET /localize/error-messages?errorCode=ERR_INSUFFICIENT_BALANCE&cursor=null&page_size=10&direction=Forward&sort_by=language&include_total_count=true
```

---

## Supported Languages

| Language | Code | Name |
|----------|------|------|
| English | `en` | English (Default) |
| Spanish | `es` | Español |
| French | `fr` | Français |
| German | `de` | Deutsch |

### Adding New Language Support

1. Set up default translations for all error codes
2. Use bulk upsert to add translations
3. Test with `Accept-Language` header
4. Update API documentation

---

## Error Code Localization Format

### Placeholder Syntax

Use numbered placeholders for dynamic values:

**English:**
```
"Insufficient leave balance. You have {0} days remaining but requested {1} days."
```

**Spanish:**
```
"Saldo de licencia insuficiente. Tiene {0} días restantes pero solicitó {1} días."
```

**Usage in Code:**
```csharp
var message = await errorMessageLocalize.ResolveAsync(
    "ERR_INSUFFICIENT_BALANCE", 
    "es", 
    remaining: 3, 
    requested: 5
);
// Result: "Saldo de licencia insuficiente. Tiene 3 días restantes pero solicitó 5 días."
```

---

## Complete Error Code Reference

### Authentication & Authorization
| Error Code | English | Spanish | French | German |
|------------|---------|---------|--------|--------|
| `ERR_UNAUTHORIZED` | Authentication required | Autenticación requerida | Authentification requise | Authentifizierung erforderlich |
| `ERR_INVALID_TOKEN` | Invalid or expired token | Token inválido o expirado | Jeton invalide ou expiré | Ungültiges oder abgelaufenes Token |
| `ERR_FORBIDDEN` | Insufficient permissions | Permisos insuficientes | Autorisations insuffisantes | Unzureichende Berechtigungen |

### Leave Management
| Error Code | English | Spanish | French | German |
|------------|---------|---------|--------|--------|
| `ERR_INSUFFICIENT_BALANCE` | Insufficient leave balance. You have {0} days remaining but requested {1} days. | Saldo de licencia insuficiente. Tiene {0} días restantes pero solicitó {1} días. | Solde de congés insuffisant. Vous avez {0} jours restants mais vous avez demandé {1} jours. | Unzureichendes Urlaubsguthaben. Sie haben {0} Tage übrig, haben aber {1} Tage beantragt. |
| `ERR_LEAVE_CONFLICT` | Leave request conflicts with existing approved leave from {0} to {1} | La solicitud entra en conflicto con la licencia aprobada del {0} al {1} | Conflit avec un congé approuvé du {0} au {1} | Konflikt mit genehmigtem Urlaub vom {0} bis {1} |
| `ERR_INVALID_DATE_RANGE` | End date must be after start date | La fecha de fin debe ser posterior a la fecha de inicio | La date de fin doit être postérieure à la date de début | Enddatum muss nach dem Startdatum liegen |

---

## Language Detection

### Client-Side Detection

The API detects the user's preferred language from the `Accept-Language` header:

```http
GET /api/v1/leave-request
Accept-Language: es
```

### Priority Order

1. `Accept-Language` header
2. User profile language preference
3. Default language (English)

### Multiple Languages

If `Accept-Language` includes multiple languages, the API chooses the first supported language:

```http
Accept-Language: es-MX, es;q=0.9, en;q=0.8
```

Result: Spanish (es)

---

## Best Practices

### Translation Guidelines

1. **Keep Placeholders Consistent:** Maintain the same placeholder numbers across languages
2. **Context Matters:** Consider cultural differences in phrasing
3. **Professional Tone:** Use formal language for business applications
4. **Test with Native Speakers:** Validate translations with native speakers
5. **Handle Pluralization:** Some languages have complex plural rules

### Development Workflow

```javascript
// 1. Define error codes in constants
const ErrorCodes = {
  INSUFFICIENT_BALANCE: 'ERR_INSUFFICIENT_BALANCE',
  LEAVE_CONFLICT: 'ERR_LEAVE_CONFLICT'
};

// 2. Use placeholders for dynamic values
const message = await localizeError(
  ErrorCodes.INSUFFICIENT_BALANCE,
  getUserLanguage(),
  { remaining: 3, requested: 5 }
);

// 3. Always provide English fallback
const fallbackMessage = 
  "Insufficient leave balance. You have 3 days remaining but requested 5 days.";
```

---

## Common Use Cases

### Use Case 1: Setup Initial Translations
```javascript
async function setupInitialTranslations() {
  const translations = [
    // English (required baseline)
    {
      errorCode: "ERR_INSUFFICIENT_BALANCE",
      language: "en",
      message: "Insufficient leave balance. You have {0} days remaining but requested {1} days."
    },
    // Spanish
    {
      errorCode: "ERR_INSUFFICIENT_BALANCE",
      language: "es",
      message: "Saldo de licencia insuficiente. Tiene {0} días restantes pero solicitó {1} días."
    },
    // French
    {
      errorCode: "ERR_INSUFFICIENT_BALANCE",
      language: "fr",
      message: "Solde de congés insuffisant. Vous avez {0} jours restants mais vous avez demandé {1} jours."
    }
  ];

  await bulkUpsertErrorMessages({ errorMessages: translations });
}
```

### Use Case 2: Add Support for New Language
```javascript
async function addGermanSupport() {
  // Get all English messages
  const englishMessages = await getAllErrorMessages({ language: 'en' });
  
  // Translate to German (use translation service or manual)
  const germanMessages = englishMessages.map(msg => ({
    errorCode: msg.errorCode,
    language: 'de',
    message: translateToGerman(msg.message)
  }));
  
  // Bulk insert German translations
  await bulkUpsertErrorMessages({ errorMessages: germanMessages });
}
```

### Use Case 3: Export/Import Translations
```javascript
// Export to JSON for translators
async function exportTranslations(language) {
  const messages = await getAllErrorMessages({ language });
  const json = JSON.stringify(messages, null, 2);
  downloadFile(`translations-${language}.json`, json);
}

// Import from translated JSON
async function importTranslations(file) {
  const translations = JSON.parse(await file.text());
  await bulkUpsertErrorMessages({ errorMessages: translations });
}
```

---

## Related Endpoints
All API endpoints automatically use localization based on `Accept-Language` header.

---

[? Back to Documentation Home](./README.md)
