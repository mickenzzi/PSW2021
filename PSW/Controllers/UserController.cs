using Microsoft.AspNetCore.Mvc;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using PSW.Service;
using PSW.Model;

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
    }
}
