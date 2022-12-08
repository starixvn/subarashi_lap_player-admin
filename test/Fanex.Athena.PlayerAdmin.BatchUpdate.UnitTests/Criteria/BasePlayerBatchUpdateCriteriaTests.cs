using Fanex.Athena.PlayerAdmin.BatchUpdate.Criteria;
using Fanex.Athena.PlayerAdmin.BatchUpdate.MetaModels;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using Fanex.Data;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.PlayerAdmin.BatchUpdate.UnitTests.Criteria
{
    public class BasePlayerBatchUpdateCriteriaTests
    {
        [Test]
        public void GetSPName_WhenCall_ReturnEmpty()
        {
            //Arrange
            var criteria = CreateCriteria();

            //Assert
            Assert.IsEmpty(criteria.GetSPName());
        }

        [Test]
        public void SetParameterValues_WhenCall_CallReleventMetho()
        {
            //Arrange
            var db = Substitute.For<IObjectDb>();
            var criteria = CreateCriteria();

            //Act
            criteria.SetParameterValues(db);

            //Assert
            db.Received(1).SetParameterValue("CustId", criteria.CustId);
            db.Received(1).SetParameterValue("Ip", criteria.Ip);
            db.Received(1).SetParameterValue("langkey", criteria.Language);
            db.Received(1).SetParameterValue("ErrCode", null);
            db.Received(1).SetParameterValue("ErrMsg", null);
        }

        private BasePlayerBatchUpdateCriteria CreateCriteria()
            => new BasePlayerBatchUpdateCriteria(
                FakeHelper.Create<BasePlayerBatchUpdateApiMetaModel>(), 
                FakeHelper.Create<string>());
    }
}
