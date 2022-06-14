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
    public class TermServiceTests
    {
        private readonly TermService _termService;
        private readonly DoctorService _doctorService;
        private readonly Mock<ITermRepository> _termRepoMoq = new Mock<ITermRepository>();
        private readonly Mock<IUserRepository> _userRepoMoq = new Mock<IUserRepository>();

        public TermServiceTests()
        {
            _termService = new TermService(_termRepoMoq.Object, _userRepoMoq.Object, _doctorService);
        }

        [Fact]
        public void Get_reserved_term()
        {
            var stubRepository = CreateStubRepository();
            TermService service = new TermService(stubRepository.Object, _userRepoMoq.Object, _doctorService);
            var term = service.GetAllTerms();
            term.ShouldNotBe(null);
        }

        [Fact]
        public void Get_free_term()
        {
            var stubRepository = CreateStubRepository();
            TermService service = new TermService(stubRepository.Object, _userRepoMoq.Object, _doctorService);
            var term = service.GetTermById("10");
            term.ShouldBe(null);
        }

        [Fact]
        public void Get_doctor_terms()
        {
            var stubRepository = CreateStubRepository();
            var doctorRepository = CreateDoctorRepository();
            TermService service = new TermService(stubRepository.Object, doctorRepository.Object, _doctorService);
            var term = service.GetAllDoctorTerms("jovan");
            term.ShouldNotBe(null);
        }

        [Fact]
        public void Get_patient_terms()
        {
            var stubRepository = CreateStubRepository();
            var patientRepository = CreateDoctorRepository();
            TermService service = new TermService(stubRepository.Object, patientRepository.Object, _doctorService);
            var term = service.GetAllPatientTerms("ivan");
            term.ShouldNotBe(null);
        }

        private static Mock<ITermRepository> CreateStubRepository()
        {
            var stubRepository = new Mock<ITermRepository>();
            var terms = new List<Term>();
            Term term = CreateTerm("today", "jovan", "ivan");
            terms.Add(term);
            stubRepository.Setup(x => x.GetAll()).Returns(terms);

            return stubRepository;
        }

        private static Mock<IUserRepository> CreateDoctorRepository()
        {
            var stubRepository = new Mock<IUserRepository>();
            var doctors = new List<User>();
            User doctor = CreateDoctor("jovan");
            doctors.Add(doctor);
            stubRepository.Setup(x => x.GetAll()).Returns(doctors);

            return stubRepository;
        }

        private static Mock<IUserRepository> CreatePatientRepository()
        {
            var stubRepository = new Mock<IUserRepository>();
            var patients = new List<User>();
            User user = CreatePatient("ivan");
            patients.Add(user);
            stubRepository.Setup(x => x.GetAll()).Returns(patients);

            return stubRepository;
        }

        private static Term CreateTerm(string date, string doctor, string user)
        {
            Term term = new Term();
            term.Id = "1";
            term.UserId = user;
            term.DoctorId = doctor;
            term.DateTimeTerm = date;
            term.IsRejected = false;
            return term;
        }

        private static User CreateDoctor(string doctor)
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

        private static User CreatePatient(string patient)
        {
            User user = new User();
            user.Id = patient;
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

    }
}
