
using Fanex.Athena.PlayerAdmin.BatchUpdate.Criteria;
using Fanex.Athena.PlayerAdmin.BatchUpdate.MetaModels;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using Fanex.Data;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.PlayerAdmin.BatchUpdate.UnitTests.Criteria
{
    public class UpdateSpecialLevelCriteriaTests
    {
        [Test]
        public void GetSPName_WhenCall_ReturnCorrectValue()
        {
            //Arrange
            var criteria = CreateCriteria();

            //Assert
            Assert.AreEqual("Lrf_CustomerUpdSpecialLevel", criteria.GetSPName());
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
            db.Received(1).SetParameterValue("UserId", criteria.UserId);
            db.Received(1).SetParameterValue("SpecialLevel", criteria.SpecialLevelId);
        }

        private UpdateSpecialLevelCriteria CreateCriteria()
            => new UpdateSpecialLevelCriteria(
                FakeHelper.Create<UpdateSpecialLevelApiMetaModel>(),
                FakeHelper.Create<string>()
                );
    }
}
