namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Mediator
{
    using Core.Mediator;

    public class PlayerUpdatedMultipleBAToCTSEventTests
    {
        [Fact]
        public void CanCreateInstanceAndSetProperties()
        {
            var newValue = "testValue";
            var custIdList = "1,2,3";
            
            var anEvent = new PlayerUpdatedMultipleBAToCTSEvent
            {
                NewValue = newValue,
                CustIDList = custIdList
            };
            
            Assert.Equal(newValue, anEvent.NewValue);
            Assert.Equal(custIdList, anEvent.CustIDList);
        }
    }
}
