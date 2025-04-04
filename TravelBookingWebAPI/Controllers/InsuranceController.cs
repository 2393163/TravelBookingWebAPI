using InsuranceClass.Entity;
using InsuranceClass.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsuranceController : ControllerBase
    {
        private readonly BookingRepository _bookingRepository;

    public InsuranceController()
    {
        _bookingRepository = new BookingRepository();
    }
    
        [HttpGet("GetInsuranceByProvider/{provider}")]
        public IActionResult GetInsuranceByProvider(string provider)
        {
            var insurances = _bookingRepository.GetInsuranceByProvider(provider);
            return Ok(insurances);
        }

        // Get total insurance count
        [HttpGet("GetTotalInsuranceCount")]
        public IActionResult GetTotalInsuranceCount()
        {
            var count = _bookingRepository.GetTotalInsuranceCount();
            return Ok($"Total insurance count: {count}");
        }

        // Assign insurance based on date range
        [HttpPost("AssignInsuranceBasedOnDateRange")]
        public async Task<IActionResult> AssignInsuranceBasedOnDateRange([FromBody] Booking booking)
        {
            try
            {
                await Task.Run(() => _bookingRepository.AssignInsuranceAndUpdateInsuranceTable(booking));
                return Ok("Insurance assigned based on date range successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
