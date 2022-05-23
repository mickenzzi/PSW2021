using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PSW.Model;
using PSW.Service;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
namespace PSW.Controllers
{
    [ApiController]
    [Route("/doctors")]
    public class DoctorController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly DoctorService _doctorService;

        public DoctorController(IConfiguration configuration, IMapper mapper, DoctorService doctorService)
        {
            _configuration = configuration;
            _mapper = mapper;
            _doctorService = doctorService;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetAllDoctors()
        {
            return Ok(_doctorService.GetAllDoctors());
        }

        [HttpGet("specialist")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetAllSpecialist()
        {
            return Ok(_doctorService.GetAllSpecialist());
        }

        [HttpGet("nonspecialist")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetAllNonSpecialist()
        {
            return Ok(_doctorService.GetAllNonSpecialist());
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetDoctorById([FromRoute] string id)
        {
            Doctor doctor = _doctorService.GetDoctorById(id);
            if (doctor == null)
                return BadRequest(new { message = "Invalid id" });
            return Ok(doctor);
        }
    }
}
