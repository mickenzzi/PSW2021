using Moq;
using PSW.Model;
using PSW.Repository.Interface;
using PSW.Service;
using Shouldly;
using System.Collections.Generic;
using Xunit;

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
        public void Get_Specialist_Doctors()
        {
            Mock<IUserRepository> doctorRepository = CreateDoctorRepository();
            DoctorService service = new DoctorService(doctorRepository.Object);
            List<User> doctors = service.GetAllSpecialist();
            doctors.ShouldNotBe(null);
        }

        [Fact]
        public void Get_Non_Specialist_Doctors()
        {
            Mock<IUserRepository> doctorRepository = CreateDoctorRepository();
            DoctorService service = new DoctorService(doctorRepository.Object);
            List<User> doctors = service.GetAllNonSpecialist();
            doctors.ShouldNotBe(null);
        }

        private static Mock<IUserRepository> CreateDoctorRepository()
        {
            Mock<IUserRepository> stubRepository = new Mock<IUserRepository>();
            List<User> doctors = new List<User>();
            User doctor = CreateSpecialist("jovan");
            User nonSpecialist = CreateNonSpecialist("ivan");
            doctors.Add(doctor);
            doctors.Add(nonSpecialist);
            stubRepository.Setup(x => x.GetAll()).Returns(doctors);

            return stubRepository;
        }


        private static User CreateSpecialist(string doctor)
        {
            User user = new User
            {
                Id = doctor,
                FirstName = "name",
                LastName = "surname",
                Username = "username",
                Password = "password",
                Role = User.UserRole.Doctor.ToString(),
                DateOfBirth = "birthday",
                Address = "address",
                Country = "country",
                PhoneNumber = "phone",
                Specialization = "Oncology",
                IsBlocked = false
            };
            return user;
        }

        private static User CreateNonSpecialist(string doctor)
        {
            User user = new User
            {
                Id = doctor,
                FirstName = "name",
                LastName = "surname",
                Username = "username",
                Password = "password",
                Role = User.UserRole.Doctor.ToString(),
                DateOfBirth = "birthday",
                Address = "address",
                Country = "country",
                PhoneNumber = "phone",
                Specialization = "GeneralPractitioner",
                IsBlocked = false
            };
            return user;
        }
    }
}
