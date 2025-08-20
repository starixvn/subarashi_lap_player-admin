namespace Fanex.Athena.PlayerAdmin.Commission.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Helpers;

    public class PlayerCommissionSettingViewModel
    {
        public int GroupCommId { get; set; }

        public int Sporttype { get; set; }

        public int Bettype { get; set; }

        public decimal Discount { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public void MapData(IDataReader reader)
        {
            Sporttype = Convert.ToInt32(reader["Sporttype"]);
            Bettype = Convert.ToInt32(reader["Bettype"]);
            Discount = Convert.ToDecimal(reader["Discount"]);
            var productCodeObj = reader["ProductCode"];
            ProductCode = productCodeObj == null || productCodeObj == DBNull.Value ? null : Convert.ToString(productCodeObj);
            GroupCommId = Convert.ToInt32(reader["GroupDiscountID"]);
        }

        public static int GetComparerValue(PlayerCommissionSettingViewModel setting)
        {
            return CommissionGroupHelper.ProductListComparer(setting.ProductCode);
        }
    }

    public class GroupCommSettingComparer : IComparer<PlayerCommissionSettingViewModel>
    {
        public int Compare(PlayerCommissionSettingViewModel x, PlayerCommissionSettingViewModel y)
        {
            var xValue = PlayerCommissionSettingViewModel.GetComparerValue(x);
            var yValue = PlayerCommissionSettingViewModel.GetComparerValue(y);

            if (xValue > yValue)
            {
                return 1;
            }

            if (xValue < yValue)
            {
                return -1;
            }

            return 0;
        }
    }
}