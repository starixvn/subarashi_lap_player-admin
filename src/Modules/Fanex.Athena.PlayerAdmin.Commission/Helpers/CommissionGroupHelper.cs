using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Fanex.Athena.PlayerAdmin.Commission.Helpers
{
    public static class CommissionGroupHelper
    {
        public const string ParlayName = "Parlay";

        private static readonly IReadOnlyDictionary<Tuple<int, int>, string> ProductSettingMapper = new Dictionary<Tuple<int, int>, string>
        {
            { Tuple.Create(1, 1), ProductCode.SB },
            { Tuple.Create(1, 9), ParlayName },
            { Tuple.Create(212, 4001), ProductCode.VB },
            { Tuple.Create(248, 4101), ProductCode.PP },
            { Tuple.Create(243, 3101), ProductCode.SXG },
            { Tuple.Create(247, 4301), ProductCode.SG },
            { Tuple.Create(220, 1021), ProductCode.LT },
            { Tuple.Create(9908, 1201), ProductCode.VS },
            { Tuple.Create(222, 4901), ProductCode.TG },
            { Tuple.Create(245, 4601), ProductCode.VRG },
            { Tuple.Create(165, 1901), ProductCode.CC },
            { Tuple.Create(200, 1611), ProductCode.SBC },
            { Tuple.Create(244, 3001), ProductCode.SAG },
            { Tuple.Create(232, 6001), ProductCode.PGS },
            { Tuple.Create(211, 2401), ProductCode.AB },
            { Tuple.Create(208, 2101), ProductCode.CS },
            { Tuple.Create(233, 3401), ProductCode.WM },
            { Tuple.Create(239, 3501), ProductCode.BGG },
            { Tuple.Create(234, 6401), ProductCode.JK },
            { Tuple.Create(238, 7001), ProductCode.HB },
            { Tuple.Create(171, 3801), ProductCode.VGA },
            { Tuple.Create(167, 7301), ProductCode.CG },
            { Tuple.Create(172, 7701), ProductCode.AP },
            { Tuple.Create(174, 7801), ProductCode.APM },
            { Tuple.Create(237, 5401), ProductCode.MXG },
            { Tuple.Create(176, 8201), ProductCode.FGGL },
            { Tuple.Create(158, 5100), ProductCode.JIG },
            { Tuple.Create(159, 3151), ProductCode.YB },
            { Tuple.Create(155, 8400), ProductCode.MGRNG },
            { Tuple.Create(161, 81), ProductCode.NG },
            { Tuple.Create(202, 1501), ProductCode.KN }
        };

        public static string GetProductCode(int sportype, int bettype)
        {
            var key = Tuple.Create(sportype, bettype);

            return ProductSettingMapper.TryGetValue(key, out string value) ? value : string.Empty;
        }

        public static Tuple<int, int> GetSetting(string productCode)
        {
            return ProductSettingMapper.FirstOrDefault(x => x.Value == productCode).Key;
        }

        private static readonly Dictionary<string, string> FollowPermissions = new Dictionary<string, string>
        {
            [ParlayName] = ProductCode.SB
        };

        public static readonly string[] ExcludeProductsSPParam = new[] { "SB", "Parlay" };

        public static readonly IReadOnlyDictionary<string, string> ProductCodeMapper = new Dictionary<string, string>
        {
            [ProductCode.CS] = "RNG",
            [ProductCode.VB] = "Macau",
            [ProductCode.AGC] = "AG",
            [ProductCode.SXG] = "SexyGaming",
            [ProductCode.SG] = "SpadeGaming",
            [ProductCode.KN] = "Keno",
            [ProductCode.LT] = "Lottery",
            [ProductCode.TG] = "TableGame",
            [ProductCode.AB] = "Allbet",
            [ProductCode.VRG] = "VirtualGames",
            [ProductCode.CC] = "ArcadiaGaming",
            [ProductCode.SBC] = "Sabaclub",
            [ProductCode.SAG] = "SAGaming",
            [ProductCode.PGS] = "PGSoft",
            [ProductCode.WM] = "WMCasino",
            [ProductCode.BGG] = "BigGaming",
            [ProductCode.JK] = "Joker",
            [ProductCode.HB] = "Habanero",
            [ProductCode.VGA] = "VGaming",
            [ProductCode.CG] = "CG",
            [ProductCode.AP] = "Advant",
            [ProductCode.APM] = "AdvantMini",
            [ProductCode.FGGL] = "FGG",
            [ProductCode.MXG] = "MaxGame",
            [ProductCode.JIG] = "Jili",
            [ProductCode.YB] = "YeeBet",
            [ProductCode.MGRNG] = "MgRNG"
        };

        public static IDictionary<string, string> MapHardcodedNameFromDB(IDictionary<string, bool> permissions, bool isUpdate)
        {
            var result = new Dictionary<string, string>();

            foreach (var p in permissions)
            {
                var code = ProductCodeMapper.ContainsKey(p.Key) ? ProductCodeMapper[p.Key] : p.Key;
                if (isUpdate && ExcludeProductsSPParam.Any(s => string.Equals(s, code)))
                {
                    continue;
                }

                result.Add(p.Key, code);
            }

            return result;
        }

        public static IDictionary<string, bool> GetProductPermissions(string[] supportedProducts, ProductAccess productAccess)
        {
            var result = new Dictionary<string, bool>();

            foreach (var p in supportedProducts)
            {
                var code = FollowPermissions.ContainsKey(p) ? FollowPermissions[p] : p;
                result.Add(p, productAccess.IsAllow(code));
            }

            return result;
        }

        public static IDictionary<string, bool> GetProductColumns(IDataReader reader, IDictionary<string, string> hardcodedColumns)
        {
            var columnsInTable = new Dictionary<string, bool>();
            var schema = reader.GetSchemaTable()
               .Rows
               .OfType<DataRow>();

            foreach (var p in hardcodedColumns)
            {
                columnsInTable.Add(p.Key, schema.Any(row => string.Equals(row["ColumnName"].ToString(), p.Value + "Comm", StringComparison.OrdinalIgnoreCase)));
            }

            return columnsInTable;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S109:Magic numbers should not be used", Justification = "Howard")]
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