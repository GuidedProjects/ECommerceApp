# Models
- **Customer**: Represents users who can place orders, including authentication details. 
- **Address**: Stores multiple addresses per customer for billing and shipping purposes. 
- **Category**: Manages product categories. 
- **Product**: Represents items available for purchase, linked to categories and supporting discounts. 
- **Cart**: Represents a shopping cart associated with a customer. 
- **CartItems**: Represents an individual product entry within a shopping cart. 
- **Order**: Represents a customer’s order with detailed financial tracking and status. 
- **OrderItem**: Represents individual items within an order, including discounts. 
- **Status**: Represents the different statuses that can be used with order and payment. 
- **Payment**: Represents the Payment associated with a specific order.
- **Cancellation**: This entity captures the details of a cancellation request made by a customer.
- **Refund**: This entity manages the refund details associated with a cancellation or return.
- **Feedback**: This entity stores the feedback provided by a user on the purchase of a product.

# RESTful API + C# Clean Architecture
## `ApiResponse<ConfirmationResponseDTO>`
- When the API operation performs a write action (such as Create, Update, Delete) and you only need to confirm the result rather than return the full object data.
  - Common scenarios:
    - Creating a record but not returning full details.
    - Updating existing data.
    - Soft/hard deleting records.
    - Changing user credentials (e.g., password).
    - Performing other state-changing actions (e.g., activation/deactivation).
    
## `ApiResponse<T>`
- When your API operation is intended to retrieve and return data to the client.
  - Common scenarios:
    - Fetching details of a single record by ID.
    - Returning a newly created object after a successful POST.
    - Returning a list of data (use ApiResponse<List<T>>).
    - Any read-only operation where the client needs the full or partial entity data.

# Mapping Between Entities and DTOs in ASP.NET Core + EF Core

## 1. Entity vs. DTO

- **Entity (Model)**
  - Maps directly to a database table.
  - Contains all columns (including internal/sensitive fields and navigation properties).
  - Annotated with `[Required]`, `[StringLength]`, `[ForeignKey]`, etc.
  - Example fields: `Id`, `FirstName`, `Password`, `IsActive`, navigation collections.

- **DTO (Data Transfer Object)**
  - Plain class used for communication between client ↔ server.
  - Contains only the fields needed by the client (no sensitive/internal data).
  - Separate into Request DTOs (for incoming data) and Response DTOs (for outgoing data).
  - Example: `CustomerRegistrationDTO` (FirstName, Email, Password) vs. `CustomerResponseDTO` (Id, FirstName, Email).

## 2. When to Map DTO → Entity

- **Create (POST)**
  - Client sends a Request DTO (e.g. `CustomerRegistrationDTO`).
  - Service hashes password or sets defaults.
  - Map DTO fields onto a new Entity instance (`new Customer { … }`).
  - Add to DbContext and `SaveChangesAsync()`.

- **Update (PUT/PATCH)**
  - Client sends an Update DTO (e.g. `CustomerUpdateDTO`).
  - Service loads the existing Entity with `FindAsync(id)` (tracking enabled).
  - Map updated fields from DTO onto that Entity.
  - Call `SaveChangesAsync()` to generate `UPDATE`.

- **Other write operations**
  - Any time client input must become part of the database (e.g. changing password, creating an order, adding CartItem).

## 3. When to Map Entity → DTO

- **Read (GET)**
  - Service queries with `AsNoTracking()` (read-only).
  - Converts the retrieved Entity (plus included relations) into a Response DTO (e.g. `CustomerResponseDTO`, `AddressResponseDTO`).
  - Returns only the necessary fields (no Password, no IsActive, etc.).

- **Return value after Create/Update**
  - After saving a new Entity (with generated Id), map it to a Response DTO so the client knows the new record’s details.

## 4. Why Map

1. **Security**
  - Do not expose sensitive or internal fields (e.g. `Password`, `IsActive`, navigation collections) to the client.

2. **Separation of Concerns**
  - Entities represent the database schema.
  - DTOs define the API contract (request/response shapes).

3. **Payload Optimization**
  - Sending a smaller, targeted DTO minimizes data transferred over the network.

4. **Maintainability**
  - DB schema changes do not necessarily break the API contract if mapping is centralized.
  - You can add or reshape DTOs without modifying Entities.

5. **Custom Transformations**
  - In mapping, you can combine fields (e.g. `FullName = FirstName + " " + LastName`) or format data (e.g. date formatting) before sending to the client.

## 5. Summary

- **DTO → Entity**: perform whenever you need to save or update data in the database (POST, PUT, PATCH).
- **Entity → DTO**: perform whenever you return data to the client (GET or after a successful write).
- Always keep Entities focused on database structure and DTOs focused on client-facing data.
