using Fanex.Athena.PlayerAdmin.Core.Resources;
using System;
using System.Collections.Generic;
using Fanex.Athena.PlayerAdmin.Core.Constants;

namespace Fanex.Athena.PlayerAdmin.Core.Helpers
{
    public static class ProductHelper
    {
        private static readonly IDictionary<string, string> ProductNameMap =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { ProductCode.BA, nameof(ViewResources.ProductBA) },
                { ProductCode.NG, nameof(ViewResources.ProductNG) },
                { ProductCode.SB, nameof(ViewResources.Sportsbook) },
                { ProductCode.VS, nameof(ViewResources.ProductVS) },
                { "VS180", nameof(ViewResources.VirtualSoccer) },
                { "VS181", nameof(ViewResources.VirtualHorseRacing) },
                { "VS182", nameof(ViewResources.VirtualGreyhounds) },
                { "VS183", nameof(ViewResources.VirtualSpeedway) },
                { "VS184", nameof(ViewResources.VirtualMotoRacing) },
                { "VS185", nameof(ViewResources.VirtualCycling) },
                { "VS186", nameof(ViewResources.VirtualTennis) },
                { ProductCode.CS, nameof(ViewResources.ProductCS) },
                { ProductCode.KN, nameof(ViewResources.ProductKN) },
                { ProductCode.VB, nameof(ViewResources.ProductVB) },
                { ProductCode.VS2, nameof(ViewResources.ProductVS2) },
                { "VS2190", nameof(ViewResources.VirtualSoccerLeague) },
                { "VS2191", nameof(ViewResources.VirtualSoccerNation) },
                { "VS2192", nameof(ViewResources.VirtualSoccerWorldCup) },
                { "VS2193", nameof(ViewResources.VirtualBasketball) },
                { "VS2194", nameof(ViewResources.VirtualSoccerAsianCup) },
                { "VS2195", nameof(ViewResources.VirtualSoccerEnglishPremier) },
                { "VS2196", nameof(ViewResources.VirtualSoccerChampionsCup) },
                { "VS2197", nameof(ViewResources.VirtualSoccerEuroCup) },
                { ProductCode.LT, nameof(ViewResources.ProductLT) },
                { ProductCode.AB, nameof(ViewResources.ProductAB) },
                { ProductCode.PP, nameof(ViewResources.ProductPP) },
                { ProductCode.CC, nameof(ViewResources.ProductCC) },
                { ProductCode.AGC, nameof(ViewResources.ProductAGC) },
                { ProductCode.SG, nameof(ViewResources.ProductSG) },
                { ProductCode.VRG, nameof(ViewResources.ProductVRG) },
                { ProductCode.TG, nameof(ViewResources.ProductTG) },
                { ProductCode.I4D, nameof(ViewResources.ProductI4D) },
                { ProductCode.SXG, nameof(ViewResources.ProductSXG) },
                { ProductCode.SBC, nameof(ViewResources.ProductSBC) },
                { ProductCode.SAG, nameof(ViewResources.ProductSAG) },
                { ProductCode.PGS, nameof(ViewResources.ProductPGS) },
                { ProductCode.JK, nameof(ViewResources.ProductJK) },
                { ProductCode.SLT, nameof(ViewResources.ProductSLT) },
                { ProductCode.HB, nameof(ViewResources.ProductHB) },
                { ProductCode.WM, nameof(ViewResources.ProductWM) },
                { ProductCode.BGG, nameof(ViewResources.ProductBGG) },
                { ProductCode.VGA, nameof(ViewResources.ProductVGA) },
                { ProductCode.CG, nameof(ViewResources.ProductCG) },
                { ProductCode.CO, nameof(ViewResources.ProductCO) },
                { ProductCode.BX, nameof(ViewResources.ProductBX) },
                { ProductCode.AP, nameof(ViewResources.ProductAP) },
                { ProductCode.APM, nameof(ViewResources.ProductAPM) },
                { ProductCode.VS3, nameof(ViewResources.ProductVS3) },
                { ProductCode.BTC, nameof(ViewResources.ProductBTC) },
                { ProductCode.FGGL, nameof(ViewResources.ProductFGGL) },
                { ProductCode.MXG, nameof(ViewResources.ProductMXG) },
                { ProductCode.JIG, nameof(ViewResources.ProductJIG) },
                { ProductCode.YB, nameof(ViewResources.ProductYB) },
                { ProductCode.MGRNG, nameof(ViewResources.ProductMGRNG) },
                { ProductCode.LG, nameof(ViewResources.ProductLG) },
                { ProductCode.SEA, nameof(ViewResources.ProductSEA) },
                { ProductCode.WE, nameof(ViewResources.ProductWE) },
                { ProductCode.L22, nameof(ViewResources.ProductL22) },
                { ProductCode.UUSL, nameof(ViewResources.ProductUUSL) },
                { ProductCode.FS, nameof(ViewResources.ProductFS) },
                { ProductCode.FAC, nameof(ViewResources.ProductFAC) },
                { ProductCode.NS, nameof(ViewResources.ProductNS) }
            };

        public static string GetProductName(string productCode, string defaultValue = null)
        {
            if (!ProductNameMap.ContainsKey(productCode))
            {
                return defaultValue ?? productCode;
            }

            var resourceKey = ProductNameMap[productCode];
            return ViewResources.ResourceManager.GetString(resourceKey);
        }
    }
}