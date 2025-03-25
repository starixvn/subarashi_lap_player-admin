namespace Fanex.Athena.PlayerAdmin.Commission.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Core.Constants;
    using Core.Models;

    public static class CommissionGroupHelper
    {
        private const string ParlayName = "Parlay";

        private static readonly string[] ProductSettingOrder =
        {
            ProductCode.SB,
            ParlayName,
            ProductCode.NG,
            ProductCode.CS,
            ProductCode.VB,
            ProductCode.PP,
            ProductCode.SXG,
            ProductCode.SG,
            ProductCode.KN,
            ProductCode.LT,
            ProductCode.TG,
            ProductCode.VS,
            ProductCode.VRG,
            ProductCode.CC,
            ProductCode.SBC,
            ProductCode.SAG,
            ProductCode.PGS,
            ProductCode.AB,
            ProductCode.WM,
            ProductCode.BGG,
            ProductCode.JK,
            ProductCode.HB,
            ProductCode.VGA,
            ProductCode.CG,
            ProductCode.AP,
            ProductCode.APM,
            ProductCode.FGGL,
            ProductCode.MXG,
            ProductCode.JIG,
            ProductCode.YB,
            ProductCode.MGRNG,
            ProductCode.WE,
            ProductCode.SEA,
            ProductCode.L22,
            ProductCode.UUSL,
            ProductCode.FS,
            ProductCode.FAC,
            ProductCode.NS,
            ProductCode.ON,
            ProductCode.GPIRNG,
            ProductCode.PTRNG,
            ProductCode.GPILC,
            ProductCode.PS,
            ProductCode.ASK,
            ProductCode.FG
        };

        private static readonly Dictionary<string, string> FollowPermissions = new()
        {
            [ParlayName] = ProductCode.SB
        };

        public static int ProductListComparer(string productCode)
        {
            var index = Array.IndexOf(ProductSettingOrder, productCode);

            return index >= 0 ? index : int.MaxValue;
        }

        public static bool GetProductPermissions(ProductAccess productAccess, string productCode)
        {
            var code = FollowPermissions.ContainsKey(productCode) ? FollowPermissions[productCode] : productCode;

            return productAccess.IsAllow(code);
        }

        [SuppressMessage("Major Code Smell", "S109:Magic numbers should not be used", Justification = "Howard")]
        public static int GetCommPart(decimal comm, int position)
        {
            var actualComm = comm * 10000;
            switch (position)
            {
                case 1:
                    return (int)(actualComm / 100);

                case 2:
                    return (int)(actualComm % 100) / 10;

                case 3:
                    return (int)actualComm % 10;

                default:
                    return 0;
            }
        }
    }
}