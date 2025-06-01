# Common Keywords and Methods 

## 1. AsNoTracking()
- **Purpose**: Disable EF Core’s change tracking for entities returned by a query.
- **When to use**:
    - You only need to **read** data (no insert/update/delete).
    - You want better performance and lower memory usage when loading large result sets.
- **Key effect**:
    - The returned entities are **not** tracked in the `DbContext`’s change tracker.
    - Subsequent calls like `SaveChanges()` will ignore these entities (cannot update without re‐attaching).
### Example
```csharp
var product = await _context.Products
    .AsNoTracking()                       // Disable tracking
    .FirstOrDefaultAsync(p => p.Id == id);

if (product != null)
{
    // product is read-only; EF won’t watch for changes
    Console.WriteLine(product.Name);
}
``````
## 2. Include()
- **Purpose**: Eagerly load related navigation properties (e.g., parent/child tables) in a single query.
- **When to use:**:
  - You plan to read data from one entity and also need data from its related entity (e.g., load Category when querying Product).
  - You want to avoid separate (lazy) queries or the “N+1” problem.
- **Key effect**:
  - EF Core generates a SQL JOIN under the hood to fetch both tables together. 
### Example
```csharp
// Load all products in a category, including Category details:
var products = await _context.Products
    .AsNoTracking()
    .Include(p => p.Category)             // Eagerly load the Category navigation
    .Where(p => p.CategoryId == someCategoryId)
    .ToListAsync();

foreach (var p in products)
{
    // Now p.Category is already populated (no extra query needed)
    Console.WriteLine($"{p.Name} belongs to {p.Category.Name}");
}

``````
## 3. FindAsync()
- **Purpose**: Asynchronously retrieves an entity by its primary key.
- **When to use**:
    - You know the primary key value (e.g., id) and want to load that exact entity.
    - You plan to modify or delete this entity (tracking is needed).
- **Key behavior**:
    - Checks the DbContext’s local cache first (if it’s already tracked).
    - If not found in cache, queries the database.
    - Always returns a tracked entity (so you can update fields and save).



## 4. FirstOrDefaultAsync()

- **ToListAsync()**: Converts the result into a List<T> asynchronously.
- **Select()**: Transforms and selects specific properties. Commonly used to project into DTOs.
- **Where()**: Filters data based on given conditions.
- **AnyAsync()**: Returns true if any element satisfies the condition. Useful for checking duplicates.


