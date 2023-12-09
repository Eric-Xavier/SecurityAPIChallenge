using ApiClient.Interfaces;
using ApiClient.Models;
using ApiClient.Services;
using Microsoft.Extensions.Logging;
using Xunit;
using Moq;
using ApiClient.Helper;
using ApiClient.Models.Enums;


namespace UnitTests
{
    public class UnitTest1
    {
        private readonly Mock<ILogger<SecurityOrchestratorService>> logger;
        private Mock<IStockService> _stockService;
        

        public UnitTest1()
        {
            logger = new Mock<ILogger<SecurityOrchestratorService>>();
            _stockService = new Mock<IStockService>();
            
            _stockService.Setup(x=>x.GetSecurityPrice(It.IsAny<string>()))
                .ReturnsAsync(new SecurityModel{ ISINCode = "US4581401001", Price = 9999.99m });
        }




        [Fact]
        public void HappyPathTest()
        {
            #region arrange

            var _repo = new Mock<IRepository>();
            _repo.Setup(r => r.Insert(It.IsAny<SecurityModel>()))
                .ReturnsAsync(true);

            #endregion

            #region Act
            var service = new SecurityOrchestratorService(_repo.Object, _stockService.Object, logger.Object);
            var result = service.ExecuteAsync(new List<string>() { "US4581401001" });
            #endregion


            #region Result
            Assert.NotNull(result);
            #endregion


        }


        [Theory]
        [MemberData(nameof(ValidationScenariosMemberData))]
        public async Task ValidationScenarios(string[] expectedScenarios, string expectedResult)
        {
            #region arrange
            
            var _repo = new Mock<IRepository>();
            _repo.Setup(r => r.Insert(It.IsAny<SecurityModel>()))
                .ReturnsAsync(true);
            
            
            #endregion

            #region Act
            var service = new SecurityOrchestratorService(_repo.Object, _stockService.Object, logger.Object);
            var result = await service.ExecuteAsync(expectedScenarios).ToArrayAsync();

            Assert.Equal(expectedResult, result.First().status);
            #endregion
        }

        [Theory]
        [MemberData(nameof(ValidationSkippedScenariosMemberData))]
        public async Task ValidateSkippedScenarios(string[] expectedScenarios)
        {
            #region Arrange
            var _repo = new Mock<IRepository>();
            _repo.Setup(r => r.Insert(It.IsAny<SecurityModel>()))
                .ReturnsAsync(true);

            _stockService.Setup(x=>x.GetSecurityPrice(It.IsAny<string>()))
                .ReturnsAsync(new SecurityModel{ ISINCode = "ABCDEFGHIJKL", Price = 1.99m });
            #endregion

            #region Act
            var service = new SecurityOrchestratorService(_repo.Object, _stockService.Object, logger.Object);
            var result = await service.ExecuteAsync(expectedScenarios).ToListAsync();
            #endregion

            #region Result
            Assert.Empty(result.Where(x=>x.status == ErrorCodes.NoError.ToString()));
            #endregion
        }

        [Fact]
        public async void NullSecurityListScenarioTestAsync()
        {
            #region arrange
            var moq = new Mock<ISecurityService>();

            var _repo = new Mock<IRepository>();
            _repo.Setup(r => r.Insert(It.IsAny<SecurityModel>()))
                .ReturnsAsync(true);

            _stockService.Setup(x=>x.GetSecurityPrice(It.IsAny<string>()))
                .ReturnsAsync(new SecurityModel{ ISINCode = "ABCDEFGHIJKL", Price = 1.99m });
            #endregion

            #region Act
            var service = new SecurityOrchestratorService(_repo.Object, _stockService.Object, logger.Object);
            //var result = await service.ExecuteAsync(It.IsAny<List<string>>()).ToArrayAsync();
            #endregion


            #region Result
            _ = Assert.ThrowsAsync<NullReferenceException>(async () => 
                    await service.ExecuteAsync(It.IsAny<List<string>>()).ToArrayAsync());
            #endregion

        }




        [Theory]
        [InlineData("asdf", false)]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("abcabcabcabca", false)]
        [InlineData("abcabcabcabc", true)]
        public void ValidateModel(string isin, bool result)
        {
            Assert.Equal(result, ValidatorHelper.ValidateSecurityCode(isin));
        }



        #region  ArrayRange

        public static IEnumerable<object[]> ValidationScenariosMemberData()
        {

            yield return new object[] { new string[1] { "" }, ErrorCodes.NotValidSecurityCode.ToString() };
            yield return new object[] { new string[1] { "   " }, ErrorCodes.NotValidSecurityCode.ToString() };
            yield return new object[] { new string[1] { "abc" }, ErrorCodes.NotValidSecurityCode.ToString() };
            yield return new object[] { new string[1] { "abcdefghijkl" }, ErrorCodes.NoError.ToString() };

        }

        public static IEnumerable<object[]> ValidationSkippedScenariosMemberData()
        {
            yield return new object[] { new string[0] { } };
            yield return new object[] { new string[1] { null } };

        }



        #endregion




    }
}