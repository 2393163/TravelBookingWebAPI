using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using TravelBookingClassLibrary.BusinessLogic;
using TravelBookingClassLibrary.Entity;

namespace TravelBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly ReviewBusinessLogic _reviewBusinessLogic;

        public ReviewsController()
        {
            _reviewBusinessLogic = new ReviewBusinessLogic();
        }

        private List<Dictionary<string, object>> ConvertDataTableToList(DataTable dt)
        {
            var list = new List<Dictionary<string, object>>();
            foreach (DataRow row in dt.Rows)
            {
                var dict = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    dict[col.ColumnName] = row[col];
                }
                list.Add(dict);
            }
            return list;
        }

        [HttpPost]
        public IActionResult SaveReview([FromBody] Review review)
        {
            if (review == null)
            {
                return BadRequest("Review is null.");
            }

            try
            {
                int status = _reviewBusinessLogic.SaveReview(review);
                if (status != 0)
                {
                    return Ok("Review saved successfully.");
                }
                else
                {
                    return StatusCode(500, "An error occurred while saving the review.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework like Serilog or NLog)
                Console.WriteLine($"An error occurred while saving the review: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return StatusCode(500, $"An error occurred while saving the review: {ex.Message}");
            }
        }

        [HttpPut("{reviewId}")]
        public IActionResult UpdateReview(int reviewId, [FromBody] Review review)
        {
            if (review == null || review.ReviewID != reviewId)
            {
                return BadRequest("Review is null or ReviewID does not match.");
            }

            try
            {
                int status = _reviewBusinessLogic.UpdateReview(review);
                if (status != 0)
                {
                    return Ok("Review updated successfully.");
                }
                else
                {
                    return StatusCode(500, "An error occurred while updating the review.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework like Serilog or NLog)
                Console.WriteLine($"An error occurred while updating the review: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return StatusCode(500, $"An error occurred while updating the review: {ex.Message}");
            }
        }

        [HttpDelete("{reviewId}")]
        public IActionResult DeleteReview(int reviewId)
        {
            try
            {
                int status = _reviewBusinessLogic.DeleteReview(reviewId);
                if (status != 0)
                {
                    return Ok("Review deleted successfully.");
                }
                else
                {
                    return StatusCode(500, "An error occurred while deleting the review.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework like Serilog or NLog)
                Console.WriteLine($"An error occurred while deleting the review: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return StatusCode(500, $"An error occurred while deleting the review: {ex.Message}");
            }
        }

        [HttpGet]
        public IActionResult GetAllReviews()
        {
            DataTable dt = _reviewBusinessLogic.FetchCustomerData();
            var reviews = ConvertDataTableToList(dt);
            return Ok(reviews);
        }

        [HttpGet("count")]
        public IActionResult GetReviewCount()
        {
            int count = _reviewBusinessLogic.ReviewCount();
            return Ok(count);
        }

        [HttpGet("package/{packageId}")]
        public IActionResult GetReviewsByPackageID(int packageId)
        {
            DataTable dt = _reviewBusinessLogic.FetchReviewsByPackageID(packageId);
            var reviews = ConvertDataTableToList(dt);
            return Ok(reviews);
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetReviewsByUser(string userId)
        {
            DataTable dt = _reviewBusinessLogic.FetchReviewsByUser(userId);
            var reviews = ConvertDataTableToList(dt);
            return Ok(reviews);
        }

        [HttpGet("rating/{rating}")]
        public IActionResult GetReviewsByRating(int rating)
        {
            DataTable dt = _reviewBusinessLogic.FetchReviewsByRating(rating);
            var reviews = ConvertDataTableToList(dt);
            return Ok(reviews);
        }

        [HttpGet("recent/{count}")]
        public IActionResult GetRecentReviews(int count)
        {
            DataTable dt = _reviewBusinessLogic.FetchRecentReviews(count);
            var reviews = ConvertDataTableToList(dt);
            return Ok(reviews);
        }

        [HttpGet("top-rated/{topCount}")]
        public IActionResult GetTopRatedReviews(int topCount)
        {
            DataTable dt = _reviewBusinessLogic.FetchTopRatedReviews(topCount);
            var reviews = ConvertDataTableToList(dt);
            return Ok(reviews);
        }

        [HttpGet("Average/{PackageID:int}")]
        public IActionResult AverageRating(int PackageID)
        {
            double averageRating = _reviewBusinessLogic.FetchAverageRating(PackageID);
            return Ok(averageRating);
        }

        [HttpGet("keyword/{keyword}")]
        public IActionResult GetReviewsByKeyword(string keyword)
        {
            DataTable dt = _reviewBusinessLogic.FetchReviewsByKeyword(keyword);
            var reviews = ConvertDataTableToList(dt);
            return Ok(reviews);
        }
    }
}
