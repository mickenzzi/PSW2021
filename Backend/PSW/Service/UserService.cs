using PSW.DTO;
using PSW.Model;
using PSW.Repository.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WinSCP;

namespace PSW.Service
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly TermService _termService;

        public UserService(IUserRepository userRepository, TermService termService)
        {
            _userRepository = userRepository;
            _termService = termService;
        }

        public List<User> GetAllUsers()
        {
            return _userRepository.GetAll();
        }

        public List<SuspiciousUser> GetSuspiciousUsers()
        {
            List<User> users = _userRepository.GetAll();
            List<User> clients = new List<User>();
            List<SuspiciousUser> suspciousUsers = new List<SuspiciousUser>();
            foreach(User u in users)
            {
                if (u.Role.Equals("Client"))
                {
                    clients.Add(u);
                }
            }

            foreach(User u in clients)
            {
                SuspiciousUser user = new SuspiciousUser();
                user.Id = u.Id;
                user.FirstName = u.FirstName;
                user.LastName = u.LastName;
                user.Username = u.Username;
                user.IsBlocked = u.IsBlocked;
                user.RejectsNumber = FindRejectNumber(_termService.GetAllPatientTerms(u.Id));
                suspciousUsers.Add(user);
            }
            return suspciousUsers;
        }

        public int FindRejectNumber(List<TermResponse> terms)
        {
            DateTime now = DateTime.Now;
            DateTime start = new DateTime(now.Year, now.Month, 1);
            DateTime end = start.AddMonths(1).AddDays(-1);
            List<TermResponse> rejectedTerms = new List<TermResponse>();
            foreach(TermResponse t in terms)
            {
                if(DateTime.Parse(t.DateTimeTerm)>= start && DateTime.Parse(t.DateTimeTerm) <= end && t.IsRejected)
                {
                    rejectedTerms.Add(t);
                }
            }

            return rejectedTerms.Count;
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

        public bool BlockUser(string id)
        {
            User user = _userRepository.GetUserById(id);
            if (user == null)
            {
                return false;
            }
            user.IsBlocked = true;
            return _userRepository.Update(user);
        }


        public bool UnblockUser(string id)
        {
            User user = _userRepository.GetUserById(id);
            if (user == null)
            {
                return false;
            }
            user.IsBlocked = false;
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
