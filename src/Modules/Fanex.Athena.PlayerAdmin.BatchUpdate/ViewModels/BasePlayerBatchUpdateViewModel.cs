namespace Fanex.Athena.PlayerAdmin.BatchUpdate.ViewModels
{
    using MetaModels;

    public class BasePlayerBatchUpdateViewModel
    {
        public BasePlayerBatchUpdateViewModel(PlayerBatchUpdateMetaModel metaModel)
        {
            CustIdsStr = metaModel.CustIds ?? string.Empty;
            CustNamesStr = metaModel.CustNames ?? string.Empty;
        }

        public string CustIdsStr { get; set; }

        public string CustNamesStr { get; set; }
    }
}