# Common Keywords and Methods 
- **AsNoTracking()**: Disables tracking for read-only queries to improve performance.
- **Include()**: Loads related data (navigation properties) along with the main entity.
- **FindAsync()**: Finds an entity by its primary key asynchronously. Uses tracking and internal cache.
- **FirstOrDefaultAsync()**: Returns the first element that matches the condition, or null if none is found.
- **ToListAsync()**: Converts the result into a List<T> asynchronously.
- **Select()**: Transforms and selects specific properties. Commonly used to project into DTOs.
- **Where()**: Filters data based on given conditions.
- **AnyAsync()**: Returns true if any element satisfies the condition. Useful for checking duplicates.

