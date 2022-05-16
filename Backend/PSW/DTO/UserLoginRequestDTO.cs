using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSW.DTO
{
    public class UserLoginRequestDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public UserLoginRequestDTO() { }
    }
}
