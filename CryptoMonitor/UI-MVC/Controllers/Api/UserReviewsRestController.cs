using CryptoMonitor.BL;
using Microsoft.AspNetCore.Mvc;

namespace UI_MVC.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserReviewsRestController : ControllerBase
    {
        private readonly IManager _manager;

        public UserReviewsRestController(IManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var reviews = _manager.GetAllUserReviews();
            if (reviews == null || !reviews.Any())
            {
                return NoContent();
            }
            
            var reviewsDto = reviews.Select(r => new 
            {
                r.Id,
                r.UserName,
                r.Comment,
                r.Rating,
                r.DatePosted,
                ExchangeName = r.Exchange.Name
            });

            return Ok(reviewsDto);
        }

        [HttpPost]
        public IActionResult AddReview([FromBody] CreateUserReviewDto reviewDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var exchange = _manager.GetExchange(reviewDto.ExchangeId);
            if (exchange == null)
            {
                return NotFound($"Exchange with ID {reviewDto.ExchangeId} not found.");
            }

            var validationResults = _manager.AddUserReview(
                reviewDto.UserName,
                reviewDto.Comment,
                reviewDto.Rating,
                exchange
            );

            if (validationResults.Any())
            {
                foreach (var validationResult in validationResults)
                {
                    ModelState.AddModelError("", validationResult.ErrorMessage ?? "Validation error");
                }
                return BadRequest(ModelState);
            }

            return CreatedAtAction(nameof(GetAll), null);
        }
    }

    public class CreateUserReviewDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public int Rating { get; set; }
        public int ExchangeId { get; set; }
    }
}
