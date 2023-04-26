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
            [ProductCode.VS3] = "VS3",
            [ProductCode.FGGL] = "FGG",
            [ProductCode.MXG] = "MaxGame",
            [ProductCode.JIG] = "Jili"
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