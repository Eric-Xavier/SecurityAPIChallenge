using ApiClient.Interfaces;
using ApiClient.Models;
using ApiClient.Services;
using Microsoft.Extensions.Logging;
using Xunit;
using Moq;
using ApiClient.Helper;


namespace UnitTests
{
    public class UnitTest1
    {
        private readonly Mock<ILogger<SecurityService>> logger;
        private Mock<IStockService> _stockService;
        

        public UnitTest1()
        {
            logger = new Mock<ILogger<SecurityService>>();
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
            var service = new SecurityService(_repo.Object, _stockService.Object, logger.Object);
            var result = service.ExecuteAsync(new List<string>() { "US4581401001" });
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
            
            var _repo = new Mock<IRepository>();
            _repo.Setup(r => r.Insert(It.IsAny<SecurityModel>()))
                .ReturnsAsync(true);
            
            
            #endregion

            #region Act
            var service = new SecurityService(_repo.Object, _stockService.Object, logger.Object);
            var result = await service.ExecuteAsync(expectedScenarios);

            Assert.Equal(expectedResult, result.First().Value);
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
            var service = new SecurityService(_repo.Object, _stockService.Object, logger.Object);
            var result = await service.ExecuteAsync(expectedScenarios);
            #endregion

            #region Result
            Assert.Empty(result.Where(x=>x.Value));
            #endregion
        }

        [Fact]
        public void NullSecurityListScenarioTestAsync()
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
            var service = new SecurityService(_repo.Object, _stockService.Object, logger.Object);
            #endregion


            #region Result
            _ = Assert.ThrowsAsync<ApplicationException>(async () => await service.ExecuteAsync(It.IsAny<List<string>>()));
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

            yield return new object[] { new string[1] { "" }, false };
            yield return new object[] { new string[1] { "   " }, false };
            yield return new object[] { new string[1] { "abc" }, false };
            yield return new object[] { new string[1] { "abcdefghijkl" }, true };

        }

        public static IEnumerable<object[]> ValidationSkippedScenariosMemberData()
        {
            yield return new object[] { new string[0] { } };
            yield return new object[] { new string[1] { null } };

        }



        #endregion




    }
}