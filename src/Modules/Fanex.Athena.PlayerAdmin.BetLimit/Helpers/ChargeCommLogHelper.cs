namespace Fanex.Athena.PlayerAdmin.BetLimit.Helpers
{
    using DataDomains;
    using Mediator.Events;
    using MetaModel;

    public static class ChargeCommLogHelper
    {
        public const string Success = "Success";

        public static PlayerUpdatedChargeCommEvent CreateLogEvent(PlayerUpdateChargeCommMetaModel metaModel, int productId)
            => new PlayerUpdatedChargeCommEvent
            {
                CustID = metaModel.CustId,
                UserId = metaModel.UserId,
                SiteName = metaModel.SiteName,
                LicenseeCustId = metaModel.LicenseeCustId,
                Detail = new PlayerUpdatedProductDetailedLog
                {
                    Status = Success,
                    DetailedId = LogActionDetailId.UpdatingChargeComm,
                    ProductId = productId.ToString(),
                    Old = metaModel.OldComm + "%",
                    New = metaModel.Comm + "%"
                }
            };
    }
}
