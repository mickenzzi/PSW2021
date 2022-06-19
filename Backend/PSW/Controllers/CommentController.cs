using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PSW.Model;
using PSW.Service;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace PSW.Controllers
{
    [ApiController]
    [Route("/comments")]
    public class CommentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly CommentService _commentService;

        public CommentController(IConfiguration configuration, IMapper mapper, CommentService commentService)
        {
            _configuration = configuration;
            _mapper = mapper;
            _commentService = commentService;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetAllComments()
        {
            return Ok(_commentService.GetAllComments());
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetCommentById([FromRoute] string id)
        {
            Comment comment = _commentService.GetCommentById(id);
            if (comment == null)
            {
                return BadRequest(new { message = "Invalid id" });
            }

            return Ok(comment);
        }


        [HttpDelete]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult DeleteComment([FromQuery] string id)
        {
            Comment comment = _commentService.GetCommentById(id);

            if (comment != null)
            {
                _commentService.DeleteComment(comment);
                return Ok(new { message = "Success" });
            }

            return BadRequest(new { message = "Invalid id" });
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult UpdateComment([FromBody] Comment newComment)
        {
            if (_commentService.UpdateComment(newComment))
            {
                return Ok(new { message = "Success" });
            }

            return BadRequest(new { message = "Invalid id" });
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult CreateComment([FromBody] Comment newComment)
        {

            Comment comment = new Comment(newComment);
            if (_commentService.CreateComment(comment))
            {
                return Ok(new { message = "Success" });
            }

            return BadRequest(new { message = "Comment already exist" });
        }

    }
}
