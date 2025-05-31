using ECommerceApp.DTOs;
using ECommerceApp.DTOs.CustomerDTO;
using ECommerceApp.DTOs.CustomerDTOs;
using ECommerceApp.Service;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly CustomerService _customerService;

    public CustomerController(CustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpPost("RegisterCustomer")]
    public async Task<ActionResult<ApiResponse<CustomerResponseDTO>>> RegisterCustomer(
        [FromBody] CustomerRegistrationDTO customerDto)
    {
        var response = await _customerService.RegisterCustomerAsync(customerDto);
        if (response.StatusCode != 200)
        {
            return StatusCode((int)response.StatusCode, response);
        }

        return Ok(response);
    }

    [HttpPost("LoginCustomer")]
    public async Task<ActionResult<ApiResponse<LoginResponseDTO>>> LoginCustomer([FromBody] LoginDTO loginDto)
    {
        var response = await _customerService.LoginAsync(loginDto);
        if (response.StatusCode != 200)
        {
            return StatusCode((int)response.StatusCode, response);
        }

        return Ok(response);
    }

    [HttpGet("GetCustomerById/{id}")]
    public async Task<ActionResult<ApiResponse<CustomerResponseDTO>>> GetCustomerById(int id)
    {
        var response = await _customerService.GetCustomerByIdAsync(id);
        if (response.StatusCode != 200)
        {
            return StatusCode((int)response.StatusCode, response);
        }

        return Ok(response);
    }

    [HttpPut("UpdateCustomer")]
    public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> UpdateCustomer(
        [FromBody] CustomerUpdateDTO customerDto)
    {
        var response = await _customerService.UpdateCustomerAsync(customerDto);
        if (response.StatusCode != 200)
        {
            return StatusCode((int)response.StatusCode, response);
        }

        return Ok(response);
    }

    [HttpDelete("DeleteCustomer/{id}")]
    public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> DeleteCustomer(int id)
    {
        var response = await _customerService.DeleteCustomerAsync(id);
        if (response.StatusCode != 200)
        {
            return StatusCode((int)response.StatusCode, response);
        }
        return Ok(response);
    }

    [HttpPost("ChangePassword")]
    public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> ChangePassword(
        [FromBody] ChangePasswordDTO changePasswordDto)
    {
        var response = await _customerService.ChangePasswordAsync(changePasswordDto);
        if (response.StatusCode != 200)
        {
            return StatusCode((int)response.StatusCode, response);
        }
        return Ok(response);
    }
}









