using PSW.DTO;
using PSW.Model;
using PSW.Repository.Interface;
using System.Collections.Generic;

namespace PSW.Service
{
    public class DoctorService
    {
        private readonly IUserRepository _userRepository;
        
        public DoctorService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public List<User> GetAllDoctors()
        {
            List<User> users = _userRepository.GetAll();
            List<User> doctors = new List<User>();
            foreach(User u in users)
            {
                if (u.Role.Equals("Doctor"))
                {
                    doctors.Add(u);
                }
            }
            return doctors;
        }

        public List<User> GetAllSpecialist()
        {
            List<User> users = _userRepository.GetAll();
            List<User> doctors = new List<User>();
            foreach (User u in users)
            {
                if (u.Role.Equals("Doctor") && !u.Specialization.ToLower().Equals("GeneralPractitioner".ToLower()))
                {
                    doctors.Add(u);
                }
            }
            return doctors;
        }
        public List<User> GetAllNonSpecialist()
        {
            List<User> users = _userRepository.GetAll();
            List<User> doctors = new List<User>();
            foreach (User u in users)
            {
                if (u.Role.Equals("Doctor") && u.Specialization.ToLower().Equals("GeneralPractitioner".ToLower()))
                {
                    doctors.Add(u);
                }
            }
            return doctors;
        }

    }
}
