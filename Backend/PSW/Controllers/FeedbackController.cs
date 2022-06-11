using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PSW.Model;
using PSW.Service;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace PSW.Controllers
{
    [ApiController]
    [Route("/feedbacks")]
    public class FeedbackController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly FeedbackService _feedbackService;

        public FeedbackController(IConfiguration configuration, IMapper mapper, FeedbackService feedbackService)
        {
            _configuration = configuration;
            _mapper = mapper;
            _feedbackService = feedbackService;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetAllFeedbacks()
        {
            return Ok(_feedbackService.GetAllFeedbacks());
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetFeedbackById([FromRoute] string id)
        {
            Feedback feedback = _feedbackService.GetFeedbackById(id);
            if (feedback == null)
                return BadRequest(new { message = "Invalid id" });
            return Ok(feedback);
        }


        [HttpDelete]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult DeleteFeedback([FromQuery] string id)
        {
            Feedback feedback = _feedbackService.GetFeedbackById(id);

            if (feedback != null)
            {
                _feedbackService.DeleteFeedback(feedback);
                return Ok(new { message = "Success" });
            }

            return BadRequest(new { message = "Invalid id" });
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult UpdateFeedback([FromBody] Feedback newFeedback)
        {
            if (_feedbackService.UpdateFeedback(newFeedback))
            {
                return Ok(new { message = "Success" });
            }

            return BadRequest(new { message = "Invalid id" });
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult CreateFeedback([FromBody] Feedback newFeedback)
        {

            Feedback feedback = new Feedback(newFeedback);
            if (_feedbackService.CreateFeedback(feedback))
            {
                return Ok(new { message = "Success" });
            }

            return BadRequest(new { message = "Comment already exist" });
        }

    }
}
