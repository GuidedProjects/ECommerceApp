using ECommerceApp.Data;
using ECommerceApp.DTOs;
using ECommerceApp.DTOs.ShoppingCartDTOs;
using ECommerceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Service
{
    public class ShoppingCartService
    {
        private readonly ApplicationDbContext _context;
        public ShoppingCartService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResponse<CartResponseDTO>> GetCartByCustomerIdAsync(int id)
        {
            try
            {
                var cart = await _context.Carts
                    .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                    .FirstOrDefaultAsync(c => c.CustomerId == id && !c.IsCheckedOut);
                if (cart == null)
                {
                    var emptyCartDto = new CartResponseDTO()
                    {
                        CustomerId = id,
                        IsCheckedOut = false,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        CartItems = new List<CartItemResponseDTO>(),
                        TotalBasePrice = 0,
                        TotalDiscount = 0,
                        TotalAmount = 0
                    };
                    // Return the empty cart wrapped in an ApiResponse with status code 200 (OK)
                    return new ApiResponse<CartResponseDTO>(200, emptyCartDto);
                }
                var cartDto = MapCartToDTO(cart);
                return new ApiResponse<CartResponseDTO>(200, cartDto);
            }
            catch(Exception ex)
            {
                return new ApiResponse<CartResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }


        private CartResponseDTO MapCartToDTO(Cart cart)
        {
            var carItemDto = cart.CartItems?
                .Select(ci => new CartItemResponseDTO()
                {
                    Id = ci.Id,
                    ProductId = ci.ProductId,
                    ProductName = ci.Product?.Name,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.UnitPrice,
                    Discount = ci.Discount,
                    TotalPrice = ci.TotalPrice,
                }).ToList() ?? new List<CartItemResponseDTO>();
            decimal totalBasePrice = 0;
            decimal totalDiscount = 0;
            decimal totalAmount = 0;
            foreach (var item in carItemDto)
            {
                totalBasePrice += item.UnitPrice;
                totalDiscount += item.Discount * item.Quantity;
                totalAmount += item.TotalPrice;
            }

            return new CartResponseDTO
            {
                Id = cart.Id,
                CustomerId = cart.CustomerId,
                IsCheckedOut = cart.IsCheckedOut,
                CreatedAt = cart.CreatedAt,
                UpdatedAt = cart.UpdatedAt,
                CartItems = carItemDto,
                TotalBasePrice = totalBasePrice,
                TotalDiscount = totalDiscount,
                TotalAmount = totalAmount
            };
        } 
    }
}
