using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelBookingClassLibrary.Entity;
using TravelBookingClassLibrary.Repository;

namespace WebTravelBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssistanceController : ControllerBase
    {
        private readonly AssistanceRepository _repository;

        public AssistanceController()
        {
            _repository = new AssistanceRepository();
        }

        // GET: api/Assistance
        [HttpGet]
        public ActionResult<IEnumerable<Assistance>> GetAllAssistanceRequests()
        {
            var requests = _repository.GetAllAssistanceRequest();
            return Ok(requests);
        }

        // GET: api/Assistance/User/5
        [HttpGet("User/{userID}")]
        public ActionResult<IEnumerable<Assistance>> GetAssistanceByUserID(int userID)
        {
            var requests = _repository.GetAssistanceByUserID(userID);
            if (requests == null || !requests.Any())
            {
                return NotFound($"No assistance requests found for UserID: {userID}");
            }
            return Ok(requests);
        }

        // GET: api/Assistance/Request/5
        [HttpGet("Request/{requestID}")]
        public ActionResult<Assistance> GetAssistanceByRequestID(int requestID)
        {
            var request = _repository.GetAssistanceByRequestID(requestID);
            if (request == null)
            {
                return NotFound($"No assistance request found for RequestID: {requestID}");
            }
            return Ok(request);
        }

        // POST: api/Assistance
        [HttpPost]
        public ActionResult AddAssistanceRequest([FromBody] Assistance newRequest)
        {
            _repository.AddUsers(newRequest);
            return CreatedAtAction(nameof(GetAssistanceByRequestID), new { requestID = newRequest.RequestID }, newRequest);
        }

        // PUT: api/Assistance/5
        [HttpPut("{requestID}")]
        public ActionResult UpdateAssistanceRequest(int requestID, [FromBody] Assistance updatedRequest)
        {
            var existingRequest = _repository.GetAssistanceByRequestID(requestID);
            if (existingRequest == null)
            {
                return NotFound($"No assistance request found for RequestID: {requestID}");
            }

            _repository.UpdateAssistanceRequest(requestID, updatedRequest.IssueDescription);
            return NoContent();
        }

        // DELETE: api/Assistance/5
        [HttpDelete("{requestID}")]
        public ActionResult DeleteAssistanceRequest(int requestID)
        {
            var existingRequest = _repository.GetAssistanceByRequestID(requestID);
            if (existingRequest == null)
            {
                return NotFound($"No assistance request found for RequestID: {requestID}");
            }

            _repository.DeleteAssistanceRequest(requestID);
            return NoContent();
        }

        // Additional Example: Get Resolved Requests
        [HttpGet("Resolved")]
        public ActionResult<IEnumerable<Assistance>> GetResolvedRequests()
        {
            var resolvedRequests = _repository.GetResolvedAssistanceRequests();
            return Ok(resolvedRequests);
        }
    }
}
