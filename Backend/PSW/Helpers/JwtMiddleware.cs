﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PSW.Service;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSW.Helpers
{
    public class JwtMiddleware
    {
        public class JWTMiddleware
        {
            private readonly RequestDelegate _next;
            private readonly IConfiguration _configuration;
            private UserService _userService;

            public JWTMiddleware(RequestDelegate next, IConfiguration configuration)
            {
                _next = next;
                _configuration = configuration;
            }

            public async Task Invoke(HttpContext context, UserService userService)
            {
                string token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (token != null)
                {
                    _userService = userService;
                    AttachAccountToContext(context, token);
                }
                await _next(context);
            }

            private void AttachAccountToContext(HttpContext context, string token)
            {
                try
                {
                    JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                    byte[] key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedToken);

                    JwtSecurityToken jwtToken = (JwtSecurityToken)validatedToken;
                    string accountId = jwtToken.Claims.First(x => x.Type == "id").Value;

                    context.Items["User"] = _userService.GetUserByUsername(accountId);
                }
                catch
                {
                }
            }
        }

    }
}
