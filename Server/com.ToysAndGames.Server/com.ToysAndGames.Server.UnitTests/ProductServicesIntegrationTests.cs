using com.ToysAndGames.Server.WebAPI.DAL.Models;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace com.ToysAndGames.Server.UnitTests
{
    public class ProductServicesIntegrationTests : IDisposable
    {
        private readonly TestClientProvider clientProvider;

        public ProductServicesIntegrationTests()
        {
            this.clientProvider = new TestClientProvider();
        }

        [Fact]
        public async Task TestReadMethods()
        {
            var getByIdResult = await clientProvider.Client.GetAsync("/api/Product/GetById/1");

            Assert.Equal(HttpStatusCode.OK, getByIdResult.StatusCode);

            var getAllResult = await clientProvider.Client.GetAsync("/api/Product/GetAll");

            Assert.Equal(HttpStatusCode.OK, getAllResult.StatusCode);
        }

        [Fact]
        public async Task ReturnNotFoundForUnexistantProductIds()
        {
            var getByIdResult = await clientProvider.Client.GetAsync("/api/Product/GetById/404");

            Assert.Equal(HttpStatusCode.NotFound, getByIdResult.StatusCode);
        }

        [Fact]
        public async Task ReturnOkWhenCreatingValidProduct()
        {
            var validProduct = new Product()
            {
                Id = 0,
                Name = "Mock Product",
                Description = "Mock Description",
                AgeRestriction = 15,
                Company = "Mock Company",
                Price = 99
            };
            var json = JsonConvert.SerializeObject(validProduct);

            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{clientProvider.Client.BaseAddress}api/Product/Create"),
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var result = await clientProvider.Client.SendAsync(httpRequestMessage);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task ReturnOkWhenUpdatingValidProduct()
        {
            var validProduct = new Product()
            {
                Id = 1,
                Name = "Mock Product",
                Description = "Mock Description",
                AgeRestriction = 15,
                Company = "Mock Company",
                Price = 99
            };

            var json = JsonConvert.SerializeObject(validProduct);

            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"{clientProvider.Client.BaseAddress}api/Product/Update"),
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var result = await clientProvider.Client.SendAsync(httpRequestMessage);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task ReturnOkWhenDeletingValidProduct()
        {
            int validDeleteId = 2;

            var json = JsonConvert.SerializeObject(validDeleteId);

            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{clientProvider.Client.BaseAddress}api/Product/Delete/{validDeleteId}")
            };

            var result = await clientProvider.Client.SendAsync(httpRequestMessage);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        public void Dispose()
        {
            clientProvider.Dispose();
        }
    }
}