using PSW.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PSW.Model
{
    [Table("User")]
    public class User
    {

        public enum UserRole
        {
            Client,
            Admin
        }

        [Key]
        [Column("id")]
        public String Id { get; set; }
        [Column("firstName")]
        public string FirstName { get; set; }
        [Column("lastName")]
        public string LastName { get; set; }
        [Column("username")]
        public string Username { get; set; }
        [Column("password")]
        public string Password { get; set; }
        [Column("role")]
        public String Role { get; set; }
        [Column("birthday")]
        public string DateOfBirth { get; set; }
        [Column("address")]
        public string Address { get; set; }
        [Column("country")]
        public string Country { get; set; }
        [Column("phone")]
        public string PhoneNumber { get; set; }

        public User()
        {
            Id = "user_" + Guid.NewGuid();
        }

        public User(UserRegistrationDTO userDTO)
        {
            Id = "user_" + Guid.NewGuid();
            FirstName = userDTO.FirstName;
            LastName = userDTO.LastName;
            Username = userDTO.Username;
            Password = userDTO.Password;
            DateOfBirth = userDTO.DateOfBirth;
            Address = userDTO.Address;
            Country = userDTO.Country;
            PhoneNumber = userDTO.PhoneNumber;
        }

    }
}
