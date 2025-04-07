using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAssistance.Data;
using TravelAssistance.Entity;

using TravelAssistance.Repository;

namespace TravelAssistance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssistanceController : ControllerBase
    {
        private readonly AssistanceRepository _repository;

        public AssistanceController()
        {
            var options = new DbContextOptionsBuilder<AssistanceContext>()
                .UseSqlServer("Data Source=LTIN593162;Initial Catalog=AssistanceDatabases;Integrated Security=True;TrustServerCertificate=true")
                .Options;

            var context = new AssistanceContext();
            _repository = new AssistanceRepository(context);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult AddAssistanceRequest([FromBody] Assistance request)
        {
            var addedRequest = _repository.AddAssistanceRequest(request);
            return CreatedAtAction(nameof(GetAssistanceByRequestId), new { id = addedRequest.RequestID }, addedRequest);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetAssistanceByRequestId(int id)
        {
            var request = _repository.GetAssistanceByRequestId(id);
            if (request == null) return NotFound();
            return Ok(request);
        }

        [HttpGet]
        [Route("All")]
        public IActionResult GetAllAssistanceRequests()
        {
            var requests = _repository.GetAllAssistanceRequests();
            return Ok(requests);
        }

        [HttpPut]
        [Route("UpdateResolutionTime/{userId}")]
        public IActionResult UpdateResolutionTime(int userId)
        {
            _repository.UpdateResolutionTime(userId);
            return NoContent(); // Return 204 status for a successful update
        }


        [HttpPut]
        [Route("Update")]
        public IActionResult UpdateAssistanceRequest([FromBody] Assistance updatedRequest)
        {
            var request = _repository.UpdateAssistanceRequest(updatedRequest);
            if (request == null) return NotFound();
            return Ok(request);
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public IActionResult DeleteAssistanceRequest(int id)
        {
            var success = _repository.DeleteAssistanceRequest(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
