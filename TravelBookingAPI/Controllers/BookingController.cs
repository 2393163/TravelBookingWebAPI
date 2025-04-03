using Microsoft.AspNetCore.Mvc;
using TravelBookingClassLibrary;

namespace TravelBookingWebApi
{

    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly BookandPaymentRepository _userRepository;

        public BookingController(BookandPaymentRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // CREATE User
        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] TravelBookingClassLibrary.Booking book)
        {
            if (ModelState.IsValid)
            {
                await Task.Run(() => _userRepository.AddBooking(book));
                return Ok(book);
            }
            return BadRequest(ModelState);
        }

        // READ (Get All Users)
        [HttpGet("Bookings")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await Task.Run(() => _userRepository.GetAllUsers());
                return Ok(users);
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework like Serilog, NLog, etc.)
                // For simplicity, we're just returning the error message here
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // READ (Get User By ID)
        [HttpGet("Bookings/{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            var book = (await _userRepository.GetAllUsers()).Find(b => b.BookingID == id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        // UPDATE User
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(int id, [FromBody] TravelBookingClassLibrary.Booking updatedbook)
        {
            if (id != updatedbook.BookingID)
            {
                return BadRequest("Booking ID mismatch");
            }

            var existingBook = (await Task.Run(() => _userRepository.GetAllUsers())).Find(b => b.BookingID == id);
            if (existingBook == null)
            {
                return NotFound();
            }

            await Task.Run(() => _userRepository.updateBooking(id, updatedbook.StartDate));
            return Ok(updatedbook);
        }

        // DELETE User
        [HttpDelete("Booking/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var book = (await Task.Run(() => _userRepository.GetAllUsers())).Find(b => b.BookingID == id);
            if (book == null)
            {
                return NotFound();
            }

            await Task.Run(() => _userRepository.DeleteBooking(id));
            return Ok($"Booking with ID {id} deleted successfully");
        }
        [HttpPut("CancelBooking/{id}")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            using (var context = new BookingPaymentContext())
            {
                var booking = await context.Bookings.FindAsync(id);
                if (booking != null)
                {
                    booking.Status = "Cancelled";
                    await context.SaveChangesAsync();
                    return Ok($"Booking with ID {id} cancelled successfully");
                }
                return NotFound();
            }
        }

        // Search by name using Query
        //[HttpGet("search")]
        //public async Task<IActionResult> SearchUser([FromQuery] string name)
        //{
        //    if (string.IsNullOrEmpty(name))
        //    {
        //        return BadRequest("Invalid user data");
        //    }

        //    var users = await Task.Run(() => _userRepository.GetAllUsers().Where(u => u.Name.Contains(name)).ToList());
        //    if (users == null || users.Count == 0)
        //    {
        //        return NotFound($"No users found with name containing '{name}'");
        //    }
        //    return Ok(users);
        //}
    }
}