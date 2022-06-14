using Moq;
using PSW.Model;
using PSW.Repository.Interface;
using PSW.Service;
using System;
using Xunit;
using System.Collections.Generic;
using Shouldly;
using PSW.DTO;

namespace PSW.UnitTests
{
    public class DoctorServiceTests
    {
        private readonly DoctorService _doctorService;
        private readonly Mock<IUserRepository> _userRepoMoq = new Mock<IUserRepository>();

        public DoctorServiceTests()
        {
            _doctorService = new DoctorService(_userRepoMoq.Object);
        }


        [Fact]
        public void Get_specialist_doctors()
        {
            var doctorRepository = CreateDoctorRepository();
            DoctorService service = new DoctorService(doctorRepository.Object);
            var doctors = service.GetAllSpecialist();
            doctors.ShouldNotBe(null);
        }

        [Fact]
        public void Get_non_specialist_doctors()
        {
            var doctorRepository = CreateDoctorRepository();
            DoctorService service = new DoctorService(doctorRepository.Object);
            var doctors = service.GetAllNonSpecialist();
            doctors.ShouldNotBe(null);
        }

        private static Mock<IUserRepository> CreateDoctorRepository()
        {
            var stubRepository = new Mock<IUserRepository>();
            var doctors = new List<User>();
            User doctor = CreateSpecialist("jovan");
            User nonSpecialist = CreateNonSpecialist("ivan");
            doctors.Add(doctor);
            doctors.Add(nonSpecialist);
            stubRepository.Setup(x => x.GetAll()).Returns(doctors);

            return stubRepository;
        }


        private static User CreateSpecialist(string doctor)
        {
            User user = new User();
            user.Id = doctor;
            user.FirstName = "name";
            user.LastName = "surname";
            user.Username = "username";
            user.Password = "password";
            user.Role = User.UserRole.Doctor.ToString();
            user.DateOfBirth = "birthday";
            user.Address = "address";
            user.Country = "country";
            user.PhoneNumber = "phone";
            user.Specialization = "Oncology";
            user.IsBlocked = false;
            return user;
        }

        private static User CreateNonSpecialist(string doctor)
        {
            User user = new User();
            user.Id = doctor;
            user.FirstName = "name";
            user.LastName = "surname";
            user.Username = "username";
            user.Password = "password";
            user.Role = User.UserRole.Doctor.ToString();
            user.DateOfBirth = "birthday";
            user.Address = "address";
            user.Country = "country";
            user.PhoneNumber = "phone";
            user.Specialization = "GeneralPractitioner";
            user.IsBlocked = false;
            return user;
        }
    }
}
