﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PSW.DTO;
using PSW.Model;
using PSW.Service;
using System;
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


        [HttpPost("schedule")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetAvailableTerms([FromBody] TermDTO termDTO)
        {
            Console.Out.WriteLine(Request.Headers.Values);
            DateTime start = new DateTime(termDTO.StartDate.Year,termDTO.StartDate.Month,termDTO.StartDate.Day,8,0,0);
            DateTime end = new DateTime(termDTO.StartDate.Year, termDTO.StartDate.Month, termDTO.StartDate.Day, 19, 0, 0);
            if (termDTO.StartDate < start || termDTO.StartDate > end)
            {
                return BadRequest(new { message = "Worktime is 08:00-20:00" });
            }
            else if(termDTO.StartDate.Minute != 0)
            {
                return BadRequest(new { message = "You can reserve term only at full hours." });
            }
            return Ok(_termService.ScheduleTerm(termDTO));
        }

        [HttpPost("reserve")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult ReserveTerm([FromBody] Term term)
        {
            return Ok(_termService.ReserveTerm(term));
        }

        [HttpDelete("reject")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult RejectTerm([FromBody] Term term)
        {
            _termService.RejectTerm(term);
            return Ok(new { message = "Success" });
        }


    }
}
