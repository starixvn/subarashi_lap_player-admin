
using Fanex.Athena.PlayerAdmin.BatchUpdate.Criteria;
using Fanex.Athena.PlayerAdmin.BatchUpdate.MetaModels;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using Fanex.Data;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.PlayerAdmin.BatchUpdate.UnitTests.Criteria
{
    public class UpdateBAStatusCriteriaTests
    {
        [Test]
        public void GetSPName_WhenCall_ReturnCorrectValue()
        {
            //Arrange
            var criteria = CreateCriteria();

            //Assert
            Assert.AreEqual("Lrf_CustomerUpdMemberBAStatus", criteria.GetSPName());
        }

        [Test]
        public void SetParameterValues_WhenCall_CallRelevantMethod()
        {
            //Arrange
            var db = Substitute.For<IObjectDb>();
            var criteria = CreateCriteria();

            //Act
            criteria.SetParameterValues(db);

            //Assert
            db.Received(1).SetParameterValue("LicenseeName", criteria.SiteName);
            db.Received(1).SetParameterValue("ModifyBy", criteria.UserId);
            db.Received(1).SetParameterValue("IsBA", criteria.BAStatusId == "1");
            db.Received(1).SetParameterValue("OddsGroup", null);
        }

        private UpdateBAStatusCriteria CreateCriteria()
            => new UpdateBAStatusCriteria(
                FakeHelper.Create<UpdateBAStatusApiMetaModel>(),
                FakeHelper.Create<string>()
                );
    }
}
