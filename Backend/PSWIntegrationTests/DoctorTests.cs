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
    public class DoctorTests : IClassFixture<IntegrationTestsFactory<Startup>>
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


        public DoctorTests(IntegrationTestsFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/doctors", "OK")]
        [InlineData("/doctors/specialist", "OK")]
        [InlineData("/doctors/nonspecialist", "OK")]
        [InlineData("/doctors/2", "OK")]
        [InlineData("/doctors/{2}", "BadRequest")]
        public async Task Get_Http_Request(string url, string expectedStatusCode)
        {
            HttpClient client = createClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6Im1hamEiLCJuYmYiOjE2NTUzMjI0NTEsImV4cCI6MTY1NTMyNjA1MSwiaWF0IjoxNjU1MzIyNDUxLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo0NDM0MSIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NDIwMCJ9.a24bTPQaUv2htFt9De_kO97xlmJiK5dTHBBXNT5Zw3A");
            HttpResponseMessage response = await client.GetAsync(url);
            Console.Out.Write(response.Content);
            Assert.Equal(expectedStatusCode, response.StatusCode.ToString());
        }

        [Theory]
        [InlineData("/doctors", "Unauthorized")]
        [InlineData("/doctors/specialist", "Unauthorized")]
        [InlineData("/doctors/nonspecialist", "Unauthorized")]
        [InlineData("/doctors/2", "Unauthorized")]
        public async Task Get_Http_Request_401(string url, string expectedStatusCode)
        {
            HttpClient client = createClient();
            HttpResponseMessage response = await client.GetAsync(url);
            Console.Out.Write(response.Content);
            Assert.Equal(expectedStatusCode, response.StatusCode.ToString());
        }

        [Theory]
        [InlineData("/doctors/recipe", "OK")]
        public async Task Create_Recipe_Successfully(string url, string expectedStatusCode)
        {
            HttpClient client = createClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6Im1hamEiLCJuYmYiOjE2NTUzMjI0NTEsImV4cCI6MTY1NTMyNjA1MSwiaWF0IjoxNjU1MzIyNDUxLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo0NDM0MSIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NDIwMCJ9.a24bTPQaUv2htFt9De_kO97xlmJiK5dTHBBXNT5Zw3A");
            RecipeDTO recipeDTO = new RecipeDTO
            {
                Doctor = "Doctor",
                Patient = "Patient",
                Medicine = "Brufen",
                Dose = 100
            };
            HttpResponseMessage response = await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(recipeDTO), Encoding.UTF8, "application/json"));
            Assert.Equal(expectedStatusCode, response.StatusCode.ToString());
        }

    }
}
