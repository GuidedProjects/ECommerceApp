using ECommerceApp.Data;
using ECommerceApp.DTOs;
using ECommerceApp.DTOs.CategoryDTOs;
using ECommerceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Service;

public class CategoryService
{
    private readonly ApplicationDbContext _context;

    public CategoryService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<ApiResponse<CategoryResponseDTO>> CreateCategory(CategoryCreateDTO categoryDto)
    {
        try
        {
            if (await _context.Categories.AnyAsync(c => c.Name.ToLower() == categoryDto.Name.ToLower()))
            {
                return new ApiResponse<CategoryResponseDTO>(400, $"Category with Name: {categoryDto.Name} does not exist");
            }

            var category = new Category
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description,
                IsActive = true
            };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            var categoryResponse = new CategoryResponseDTO
            {
                Id = category.Id,
                Name = category.Name,
                Description = categoryDto.Description,
                IsActive = category.IsActive
            };
            return new ApiResponse<CategoryResponseDTO>(200, categoryResponse);
        }
        catch (Exception ex)
        {
            return new ApiResponse<CategoryResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }

    public async Task<ApiResponse<CategoryResponseDTO>> GetCategoryByIdAsync(int id)
    {
        try
        {
            var category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if(category == null) return new ApiResponse<CategoryResponseDTO>(404, $"Category with Id: {id} does not exist");
            var categoryResponse = new CategoryResponseDTO
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                IsActive = category.IsActive
            };
            return new ApiResponse<CategoryResponseDTO>(200, categoryResponse);
        }
        catch (Exception ex)
        {
            return new ApiResponse<CategoryResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }

    public async Task<ApiResponse<ConfirmationResponseDTO>> UpdateCategoryAsync(CategoryUpdateDTO categoryDto)
    {
        try
        {
            var category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == categoryDto.Id);
            if(category == null) return new ApiResponse<ConfirmationResponseDTO>(404, $"Category with Id: {categoryDto.Id} does not exist");
            if (await _context.Categories.AnyAsync(c => c.Name.ToLower() == categoryDto.Name.ToLower()))
            {
                return new ApiResponse<ConfirmationResponseDTO>(400, $"Category with Name: {categoryDto.Name} already exists");
            }
            category.Name = categoryDto.Name;
            category.Description = categoryDto.Description;
            await _context.SaveChangesAsync();
            var confirmationResponse = new ConfirmationResponseDTO
            {
                Message = $"Category with Id {categoryDto.Id} updated successfully."
            };
            return new ApiResponse<ConfirmationResponseDTO>(200, confirmationResponse);
        }
        catch (Exception ex)
        {
            return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }

    public async Task<ApiResponse<ConfirmationResponseDTO>> DeleteCategoryAsync(int id)
    {
        try
        {
            var category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if(category == null) return new ApiResponse<ConfirmationResponseDTO>(404, $"Category with Id: {id} does not exist");
            category.IsActive = false;
            await _context.SaveChangesAsync();
            var confirmationResponse = new ConfirmationResponseDTO
            {
                Message = $"Category with Id {id} deleted successfully."
            };
            return new ApiResponse<ConfirmationResponseDTO>(200, confirmationResponse);
        }
        catch (Exception ex)
        {
            return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }

    public async Task<ApiResponse<List<CategoryResponseDTO>>> GetAllCategories()
    {
        try
        {
            var categories = await _context.Categories.AsNoTracking().ToListAsync();
            var categoriesList = categories.Select(c => new CategoryResponseDTO
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                IsActive = c.IsActive
            }).ToList();
            return new ApiResponse<List<CategoryResponseDTO>>(200, categoriesList);
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<CategoryResponseDTO>>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }
}