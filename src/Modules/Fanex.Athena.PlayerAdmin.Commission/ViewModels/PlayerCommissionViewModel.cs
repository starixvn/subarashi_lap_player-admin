using Fanex.Athena.PlayerAdmin.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Data;

namespace Fanex.Athena.PlayerAdmin.Commission.ViewModels
{
    public class PlayerCommissionViewModel
    {
        public int LicenseeId { get; set; }

        public int GroupCommId { get; set; }

        public string GroupName { get; set; }

        public Dictionary<string, decimal> CommValues { get; set; }

        public void MapData(IDataReader reader, IDictionary<string, string> columns, IDictionary<string, bool> visibility)
        {
            // Support config for new products
            GroupCommId = reader.ToInt("GroupCommId");
            CommValues = new Dictionary<string, decimal>();
            foreach (var c in columns)
            {
                CommValues[c.Key] = visibility[c.Key] ? reader.ToDecimal(c.Value + "Comm") : 0;
            }
        }

        public void InitDefaultValue(IDictionary<string, bool> permissions)
        {
            CommValues = new Dictionary<string, decimal>();
            foreach (var p in permissions)
            {
                CommValues.Add(p.Key, 0);
            }
        }
    }
}
