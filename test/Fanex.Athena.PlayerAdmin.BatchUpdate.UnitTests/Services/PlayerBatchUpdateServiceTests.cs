

using Fanex.Athena.PlayerAdmin.BatchUpdate.Criteria;
using Fanex.Athena.PlayerAdmin.BatchUpdate.MetaModels;
using Fanex.Athena.PlayerAdmin.BatchUpdate.Repositories;
using Fanex.Athena.PlayerAdmin.BatchUpdate.Services;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Resources;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using MediatR;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BatchUpdate.UnitTests.Services
{
    public class PlayerBatchUpdateServiceTests
    {
        private const int TurnOffVIPErrorCode = 105;

        private IPublisher _publisher;
        private IPlayerBatchUpdateRepository _repository;
        private ILicenseeService _licenseeService;
        private IContextService _contextService;

        [SetUp]
        public void Setup()
        {
            _publisher = Substitute.For<IPublisher>();
            _repository = Substitute.For<IPlayerBatchUpdateRepository>();
            _licenseeService = Substitute.For<ILicenseeService>();
            _contextService= Substitute.For<IContextService>();
        }

        [Test]
        public async Task UpdateBAStatus_WhenEmptyCumtomers_ReturnSuccessfulResult()
        {
            //Arrange
            var metaModel = FakeHelper.Create<UpdateBAStatusApiMetaModel>();
            AssignCustomersToMetaModel(metaModel, Enumerable.Empty<Customer>());

            var service = CreateService();

            //Act
            var result = await service.UpdateBAStatus(metaModel);

            //Assert
            Assert.IsTrue(result.IsSuccessful);
        }

        [Test]
        public async Task UpdateBAStatus_WhenEmptyValidCustomers_ReturnSuccessfulResult()
        {
            //Arrange
            var metaModel = FakeHelper.Create<UpdateBAStatusApiMetaModel>();
            var customers = FakeHelper.CreateMany<Customer>();
            AssignCustomersToMetaModel(metaModel, customers);

            _licenseeService.GetCustomersBelongTo(default, default).ReturnsForAnyArgs(new List<int>());

            var service = CreateService();

            //Act
            var result = await service.UpdateBAStatus(metaModel);

            //Assert
            Assert.IsTrue(result.IsSuccessful);
            Assert.That(result.Message, Contains.Substring("Access Denied"));
        }

        [Test]
        public async Task UpdateBAStatus_WhenOldValuesContainAllBAStatusId_ReturnSuccessfulResult()
        {
            //Arrange
            var metaModel = FakeHelper.Create<UpdateBAStatusApiMetaModel>();
            var customers = FakeHelper.CreateMany<Customer>();
            AssignCustomersToMetaModel(metaModel, customers);
            _licenseeService.GetCustomersBelongTo(default, default).ReturnsForAnyArgs(customers.Select(_ => _.Id).ToList());
            _repository.GetOldValues(Arg.Any<ICollection<int>>(), GetOldValuesCriteria.BAStatusType)
                .Returns(customers.ToDictionary(_ => _.Id.ToString(), _ => metaModel.BAStatusId));

            var service = CreateService();

            //Act
            var result = await service.UpdateBAStatus(metaModel);

            //Assert
            Assert.IsTrue(result.IsSuccessful);
            Assert.AreEqual(ViewResources.UpdateSuccess, result.Message);
        }

        [Test]
        public async Task UpdateBAStatus_WhenRepoUpdateResultReturnTurnOffVIPErrorCode_ReturnResultMsgHaveMessageTurnOffVIPError()
        {
            //Arrange
            var metaModel = FakeHelper.Create<UpdateBAStatusApiMetaModel>();
            var customers = FakeHelper.CreateMany<Customer>();
            AssignCustomersToMetaModel(metaModel, customers);
            _licenseeService.GetCustomersBelongTo(default, default).ReturnsForAnyArgs(customers.Select(_ => _.Id).ToList());
            _repository.Update(default).ReturnsForAnyArgs(new ResultInfo { Code = TurnOffVIPErrorCode });
            var service = CreateService();

            //Act
            var result = await service.UpdateBAStatus(metaModel);

            //Assert
            Assert.That(result.Message, Contains.Substring(ViewResources.MessageTurnOffVIPError));
        }

        [Test]
        public async Task UpdateBAStatus_WhenRepoUpdateResultNotSuccessful_ReturnUnSuccessfulResult()
        {
            //Arrange
            var metaModel = FakeHelper.Create<UpdateBAStatusApiMetaModel>();
            var customers = FakeHelper.CreateMany<Customer>();
            AssignCustomersToMetaModel(metaModel, customers);
            metaModel.BAStatusId = "1";
            _licenseeService.GetCustomersBelongTo(default, default).ReturnsForAnyArgs(customers.Select(_ => _.Id).ToList());
            _repository.Update(default).ReturnsForAnyArgs(new ResultInfo { Code = 4, Message = "Error" });

            var service = CreateService();

            //Act
            var result = await service.UpdateBAStatus(metaModel);

            //Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.That(result.Code, Is.EqualTo(1));
            Assert.That(result.Message, Contains.Substring("Error"));
        }

        [Test]
        public async Task UpdateBAStatus_WhenRepoUpdateResultSuccessful_ReturnSuccessfulResult()
        {
            //Arrange
            var metaModel = FakeHelper.Create<UpdateBAStatusApiMetaModel>();
            var customers = FakeHelper.CreateMany<Customer>();
            AssignCustomersToMetaModel(metaModel, customers);
            _licenseeService.GetCustomersBelongTo(default, default).ReturnsForAnyArgs(customers.Select(_ => _.Id).ToList());
            _repository.Update(default).ReturnsForAnyArgs(new ResultInfo { Code = 0 });

            var service = CreateService();

            //Act
            var result = await service.UpdateBAStatus(metaModel);

            //Assert
            Assert.IsTrue(result.IsSuccessful);
            Assert.IsNotEmpty(result.ExtendedData);
        }

        [Test]
        public async Task UpdateOddsGroup_WhenEmptyCumtomers_ReturnSuccessfulResult()
        {
            //Arrange
            var metaModel = FakeHelper.Create<UpdateOddsGroupApiMetaModel>();
            AssignCustomersToMetaModel(metaModel, Enumerable.Empty<Customer>());

            var service = CreateService();

            //Act
            var result = await service.UpdateOddsGroup(metaModel);

            //Assert
            Assert.IsTrue(result.IsSuccessful);
        }

        [Test]
        public async Task UpdateOddsGroup_WhenEmptyValidCustomers_ReturnUnSuccessfulResult()
        {
            //Arrange
            var metaModel = FakeHelper.Create<UpdateOddsGroupApiMetaModel>();
            var customers = FakeHelper.CreateMany<Customer>();
            AssignCustomersToMetaModel(metaModel, customers);

            _licenseeService.GetCustomersBelongTo(default, default).ReturnsForAnyArgs(new List<int>());

            var service = CreateService();

            //Act
            var result = await service.UpdateOddsGroup(metaModel);

            //Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.That(result.Message, Contains.Substring("Access Denied"));
        }

        [Test]
        public async Task UpdateOddsGroup_WhenOldOddsGroupTypeContainAllOddGroupId_ReturnSuccessfulResult()
        {
            //Arrange
            var metaModel = FakeHelper.Create<UpdateOddsGroupApiMetaModel>();
            var customers = FakeHelper.CreateMany<Customer>();
            AssignCustomersToMetaModel(metaModel, customers);
            _licenseeService.GetCustomersBelongTo(default, default).ReturnsForAnyArgs(customers.Select(_ => _.Id).ToList());
            _repository.GetOldValues(Arg.Any<ICollection<int>>(), GetOldValuesCriteria.OddsGroupType)
                .Returns(customers.ToDictionary(_ => _.Id.ToString(), _ => metaModel.OddsGroupId));

            var service = CreateService();

            //Act
            var result = await service.UpdateOddsGroup(metaModel);

            //Assert
            Assert.IsTrue(result.IsSuccessful);
            Assert.AreEqual(ViewResources.UpdateSuccess, result.Message);
        }

        [Test]
        public async Task UpdateOddsGroup_WhenOldBAContainErrorId_ReturnBAErrorMsg()
        {
            //Arrange
            var metaModel = FakeHelper.Create<UpdateOddsGroupApiMetaModel>();
            var customers = FakeHelper.CreateMany<Customer>();
            AssignCustomersToMetaModel(metaModel, customers);
            _licenseeService.GetCustomersBelongTo(default, default).ReturnsForAnyArgs(customers.Select(_ => _.Id).ToList());
            _repository.GetOldValues(Arg.Any<ICollection<int>>(), GetOldValuesCriteria.BAStatusType)
                .Returns(customers.ToDictionary(_ => _.Id.ToString(), _ => "1"));

            var service = CreateService();

            //Act
            var result = await service.UpdateOddsGroup(metaModel);

            //Assert
            Assert.That(result.Message, Contains.Substring(ViewResources.MessageUpdateOddsGroupForBAPlayers));
        }

        [Test]
        public async Task UpdateOddsGroup_WhenRepoUpdateHaveOneError_ReturnUnSuccessfulResult()
        {
            //Arrange
            var metaModel = FakeHelper.Create<UpdateOddsGroupApiMetaModel>();
            var customers = FakeHelper.CreateMany<Customer>();
            AssignCustomersToMetaModel(metaModel, customers);
            _licenseeService.GetCustomersBelongTo(default, default).ReturnsForAnyArgs(customers.Select(_ => _.Id).ToList());
            _repository.Update(default).ReturnsForAnyArgs(new ResultInfo { Code = 1, Message = "Error" });

            var service = CreateService();

            //Act
            var result = await service.UpdateOddsGroup(metaModel);

            //Assert
            Assert.IsFalse(result.IsSuccessful);
        }

        [Test]
        public async Task UpdateOddsGroup_WhenRepoUpdateSuccessful_ReturnSuccessfulResult()
        {
            //Arrange
            var metaModel = FakeHelper.Create<UpdateOddsGroupApiMetaModel>();
            var customers = FakeHelper.CreateMany<Customer>();
            AssignCustomersToMetaModel(metaModel, customers);
            _licenseeService.GetCustomersBelongTo(default, default).ReturnsForAnyArgs(customers.Select(_ => _.Id).ToList());
            _repository.Update(default).ReturnsForAnyArgs(new ResultInfo { Code = 0 });

            var service = CreateService();

            //Act
            var result = await service.UpdateOddsGroup(metaModel);

            //Assert
            Assert.IsTrue(result.IsSuccessful);
        }

        [Test]
        public async Task UpdateSpecialLevel_WhenEmptyCumtomers_ReturnSuccessfulResult()
        {
            //Arrange
            var metaModel = FakeHelper.Create<UpdateSpecialLevelApiMetaModel>();
            AssignCustomersToMetaModel(metaModel, Enumerable.Empty<Customer>());

            var service = CreateService();

            //Act
            var result = await service.UpdateSpecialLevel(metaModel);

            //Assert
            Assert.IsTrue(result.IsSuccessful);
        }

        [Test]
        public async Task UUpdateSpecialLevel_WhenEmptyValidCustomers_ReturnSuccessfulResult()
        {
            //Arrange
            var metaModel = FakeHelper.Create<UpdateSpecialLevelApiMetaModel>();
            var customers = FakeHelper.CreateMany<Customer>();
            AssignCustomersToMetaModel(metaModel, customers);

            _licenseeService.GetCustomersBelongTo(default, default).ReturnsForAnyArgs(new List<int>());

            var service = CreateService();

            //Act
            var result = await service.UpdateSpecialLevel(metaModel);

            //Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.That(result.Message, Contains.Substring("Access Denied"));
        }

        [Test]
        public async Task UpdateSpecialLevel_WhenContainOldSpecialLevlId_ReturnSuccessfulResult()
        {
            //Arrange
            var metaModel = FakeHelper.Create<UpdateSpecialLevelApiMetaModel>();
            var customers = FakeHelper.CreateMany<Customer>();
            AssignCustomersToMetaModel(metaModel, customers);
            _licenseeService.GetCustomersBelongTo(default, default).ReturnsForAnyArgs(customers.Select(_ => _.Id).ToList());
            _repository.GetOldValues(Arg.Any<ICollection<int>>(), GetOldValuesCriteria.SpecialLevelType)
                .Returns(customers.ToDictionary(_ => _.Id.ToString(), _ => metaModel.SpecialLevelId));

            var service = CreateService();

            //Act
            var result = await service.UpdateSpecialLevel(metaModel);

            //Assert
            Assert.IsTrue(result.IsSuccessful);
            Assert.AreEqual(ViewResources.UpdateSuccess, result.Message);
        }

        [Test]
        public async Task UpdateSpecialLevel_WhenRepoUpdateResultNotSuccessful_ReturnUnSuccessfulResult()
        {
            //Arrange
            var metaModel = FakeHelper.Create<UpdateSpecialLevelApiMetaModel>();
            var customers = FakeHelper.CreateMany<Customer>();
            AssignCustomersToMetaModel(metaModel, customers);
            _licenseeService.GetCustomersBelongTo(default, default).ReturnsForAnyArgs(customers.Select(_ => _.Id).ToList());
            _repository.Update(default).ReturnsForAnyArgs(new ResultInfo { Code = 4, Message = "Error" });

            var service = CreateService();

            //Act
            var result = await service.UpdateSpecialLevel(metaModel);

            //Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.That(result.Code, Is.EqualTo(1));
            Assert.That(result.Message, Contains.Substring("Error"));
        }

        [Test]
        public async Task UpdateSpecialLevel_WhenRepoUpdateResultSuccessful_ReturnSuccessfulResult()
        {
            //Arrange
            var metaModel = FakeHelper.Create<UpdateSpecialLevelApiMetaModel>();
            var customers = FakeHelper.CreateMany<Customer>();
            AssignCustomersToMetaModel(metaModel, customers);
            _licenseeService.GetCustomersBelongTo(default, default).ReturnsForAnyArgs(customers.Select(_ => _.Id).ToList());
            _repository.Update(default).ReturnsForAnyArgs(new ResultInfo { Code = 0 });

            var service = CreateService();

            //Act
            var result = await service.UpdateSpecialLevel(metaModel);

            //Assert
            Assert.IsTrue(result.IsSuccessful);
            Assert.IsNotEmpty(result.ExtendedData);
        }


        private PlayerBatchUpdateService CreateService()
            => new PlayerBatchUpdateService(_publisher, _contextService, _licenseeService, _repository);

        private static void AssignCustomersToMetaModel(BasePlayerBatchUpdateApiMetaModel metaModel, IEnumerable<Customer> customers)
        {
            metaModel.CustIds = customers.Any() ? string.Join(",", customers.Select(_ => _.Id)) : string.Empty;
            metaModel.CustNames = customers.Any() ? string.Join(",", customers.Select(_ => _.Name)) : string.Empty;
        }
    }
}
