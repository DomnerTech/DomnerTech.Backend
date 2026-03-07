# Localization API

## Overview

Manages multi-language error messages and localized content for internationalization support.

**Base Path:** `/api/v1/localize`

---

## Endpoints Summary

| Method | Endpoint                           | Description                    | Authorization    |
| ------ | ---------------------------------- | ------------------------------ | ---------------- |
| POST   | `/localize/error-messages/upsert`  | Upsert error message           | `Localize.Write` |
| POST   | `/localize/error-messages/upserts` | Bulk upsert messages           | `Localize.Write` |
| GET    | `/localize/error-messages`         | Get error messages (paginated) | `Localize.Read`  |

---

## 1. Upsert Error Message

## Localization API

## Overview

Manages multi-language error messages and localized content for internationalization support.

**Base Path:** `/api/v1/localize`

---

## Endpoints Summary

| Method | Endpoint                           | Description                    | Authorization    |
| ------ | ---------------------------------- | ------------------------------ | ---------------- |
| POST   | `/localize/error-messages/upsert`  | Upsert error message           | `Localize.Write` |
| POST   | `/localize/error-messages/upserts` | Bulk upsert messages           | `Localize.Write` |
| GET    | `/localize/error-messages`         | Get error messages (paginated) | `Localize.Read`  |

---

## 1. Upsert Error Message

Creates or updates a localized error message for a specific error code and language.

**Endpoint:** `POST /api/v1/localize/error-messages/upsert`

**Authorization:** Required - Role: `Localize.Write`

### Request

```json
{
  "error_code": "ERR_INSUFFICIENT_BALANCE",
  "language": "es",
  "message": "Saldo de licencia insuficiente. Tiene {0} d�as restantes pero solicit� {1} d�as."
}
```

**Request Schema:**
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `error_code` | string | Yes | Error code identifier (e.g., ERR_INSUFFICIENT_BALANCE) |
| `language` | string | Yes | ISO 639-1 language code (en, es, fr, de) |
| `message` | string | Yes | Localized error message (max 1000 chars) |

### Response `200 OK`

```json
{
  "data": true,
  "status": {
    "status_code": 200,
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
  "error_messages": [
    {
      "error_code": "ERR_INSUFFICIENT_BALANCE",
      "language": "es",
      "message": "Saldo de licencia insuficiente. Tiene {0} d�as restantes pero solicit� {1} d�as."
    },
    {
      "error_code": "ERR_INSUFFICIENT_BALANCE",
      "language": "fr",
      "message": "Solde de cong�s insuffisant. Vous avez {0} jours restants mais vous avez demand� {1} jours."
    },
    {
      "error_code": "ERR_INSUFFICIENT_BALANCE",
      "language": "de",
      "message": "Unzureichendes Urlaubsguthaben. Sie haben {0} Tage �brig, haben aber {1} Tage beantragt."
    },
    {
      "error_code": "ERR_LEAVE_CONFLICT",
      "language": "es",
      "message": "La solicitud de licencia entra en conflicto con la licencia aprobada existente del {0} al {1}"
    },
    {
      "error_code": "ERR_LEAVE_CONFLICT",
      "language": "fr",
      "message": "La demande de cong� entre en conflit avec les cong�s approuv�s existants du {0} au {1}"
    }
  ]
}
```

**Request Schema:**
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `error_messages` | array | Yes | Array of error message localizations |
| `error_messages[].error_code` | string | Yes | Error code |
| `error_messages[].language` | string | Yes | Language code |
| `error_messages[].message` | string | Yes | Localized message |

### Response `200 OK`

```json
{
  "data": 5,
  "status": {
    "status_code": 200,
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

| Parameter             | Type    | Required | Description                           |
| --------------------- | ------- | -------- | ------------------------------------- |
| `cursor`              | string  | No       | Pagination cursor                     |
| `page_size`           | integer | Yes      | Items per page (1-100)                |
| `direction`           | enum    | Yes      | `Forward` or `Backward`               |
| `sort_by`             | string  | Yes      | `error_code`, `language`, `createdAt` |
| `include_total_count` | boolean | Yes      | Include total count                   |
| `language`            | string  | No       | Filter by language code               |
| `error_code`          | string  | No       | Filter by error code                  |

### Response `200 OK`

```json
{
  "data": {
    "items": [
      {
        "id": "678cf2a4b3945e0001ac4dd0",
        "error_code": "ERR_INSUFFICIENT_BALANCE",
        "language": "en",
        "message": "Insufficient leave balance. You have {0} days remaining but requested {1} days.",
        "created_at": "2024-01-15T10:00:00Z",
        "updated_at": "2025-01-20T14:30:00Z"
      },
      {
        "id": "678cf2a4b3945e0001ac4dd1",
        "error_code": "ERR_INSUFFICIENT_BALANCE",
        "language": "es",
        "message": "Saldo de licencia insuficiente. Tiene {0} d�as restantes pero solicit� {1} d�as.",
        "created_at": "2024-01-15T10:05:00Z",
        "updated_at": "2025-01-20T14:35:00Z"
      },
      {
        "id": "678cf2a4b3945e0001ac4dd2",
        "error_code": "ERR_INSUFFICIENT_BALANCE",
        "language": "fr",
        "message": "Solde de cong�s insuffisant. Vous avez {0} jours restants mais vous avez demand� {1} jours.",
        "created_at": "2024-01-15T10:10:00Z",
        "updated_at": "2025-01-20T14:40:00Z"
      }
    ],
    "next_cursor": "eyJpZCI6IjY3OGNmMmE0YjM5NDVlMDAwMWFjNGRkMiJ9",
    "previous_cursor": null,
    "has_previous": false,
    "has_next": true,
    "total_count": 156
  },
  "status": {
    "status_code": 200
  }
}
```

### Example Request URLs

**Get all Spanish translations:**

```
GET /localize/error-messages?language=es&cursor=null&page_size=50&direction=Forward&sort_by=error_code&include_total_count=true
```

**Get all translations for specific error code:**

```
GET /localize/error-messages?error_code=ERR_INSUFFICIENT_BALANCE&cursor=null&page_size=10&direction=Forward&sort_by=language&include_total_count=true
```

---

## Supported Languages

| Language | Code | Name              |
| -------- | ---- | ----------------- |
| English  | `en` | English (Default) |
| Spanish  | `es` | Espa�ol           |
| French   | `fr` | Fran�ais          |
| German   | `de` | Deutsch           |

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
"Saldo de licencia insuficiente. Tiene {0} d�as restantes pero solicit� {1} d�as."
```

**Usage in Code:**

```csharp
var message = await errorMessageLocalize.ResolveAsync(
    "ERR_INSUFFICIENT_BALANCE",
    "es",
    remaining: 3,
    requested: 5
);
// Result: "Saldo de licencia insuficiente. Tiene 3 d�as restantes pero solicit� 5 d�as."
```

---

## Complete Error Code Reference

### Authentication & Authorization

| Error Code          | English                  | Spanish                   | French                   | German                             |
| ------------------- | ------------------------ | ------------------------- | ------------------------ | ---------------------------------- |
| `ERR_UNAUTHORIZED`  | Authentication required  | Autenticaci�n requerida   | Authentification requise | Authentifizierung erforderlich     |
| `ERR_INVALID_TOKEN` | Invalid or expired token | Token inv�lido o expirado | Jeton invalide ou expir� | Ung�ltiges oder abgelaufenes Token |

### Leave Management

| Error Code                 | English                                                                         | Spanish                                                                          | French                                                                                      | German                                                                                   |
| -------------------------- | ------------------------------------------------------------------------------- | -------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------- |
| `ERR_INSUFFICIENT_BALANCE` | Insufficient leave balance. You have {0} days remaining but requested {1} days. | Saldo de licencia insuficiente. Tiene {0} d�as restantes pero solicit� {1} d�as. | Solde de cong�s insuffisant. Vous avez {0} jours restants mais vous avez demand� {1} jours. | Unzureichendes Urlaubsguthaben. Sie haben {0} Tage �brig, haben aber {1} Tage beantragt. |
| `ERR_LEAVE_CONFLICT`       | Leave request conflicts with existing approved leave from {0} to {1}            | La solicitud entra en conflicto con la licencia aprobada del {0} al {1}          | Conflit avec un cong� approuv� du {0} au {1}                                                | Konflikt mit genehmigtem Urlaub vom {0} bis {1}                                          |
| `ERR_INVALID_DATE_RANGE`   | End date must be after start date                                               | La fecha de fin debe ser posterior a la fecha de inicio                          | La date de fin doit �tre post�rieure � la date de d�but                                     | Enddatum muss nach dem Startdatum liegen                                                 |

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
  INSUFFICIENT_BALANCE: "ERR_INSUFFICIENT_BALANCE",
  LEAVE_CONFLICT: "ERR_LEAVE_CONFLICT",
};

// 2. Use placeholders for dynamic values
const message = await localizeError(
  ErrorCodes.INSUFFICIENT_BALANCE,
  getUserLanguage(),
  { remaining: 3, requested: 5 },
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
      error_code: "ERR_INSUFFICIENT_BALANCE",
      language: "en",
      message:
        "Insufficient leave balance. You have {0} days remaining but requested {1} days.",
    },
    // Spanish
    {
      error_code: "ERR_INSUFFICIENT_BALANCE",
      language: "es",
      message:
        "Saldo de licencia insuficiente. Tiene {0} d�as restantes pero solicit� {1} d�as.",
    },
    // French
    {
      error_code: "ERR_INSUFFICIENT_BALANCE",
      language: "fr",
      message:
        "Solde de cong�s insuffisant. Vous avez {0} jours restants mais vous avez demand� {1} jours.",
    },
  ];

  await bulkUpsertErrorMessages({ error_messages: translations });
}
```

### Use Case 2: Add Support for New Language

```javascript
async function addGermanSupport() {
  // Get all English messages
  const englishMessages = await getAllErrorMessages({ language: "en" });

  // Translate to German (use translation service or manual)
  const germanMessages = englishMessages.map((msg) => ({
    error_code: msg.error_code,
    language: "de",
    message: translateToGerman(msg.message),
  }));

  // Bulk insert German translations
  await bulkUpsertErrorMessages({ error_messages: germanMessages });
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
  await bulkUpsertErrorMessages({ error_messages: translations });
}
```

    "status_code": 200

}
}

```

### Example Request URLs

**Get all Spanish translations:**
```

GET /localize/error-messages?language=es&cursor=null&page_size=50&direction=Forward&sort_by=error_code&include_total_count=true

```

**Get all translations for specific error code:**
```

GET /localize/error-messages?error_code=ERR_INSUFFICIENT_BALANCE&cursor=null&page_size=10&direction=Forward&sort_by=language&include_total_count=true

```

---

## Supported Languages

| Language | Code | Name |
|----------|------|------|
| English | `en` | English (Default) |
| Spanish | `es` | Espa�ol |
| French | `fr` | Fran�ais |
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

"Saldo de licencia insuficiente. Tiene {0} d�as restantes pero solicit� {1} d�as."

````

**Usage in Code:**
```csharp
var message = await errorMessageLocalize.ResolveAsync(
    "ERR_INSUFFICIENT_BALANCE",
    "es",
    remaining: 3,
    requested: 5
);
// Result: "Saldo de licencia insuficiente. Tiene 3 d�as restantes pero solicit� 5 d�as."
````

---

## Complete Error Code Reference

### Authentication & Authorization

| Error Code          | English                  | Spanish                   | French                   | German                             |
| ------------------- | ------------------------ | ------------------------- | ------------------------ | ---------------------------------- |
| `ERR_UNAUTHORIZED`  | Authentication required  | Autenticaci�n requerida   | Authentification requise | Authentifizierung erforderlich     |
| `ERR_INVALID_TOKEN` | Invalid or expired token | Token inv�lido o expirado | Jeton invalide ou expir� | Ung�ltiges oder abgelaufenes Token |

### Leave Management

| Error Code                 | English                                                                         | Spanish                                                                          | French                                                                                      | German                                                                                   |
| -------------------------- | ------------------------------------------------------------------------------- | -------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------- |
| `ERR_INSUFFICIENT_BALANCE` | Insufficient leave balance. You have {0} days remaining but requested {1} days. | Saldo de licencia insuficiente. Tiene {0} d�as restantes pero solicit� {1} d�as. | Solde de cong�s insuffisant. Vous avez {0} jours restants mais vous avez demand� {1} jours. | Unzureichendes Urlaubsguthaben. Sie haben {0} Tage �brig, haben aber {1} Tage beantragt. |
| `ERR_LEAVE_CONFLICT`       | Leave request conflicts with existing approved leave from {0} to {1}            | La solicitud entra en conflicto con la licencia aprobada del {0} al {1}          | Conflit avec un cong� approuv� du {0} au {1}                                                | Konflikt mit genehmigtem Urlaub vom {0} bis {1}                                          |
| `ERR_INVALID_DATE_RANGE`   | End date must be after start date                                               | La fecha de fin debe ser posterior a la fecha de inicio                          | La date de fin doit �tre post�rieure � la date de d�but                                     | Enddatum muss nach dem Startdatum liegen                                                 |

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
  INSUFFICIENT_BALANCE: "ERR_INSUFFICIENT_BALANCE",
  LEAVE_CONFLICT: "ERR_LEAVE_CONFLICT",
};

// 2. Use placeholders for dynamic values
const message = await localizeError(
  ErrorCodes.INSUFFICIENT_BALANCE,
  getUserLanguage(),
  { remaining: 3, requested: 5 },
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
      error_code: "ERR_INSUFFICIENT_BALANCE",
      language: "en",
      message:
        "Insufficient leave balance. You have {0} days remaining but requested {1} days.",
    },
    // Spanish
    {
      error_code: "ERR_INSUFFICIENT_BALANCE",
      language: "es",
      message:
        "Saldo de licencia insuficiente. Tiene {0} d�as restantes pero solicit� {1} d�as.",
    },
    // French
    {
      error_code: "ERR_INSUFFICIENT_BALANCE",
      language: "fr",
      message:
        "Solde de cong�s insuffisant. Vous avez {0} jours restants mais vous avez demand� {1} jours.",
    },
  ];

  await bulkUpsertErrorMessages({ error_messages: translations });
}
```

### Use Case 2: Add Support for New Language

```javascript
async function addGermanSupport() {
  // Get all English messages
  const englishMessages = await getAllErrorMessages({ language: "en" });

  // Translate to German (use translation service or manual)
  const germanMessages = englishMessages.map((msg) => ({
    error_code: msg.error_code,
    language: "de",
    message: translateToGerman(msg.message),
  }));

  // Bulk insert German translations
  await bulkUpsertErrorMessages({ error_messages: germanMessages });
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
  await bulkUpsertErrorMessages({ error_messages: translations });
}
```
