using Microsoft.AspNetCore.Mvc;
using TravelBookingClassLibrary;
using TravelBookingClassLibrary.Entities;
using Microsoft.EntityFrameworkCore;
using TravelBookingClassLibrary.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using System.Security.Claims;
using TravelBookingAPI.Services;


namespace TravelBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [Authorize(Roles = "Admin")] //by admin
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsers();
            return Ok(users);
        }

        [Authorize] //by self and user
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(long id)
        {
            var user = (await _userRepository.GetAllUsers()).Find(u => u.UserID == id);
            if (user == null) return NotFound("User not found");

            var loggedInUserIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(loggedInUserIdClaim))
            {
                return Unauthorized("User ID claim not found");
            }

            if (!long.TryParse(loggedInUserIdClaim, out var loggedInUserId))
            {
                return Unauthorized("Invalid User ID claim");
            }

            if (User.IsInRole("Admin") || loggedInUserId == id)
            {
                return Ok(user);
            }

            return Forbid();
        }

        [Authorize]  //self and admin
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(long id, [FromBody] User updatedUser)
        {
            var loggedInUserIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(loggedInUserIdClaim))
            {
                return Unauthorized("User ID claim not found");
            }

            if (!long.TryParse(loggedInUserIdClaim, out var loggedInUserId))
            {
                return Unauthorized("Invalid User ID claim");
            }

            if (User.IsInRole("Admin") || loggedInUserId == id)
            {
                await _userRepository.UpdateUser(id, updatedUser.Name, updatedUser.Email, updatedUser.ContactNumber);
                return Ok("User updated successfully");
            }

            return Forbid();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            var user = (await _userRepository.GetAllUsers()).Find(u => u.UserID == id);
            if (user == null) return NotFound("User not found");

            await _userRepository.DeleteUser(id);
            return Ok("User deleted");
        }

        [Authorize(Roles = "Admin")] //only admin
        [HttpGet("search")]
        public async Task<IActionResult> SearchUser([FromQuery] string name)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest("Invalid user data");

            var users = await _userRepository.GetUsersByName(name);
            if (users == null || users.Count == 0)
                return NotFound($"No users found with name '{name}'");

            return Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin/total-users")]
        public async Task<IActionResult> GetTotalUsers()
        {
            var totalUsers = await _userRepository.GetTotalUsers();
            return Ok(new { TotalUsers = totalUsers });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin/users-by-role")]
        public async Task<IActionResult> GetUsersByRole([FromQuery] string role)
        {
            if (string.IsNullOrEmpty(role)) return BadRequest("Role is required.");

            var users = await _userRepository.GetUsersByRole(role);
            return Ok(users);
        }





    }


}

