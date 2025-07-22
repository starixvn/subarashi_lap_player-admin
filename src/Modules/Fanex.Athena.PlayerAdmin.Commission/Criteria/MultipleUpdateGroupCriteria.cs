using Fanex.Athena.PlayerAdmin.Commission.MetaModels;
using Fanex.Athena.PlayerAdmin.Commission.ViewModels;
using Fanex.Data;
using System.Collections.Generic;
using System.Text;

namespace Fanex.Athena.PlayerAdmin.Commission.Criteria
{
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