using Microsoft.AspNetCore.Mvc.Testing;
using PSW;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Newtonsoft.Json;
using Shouldly;
using System.Collections.Generic;
using PSW.Model;

namespace PSWIntegrationTests
{
    public class TermTests : IClassFixture<IntegrationTestsFactory<Startup>>
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

            var client = _factory.CreateClient(clientOptions);
            return client;

        }


        public TermTests(IntegrationTestsFactory<Startup> factory)
        {
            _factory = factory;
        }


        [Theory]
        [InlineData("/terms/patient/3", "OK")]
        [InlineData("/terms/completed/3", "OK")]
        [InlineData("/terms/future/3", "OK")]
        [InlineData("/terms/doctor/3", "OK")]
        public async Task Get_http_request(string url, string expectedStatusCode)
        {
            var client = createClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6Im1hamEiLCJuYmYiOjE2NTUzMjI0NTEsImV4cCI6MTY1NTMyNjA1MSwiaWF0IjoxNjU1MzIyNDUxLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo0NDM0MSIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NDIwMCJ9.a24bTPQaUv2htFt9De_kO97xlmJiK5dTHBBXNT5Zw3A");
            var response = await client.GetAsync(url);
            Console.Out.Write(response.Content);
            Assert.Equal(expectedStatusCode, response.StatusCode.ToString());
        }

        [Theory]
        [InlineData("/terms/patient/3", "Unauthorized")]
        [InlineData("/terms/completed/3", "Unauthorized")]
        [InlineData("/terms/future/3", "Unauthorized")]
        [InlineData("/terms/doctor/3", "Unauthorized")]
        public async Task Get_http_request_401(string url, string expectedStatusCode)
        {
            var client = createClient();
            var response = await client.GetAsync(url);
            Console.Out.Write(response.Content);
            Assert.Equal(expectedStatusCode, response.StatusCode.ToString());
        }
    }
}
