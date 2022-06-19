using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using PSW;
using PSW.DTO;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PSWIntegrationTests
{
    public class UserTests : IClassFixture<IntegrationTestsFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly IntegrationTestsFactory<Startup> _factory;

        public HttpClient createClient()
        {
            WebApplicationFactoryClientOptions clientOptions = new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = true,
                BaseAddress = new Uri("http://localhost:8000"),
                HandleCookies = true
            };

            HttpClient client = _factory.CreateClient(clientOptions);
            return client;

        }


        public UserTests(IntegrationTestsFactory<Startup> factory)
        {
            _factory = factory;
        }


        [Theory]
        [InlineData("/users", "OK")]
        [InlineData("/users/drugs/Brufen/1", "OK")]
        [InlineData("/users/drugs/{name}/{quantity}", "BadRequest")]
        public async Task Get_Http_Request(string url, string expectedStatusCode)
        {
            HttpClient client = createClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6Im1hamEiLCJuYmYiOjE2NTUzMjI0NTEsImV4cCI6MTY1NTMyNjA1MSwiaWF0IjoxNjU1MzIyNDUxLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo0NDM0MSIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NDIwMCJ9.a24bTPQaUv2htFt9De_kO97xlmJiK5dTHBBXNT5Zw3A");
            HttpResponseMessage response = await client.GetAsync(url);
            Assert.Equal(expectedStatusCode, response.StatusCode.ToString());
        }

        [Theory]
        [InlineData("/users", "Unauthorized")]
        [InlineData("/users/suspicious", "Unauthorized")]
        public async Task Get_Http_Request_401(string url, string expectedStatusCode)
        {
            HttpClient client = createClient();
            HttpResponseMessage response = await client.GetAsync(url);
            Assert.Equal(expectedStatusCode, response.StatusCode.ToString());
        }

        [Theory]
        [InlineData("/users/registration", "OK")]
        public async Task Create_New_User_Successfully(string url, string expectedStatusCode)
        {
            Guid username = Guid.NewGuid();
            HttpClient client = createClient();
            UserRegistrationDTO userDTO = new UserRegistrationDTO
            {
                FirstName = "firstname",
                LastName = "lastname",
                Address = "address",
                Country = "country",
                DateOfBirth = "birthday",
                Username = username.ToString(),
                PhoneNumber = "phone",
                Password = "password"
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6Im1hamEiLCJuYmYiOjE2NTUzMjI0NTEsImV4cCI6MTY1NTMyNjA1MSwiaWF0IjoxNjU1MzIyNDUxLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo0NDM0MSIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NDIwMCJ9.a24bTPQaUv2htFt9De_kO97xlmJiK5dTHBBXNT5Zw3A");
            HttpResponseMessage response = await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(userDTO), Encoding.UTF8, "application/json"));
            Assert.Equal(expectedStatusCode, response.StatusCode.ToString());
        }


        [Theory]
        [InlineData("/users/login", "OK")]
        public async Task Login_User(string url, string expectedStatusCode)
        {
            HttpClient client = createClient();
            UserLoginRequestDTO userLoginRequestDTO = new UserLoginRequestDTO
            {
                Username = "username3",
                Password = "password3"
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6Im1hamEiLCJuYmYiOjE2NTUzMjI0NTEsImV4cCI6MTY1NTMyNjA1MSwiaWF0IjoxNjU1MzIyNDUxLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo0NDM0MSIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NDIwMCJ9.a24bTPQaUv2htFt9De_kO97xlmJiK5dTHBBXNT5Zw3A");
            HttpResponseMessage response = await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(userLoginRequestDTO), Encoding.UTF8, "application/json"));
            Assert.Equal(expectedStatusCode, response.StatusCode.ToString());
        }

        [Theory]
        [InlineData("/users/block/3", "OK")]
        public async Task Block_User(string url, string expectedStatusCode)
        {
            HttpClient client = createClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6Im1hamEiLCJuYmYiOjE2NTUzMjI0NTEsImV4cCI6MTY1NTMyNjA1MSwiaWF0IjoxNjU1MzIyNDUxLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo0NDM0MSIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NDIwMCJ9.a24bTPQaUv2htFt9De_kO97xlmJiK5dTHBBXNT5Zw3A");
            HttpResponseMessage response = await client.PutAsync(url, null);
            Assert.Equal(expectedStatusCode, response.StatusCode.ToString());
        }

        [Theory]
        [InlineData("/users/unblock/3", "OK")]
        public async Task Unblock_User(string url, string expectedStatusCode)
        {
            HttpClient client = createClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6Im1hamEiLCJuYmYiOjE2NTUzMjI0NTEsImV4cCI6MTY1NTMyNjA1MSwiaWF0IjoxNjU1MzIyNDUxLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo0NDM0MSIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NDIwMCJ9.a24bTPQaUv2htFt9De_kO97xlmJiK5dTHBBXNT5Zw3A");
            HttpResponseMessage response = await client.PutAsync(url, null);
            Assert.Equal(expectedStatusCode, response.StatusCode.ToString());
        }

    }
}
