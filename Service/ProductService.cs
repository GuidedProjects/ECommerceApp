using ECommerceApp.Data;
using ECommerceApp.DTOs;
using ECommerceApp.DTOs.ProductDTOs;
using ECommerceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Service;

public class ProductService
{
    private readonly ApplicationDbContext _context;

    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<ProductResponseDTO>> CreateProductAsync(ProductCreateDTO productDto)
    {
        try
        {
            if (await _context.Products.AnyAsync(p => p.Name.ToLower() == productDto.Name.ToLower()))
            {
                return new ApiResponse<ProductResponseDTO>(400, "Product name already exists");
            }

            if (!await _context.Categories.AnyAsync(cat => cat.Id == productDto.CategoryId))
            {
                return new ApiResponse<ProductResponseDTO>(400, "Category does not exist");
            }

            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                CategoryId = productDto.CategoryId,
                ImageUrl = productDto.ImageUrl,
                StockQuantity = productDto.StockQuantity,
                DiscountPercentage = productDto.DiscountPercentage,
                IsAvailable = true
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            var productResponse = new ProductResponseDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                ImageUrl = product.ImageUrl,
                StockQuantity = product.StockQuantity,
                DiscountPercentage = product.DiscountPercentage,
                IsAvailable = product.IsAvailable
            };
            return new ApiResponse<ProductResponseDTO>(200, productResponse);
        }
        catch (Exception ex)
        {
            return new ApiResponse<ProductResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }

    public async Task<ApiResponse<ProductResponseDTO>> GetProductByIdAsync(int id)
    {
        try
        {
            var product = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return new ApiResponse<ProductResponseDTO>(404, "Product not found");
            }

            var productResponseDto = new ProductResponseDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                ImageUrl = product.ImageUrl,
                StockQuantity = product.StockQuantity,
                DiscountPercentage = product.DiscountPercentage,
                IsAvailable = product.IsAvailable
            };
            return new ApiResponse<ProductResponseDTO>(200, productResponseDto);
        }
        catch (Exception ex)
        {
            return new ApiResponse<ProductResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }

    public async Task<ApiResponse<ConfirmationResponseDTO>> UpdateProductAsync(ProductUpdateDTO productDto)
    {
        try
        {
            var product = await _context.Products.FindAsync(productDto.Id);
            if (product == null)
            {
                return new ApiResponse<ConfirmationResponseDTO>(404, "Product not found");
            }

            if (await _context.Products.AnyAsync(p => p.Name.ToLower() == productDto.Name.ToLower()))
            {
                return new ApiResponse<ConfirmationResponseDTO>(400, "Another Product with the same name already exists");
            }

            if (!await _context.Categories.AnyAsync(cat => cat.Id == productDto.CategoryId))
            {
                return new ApiResponse<ConfirmationResponseDTO>(400, "Category does not exist");
            }
            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Price = productDto.Price;
            product.CategoryId = productDto.CategoryId;
            product.ImageUrl = productDto.ImageUrl;
            product.StockQuantity = productDto.StockQuantity;
            product.DiscountPercentage = productDto.DiscountPercentage;
            await _context.SaveChangesAsync();
            var confirmationMessage = new ConfirmationResponseDTO
            {
                Message = $"Product {productDto.Name} was successfully updated"
            };
            return new ApiResponse<ConfirmationResponseDTO>(200, confirmationMessage);
        }
        catch (Exception ex)
        {
            return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }

    public async Task<ApiResponse<ConfirmationResponseDTO>> DeleteProductAsync(int id)
    {
        try
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return new ApiResponse<ConfirmationResponseDTO>(404, "Product not found");
            }
            product.IsAvailable = false;
            await _context.SaveChangesAsync();
            var confirmationMessage = new ConfirmationResponseDTO
            {
                Message = $"Product {product.Name} was successfully deleted"
            };
            return new ApiResponse<ConfirmationResponseDTO>(200, confirmationMessage);
        }
        catch (Exception ex)
        {
            return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }

    public async Task<ApiResponse<List<ProductResponseDTO>>> GetAllProductsAsync()
    {
        try
        {
            var product = await _context.Products.AsNoTracking().ToListAsync();
            var productList = product.Select(p => new ProductResponseDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                CategoryId = p.CategoryId,
                ImageUrl = p.ImageUrl,
                StockQuantity = p.StockQuantity,
                DiscountPercentage = p.DiscountPercentage,
                IsAvailable = p.IsAvailable
            }).ToList();
            return new ApiResponse<List<ProductResponseDTO>>(200, productList);
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<ProductResponseDTO>>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }

    public async Task<ApiResponse<List<ProductResponseDTO>>> GetAllProductsByCategoryAsync(int categoryId)
    {
        try
        {
            var product = await _context.Products.AsNoTracking().Include(p => p.Category
            ).Where(p => p.CategoryId == categoryId && p.IsAvailable).ToListAsync();
            if (product == null || product.Count == 0)
            {
                return new ApiResponse<List<ProductResponseDTO>>(404, "Product not found");
            }

            var productList = product.Select(p => new ProductResponseDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                CategoryId = p.CategoryId,
                ImageUrl = p.ImageUrl,
                StockQuantity = p.StockQuantity,
                DiscountPercentage = p.DiscountPercentage,
                IsAvailable = p.IsAvailable
            }).ToList();
            return new ApiResponse<List<ProductResponseDTO>>(200, productList);
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<ProductResponseDTO>>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }

    public async Task<ApiResponse<ConfirmationResponseDTO>> UpdateProductStatusAsync(
        ProductStatusUpdateDTO productStatusUpdateDto)
    {
        try
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productStatusUpdateDto.ProductId);
            if (product == null)
            {
                return new ApiResponse<ConfirmationResponseDTO>(404, "Product not found");
            }
            product.IsAvailable = productStatusUpdateDto.IsAvailable;
            await _context.SaveChangesAsync();
            var confirmationMessage = new ConfirmationResponseDTO
            {
                Message = $"Product with id {product.Id} was successfully status updated"
            };
            return new ApiResponse<ConfirmationResponseDTO>(200, confirmationMessage);
        }
        catch (Exception ex)
        {
            return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }
}