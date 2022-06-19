using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using PSW;
using PSW.Model;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PSWIntegrationTests
{
    public class CommentTests : IClassFixture<IntegrationTestsFactory<Startup>>
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


        public CommentTests(IntegrationTestsFactory<Startup> factory)
        {
            _factory = factory;
        }


        [Theory]
        [InlineData("/comments", "OK")]
        [InlineData("/comments/1", "OK")]
        public async Task Get_Http_Request(string url, string expectedStatusCode)
        {
            HttpClient client = createClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6Im1hamEiLCJuYmYiOjE2NTUzMjI0NTEsImV4cCI6MTY1NTMyNjA1MSwiaWF0IjoxNjU1MzIyNDUxLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo0NDM0MSIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NDIwMCJ9.a24bTPQaUv2htFt9De_kO97xlmJiK5dTHBBXNT5Zw3A");
            HttpResponseMessage response = await client.GetAsync(url);
            Assert.Equal(expectedStatusCode, response.StatusCode.ToString());
        }

        [Theory]
        [InlineData("/comments", "Unauthorized")]
        [InlineData("/comments/1", "Unauthorized")]
        public async Task Get_Http_Request_401(string url, string expectedStatusCode)
        {
            HttpClient client = createClient();
            HttpResponseMessage response = await client.GetAsync(url);
            Assert.Equal(expectedStatusCode, response.StatusCode.ToString());
        }

        [Theory]
        [InlineData("/comments", "OK")]
        public async Task Create_Comment_Successfully(string url, string expectedStatusCode)
        {
            HttpClient client = createClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6Im1hamEiLCJuYmYiOjE2NTUzMjI0NTEsImV4cCI6MTY1NTMyNjA1MSwiaWF0IjoxNjU1MzIyNDUxLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo0NDM0MSIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NDIwMCJ9.a24bTPQaUv2htFt9De_kO97xlmJiK5dTHBBXNT5Zw3A");
            Comment comment = new Comment
            {
                Id = "1",
                Grade = 3,
                Content = "Comment",
                UserId = "1",
                TermId = "1"
            };
            HttpResponseMessage response = await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(comment), Encoding.UTF8, "application/json"));
            Assert.Equal(expectedStatusCode, response.StatusCode.ToString());
        }
    }
}
