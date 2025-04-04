using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InsuranceClass.Entity;
using InsuranceClass.Repository;

namespace InsuranceWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly BookingRepository _bookingRepository;

        public BookingController()
        {
            _bookingRepository = new BookingRepository();
        }

        // Add a new booking
        [HttpPost("AddBooking")]
        public async Task<IActionResult> AddBooking([FromBody] Booking booking)
        {
            if (ModelState.IsValid)
            {
                await Task.Run(() => _bookingRepository.AddBooking(booking));
                return Ok("Booking added successfully!");
            }
            return BadRequest(ModelState);
        }

        // Get all bookings
        [HttpGet("GetAllBookings")]
        public IActionResult GetAllBookings()
        {
            var bookings = _bookingRepository.GetAllBookings();
            return Ok(bookings);
        }

        // Update booking
        [HttpPut("UpdateBooking/{bookingId}")]
        public async Task<IActionResult> UpdateBooking(int bookingId, [FromBody] DateTime startDate)
        {
            await Task.Run(() => _bookingRepository.updateBooking(bookingId, startDate));
            return Ok("Booking updated successfully!");
        }

        // Delete booking
        [HttpDelete("DeleteBooking/{bookingId}")]
        public async Task<IActionResult> DeleteBooking(int bookingId)
        {
            await Task.Run(() => _bookingRepository.DeleteBooking(bookingId));
            return Ok("Booking deleted successfully!");
        }

        // Get insurances by provider
        
    }
}
