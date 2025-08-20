namespace Fanex.Athena.PlayerAdmin.Commission.Criteria
{
    using System.Collections.Generic;
    using System.Text;
    using Data;
    using MetaModels;
    using ViewModels;

    public class MultipleUpdateGroupCriteria : BasePlayerCommissionUpdateCriteria
    {
        public MultipleUpdateGroupCriteria(MultipleUpdateGroupQuery query) : base(query)
        {
            CommValues = query.CommValues;
        }

        public IEnumerable<PlayerCommissionSettingViewModel> CommValues { get; set; }

        public override string GetSPName() => "Lrf_LicenseeCustCommUpd";

        public override void SetParameterValues(IObjectDb db)
        {
            base.SetParameterValues(db);
            db.SetParameterValue("DiscountSettings", BuildDiscountSettingsXmlString());
        }

        private string BuildDiscountSettingsXmlString()
        {
            const string XmlPattern = "<Root>{0}</Root>";
            const string XmlElementPattern = "<r SportType=\"{0}\" BetType=\"{1}\" Discount=\"{2}\" />";
            var xmlElementBuilder = new StringBuilder();

            foreach (var setting in CommValues)
            {
                xmlElementBuilder.Append(string.Format(
                    XmlElementPattern,
                    setting.Sporttype,
                    setting.Bettype,
                    setting.Discount));
            }

            return string.Format(XmlPattern, xmlElementBuilder);
        }
    }
}