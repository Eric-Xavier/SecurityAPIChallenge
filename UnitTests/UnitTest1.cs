using ApiClient.Interfaces;
using ApiClient.Models;
using ApiClient.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Xunit;
using Moq;

namespace UnitTests
{
    public class UnitTest1
    {
        private readonly Mock<ILogger<SecurityService>> logger;
        private readonly IConfiguration configuration;

        public UnitTest1()
        {
            logger = new Mock<ILogger<SecurityService>>();

            var appSettings = new Dictionary<string, string>(){
                 {SecurityService.ServiceApiUrlConfigName, "http://fakeurl.com/{param1}" }
         };

            configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(appSettings)
                .Build();


        }





        [Fact]
        public void HappyPathTest()
        {
            #region arrange
            var handler = new DummyHttpMessageHandler();
            var _httpClient = new HttpClient(handler);

            var _repo = new Mock<IRepository>();
            _repo.Setup(r => r.Insert(It.IsAny<SecurityModel>()))
                .ReturnsAsync(true);
            #endregion

            #region Act
            var service = new SecurityService(_httpClient, _repo.Object, logger.Object, configuration);
            var result = service.ExecuteAsync(new List<string>() { "abcdefghijkl" });
            #endregion


            #region Result
            Assert.NotNull(result);
            #endregion


        }


        [Theory]
        [MemberData(nameof(ValidationScenariosMemberData))]
        public async Task ValidationScenarios(string[] expectedScenarios, bool expectedResult)
        {
            #region arrange
            var handler = new DummyHttpMessageHandler();
            var _httpClient = new HttpClient(handler);

            var _repo = new Mock<IRepository>();
            _repo.Setup(r => r.Insert(It.IsAny<SecurityModel>()))
                .ReturnsAsync(true);
            #endregion

            #region Act
            var service = new SecurityService(_httpClient, _repo.Object, logger.Object, configuration);
            var result = await service.ExecuteAsync(expectedScenarios);

            Assert.Equal(expectedResult, result.First().Value);
            #endregion
        }

        [Theory]
        [MemberData(nameof(ValidationSkippedScenariosMemberData))]
        public async Task ValidateSkippedScenarios(string[] expectedScenarios, bool expectedResult)
        {
            #region arrange
            var handler = new DummyHttpMessageHandler();
            var _httpClient = new HttpClient(handler);

            var _repo = new Mock<IRepository>();
            _repo.Setup(r => r.Insert(It.IsAny<SecurityModel>()))
                .ReturnsAsync(true);
            #endregion

            #region Act
            var service = new SecurityService(_httpClient, _repo.Object, logger.Object, configuration);
            var result = await service.ExecuteAsync(expectedScenarios);

            Assert.Empty(result);
            #endregion
        }


        public static IEnumerable<object[]> ValidationScenariosMemberData()
        {

            yield return new object[] { new string[1] { "" }, false };
            yield return new object[] { new string[1] { "   " }, false };
            yield return new object[] { new string[1] { "abc" }, false };
            yield return new object[] { new string[1] { "abcdefghijkl" }, true };

        }

        public static IEnumerable<object[]> ValidationSkippedScenariosMemberData()
        {
            yield return new object[] { new string[0] { }, false };
            yield return new object[] { new string[1] { null }, false };

        }



        [Fact]
        public void NotFoundSecurityCodeTestAsync()
        {
            #region arrange
            var moq = new Mock<ISecurityService>();

            var handler = new DummyHttpMessageHandler();
            var _httpClient = new HttpClient(handler);

            var _repo = new Mock<IRepository>();
            _repo.Setup(r => r.Insert(It.IsAny<SecurityModel>()))
                .ReturnsAsync(true);
            #endregion

            #region Act
            var service = new SecurityService(_httpClient, _repo.Object, logger.Object, configuration);
            #endregion


            #region Result
            _ = Assert.ThrowsAsync<ApplicationException>(async () => await service.ExecuteAsync(It.IsAny<List<string>>()));
            #endregion


        }


        [Fact]
        public void InvalidSecurityCodeTestAsync()
        {
            #region arrange
            var moq = new Mock<ISecurityService>();
            moq.Setup(m => m.ExecuteAsync(It.IsAny<List<string>>()))
                .Throws<ApplicationException>().Verifiable("ISIN not found");

            var handler = new DummyHttpMessageHandler();
            var _httpClient = new HttpClient(handler);

            var _repo = new Mock<IRepository>();
            _repo.Setup(r => r.Insert(It.IsAny<SecurityModel>()))
                .ReturnsAsync(true);
            #endregion

            #region Act
            var service = new SecurityService(_httpClient, _repo.Object, logger.Object, configuration);
            #endregion


            #region Result
            _ = Assert.ThrowsAsync<ApplicationException>(() => service.ExecuteAsync(It.IsAny<List<string>>()));
            #endregion


        }
    }
}