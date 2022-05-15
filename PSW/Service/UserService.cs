using PSW.Model;
using PSW.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSW.Service
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public List<User> GetAllUsers()
        {
            return _userRepository.GetAll();
        }

        public void DeleteUser(User user)
        {
            _userRepository.Delete(user);
        }

        public bool CreateUser(User user)
        {
            return _userRepository.Create(user);
        }

        public bool UpdateUser(User user)
        {
            return _userRepository.Update(user);
        }

        public User GetUserById(string id)
        {
            return _userRepository.GetUserById(id);
        }

        public User GetUserByUsername(string username)
        {
            return _userRepository.GetUserByUsername(username);
        }


    }
}
