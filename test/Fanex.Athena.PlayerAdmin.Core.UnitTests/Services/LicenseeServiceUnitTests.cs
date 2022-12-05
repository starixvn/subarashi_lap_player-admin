
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Repositories;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Athena.PlayerAdmin.Core.Settings;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using Fanex.Data;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Services
{
    public class LicenseeServiceUnitTests
    {
        private IMemoryCache _cache;
        private IDbFactory _dbFactory;
        private IObjectDb _objectDb;
        private IDataReader _reader;
        private AppSetting _appSetting;
        private FunctionAllowedProductsSetting _functionSetting;

        [SetUp]
        public void SetUp()
        {
            _cache = Substitute.For<IMemoryCache>();
            _dbFactory = Substitute.For<IDbFactory>();
            _objectDb = Substitute.For<IObjectDb>();
            _reader = Substitute.For<IDataReader>();
            _dbFactory.GetObjectDb(default, default).ReturnsForAnyArgs(_objectDb);
            _objectDb.ExecuteReaderAsync(default, default).ReturnsForAnyArgs(_reader);
            _appSetting = FakeHelper.Create<AppSetting>();
            _functionSetting = FakeHelper.Create<FunctionAllowedProductsSetting>();
        }

        [Test]
        public async Task IsAllowUpdateBA_WhenCallWithSpecialBAAdminsContainUserName_ReturnTrue()
        {
            //Arrange
            var service = CreateService();
            var spectialBAAdmins = FakeHelper.CreateMany<string>();
            var userName = spectialBAAdmins.First();

            //Act
            var result = await service.IsAllowUpdateBA(1, userName, spectialBAAdmins);

            //Assert
            Assert.IsTrue(result);
            _dbFactory.DidNotReceiveWithAnyArgs().GetObjectDb(default);
        }

        [Test]
        public async Task IsAllowUpdateBA_WhenCallWithLicenseeAllowUpdateBA_ReturnTrue()
        {
            //Arrange
            _objectDb.GetParameterValue("isallow").Returns(true);
            var service = CreateService();

            //Act
            var result = await service.IsAllowUpdateBA(1, default, null);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void GetAllowedProductsByFunction_WhenCallWithFunctionNotInFunctionSetting_ReturnNull()
        {
            //Arrange
            var functionName = "abcxyz";
            var serivce = CreateService();

            //Act
            var result = serivce.GetAllowedProductsByFunction(functionName);

            //Assert
            Assert.IsNull(result);
        }

        [Test]
        public void GetAllowedProductsByFunction_WhenCallWithFunctionInFuctionSetting_ReturnNotNull()
        {
            //Arrange
            var function = "Default";
            var service = CreateService();

            //Act
            var result = service.GetAllowedProductsByFunction(function);

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetProductAccess_WhenCallWithNoAllowProductByFunction_ReturnProductsOfferByLinceseeOnly()
        {
            //Arrange
            _functionSetting.Default = null;
            var offerdProduct = FakeHelper.Create<Product>();
            _cache.TryGetValue(default, out var products).ReturnsForAnyArgs(false);
            _reader.Read().Returns(true, false);
            SetUpReturnProductFromDB(_reader, offerdProduct);
            var service = CreateService();

            //Act
            var result = await service.GetProductAccess(1, "Default");

            //Assert
            Assert.That(result.ProductList.Count, Is.EqualTo(2));
            Assert.That(result.ProductList[1].ProductID, Is.EqualTo(offerdProduct.ProductID));
        }

        [Test]
        public async Task GetProductAccess_WhenCallWithAlowProductByFunction_ReturnOfferProductFilterByFuncion()
        {
            //Arrange
            var offerdProduct = FakeHelper.Create<Product>();
            _functionSetting.Default = new string[] { offerdProduct.ProductCode };
            _cache.TryGetValue(default, out var products).ReturnsForAnyArgs(false);
            _reader.Read().Returns(true, false);
            SetUpReturnProductFromDB(_reader, offerdProduct);
            var service = CreateService();

            //Act
            var result = await service.GetProductAccess(1, "Default");

            //Assert
            Assert.True(result.ProductCodeList.Contains(offerdProduct.ProductCode));
        }

        [Test]
        public async Task GetSingleWalletProductAccess_WhenCall_ReturnSingleWalletProduct()
        {
            //Arrange
            var offerdProduct = FakeHelper.Create<Product>();
            _functionSetting.Default = new string[] { offerdProduct.ProductCode };
            offerdProduct.IsOfferedSingleWallet = true;
            _cache.TryGetValue(default, out var products).ReturnsForAnyArgs(false);
            _reader.Read().Returns(true, false);
            SetUpReturnProductFromDB(_reader, offerdProduct);
            var service = CreateService();

            //Act
            var result = await service.GetSingleWalletProductAccess(1, "Default");

            //Assert
            Assert.That(result.ProductList.Count, Is.EqualTo(1));
            Assert.True(result.ProductCodeList.Contains(offerdProduct.ProductCode));
        }

        [TestCase(null)]
        [TestCase(new int[] {1, 2, 3})]
        public async Task CheckProductBelongsTo_WhenCallWithProductNotBelongLimitedProductIds_ReturnFalse(int[] limitedProductIds )
        {
            //Arrange
            var productId = 6;

            var service = CreateService();

            //Act
            var result = await service.CheckProductBelongsTo("12B", productId, limitedProductIds);

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task CheckProductBelongsTo_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            var limitedProductIds = new int[] { 1, 2, 3 };
            var productId = 1;
            var licenseeName = "12B";
            var expectedResult = true;
            _objectDb.ExecuteScalarAsync().Returns(expectedResult);
            _cache.TryGetValue(default, out var outResult).ReturnsForAnyArgs(false);

            var service = CreateService();

            //Act
            var result = await service.CheckProductBelongsTo(licenseeName, productId, limitedProductIds);

            //Assert
            Assert.Multiple(() =>
            {
                _dbFactory.Received(1).GetObjectDb("Lrf_CheckBelonging_ValidateProduct");
                _objectDb.Received(1).SetParameterValue("productId", productId);
                _objectDb.Received(1).SetParameterValue("licenseeName", licenseeName);
                Assert.IsTrue(result);
            });
        }

        [Test]
        public async Task CheckCustomerBelongTo_WhenCallWithCustIdIsZero_ReturnTrue()
        {
            //Arrange
            var service = CreateService();
            var agentList = FakeHelper.Create<IReadOnlyDictionary<int, int[]>>();

            //Act
            var result = await service.CheckCustomerBelongTo(agentList, custId: 0, 0);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task CheckCustomerBelongTo_WhenDBNotReturnAnyData_ReturnFalse()
        {
            //Arrange
            var custId = 1;
            var agentList = FakeHelper.Create<IReadOnlyDictionary<int, int[]>>();
            _reader.Read().Returns(false);
            var service = CreateService();

            //Act
            var result = await service.CheckCustomerBelongTo(agentList, custId, 0);

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task CheckCustomerBelongTo_WhenDBHaveResultReturn_ReturnCorreclyResult()
        {
            //Arrange
            var agentList = FakeHelper.Create<IReadOnlyDictionary<int, int[]>>();
            var firstAgent = agentList.First();
            _reader["CurrencyId"].Returns(firstAgent.Key);
            _reader["CustId"].Returns(firstAgent.Value.First());
            _reader.Read().Returns(true, false);

            var service = CreateService();

            //Act
            var result = await service.CheckCustomerBelongTo(agentList, 1);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task GetCustomersBelongTo_WhenAgentListNotContainAnyCustomer_ReturnEmptyListCustId()
        {
            //Arrange
            var agentList = new ReadOnlyDictionary<int, int[]>(new Dictionary<int, int[]>());
            var customers = FakeHelper.CreateMany<Customer>();
            SetUpReturnCustomerFromDB(_reader, customers.ToArray());

            var service = CreateService();

            //Act
            var result = await service.GetCustomersBelongTo(agentList, string.Empty);

            //Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetCustomersBelongTo_WhenAgentListContainCustomer_ReturnNotEmptyListCustId()
        {
            //Arrange
            var customer = FakeHelper.Create<Customer>();
            var dicAgentList = new Dictionary<int, int[]>();
            dicAgentList.Add(customer.CurrencyId, new int[] { customer.AgentId });

            _reader.Read().Returns(true, false);
            SetUpReturnCustomerFromDB(_reader, customer);

            var service = CreateService();

            //Act
            var result = await service.GetCustomersBelongTo(new ReadOnlyDictionary<int, int[]>(dicAgentList), string.Empty);

            //Assert
            Assert.IsNotEmpty(result);
        }

        [Test]
        public async Task GetBalanceUp_WhenCallWithNoCache_CallRelevantMethod()
        {
            //Arrange
            _cache.TryGetValue(default, out var products).ReturnsForAnyArgs(false);
            var banlanceUp = FakeHelper.Create<BalanceUpData>();
            _reader["balanceupday"].Returns(banlanceUp.BalanceUpDay);
            _reader["movebetsday"].Returns(banlanceUp.MoveBetsDay);
            _reader.Read().Returns(true, false);

            var service = CreateService();

            //Act
            await service.GetBalanceUp();

            //Assert
            Assert.Multiple(() =>
            {
                _dbFactory.Received(1).GetObjectDb("Lrf_SelBalanceUpDay");
            });
        }

        [TestCase(0)]
        [TestCase(1)]
        public void GetAllowedSubProductOptions_WhenCall_WithEmptyOfferProductOrProductOpionsNotContainSportBook_ReturnCalledProductOptions(int offerProductCount)
        {
            //Arrange
            var offerProducts = FakeHelper.CreateMany<string>(offerProductCount);
            var productionOptions = FakeHelper.CreateMany<ProductOption>(1);
            productionOptions.First().Code = "test";

            var service = CreateService();

            //Act
            var result = service.GetAllowedSubProductOptions(offerProducts, productionOptions);

            //Assert
            Assert.That(result, Is.SameAs(productionOptions));
        }

        [Test]
        public void GetAllowedSubProductOptions_WhenProductOptionsHaveSportBook_ReturnCorrectlyResult()
        {
            //Arrange
            var offerProducts = FakeHelper.CreateMany<string>(3);
            var productionOptions = FakeHelper.CreateMany<ProductOption>(3);
            productionOptions.First().Code = ProductCode.SB;

            var service = CreateService();

            //Act
            var result = service.GetAllowedSubProductOptions(offerProducts, productionOptions);

            //Assert
            Assert.That(result.Count(), Is.EqualTo(6));
        }

        [TestCase(0)]
        [TestCase(1)]
        public void GetAllowedSubProductCodeList_WhenCall_WithEmptyOfferProductOrProductCodesNotContainSportBook_ReturnCalledProductCodes(int offerProductCount)
        {
            //Arrange
            var offerProducts = FakeHelper.CreateMany<string>(offerProductCount);
            var productCode = "1,2,3";

            var service = CreateService();

            //Act
            var result = service.GetAllowedSubProductCodeList(offerProducts, productCode);

            //Assert
            Assert.That(result, Is.SameAs(productCode));
        }

        [Test]
        public void GetAllowedSubProductCodeList_WhenProductCodeHaveSportBook_ReturnCorrectlyResult()
        {
            //Arrange
            var offerProducts = FakeHelper.CreateMany<string>(3);
            var productCodes = $"1,{ProductCode.SB},3";
            var expectedResult = productCodes + "," + string.Join(",", offerProducts);

            var service = CreateService();

            //Act
            var result = service.GetAllowedSubProductCodeList(offerProducts, productCodes);

            //Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }


        public LicenseeService CreateService()
        {
            var appSettingOption = Substitute.For<IOptions<AppSetting>>();
            var functionSettingOption = Substitute.For<IOptions<FunctionAllowedProductsSetting>>();
            appSettingOption.Value.Returns(_appSetting);
            functionSettingOption.Value.Returns(_functionSetting);
            
            return new LicenseeService(_cache, _dbFactory, appSettingOption, functionSettingOption);
        }

        private void SetUpReturnProductFromDB(IDataReader reader, params Product[] products)
        {
            foreach (var product in products)
            {
                reader["ProductID"].Returns(product.ProductID);
                reader["productprefix"].Returns(product.ProductCode);
                reader["comm"].Returns(product.AllowWinloss);
                reader["winloss"].Returns(product.AllowWinloss);
                reader["IsOfferedSingleWallet"].Returns(product.IsOfferedSingleWallet);
            }
        }

        private void SetUpReturnCustomerFromDB(IDataReader reader, params Customer[] customers)
        {
            foreach (var customer in customers)
            {
                reader["CustId"].Returns(customer.Id);
                reader["agentid"].Returns(customer.AgentId);
                reader["CurrencyId"].Returns(customer.CurrencyId);
            }
        }
    }
}
