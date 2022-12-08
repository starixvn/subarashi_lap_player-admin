

using Fanex.Athena.PlayerAdmin.BatchUpdate.Criteria;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using Fanex.Data;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Fanex.Athena.PlayerAdmin.BatchUpdate.UnitTests.Criteria
{
    public class GetOldValuesCriteriaTests
    {
        private IEnumerable<int> _custIds;

        [SetUp]
        public void SetUp()
        {
            _custIds = FakeHelper.CreateMany<int>();
        }

        [TestCase(GetOldValuesCriteria.SpecialLevelType, "Lrf_CustomerSpecialLevelSel")]
        [TestCase(GetOldValuesCriteria.OddsGroupType, "Lrf_CustomerUgroupSel")]
        [TestCase(GetOldValuesCriteria.BAStatusType, "Lrf_CustomerBAStatusSel")]
        [TestCase(4, "")]
        public void GetSPName_WhenCall_ReturnCorrecValue(int valueType, string expectedSpName)
        {
            //Arrange
            var criteria = CreateCriteria(valueType);

            //Act
            var result = criteria.GetSPName();

            //Assert
            Assert.AreEqual(expectedSpName, result);
        }

        [Test]
        public void SetParameterValues_WhenCall_CallConrrectMethod()
        {
            //Arrange
            var db = Substitute.For<IObjectDb>();
            var criteria = CreateCriteria();
            var stringCustIds = string.Join(",", _custIds);

            //Act
            criteria.SetParameterValues(db);

            //Assert
            db.Received(1).SetParameterValue("custids", stringCustIds);
        }

        [TestCase(GetOldValuesCriteria.SpecialLevelType, "SpecialLevel")]
        [TestCase(GetOldValuesCriteria.OddsGroupType, "ugroup")]
        [TestCase(GetOldValuesCriteria.BAStatusType, "isBA")]
        [TestCase(4, "")]
        public void GetValueColumnName_WhenCall_ReturnCorrectValue(int valueType, string expectedColumnName)
        {
            //Arrange
            var criteria = CreateCriteria(valueType);

            //Act
            var result = criteria.GetValueColumnName();

            //Assert
            Assert.AreEqual(expectedColumnName, result);
        }

        private GetOldValuesCriteria CreateCriteria(int valueType = 1)
            => new GetOldValuesCriteria(_custIds, valueType);
    }
}
