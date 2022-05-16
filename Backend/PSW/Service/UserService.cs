using PSW.DTO;
using PSW.Model;
using PSW.Repository.Interface;
using System.Collections.Generic;
using System.Linq;

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

        public bool UpdateUser(UserDTO userDTO)
        {
            User user = _userRepository.GetUserById(userDTO.Id);
            if(user == null)
            {
                return false;
            }
            user.FirstName = userDTO.FirstName;
            user.LastName = userDTO.LastName;
            user.Username = userDTO.Username;
            user.Password = userDTO.Password;
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

        public User GetUserByLoginCredentials(UserLoginRequestDTO userDTO)
        {
            User user = _userRepository.GetAll().SingleOrDefault(u => u.Username == userDTO.Username && u.Password == userDTO.Password);
            return user;
        }


    }
}
