using Moq;
using PSW.DTO;
using PSW.Model;
using PSW.Repository.Interface;
using PSW.Service;
using Shouldly;
using System.Collections.Generic;
using Xunit;

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
            string userId = "1";
            User userDTO = new User
            {
                Id = userId
            };

            _userRepoMoq.Setup(x => x.GetUserById(userId)).Returns(userDTO);

            User user = _userService.GetUserById(userId);

            Assert.Equal(userId, user.Id);
        }

        [Fact]
        public void Get_by_invalid_id()
        {
            Mock<IUserRepository> userRepository = CreateUserRepository();
            UserService service = new UserService(userRepository.Object, _medicineRepoMoq.Object, _termService);

            User user = _userService.GetUserById("10");
            user.ShouldBe(null);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Register_user(User user, bool notRegistered)
        {
            Mock<IUserRepository> userRepository = CreateUserRepository();
            UserService service = new UserService(userRepository.Object, _medicineRepoMoq.Object, _termService);

            bool registered = service.CreateUser(user);

            registered.ShouldBe(notRegistered);
        }


        [Fact]
        public void Login_With_Valid_Credentials()
        {
            Mock<IUserRepository> userRepository = CreateUserRepository();
            UserService service = new UserService(userRepository.Object, _medicineRepoMoq.Object, _termService);
            UserLoginRequestDTO credentials = new UserLoginRequestDTO
            {
                Username = "pera",
                Password = "pera123"
            };

            User user = service.GetUserByLoginCredentials(credentials);

            user.ShouldNotBe(null);
        }

        [Fact]
        public void Login_With_Invalid_Credentials()
        {
            Mock<IUserRepository> userRepository = CreateUserRepository();
            UserService service = new UserService(userRepository.Object, _medicineRepoMoq.Object, _termService);
            UserLoginRequestDTO credentials = new UserLoginRequestDTO
            {
                Username = "pera",
                Password = "pera"
            };

            User user = service.GetUserByLoginCredentials(credentials);

            user.ShouldBe(null);
        }


        [Fact]
        public void Block_User()
        {
            Mock<IUserRepository> userRepository = CreateUserRepository();
            UserService service = new UserService(userRepository.Object, _medicineRepoMoq.Object, _termService);
            bool result = service.BlockUser("1");
            result.ShouldBeFalse();
        }

        [Fact]
        public void Unblock_User()
        {
            Mock<IUserRepository> userRepository = CreateUserRepository();
            UserService service = new UserService(userRepository.Object, _medicineRepoMoq.Object, _termService);
            bool result = service.UnblockUser("1");
            result.ShouldBeFalse();
        }

        private static Mock<IUserRepository> CreateUserRepository()
        {
            Mock<IUserRepository> userRepository = new Mock<IUserRepository>();
            List<User> users = new List<User>();
            User pera = CreateUser("1", "pera", "pera123");
            users.Add(pera);

            userRepository.Setup(x => x.GetAll()).Returns(users);

            return userRepository;
        }


        public static IEnumerable<object[]> Data()
        {
            List<object[]> retval = new List<object[]>
            {
                new object[] { CreateUser("1", "pera", "123"), false },
                new object[] { CreateUser("2", "mika", "123"), false }
            };

            return retval;
        }

        private static User CreateUser(string id, string username, string password)
        {
            User user = new User
            {
                Id = id,
                FirstName = "name",
                LastName = "surname",
                Username = username,
                Password = password,
                Role = User.UserRole.Client.ToString(),
                DateOfBirth = "birthday",
                Address = "address",
                Country = "country",
                PhoneNumber = "phone",
                Specialization = "none",
                IsBlocked = false
            };

            return user;
        }
    }
}
