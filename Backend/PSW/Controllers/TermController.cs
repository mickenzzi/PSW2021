using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PSW.DTO;
using PSW.Model;
using PSW.Service;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace PSW.Controllers
{
    [ApiController]
    [Route("/terms")]
    public class TermController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly TermService _termService;

        public TermController(IConfiguration configuration, IMapper mapper, TermService termService)
        {
            _configuration = configuration;
            _mapper = mapper;
            _termService = termService;
        }


        [HttpGet("schedule")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetAvailableTerms([FromBody] TermDTO termDTO)
        {
            return Ok(_termService.ScheduleTerm(termDTO));
        }
    }
}
