using AutoMapper;
using IronPdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PSW.DTO;
using PSW.Model;
using PSW.Service;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetAllUsers()
        {
            return Ok(_userService.GetAllUsers());
        }

        [HttpGet("suspicious")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetSuspicousUsers()
        {
            return Ok(_userService.GetSuspiciousUsers());
        }


        [HttpGet("{id}")]
        public IActionResult GetUserById([FromRoute] string id)
        {
            User user = _userService.GetUserById(id);
            if (user == null)
                return BadRequest(new { message = "Invalid id"});
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("registration")]
        public IActionResult CreateUser([FromBody] UserRegistrationDTO userDTO)
        {

            User user = new User(userDTO);
            user.Role = Model.User.UserRole.Client.ToString();
            if (_userService.CreateUser(user))
            {
                return Ok(new { message = "Success" });
            }

            return BadRequest(new { message = "Username already exist" });
        }

        [HttpDelete]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
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
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult UpdateUser([FromBody] UserDTO userDTO)
        {
            if(_userService.UpdateUser(userDTO))
            {
                return Ok(new { message = "Success" });
            }

            return BadRequest(new { message = "Invalid id" });
        }

        [HttpPut("block/{id}")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult BlockUser([FromRoute] string id)
        {
            if (_userService.BlockUser(id))
            {
                return Ok(new { message = "Success" });
            }

            return BadRequest(new { message = "Invalid id" });
        }

        [HttpPut("unblock/{id}")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult UnblockUser([FromRoute] string id)
        {
            if (_userService.UnblockUser(id))
            {
                return Ok(new { message = "Success" });
            }

            return BadRequest(new { message = "Invalid id" });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginRequestDTO data)
        {
            User user = _userService.GetUserByLoginCredentials(data);

            if (user == null)
                return BadRequest("Please pass the valid username and password");

            var tokenString = GenerateJwtToken(data.Username);
            return Ok(new { Token = tokenString, LoggedUser = user, Message = "Success" });

        }


        private string GenerateJwtToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", username) }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
