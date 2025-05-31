using Microsoft.EntityFrameworkCore;
using ECommerceApp.Data;
using ECommerceApp.Models;
using ECommerceApp.DTOs;
using ECommerceApp.DTOs.AddressDTO;
using ECommerceApp.DTOs.AddressesDTO;
namespace ECommerceApp.Service
{
    public class AddressService
    {
        private readonly ApplicationDbContext _context;

        public AddressService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<AddressResponseDTO>> CreateAddressAsync(AddressCreateDTO addressDto)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(addressDto.CustomerId);
                if (customer == null)
                {
                    return new ApiResponse<AddressResponseDTO>(404, "Customer not found");
                }

                var address = new Address
                {
                    CustomerId = addressDto.CustomerId,
                    AddressLine1 = addressDto.AddressLine1,
                    AddressLine2 = addressDto.AddressLine2,
                    City = addressDto.City,
                    State = addressDto.State,
                    PostalCode = addressDto.PostalCode,
                    Country = addressDto.Country
                };
                _context.Addresses.Add(address);
                await _context.SaveChangesAsync();
                var addressResponse = new AddressResponseDTO
                {
                    Id = address.Id,
                    CustomerId = address.CustomerId,
                    AddressLine1 = address.AddressLine1,
                    AddressLine2 = address.AddressLine2,
                    City = address.City,
                    State = address.State,
                    PostalCode = address.PostalCode,
                    Country = address.Country
                };
                return new ApiResponse<AddressResponseDTO>(201, addressResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse<AddressResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<AddressResponseDTO>> GetAddressByIdAsync(int id)
        {
            try
            {
                var address = await _context.Addresses.AsNoTracking().FirstOrDefaultAsync(add => add.Id == id);
                if (address == null)
                {
                    return new ApiResponse<AddressResponseDTO>(404, "Address not found");
                }

                var addressResponse = new AddressResponseDTO
                {
                    Id = address.Id,
                    CustomerId = address.CustomerId,
                    AddressLine1 = address.AddressLine1,
                    AddressLine2 = address.AddressLine2,
                    City = address.City,
                    State = address.State,
                    PostalCode = address.PostalCode,
                    Country = address.Country
                };
                return new ApiResponse<AddressResponseDTO>(200, addressResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse<AddressResponseDTO>(500, ex.Message);
            }
        }

        public async Task<ApiResponse<ConfirmationResponseDTO>> UpdateAddressAsync(AddressUpdateDTO addressDto)
        {
            try
            {
                var address = await _context.Addresses.FirstOrDefaultAsync(
                    add => add.Id == addressDto.AddressId && add.CustomerId == addressDto.CustomerId);
                if (address == null)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Address not found");
                }

                address.AddressLine1 = addressDto.AddressLine1;
                address.AddressLine2 = addressDto.AddressLine2;
                address.City = addressDto.City;
                address.State = addressDto.State;
                address.PostalCode = addressDto.PostalCode;
                address.Country = addressDto.Country;
                await _context.SaveChangesAsync();
                var confirmationMessage = new ConfirmationResponseDTO
                {
                    Message = $"Address with Id {addressDto.AddressId} updated successfully."
                };
                return new ApiResponse<ConfirmationResponseDTO>(200, confirmationMessage);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ConfirmationResponseDTO>(500, ex.Message);
            }
        }

        public async Task<ApiResponse<ConfirmationResponseDTO>> DeleteAddressAsync(AddressDeleteDTO addressDto)
        {
            try
            {
                var address = await _context.Addresses.FirstOrDefaultAsync(add => add.Id == addressDto.AddressId);
                if (address == null)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Address not found");
                }

                _context.Addresses.Remove(address);
                await _context.SaveChangesAsync();
                var confirmationMessage = new ConfirmationResponseDTO
                {
                    Message = $"Address with Id {addressDto.AddressId} deleted successfully."
                };
                return new ApiResponse<ConfirmationResponseDTO>(200, confirmationMessage);
            }
            catch (Exception e)
            {
                return new ApiResponse<ConfirmationResponseDTO>(500, "An unexpected error occurred while processing your request.");
            }    
        }

        public async Task<ApiResponse<List<AddressResponseDTO>>> GetAddressesByCustomerAsync(int customerId)
        {
            try
            {
                var customer = await _context.Customers
                    .AsNoTracking()
                    .Include(c => c.Addresses)
                    .FirstOrDefaultAsync(c => c.Id == customerId);
                if (customer == null)
                {
                    return new ApiResponse<List<AddressResponseDTO>>(404, "Customer not found.");
                }

                var address = customer.Addresses.Select(a => new AddressResponseDTO
                {
                    Id = a.Id,
                    CustomerId = a.CustomerId,
                    AddressLine1 = a.AddressLine1,
                    AddressLine2 = a.AddressLine2,
                    City = a.City,
                    State = a.State,
                    PostalCode = a.PostalCode,
                    Country = a.Country
                }).ToList();
                return new ApiResponse<List<AddressResponseDTO>>(200, address);
            }
            catch (Exception e)
            {
                return new ApiResponse<List<AddressResponseDTO>>(500, $"An unexpected error occurred while processing your request, Error: {e.Message}");
            }
        }
    }
}
