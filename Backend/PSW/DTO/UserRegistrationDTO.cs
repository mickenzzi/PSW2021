using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSW.DTO
{
    public class UserRegistrationDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }

        public UserRegistrationDTO() { }
    }
}
