# Modules

## Customer and Address Management Module
The Customer and Address Management Module is foundational to the E-Commerce application, handling all aspects related to customer data and their associated addresses. This module ensures that customer information is securely managed, easily accessible, and efficiently utilized across various functionalities like order processing and profile management.

- Registration: Enables new customers to create accounts by providing validated personal information. Ensures data integrity through robust validation mechanisms.
- Authentication: Implements secure login procedures using hashed passwords via BCrypt. Protects user credentials and maintains session security.
- Profile Management: Allows customers to retrieve, update, and delete their profiles. Facilitates easy management of personal information and preferences.
- Multiple Addresses: Supports the addition of multiple billing and shipping addresses per customer. Enhances flexibility in order deliveries and billing processes.
- Data Integrity and Security: Uses data annotations and validation rules to maintain accurate and secure customer data. Implements indexing (e.g., unique email addresses) to prevent duplicate entries and ensure quick data retrieval.

## Product and Category Management Module
The Product and Category Management Module organizes the merchandise available in the E-Commerce platform, ensuring that products are systematically categorized for optimal navigation and management. This module plays a crucial role in inventory management, product visibility, and sales optimization.

### Category Management
The Category Module organizes products into distinct groups for better navigation and management.
- **CRUD Operations**: Create, read, update, and delete product categories. Facilitates the organization of products into logical groups.
- **Association with Products**: Each category can encompass multiple products. Enhances product discoverability and categorization.

### Product Management
The Product Module represents items available for purchase, linked to categories, and supports discounts.
- **CRUD Operations**: Create, read, update, and delete products. Manages the lifecycle of products within the inventory.
- **Discounts**: Apply percentage-based discounts to products. Drives sales promotions and marketing strategies.
- **Stock Management**: Track and manage product stock quantities. Prevents overselling and ensures inventory accuracy.
- **Image Handling**: Store and validate product image URLs. Enhances the visual appeal and user experience on the platform.

## Shopping Cart Module
The Shopping Cart Module serves as the intermediary between product browsing and order placement. It allows customers to add products they intend to purchase, review their selections, adjust quantities, and remove items before finalizing their orders. This module not only enhances user convenience but also plays a crucial role in inventory management and sales optimization.

### Key Features
- **Add to Cart**: Allows customers to add products to their shopping cart from the product listing or detail pages. Validates product availability and stock levels before addition.
- **View Cart**: Provides a detailed view of all items currently in the cart, including product details, quantities, individual prices, and total amounts.
- **Update Cart Items**: Enables customers to modify the quantity of each item in their cart. Reflects real-time stock availability to prevent overselling.
- **Remove from Cart**: Allows customers to remove individual items or clear the entire cart.
- **Persisting Cart Data**: Associates carts with authenticated customers to retain cart contents across sessions and devices. Implements session-based carts for guest users, with options to merge upon account creation.

## Order Management Module
The Order Management Module oversees the entire lifecycle of customer orders, from placement to delivery. This module ensures that orders are processed efficiently, statuses are accurately tracked, and financial details are properly managed.

### Key Features
- **Order Creation**: Allows customers to place new orders containing multiple items. Automatically calculates totals, applies discounts, and adds shipping costs.
- **Status Tracking**: Manages various order statuses such as Pending, Processing, Shipped, Delivered, and Canceled. Controls status transitions to reflect the current state of each order accurately.
- **Financial Tracking**: Provides detailed calculations of base amounts, discounts, shipping costs, and total amounts. Ensures transparency and accuracy in order billing.
- **Order Item Management**: Tracks each product’s quantity, unit price, discount, and total price within an order. Automatically updates product stock based on order quantities to maintain inventory accuracy.

## Payment Management Module
The Payment Management Module facilitates the secure and efficient processing of customer payments. This module integrates with payment gateways to handle transactions, record payment details, and update order statuses based on payment outcomes.

### Key Features
- **Payment Processing**: Handles payment transactions through integrated payment gateways (e.g., Stripe, PayPal, etc.) or Cash on Delivery (COD). Ensures secure transmission and storage of payment information.
- **Payment Records**: Stores detailed payment information associated with each order. Maintains a clear audit trail for financial transactions.
- **Integration with Orders**: Links payments to specific orders. Updates order statuses based on the success or failure of payment transactions.
- **Transaction Management**: Manages transaction IDs and statuses to reflect real-time payment states. Facilitates refund processes by maintaining accurate payment references.

## Cancellation Module
The Cancellation Module allows customers to request the cancellation of their orders before they are shipped. This module ensures that cancellations are handled efficiently, maintaining data integrity and providing a seamless user experience.

### Key Features
- **Cancellation Requests**: Customers can request to cancel orders that are in eligible statuses (e.g., Pending, Processing). Validates cancellation eligibility based on order status.
- **Status Management**: Automatically updates the order status to “Canceled” upon successful cancellation. Maintains accurate tracking of order states.
- **Stock Restoration**: Restores the stock quantities of products in the canceled order. Prevents stock discrepancies and ensures inventory accuracy.
- **Notification**: Informs relevant stakeholders (e.g., customers, administrators) about the cancellation. Enhances communication and transparency within the system.

## Refund Module
The Refund Module manages the process of returning funds to customers for canceled orders or returned products. It ensures accurate financial tracking and seamless integration with payment gateways to facilitate smooth refund operations.

### Key Features
- **Refund Requests**: Handles refund requests associated with canceled orders or returned items. Validates refund eligibility based on order and payment statuses.
- **Payment Integration**: Processes refunds through existing payment gateway integrations (e.g., Stripe, PayPal). Ensures secure and efficient refund transactions.
- **Financial Tracking**: Accurately records refund amounts and statuses. Maintains a clear audit trail for all refund operations.
- **Notification**: Notifies customers about the refund status and completion. Provides transparency and updates to enhance customer satisfaction.

## User Feedback Module
The Feedback Module is a crucial component of the E-Commerce application, designed to capture and manage customer reviews and ratings for products they have purchased. This module not only enhances customer engagement but also provides valuable insights into product performance, helping both customers and administrators make informed decisions.

### Key Features
- **Feedback Submission**: Allows authenticated customers to submit ratings and reviews for products they have purchased. Customers can rate products on a scale (e.g., 1 to 5 stars), providing quantitative feedback.
- **Feedback Validation**: Ensures that only legitimate customers who have purchased a product can leave feedback. Prevents customers from submitting multiple feedback entries for the same product and order item.
- **Update and Delete Feedback**: Allows customers to modify their existing feedback, enabling corrections or updates based on further product usage. Enables customers to remove their feedback entries if desired.
- **Average Rating Calculation**: Computes the average rating for each product, providing a quick overview of customer satisfaction.
- **Detailed Feedback Listings**: Displays individual feedback entries, including customer names, ratings, comments, and submission dates.
