namespace Fanex.Athena.PlayerAdmin.Commission.ViewModels
{
    using MetaModels;

    public class BaseUpdateGroupViewModel
    {
        public BaseUpdateGroupViewModel(BasePlayerCommissionUpdateQuery query)
        {
            CustIdsStr = query.CustIds ?? string.Empty;
            CustNamesStr = query.CustNames ?? string.Empty;
            CustIds = CustIdsStr.Split(',');
            CustNames = CustNamesStr.Split(',');
        }

        public string[] CustIds { get; set; }

        public string[] CustNames { get; set; }

        public string CustIdsStr { get; set; }

        public string CustNamesStr { get; set; }
    }
}