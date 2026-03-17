# MongoDB Offset Pagination - Implementation Guide

## Overview

This is a production-ready, scalable MongoDB offset pagination system for .NET 10 applications. It follows clean architecture principles and provides secure, high-performance pagination for datasets with millions of records.

## Features

✅ **Offset-based pagination** (pageNumber, pageSize)  
✅ **Dynamic multi-field sorting** with validation  
✅ **Dynamic filtering** (exact, contains, range, boolean)  
✅ **Field whitelisting** using attributes for security  
✅ **DTO projection** support  
✅ **Multi-tenant** support  
✅ **Performance optimized** with MongoDB builders  
✅ **Reusable** across all repositories  
✅ **Unit-test friendly** structure  

---

## Architecture

The implementation follows clean architecture with clear separation of concerns:

```
Domain Layer
├── FilterableAttribute.cs          # Marks properties as filterable

Application Layer
├── Pagination/OffsetPaging/
│   ├── OffsetPageRequest.cs        # Request model
│   ├── OffsetPageResponse.cs       # Response model
│   ├── FilterOperator.cs           # Filter operators enum
│   ├── SortOrder.cs                # Sort direction enum
│   ├── FieldFilter.cs              # Filter definition
│   ├── IOffsetPaginator.cs         # Paginator interface
│   └── PaginationExtensions.cs     # Extension methods

Infrastructure Layer
├── Pagination/OffsetPaging/
│   ├── MongoOffsetPaginator.cs     # MongoDB implementation
│   └── FieldValidationService.cs   # Security validation
```

---

## Usage Examples

### 1. Basic Usage - Query Handler

```csharp
public class GetProductsPageQueryHandler(IProductRepo productRepo)
    : IRequestHandler<GetProductsPageQuery, BaseResponse<OffsetPageResponse<ProductDto>>>
{
    public async Task<BaseResponse<OffsetPageResponse<ProductDto>>> Handle(
        GetProductsPageQuery request,
        CancellationToken cancellationToken)
    {
        // Define projection
        var projection = (ProductEntity entity) => new ProductDto
        {
            Id = entity.Id.ToString(),
            Name = entity.Name,
            Status = entity.Status.ToString(),
            CreatedAt = entity.CreatedAt
        };

        // Get paginated results
        var result = await productRepo.GetPagedAsync<ProductEntity, ProductDto>(
            request.PageRequest,
            projection,
            cancellationToken);

        return BaseResponse<OffsetPageResponse<ProductDto>>.Success(result);
    }
}
```

### 2. Marking Entity Fields as Filterable and Sortable

```csharp
public sealed class ProductEntity : IBaseEntity, ITenantEntity
{
    [Sortable(alias: "id", order: 1)]
    [Filterable(alias: "id")]
    public ObjectId Id { get; set; }

    [Filterable(alias: "name", AllowPartialMatch = true)]
    [Sortable(alias: "name")]
    public string Name { get; set; }

    [Filterable(alias: "status")]
    [Sortable(alias: "status")]
    public ProductStatus Status { get; set; }

    [Filterable(alias: "createdAt")]
    [Sortable(alias: "createdAt")]
    public DateTime CreatedAt { get; set; }
    
    // Fields without attributes cannot be filtered/sorted (security)
    public string InternalNotes { get; set; } // Not filterable
}
```

### 3. API Request Examples

#### Basic Pagination
```http
GET /api/products?pageNumber=1&pageSize=20
```

#### With Sorting (Ascending)
```http
GET /api/products?pageNumber=1&pageSize=20&sort=name
```

#### With Sorting (Descending)
```http
GET /api/products?pageNumber=1&pageSize=20&sort=-createdAt
```

#### Multiple Sort Fields
```http
GET /api/products?pageNumber=1&pageSize=20&sort=status,-createdAt,name
```

#### With Filters (JSON Body)
```json
{
  "pageNumber": 1,
  "pageSize": 20,
  "sort": "name,-createdAt",
  "filters": [
    {
      "field": "status",
      "operator": "Equal",
      "value": "Active"
    },
    {
      "field": "name",
      "operator": "Contains",
      "value": "phone"
    },
    {
      "field": "createdAt",
      "operator": "GreaterThanOrEqual",
      "value": "2024-01-01"
    }
  ],
  "includeTotalCount": true
}
```

---

## Filter Operators

| Operator | Description | Example |
|----------|-------------|---------|
| `Equal` | Exact match | `status = "Active"` |
| `NotEqual` | Not equal | `status != "Deleted"` |
| `Contains` | Partial match (case-insensitive) | `name contains "phone"` |
| `StartsWith` | Starts with (case-insensitive) | `name starts with "Apple"` |
| `EndsWith` | Ends with (case-insensitive) | `name ends with "Pro"` |
| `GreaterThan` | Greater than | `price > 100` |
| `GreaterThanOrEqual` | Greater than or equal | `stock >= 10` |
| `LessThan` | Less than | `price < 500` |
| `LessThanOrEqual` | Less than or equal | `stock <= 100` |
| `In` | In array | `status in ["Active","Pending"]` |
| `NotIn` | Not in array | `status not in ["Deleted","Archived"]` |

---

## Response Structure

```json
{
  "success": true,
  "data": {
    "items": [
      {
        "id": "507f1f77bcf86cd799439011",
        "name": "Product 1",
        "status": "Active",
        "createdAt": "2024-01-15T10:30:00Z"
      }
    ],
    "totalCount": 1250,
    "pageNumber": 1,
    "pageSize": 20,
    "totalPages": 63,
    "hasNextPage": true,
    "hasPreviousPage": false
  }
}
```

---

## Security Features

### 1. Field Whitelisting
Only fields marked with `[Filterable]` or `[Sortable]` attributes can be used in queries.

```csharp
// ✅ Allowed - Field is marked with [Filterable]
filter.Field = "status";

// ❌ Blocked - Field not marked, throws InvalidOperationException
filter.Field = "internalNotes";
```

### 2. Type Safety
Values are automatically converted and validated to the property type.

```csharp
// ✅ Valid - Converts string to int
filter.Value = "100"; // for int property

// ❌ Invalid - Throws InvalidOperationException
filter.Value = "abc"; // for int property
```

### 3. Injection Prevention
Field names are validated using attributes, preventing NoSQL injection.

---

## Performance Optimization

### 1. Index Your Filter/Sort Fields

```javascript
// MongoDB
db.products.createIndex({ status: 1, createdAt: -1 });
db.products.createIndex({ name: 1 });
db.products.createIndex({ companyId: 1, status: 1 }); // For multi-tenant
```

### 2. Disable Total Count When Not Needed

```csharp
var request = new OffsetPageRequest
{
    PageNumber = 1,
    PageSize = 20,
    IncludeTotalCount = false // Faster when count is not needed
};
```

### 3. Limit Page Size

The system automatically limits page size to maximum 100 items to prevent performance issues.

### 4. Use Projection

Always use projection to select only needed fields:

```csharp
var projection = (ProductEntity e) => new ProductDto
{
    Id = e.Id.ToString(),
    Name = e.Name
    // Only select what you need
};
```

---

## Advanced Examples

### Example 1: Complex Filtering

```csharp
var request = new OffsetPageRequest
{
    PageNumber = 1,
    PageSize = 50,
    Sort = "name,-createdAt",
    Filters = new List<FieldFilter>
    {
        new() { Field = "status", Operator = FilterOperator.In, Value = "Active,Pending" },
        new() { Field = "createdAt", Operator = FilterOperator.GreaterThanOrEqual, Value = "2024-01-01" },
        new() { Field = "createdAt", Operator = FilterOperator.LessThan, Value = "2024-12-31" },
        new() { Field = "categoryId", Operator = FilterOperator.Equal, Value = "507f1f77bcf86cd799439011" },
        new() { Field = "name", Operator = FilterOperator.Contains, Value = "iPhone" }
    }
};
```

### Example 2: Custom Repository Method

```csharp
public class ProductRepo : BaseRepo<ProductEntity>, IProductRepo
{
    public async Task<OffsetPageResponse<ProductDto>> GetActiveProductsAsync(
        OffsetPageRequest request,
        CancellationToken cancellationToken = default)
    {
        // Add custom filter for active products only
        request.Filters ??= new List<FieldFilter>();
        request.Filters.Add(new FieldFilter
        {
            Field = "status",
            Operator = FilterOperator.Equal,
            Value = "Active"
        });
        
        request.Filters.Add(new FieldFilter
        {
            Field = "isDeleted",
            Operator = FilterOperator.Equal,
            Value = "false"
        });

        return await GetPagedAsync<ProductEntity, ProductDto>(
            request,
            entity => new ProductDto { /* mapping */ },
            cancellationToken);
    }
}
```

### Example 3: Controller Action

```csharp
[HttpPost("products/page")]
public async Task<ActionResult<BaseResponse<OffsetPageResponse<ProductDto>>>> GetProductsPage(
    [FromBody] OffsetPageRequest request,
    CancellationToken cancellationToken)
{
    var query = new GetProductsPageQuery { PageRequest = request };
    var result = await _mediator.Send(query, cancellationToken);
    
    return Ok(result);
}
```

---

## Validation

Use FluentValidation to validate requests:

```csharp
public class GetProductsPageQueryValidator : AbstractValidator<GetProductsPageQuery>
{
    public GetProductsPageQueryValidator()
    {
        RuleFor(x => x.PageRequest).NotNull();
        
        RuleFor(x => x.PageRequest.PageNumber)
            .GreaterThan(0)
            .When(x => x.PageRequest != null);

        RuleFor(x => x.PageRequest.PageSize)
            .InclusiveBetween(1, 100)
            .When(x => x.PageRequest != null);
    }
}
```

---

## Testing

### Unit Test Example

```csharp
[Fact]
public async Task GetPagedAsync_ReturnsPagedResults()
{
    // Arrange
    var request = new OffsetPageRequest
    {
        PageNumber = 1,
        PageSize = 10,
        Sort = "name",
        Filters = new List<FieldFilter>
        {
            new() { Field = "status", Operator = FilterOperator.Equal, Value = "Active" }
        }
    };

    // Act
    var result = await _productRepo.GetPagedAsync<ProductEntity, ProductDto>(
        request,
        entity => new ProductDto { Id = entity.Id.ToString() });

    // Assert
    Assert.NotNull(result);
    Assert.True(result.Items.Count <= 10);
    Assert.Equal(1, result.PageNumber);
}
```

---

## Migration from Existing Code

If you have existing repositories, update them to inject `IOffsetPaginator`:

```csharp
// Before
public class ProductRepo(
    IMongoDatabase database,
    ITenantService tenant)
    : BaseRepo<ProductEntity>(database, tenant), IProductRepo
{ }

// After
public class ProductRepo(
    IMongoDatabase database,
    ITenantService tenant,
    IOffsetPaginator paginator)
    : BaseRepo<ProductEntity>(database, tenant, paginator), IProductRepo
{ }
```

---

## Best Practices

1. **Always mark filterable/sortable fields** - Security first!
2. **Create MongoDB indexes** - For fields used in filters and sorting
3. **Use projection** - Select only needed fields
4. **Disable count when not needed** - Better performance
5. **Validate input** - Use FluentValidation
6. **Limit page size** - System enforces max 100
7. **Use pagination for large datasets** - Don't return all records
8. **Cache frequently accessed pages** - Consider Redis caching

---

## Troubleshooting

### Error: "Field 'xyz' is not filterable"

**Solution:** Add `[Filterable]` attribute to the property:
```csharp
[Filterable(alias: "xyz")]
public string Xyz { get; set; }
```

### Error: "Field 'xyz' is not sortable"

**Solution:** Add `[Sortable]` or `[Filterable]` attribute:
```csharp
[Sortable(alias: "xyz")]
public string Xyz { get; set; }
```

### Slow Performance

**Solutions:**
1. Create MongoDB indexes on filtered/sorted fields
2. Set `IncludeTotalCount = false` if count not needed
3. Reduce page size
4. Use projection to select fewer fields

---

## Comparison: Offset vs Cursor Pagination

| Feature | Offset Pagination | Cursor Pagination |
|---------|------------------|-------------------|
| **Use Case** | User browsing pages | Infinite scroll, APIs |
| **Jump to Page** | ✅ Yes | ❌ No |
| **Performance (Large Skip)** | ⚠️ Slower | ✅ Fast |
| **Data Consistency** | ⚠️ Can show duplicates | ✅ Consistent |
| **Implementation** | ✅ Simple | ⚠️ Complex |
| **Random Access** | ✅ Yes | ❌ No |

**Recommendation:**
- Use **Offset Pagination** for user-facing pages with page numbers
- Use **Cursor Pagination** (keyset) for infinite scroll or APIs

---

## Support & Contact

For questions or issues, refer to:
- Project documentation
- Team technical lead
- Architecture decision records (ADRs)

---

**Version:** 1.0  
**Last Updated:** 2024  
**Compatible With:** .NET 10, MongoDB 5.0+
