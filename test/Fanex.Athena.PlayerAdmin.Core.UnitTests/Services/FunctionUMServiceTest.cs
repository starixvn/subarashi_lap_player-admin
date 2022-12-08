using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Athena.PlayerAdmin.Core.Settings;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Services
{
    public class FunctionUMServiceTest
    {
        private FunctionUMSetting _setting;

        [SetUp]
        public void SetUp()
        {
            _setting = FakeHelper.Create<FunctionUMSetting>();
        }

        [Test]
        public void GetUnderMaintenanceInfo_WhenNotExistFuntion_ReturnCorrectValue()
        {
            //Arrange
            _setting.Functions = new List<FunctionUM>();
            var service = CreateService();
            (bool, string, string) expectedResult = (false, null, null);

            //Act
            var result = service.GetUnderMaintenanceInfo(FunctionUMType.Commission);

            //Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetUnderMaintenanceInfo_WhenExistFunction_ReturnCorrectValue()
        {
            //Arrange
            var function = new FunctionUM
            {
                IsUM = true,
                TimeFrom = DateTime.Now.ToString(),
                TimeTo = DateTime.Now.ToString(),
                Type = FunctionUMType.BetSetting
            };
            _setting.Functions = new List<FunctionUM> { function };

            var service = CreateService();
            (bool, string, string) expectedResult = (function.IsUM, function.TimeFrom, function.TimeTo);

            //Act
            var result = service.GetUnderMaintenanceInfo(FunctionUMType.BetSetting);

            //Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        private FunctionUMService CreateService()
        {
            var options = Substitute.For<IOptions<FunctionUMSetting>>();
            options.Value.Returns(_setting);
            return new FunctionUMService(options);
        }
    }
}
