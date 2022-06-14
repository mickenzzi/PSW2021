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
    public class UserServiceTests
    {
        private readonly UserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly Mock<IMedicineRepository> _medicineRepoMoq = new Mock<IMedicineRepository>();
        private readonly TermService _termService;
        private readonly Mock<IUserRepository> _userRepoMoq = new Mock<IUserRepository>();

        public UserServiceTests()
        {
            _userService = new UserService(_userRepoMoq.Object, _medicineRepoMoq.Object, _termService);
        }
        [Fact]
        public void Get_by_id()
        {
            var userId = "1";
            var userDTO = new User
            {
                Id = userId
            };

            _userRepoMoq.Setup(x => x.GetUserById(userId)).Returns(userDTO);

            var user = _userService.GetUserById(userId);

            Assert.Equal(userId, user.Id);
        }

        [Fact]
        public void Get_by_invalid_id()
        {
            var stubRepository = CreateStubRepository();
            UserService service = new UserService(stubRepository.Object, _medicineRepoMoq.Object, _termService);

            var user = _userService.GetUserById("10");
            user.ShouldBe(null);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Register_user(User user, bool notRegistered)
        {
            var stubRepository = CreateStubRepository();
            UserService service = new UserService(stubRepository.Object, _medicineRepoMoq.Object, _termService);

            bool registered = service.CreateUser(user);

            registered.ShouldBe(notRegistered);
        }

        private static Mock<IUserRepository> CreateStubRepository()
        {
            var stubRepository = new Mock<IUserRepository>();
            var users = new List<User>();
            User pera = CreateUser("1", "pera", "pera123");
            users.Add(pera);

            stubRepository.Setup(x => x.GetAll()).Returns(users);

            return stubRepository;
        }

        [Fact]
        public void Login_With_Valid_Credentials()
        {
            var stubRepository = CreateStubRepository();
            UserService service = new UserService(stubRepository.Object, _medicineRepoMoq.Object, _termService);
            UserLoginRequestDTO credentials = new UserLoginRequestDTO();
            credentials.Username = "pera";
            credentials.Password = "pera123";

            User user = service.GetUserByLoginCredentials(credentials);

            user.ShouldNotBe(null);
        }

        [Fact]
        public void Login_With_Invalid_Credentials()
        {
            var stubRepository = CreateStubRepository();
            UserService service = new UserService(stubRepository.Object, _medicineRepoMoq.Object, _termService);
            UserLoginRequestDTO credentials = new UserLoginRequestDTO();
            credentials.Username = "pera";
            credentials.Password = "pera";

            User user = service.GetUserByLoginCredentials(credentials);

            user.ShouldBe(null);
        }


        public static IEnumerable<object[]> Data()
        {
            var retval = new List<object[]>(); 
            retval.Add(new object[] { CreateUser("1", "pera", "123"), false });
            retval.Add(new object[] { CreateUser("2", "mika", "123"), false });

            return retval;
        }

        private static User CreateUser(string id, string username, string password)
        {
            User user = new User();
            user.Id = id;
            user.FirstName = "name";
            user.LastName = "surname";
            user.Username = username;
            user.Password = password;
            user.Role = User.UserRole.Client.ToString();
            user.DateOfBirth = "birthday";
            user.Address = "address";
            user.Country = "country";
            user.PhoneNumber = "phone";
            user.Specialization = "none";
            user.IsBlocked = false;

            return user;
        }
    }
}
