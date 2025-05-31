using ECommerceApp.DTOs;
using ECommerceApp.DTOs.AddressDTO;
using ECommerceApp.DTOs.AddressesDTO;
using ECommerceApp.Service;
using Microsoft.AspNetCore.Mvc;
namespace ECommerceApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AddressController : ControllerBase
{
    private readonly AddressService _addressService;

    public AddressController(AddressService addressService)
    {
        _addressService = addressService;
    }

    [HttpPost("CreateAddress")]
    public async Task<ActionResult<ApiResponse<AddressResponseDTO>>> CreateAddress(
        [FromBody] AddressCreateDTO addressDTO)
    {
        var response = await _addressService.CreateAddressAsync(addressDTO);
        if (response.StatusCode != 200)
        {
            return StatusCode((int)response.StatusCode, response);
        }
        return Ok(response);
    }
    public async Task<ActionResult<ApiResponse<AddressResponseDTO>>> GetAddressById(int id)
    {
        var response = await _addressService.GetAddressByIdAsync(id);
        if (response.StatusCode != 200)
        {
            return StatusCode(response.StatusCode, response);
        }
        return Ok(response);
    }
    [HttpPut("UpdateAddress")]
    public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> UpdateAddress([FromBody] AddressUpdateDTO addressDto)
    {
        var response = await _addressService.UpdateAddressAsync(addressDto);
        if (response.StatusCode != 200)
        {
            return StatusCode(response.StatusCode, response);
        }
        return Ok(response);
    }

    [HttpDelete("DeleteAddress")]
    public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> DeleteAddress([FromBody] AddressDeleteDTO addressDeleteDTO)
    {
        var response = await _addressService.DeleteAddressAsync(addressDeleteDTO);
        if (response.StatusCode != 200)
        {
            return StatusCode(response.StatusCode, response);
        }
        return Ok(response);
    }
    [HttpGet("GetAddressesByCustomer/{customerId}")]
    public async Task<ActionResult<ApiResponse<List<AddressResponseDTO>>>> GetAddressesByCustomer(int customerId)
    {
        var response = await _addressService.GetAddressesByCustomerAsync(customerId);
        if (response.StatusCode != 200)
        {
            return StatusCode(response.StatusCode, response);
        }
        return Ok(response);
    }
    
}