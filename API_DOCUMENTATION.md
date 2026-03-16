# Product Catalog & Inventory API Documentation

## Base URL
```
https://your-domain.com/api/v1
```

## Authentication
All endpoints require Bearer token authentication:
```
Authorization: Bearer {your_jwt_token}
```

---

## Products API

### Create Product
Creates a new product in the catalog.

**Endpoint:** `POST /Products`  
**Required Role:** `Product.Write`

**Request Body:**
```json
{
  "name": {
    "en": "Product Name",
    "km": "ឈ្មោះផលិតផល",
    "vi": "Tên sản phẩm"
  },
  "description": {
    "en": "Product description"
  },
  "sku": "PROD-001",
  "categoryId": "507f1f77bcf86cd799439011",
  "brandId": "507f1f77bcf86cd799439012",
  "prices": [
    {
      "priceType": 0,
      "currency": 0,
      "amount": 99.99
    }
  ],
  "costPrice": 50.00,
  "unitOfMeasure": 0,
  "attributes": [
    { "name": "Color", "value": "Red" }
  ],
  "barcodes": [
    { "value": "1234567890123", "type": "EAN-13" }
  ],
  "images": ["https://example.com/image1.jpg"],
  "trackInventory": true,
  "isTaxable": true,
  "taxRate": 10
}
```

**Response:** `200 OK`
```json
{
  "data": "507f1f77bcf86cd799439013",
  "isSuccess": true,
  "status": {
    "statusCode": 200
  }
}
```

---

### Update Product
Updates an existing product.

**Endpoint:** `PUT /Products/{productId}`  
**Required Role:** `Product.Write`

**Request Body:** Same as Create Product

**Response:** `200 OK`
```json
{
  "data": true,
  "isSuccess": true,
  "status": {
    "statusCode": 200
  }
}
```

---

### Delete Product
Soft deletes a product.

**Endpoint:** `DELETE /Products/{productId}`  
**Required Role:** `Product.Write`

**Response:** `200 OK`
```json
{
  "data": true,
  "isSuccess": true,
  "status": {
    "statusCode": 200
  }
}
```

---

### Get Product by ID
Retrieves a single product.

**Endpoint:** `GET /Products/{productId}`  
**Required Role:** `Product.Read`

**Response:** `200 OK`
```json
{
  "data": {
    "id": "507f1f77bcf86cd799439013",
    "name": {
      "en": "Product Name"
    },
    "sku": "PROD-001",
    "categoryId": "507f1f77bcf86cd799439011",
    "categoryName": "Electronics",
    "brandId": "507f1f77bcf86cd799439012",
    "brandName": "Samsung",
    "prices": [...],
    "status": 1,
    "createdAt": "2024-01-15T10:00:00Z"
  },
  "isSuccess": true
}
```

---

### Get All Products
Retrieves all products with optional filtering.

**Endpoint:** `GET /Products`  
**Required Role:** `Product.Read`

**Query Parameters:**
- `category_id` (optional): Filter by category
- `brand_id` (optional): Filter by brand
- `status` (optional): Filter by status (0=Draft, 1=Active, 2=Discontinued)
- `page_number` (optional, default=1): Page number
- `page_size` (optional, default=20): Items per page

**Example:** `GET /Products?category_id=507f&page_size=10`

**Response:** `200 OK`
```json
{
  "data": [
    {
      "id": "507f1f77bcf86cd799439013",
      "name": { "en": "Product 1" },
      ...
    }
  ],
  "isSuccess": true
}
```

---

### Search Products
Searches products by name or SKU.

**Endpoint:** `GET /Products/search`  
**Required Role:** `Product.Read`

**Query Parameters:**
- `q` (required): Search term

**Example:** `GET /Products/search?q=iphone`

**Response:** `200 OK`
```json
{
  "data": [
    {
      "id": "507f1f77bcf86cd799439013",
      "name": { "en": "iPhone 15 Pro" },
      "sku": "APPLE-IPHONE15PRO",
      ...
    }
  ],
  "isSuccess": true
}
```

---

## Categories API

### Create Category
Creates a new product category.

**Endpoint:** `POST /Categories`  
**Required Role:** `Category.Write`

**Request Body:**
```json
{
  "name": {
    "en": "Electronics",
    "km": "គ្រឿងអេឡិចត្រូនិច",
    "vi": "Điện tử"
  },
  "description": {
    "en": "Electronic products"
  },
  "slug": "electronics",
  "parentCategoryId": null,
  "imageUrl": "https://example.com/electronics.jpg",
  "displayOrder": 1
}
```

**Response:** `200 OK`
```json
{
  "data": "507f1f77bcf86cd799439011",
  "isSuccess": true
}
```

---

### Get All Categories
Retrieves all categories.

**Endpoint:** `GET /Categories`  
**Required Role:** `Category.Read`

**Query Parameters:**
- `active_only` (optional, default=true): Show only active categories

**Response:** `200 OK`
```json
{
  "data": [
    {
      "id": "507f1f77bcf86cd799439011",
      "name": { "en": "Electronics" },
      "slug": "electronics",
      "displayOrder": 1,
      "isActive": true
    }
  ],
  "isSuccess": true
}
```

---

## Brands API

### Create Brand
Creates a new brand.

**Endpoint:** `POST /Brands`  
**Required Role:** `Brand.Write`

**Request Body:**
```json
{
  "name": "Samsung",
  "description": "Samsung Electronics",
  "slug": "samsung",
  "logoUrl": "https://example.com/samsung-logo.png",
  "websiteUrl": "https://samsung.com",
  "displayOrder": 1
}
```

**Response:** `200 OK`
```json
{
  "data": "507f1f77bcf86cd799439012",
  "isSuccess": true
}
```

---

### Get All Brands
Retrieves all brands.

**Endpoint:** `GET /Brands`  
**Required Role:** `Brand.Read`

**Query Parameters:**
- `active_only` (optional, default=true): Show only active brands

**Response:** `200 OK`
```json
{
  "data": [
    {
      "id": "507f1f77bcf86cd799439012",
      "name": "Samsung",
      "slug": "samsung",
      "isActive": true
    }
  ],
  "isSuccess": true
}
```

---

## Warehouses API

### Create Warehouse
Creates a new warehouse.

**Endpoint:** `POST /Warehouses`  
**Required Role:** `Warehouse.Write`

**Request Body:**
```json
{
  "name": "Main Warehouse",
  "code": "WH-001",
  "type": "Distribution Center",
  "address": "123 Main St",
  "city": "Phnom Penh",
  "country": "Cambodia",
  "phone": "+855 12 345 678",
  "email": "warehouse@example.com",
  "managerId": "507f1f77bcf86cd799439015",
  "isDefault": true
}
```

**Response:** `200 OK`
```json
{
  "data": "507f1f77bcf86cd799439014",
  "isSuccess": true
}
```

---

### Get All Warehouses
Retrieves all warehouses.

**Endpoint:** `GET /Warehouses`  
**Required Role:** `Warehouse.Read`

**Query Parameters:**
- `active_only` (optional, default=true): Show only active warehouses

**Response:** `200 OK`
```json
{
  "data": [
    {
      "id": "507f1f77bcf86cd799439014",
      "name": "Main Warehouse",
      "code": "WH-001",
      "city": "Phnom Penh",
      "isActive": true,
      "isDefault": true
    }
  ],
  "isSuccess": true
}
```

---

## Inventory API

### Adjust Stock
Adjusts stock quantity (increase or decrease).

**Endpoint:** `POST /Inventory/adjust`  
**Required Role:** `Inventory.Write`

**Request Body:**
```json
{
  "productId": "507f1f77bcf86cd799439013",
  "warehouseId": "507f1f77bcf86cd799439014",
  "variantId": null,
  "quantity": 100,
  "reason": 0,
  "batchLotNumber": "BATCH-001",
  "serialNumber": null,
  "referenceId": null,
  "notes": "Initial stock entry"
}
```

**Adjustment Reasons:**
- `0` = StockCount
- `1` = Damage
- `2` = Expiry
- `3` = Loss
- `4` = Found
- `5` = SystemCorrection
- `6` = Other

**Response:** `200 OK`
```json
{
  "data": true,
  "isSuccess": true
}
```

---

### Reserve Stock
Reserves stock for an order.

**Endpoint:** `POST /Inventory/reserve`  
**Required Role:** `Inventory.Write`

**Query Parameters:**
- `product_id` (required): Product ID
- `warehouse_id` (required): Warehouse ID
- `quantity` (required): Quantity to reserve
- `order_id` (required): Order ID
- `variant_id` (optional): Variant ID
- `expires_at` (optional): Reservation expiry date

**Example:**
```
POST /Inventory/reserve?product_id=507f&warehouse_id=507g&quantity=5&order_id=507h
```

**Response:** `200 OK`
```json
{
  "data": "507f1f77bcf86cd799439016",
  "isSuccess": true
}
```

---

### Create Stock Transfer
Creates a transfer between warehouses.

**Endpoint:** `POST /Inventory/transfer`  
**Required Role:** `Inventory.Write`

**Request Body:**
```json
{
  "fromWarehouseId": "507f1f77bcf86cd799439014",
  "toWarehouseId": "507f1f77bcf86cd799439015",
  "productId": "507f1f77bcf86cd799439013",
  "variantId": null,
  "quantity": 50,
  "notes": "Transfer to branch store"
}
```

**Response:** `200 OK`
```json
{
  "data": "507f1f77bcf86cd799439017",
  "isSuccess": true
}
```

---

### Get Stock by Product
Gets stock levels for a product.

**Endpoint:** `GET /Inventory/stock/{productId}`  
**Required Role:** `Inventory.Read`

**Query Parameters:**
- `warehouse_id` (optional): Filter by warehouse
- `variant_id` (optional): Filter by variant

**Response:** `200 OK`
```json
{
  "data": [
    {
      "id": "507f1f77bcf86cd799439018",
      "productId": "507f1f77bcf86cd799439013",
      "productName": "iPhone 15 Pro",
      "productSku": "APPLE-IPHONE15PRO",
      "warehouseId": "507f1f77bcf86cd799439014",
      "warehouseName": "Main Warehouse",
      "quantityOnHand": 100,
      "reservedQuantity": 10,
      "availableQuantity": 90,
      "reorderLevel": 20,
      "isLowStock": false
    }
  ],
  "isSuccess": true
}
```

---

### Get Low Stock Items
Gets products below reorder level.

**Endpoint:** `GET /Inventory/low-stock`  
**Required Role:** `Inventory.Read`

**Query Parameters:**
- `warehouse_id` (optional): Filter by warehouse

**Response:** `200 OK`
```json
{
  "data": [
    {
      "id": "507f1f77bcf86cd799439018",
      "productName": "Product Name",
      "productSku": "PROD-001",
      "warehouseName": "Main Warehouse",
      "quantityOnHand": 5,
      "availableQuantity": 5,
      "reorderLevel": 20,
      "reorderQuantity": 100,
      "isLowStock": true
    }
  ],
  "isSuccess": true
}
```

---

## Enums Reference

### ProductStatus
- `0` = Draft
- `1` = Active
- `2` = Discontinued
- `3` = OutOfStock

### PriceType
- `0` = Retail
- `1` = Wholesale
- `2` = Promotion
- `3` = Member

### CurrencyCode
- `0` = USD
- `1` = KHR (Cambodian Riel)
- `2` = VND (Vietnamese Dong)

### UnitOfMeasure
- `0` = Piece
- `1` = Box
- `2` = Case
- `3` = Kilogram
- `4` = Gram
- `5` = Liter
- `6` = Milliliter
- `7` = Meter
- `8` = Dozen
- `9` = Pair

### StockMovementType
- `0` = InitialStock
- `1` = PurchaseReceipt
- `2` = Sale
- `3` = SaleReturn
- `4` = PurchaseReturn
- `5` = TransferOut
- `6` = TransferIn
- `7` = AdjustmentIncrease
- `8` = AdjustmentDecrease
- `9` = Damage
- `10` = Reservation
- `11` = ReservationRelease
- `12` = StockCount

### InventoryAdjustmentReason
- `0` = StockCount
- `1` = Damage
- `2` = Expiry
- `3` = Loss
- `4` = Found
- `5` = SystemCorrection
- `6` = Other

---

## Error Responses

All error responses follow this format:

```json
{
  "data": null,
  "isSuccess": false,
  "status": {
    "statusCode": 400,
    "errorCode": "ERROR_CODE",
    "message": "Error description",
    "desc": "Localized error message"
  }
}
```

### Common HTTP Status Codes
- `200` - Success
- `400` - Bad Request (validation error)
- `401` - Unauthorized (invalid/missing token)
- `403` - Forbidden (insufficient permissions)
- `404` - Not Found
- `409` - Conflict (duplicate SKU, slug, etc.)
- `500` - Internal Server Error

---

## Rate Limiting

API calls are rate-limited to:
- **100 requests per minute** per user
- **1000 requests per hour** per user

Rate limit headers are included in responses:
```
X-RateLimit-Limit: 100
X-RateLimit-Remaining: 95
X-RateLimit-Reset: 1640000000
```

---

## Pagination

Responses with multiple items support pagination:

**Query Parameters:**
- `page_number`: Page number (default: 1)
- `page_size`: Items per page (default: 20, max: 100)

**Response includes:**
```json
{
  "data": [...],
  "pagination": {
    "pageNumber": 1,
    "pageSize": 20,
    "totalPages": 5,
    "totalCount": 100
  }
}
```

---

## Localization

Multi-language fields (name, description) should include:
- `en` - English (required)
- `km` - Khmer (optional)
- `vi` - Vietnamese (optional)

Example:
```json
{
  "name": {
    "en": "Product Name",
    "km": "ឈ្មោះផលិតផល",
    "vi": "Tên sản phẩm"
  }
}
```

To get localized error messages, include the language header:
```
Accept-Language: km
```

---

## Webhooks (Coming Soon)

Domain events will be published via webhooks for:
- Product created/updated
- Stock level changed
- Low stock alert
- Stock transfer completed

---

## Support

For API support or questions:
- Email: api-support@domnertech.com
- Documentation: https://docs.domnertech.com
- Status Page: https://status.domnertech.com
