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
      
        public User()
        {
            Id = "user_" + Guid.NewGuid();
        }

        public User(UserDTO userDTO)
        {
            Id = "user_" + Guid.NewGuid();
            FirstName = userDTO.FirstName;
            LastName = userDTO.LastName;
            Username = userDTO.Username;
            Password = userDTO.Password;
        }

    }
}
