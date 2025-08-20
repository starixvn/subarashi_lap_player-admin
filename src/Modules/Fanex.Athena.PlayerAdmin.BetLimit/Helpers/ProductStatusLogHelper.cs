namespace Fanex.Athena.PlayerAdmin.BetLimit.Helpers
{
    using System.Globalization;
    using DataDomains;
    using Mediator;
    using MetaModel;

    public static class ProductStatusLogHelper
    {
        public const string Success = "Success";
        public const string False = "False";
        private const string Disable = "Disable";
        private const string Enable = "Enable";

        public static PlayerUpdatedProductStatusEvent CreateLogEvent(PlayerBetLimitUpdateStatusMetaModel metaModel, bool isSuccessful)
        {
            var detail = BuildBetLimitLogDetail(metaModel, isSuccessful);

            return new PlayerUpdatedProductStatusEvent
            {
                CustID = metaModel.CustId,
                UserId = metaModel.UserId,
                SiteName = metaModel.SiteName,
                LicenseeCustId = metaModel.LicenseeCustId,
                Detail = detail
            };
        }

        private static PlayerUpdatedProductDetailedLog BuildBetLimitLogDetail(
            PlayerBetLimitUpdateStatusMetaModel metaModel,
            bool isSuccessful)
        {
            var betLimitLogDetailItem = CreateLogDetailItemByAction(LogActionDetailId.UpdatingProductStatus, metaModel.ProductId.ToString(),
                isSuccessful ? Success : False, metaModel);

            return betLimitLogDetailItem;
        }

        private static PlayerUpdatedProductDetailedLog CreateLogDetailItemByAction(
            int logDetailId,
            string productId,
            string status,
            PlayerBetLimitUpdateStatusMetaModel item)
        {
            var oldStatus = item.OldStatus ? Enable : Disable;
            var newStatus = item.Status ? Enable : Disable;

            return CreateLogDetailItem(logDetailId, productId, status, oldStatus, newStatus);
        }

        private static PlayerUpdatedProductDetailedLog CreateLogDetailItem(
            int logDetailId,
            string productId,
            string status,
            string oldValue,
            string newValue)
        {
            return new PlayerUpdatedProductDetailedLog
            {
                DetailedId = logDetailId,
                ProductId = productId,
                Status = status,
                Old = oldValue.ToString(CultureInfo.InvariantCulture),
                New = newValue.ToString(CultureInfo.InvariantCulture)
            };
        }
    }
}