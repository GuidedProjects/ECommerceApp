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
- **ApiResponse<ConfirmationResponseDTO>**: when the API operation performs a write action (such as Create, Update, Delete) and you only need to confirm the result rather than return the full object data.
  - Common scenarios:
    - Creating a record but not returning full details.
    - Updating existing data.
    - Soft/hard deleting records.
    - Changing user credentials (e.g., password).
    - Performing other state-changing actions (e.g., activation/deactivation).
    
- **ApiResponse<T>**: when your API operation is intended to retrieve and return data to the client.
  - Common scenarios:
    - Fetching details of a single record by ID.
    - Returning a newly created object after a successful POST.
    - Returning a list of data (use ApiResponse<List<T>>).
    - Any read-only operation where the client needs the full or partial entity data.