using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PSW.DTO;
using PSW.Model;
using PSW.Service;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace PSW.Controllers
{
    [ApiController]
    [Route("/users")]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly UserService _userService;

        public UserController(IConfiguration configuration, IMapper mapper, UserService userService)
        {
            _configuration = configuration;
            _mapper = mapper;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            return Ok(_userService.GetAllUsers());
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById([FromRoute] string id)
        {
            User user = _userService.GetUserById(id);
            if (user == null)
                return BadRequest(new { message = "Invalid id"});
            return Ok(user);
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] UserDTO userDTO)
        {

            User user = new User(userDTO);
            if (_userService.CreateUser(user))
            {
                return Ok(new { message = "Success" });
            }

            return BadRequest(new { message = "Username already exist" });
        }

        [HttpDelete]
        public IActionResult DeleteUser([FromQuery] string id)
        {
            User user = _userService.GetUserById(id);

            if (user != null)
            {
                _userService.DeleteUser(user);
                return Ok(new { message = "Success" });
            }

            return BadRequest(new { message = "Invalid id" });
        }

        [HttpPut]
        public IActionResult UpdateUser([FromBody] UserDTO userDTO)
        {
            if(_userService.UpdateUser(userDTO))
            {
                return Ok(new { message = "Success" });
            }

            return BadRequest(new { message = "Invalid id" });
        }
    }
}
