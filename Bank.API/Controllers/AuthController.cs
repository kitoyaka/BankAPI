using Bank.Core.DTOs;
using Bank.Core.Entities;
using Bank.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bank.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Bank.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto request)
        {
        
            await _userService.RegisterAsync(request);
            return Ok(new { Message = "User registered successfully" }); 
        }

        [HttpDelete("delete/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            await _userService.DeleteUserAsync(userId);
            return Ok(new { Message = "User deleted successfully" });
            
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto request)
        {
            
            var result = await _userService.LoginUserAsync(request.Email, request.Password);
            return Ok(new { Message = result });
            
        }
    }




}