using Fanex.Athena.PlayerAdmin.Commission.Criteria;
using Fanex.Athena.PlayerAdmin.Commission.MetaModels;
using Fanex.Athena.PlayerAdmin.Commission.Repositories.Interfaces;
using Fanex.Athena.PlayerAdmin.Commission.Services;
using Fanex.Athena.PlayerAdmin.Commission.ViewModels;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Resources;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Commission.UnitTests.Services
{
    public class PlayerCommissionServiceTests
    {
        private IPlayerCommissionRepository _repository;

        [SetUp]
        public void SetUp()
        {
            _repository = Substitute.For<IPlayerCommissionRepository>();
        }

        [Test]
        public async Task GetAllCommGroups_WhenCall_CallRepo()
        {
            //Arrange
            var query = FakeHelper.Create<BasePlayerCommissionUpdateQuery>();
            var service = CreateService();

            //Act
            await service.GetAllCommGroups(query);

            //Assert
            await _repository.Received(1).GetAllCommGroups(Arg.Any<GetAllCommGroupsCriteria>());
        }

        [Test]
        public async Task MultipleMoveGroup_WhenCommGroupsCommValueIsNull_ReturnGroupNotExistResult()
        {
            //Arrange
            var query = FakeHelper.Create<MultipleMoveGroupQuery>();
            var groupInfo = FakeHelper.Create<PlayerCommissionViewModel>();
            groupInfo.CommValues = null;
            _repository.GetCommissionGroupById(default).ReturnsForAnyArgs(groupInfo);

            var service = CreateService();

            //Act
            var result = await service.MultipleMoveGroup(query);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(1, result.Code);
                Assert.AreEqual(ViewResources.GroupNotExists, result.Message);
            });
        }

        [Test]
        public async Task MultipleMoveGroup_WhenCommGroupsCommValueNotNull_ReturnCorrectResult()
        {
            //Arrange
            var query = FakeHelper.Create<MultipleMoveGroupQuery>();
            var groupInfo = FakeHelper.Create<PlayerCommissionViewModel>();
            var expectedResult = FakeHelper.Create<ResultInfo>();
            _repository.GetCommissionGroupById(default).ReturnsForAnyArgs(groupInfo);
            _repository.MultipleMoveGroup(default).ReturnsForAnyArgs(expectedResult);

            var service = CreateService();

            //Act
            var result = await service.MultipleMoveGroup(query);

            //Assert
            Assert.AreSame(expectedResult, result);
        }

        [Test]
        public async Task MultipleRemoveFromGroup_WhenCommGroupsCommValueNotNull_ReturnCorrectResult()
        {
            //Arrange
            var query = FakeHelper.Create<MultipleRemoveFromGroupQuery>();
            var expectedResult = FakeHelper.Create<ResultInfo>();
            _repository.MultipleRemoveFromGroup(default).ReturnsForAnyArgs(expectedResult);

            var service = CreateService();

            //Act
            var result = await service.MultipleRemoveFromGroup(query);

            //Assert
            Assert.AreSame(expectedResult, result);
        }

        [Test]
        public async Task MultipleUpdateGroup_WhenCommGroupsCommValueNotNull_ReturnCorrectResult()
        {
            //Arrange
            var query = FakeHelper.Create<MultipleUpdateGroupQuery>();
            var expectedResult = FakeHelper.Create<ResultInfo>();
            _repository.MultipleUpdateGroup(default).ReturnsForAnyArgs(expectedResult);

            var service = CreateService();

            //Act
            var result = await service.MultipleUpdateGroup(query);

            //Assert
            Assert.AreSame(expectedResult, result);
        }

        [Test]
        public async Task GetPlayerCommissionGroup_WhenCommGroupsCommValueNotNull_ReturnCorrectResult()
        {
            //Arrange
            var query = FakeHelper.Create<SingleMoveGroupQuery>();
            var expectedResult = FakeHelper.Create<PlayerCommissionViewModel>();
            _repository.GetPlayerCommissionGroup(default).ReturnsForAnyArgs(expectedResult);

            var service = CreateService();

            //Act
            var result = await service.GetPlayerCommissionGroup(query);

            //Assert
            Assert.AreSame(expectedResult, result);
        }

        private PlayerCommissionService CreateService()
            => new PlayerCommissionService(_repository);
    }
}
