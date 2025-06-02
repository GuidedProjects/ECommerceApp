using ECommerceApp.Data;
using ECommerceApp.DTOs;
using ECommerceApp.DTOs.ShoppingCartDTOs;
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
                var cart = _context.Carts
                    .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                    .FirstOrDefaultAsync(c => c.CustomerId == id && !c.IsCheckedOut);
                if (cart == null)
                {

                }
            }
            catch(Exception ex)
            {

            }
        }
    }
}
