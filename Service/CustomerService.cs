using Microsoft.EntityFrameworkCore;
using ECommerceApp.Data;
using ECommerceApp.Models;
using ECommerceApp.DTOs;
using ECommerceApp.DTOs.CustomerDTO;
using ECommerceApp.DTOs.CustomerDTOs;

namespace ECommerceApp.Service;

public class CustomerService
{
    private readonly ApplicationDbContext _context;

    public CustomerService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<CustomerResponseDTO>> 
        RegisterCustomerAsync(CustomerRegistrationDTO customerDto)
    {
        try
        {
            if (await _context.Customers.AnyAsync(c => c.Email.ToLower() == customerDto.Email.ToLower()))
            {
                return new ApiResponse<CustomerResponseDTO>(400, "Email already exists");
            }

            var customer = new Customer
            {
                FirstName = customerDto.FirstName,
                LastName = customerDto.LastName,
                Email = customerDto.Email,
                PhoneNumber = customerDto.PhoneNumber,
                DateOfBirth = customerDto.DateOfBirth,
                IsActive = true,
                // Hash the password using BCrypt
                Password = BCrypt.Net.BCrypt.HashPassword(customerDto.Password)
            };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            var customerResponse = new CustomerResponseDTO
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                DateOfBirth = customer.DateOfBirth
            };
            return new ApiResponse<CustomerResponseDTO>(200, customerResponse);
        }
        catch (Exception ex)
        {
            // Log the exception (implementation depends on your logging setup)
            return new ApiResponse<CustomerResponseDTO>(500, 
                $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }

    public async Task<ApiResponse<LoginResponseDTO>> LoginAsync(LoginDTO loginDto)
    {
        try
        {
            var customer = await _context.Customers.AsNoTracking().FirstOrDefaultAsync(
                c => c.Email == loginDto.Email);
            if (customer == null) return new ApiResponse<LoginResponseDTO>(401, "Invalid email");
            // Verify password using BCrypt
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, customer.Password);
            if (!isPasswordValid) return new ApiResponse<LoginResponseDTO>(400, "Invalid password");
            // Prepare response data
            var loginResponse = new LoginResponseDTO
            {
                Message = "Login Successful",
                CustomerId = customer.Id,
                CustomerName = $"{customer.FirstName} {customer.LastName}"
            };
            return new ApiResponse<LoginResponseDTO>(200, loginResponse);
        }
        catch (Exception ex)
        {
            // Log the exception
            return new ApiResponse<LoginResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }

    public async Task<ApiResponse<CustomerResponseDTO>> GetCustomerByIdAsync(int customerId)
    {
        try
        {
            var customer = await _context.Customers.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == customerId && c.IsActive == true);
            if (customer == null) return new ApiResponse<CustomerResponseDTO>(404, "Customer not found");
            var customerResponse = new CustomerResponseDTO
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                DateOfBirth = customer.DateOfBirth
            };
            return new ApiResponse<CustomerResponseDTO>(200, customerResponse);
        }
        catch (Exception ex)
        {
            return new ApiResponse<CustomerResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }

    public async Task<ApiResponse<ConfirmationResponseDTO>> UpdateCustomerAsync(CustomerUpdateDTO customerDto)
    {
        try
        {
            var customer = await _context.Customers.FindAsync(customerDto.CustomerId);
            if (customer == null) return new ApiResponse<ConfirmationResponseDTO>(404, "Customer not found");
            // Check if email is being updated to an existing one
            if (customer.Email != customerDto.Email &&
                await _context.Customers.AnyAsync(c => c.Email == customerDto.Email))
            {
                return new ApiResponse<ConfirmationResponseDTO>(400, "Email already in use");
            }

            customer.FirstName = customerDto.FirstName;
            customer.LastName = customerDto.LastName;
            customer.Email = customerDto.Email;
            customer.PhoneNumber = customerDto.PhoneNumber;
            customer.DateOfBirth = customerDto.DateOfBirth;
            await _context.SaveChangesAsync();
            var confirmationResponse = new ConfirmationResponseDTO
            {
                Message = $"Customer with id {customer.Id} has been updated."
            };
            return new ApiResponse<ConfirmationResponseDTO>(200, confirmationResponse);
        }
        catch (Exception ex)
        {
            return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }

    public async Task<ApiResponse<ConfirmationResponseDTO>> DeleteCustomerAsync(int customerId)
    {
        try
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == customerId);
            if (customer == null) return new ApiResponse<ConfirmationResponseDTO>(404, "Customer not found");
            // soft delete
            customer.IsActive = false;
            await _context.SaveChangesAsync();
            var confirmationResponse = new ConfirmationResponseDTO
            {
                Message = $"Customer with id {customerId} has been deleted."
            };
            return new ApiResponse<ConfirmationResponseDTO>(200, confirmationResponse);
        }
        catch (Exception ex)
        {
            return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }

    public async Task<ApiResponse<ConfirmationResponseDTO>> ChangePasswordAsync(ChangePasswordDTO changePasswordDto)
    {
        try
        {
            var customer = await _context.Customers.FindAsync(changePasswordDto.CustomerId);
            if (customer == null || !customer.IsActive)
                return new ApiResponse<ConfirmationResponseDTO>(404, "Customer not found or inactive");
            bool isCurrentPasswordValid =
                BCrypt.Net.BCrypt.Verify(changePasswordDto.CurrentPassword, customer.Password);
            if (!isCurrentPasswordValid)
                return new ApiResponse<ConfirmationResponseDTO>(400, "Current password is invalid");
            // hash the new password
            customer.Password = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);
            await _context.SaveChangesAsync();
            var confirmationRespones = new ConfirmationResponseDTO
            {
                Message = $"Password changed successfully."
            };
            return new ApiResponse<ConfirmationResponseDTO>(200, confirmationRespones);
        }
        catch (Exception ex)
        {
            return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }
    
}



















