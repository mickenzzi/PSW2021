using Moq;
using PSW.DTO;
using PSW.Model;
using PSW.Repository.Interface;
using PSW.Service;
using Shouldly;
using System;
using System.Collections.Generic;
using Xunit;

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
        public void Get_Reserved_Term()
        {
            Mock<ITermRepository> termRepository = CreateTermRepository();
            TermService service = new TermService(termRepository.Object, _userRepoMoq.Object, _doctorService);
            List<Term> term = service.GetAllTerms();
            term.ShouldNotBe(null);
        }

        [Fact]
        public void Get_Free_Term()
        {
            Mock<ITermRepository> termRepository = CreateTermRepository();
            TermService service = new TermService(termRepository.Object, _userRepoMoq.Object, _doctorService);
            Term term = service.GetTermById("10");
            term.ShouldBe(null);
        }

        [Fact]
        public void Get_Doctor_Terms()
        {
            Mock<ITermRepository> termRepository = CreateTermRepository();
            Mock<IUserRepository> doctorRepository = CreateDoctorRepository();
            TermService service = new TermService(termRepository.Object, doctorRepository.Object, _doctorService);
            List<TermResponse> term = service.GetAllDoctorTerms("jovan");
            term.ShouldNotBe(null);
        }

        [Fact]
        public void Get_Patient_Terms()
        {
            Mock<ITermRepository> termRepository = CreateTermRepository();
            Mock<IUserRepository> patientRepository = CreateDoctorRepository();
            TermService service = new TermService(termRepository.Object, patientRepository.Object, _doctorService);
            List<TermResponse> term = service.GetAllPatientTerms("ivan");
            term.ShouldNotBe(null);
        }

        [Fact]
        public void Schedule_Term()
        {
            TermDTO termDTO = new TermDTO();
            DateTime now = DateTime.Now;
            termDTO.StartDate = now;
            termDTO.EndDate = now.AddDays(1);
            termDTO.DoctorPriority = true;
            termDTO.DoctorId = "jovan";
            termDTO.UserId = "ivan";
            Mock<ITermRepository> termRepository = CreateTermRepository();
            Mock<IUserRepository> doctorRepository = CreateDoctorRepository();
            TermService service = new TermService(termRepository.Object, doctorRepository.Object, _doctorService);
            List<TermResponse> result = service.ScheduleTerm(termDTO);
            result.ShouldNotBe(null);
        }

        private static Mock<ITermRepository> CreateTermRepository()
        {
            Mock<ITermRepository> termRepository = new Mock<ITermRepository>();
            List<Term> terms = new List<Term>();
            DateTime now = DateTime.Now;
            Term term = CreateTerm(now.ToString(), "jovan", "ivan");
            terms.Add(term);
            termRepository.Setup(x => x.GetAll()).Returns(terms);

            return termRepository;
        }

        private static Mock<IUserRepository> CreateDoctorRepository()
        {
            Mock<IUserRepository> stubRepository = new Mock<IUserRepository>();
            List<User> doctors = new List<User>();
            User doctor = CreateDoctor("jovan");
            doctors.Add(doctor);
            stubRepository.Setup(x => x.GetAll()).Returns(doctors);

            return stubRepository;
        }

        private static Mock<IUserRepository> CreatePatientRepository()
        {
            Mock<IUserRepository> stubRepository = new Mock<IUserRepository>();
            List<User> patients = new List<User>();
            User user = CreatePatient("ivan");
            patients.Add(user);
            stubRepository.Setup(x => x.GetAll()).Returns(patients);

            return stubRepository;
        }

        private static Term CreateTerm(string date, string doctor, string user)
        {
            Term term = new Term
            {
                Id = "1",
                UserId = user,
                DoctorId = doctor,
                DateTimeTerm = date,
                IsRejected = false
            };
            return term;
        }

        private static User CreateDoctor(string doctor)
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

        private static User CreatePatient(string patient)
        {
            User user = new User
            {
                Id = patient,
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

    }
}
