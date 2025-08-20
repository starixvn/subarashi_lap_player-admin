namespace Fanex.Athena.PlayerAdmin.API.DTO
{
    using System;
    using Newtonsoft.Json;

    public class PlayerActivityRecord
    {
        private const string DateTimeFormat = "MM/dd/yyyy hh:mm:ss tt";

        [JsonIgnore]
        public DateTime? CreateDate { get; set; }

        [JsonIgnore]
        public DateTime? LastLogged { get; set; }

        [JsonIgnore]
        public DateTime? LastTransferDate { get; set; }

        [JsonIgnore]
        public DateTime? LastUpdateBA { get; set; }

        public string BAUpdatedBy { get; set; }

        public string SpecialLevelUpdateBy { get; set; }

        [JsonIgnore]
        public DateTime? SpecialLevelUpdateDate { get; set; }

        public string CreatedDateString => CreateDate.HasValue ? CreateDate.Value.ToString(DateTimeFormat) : string.Empty;

        public string LastLoginString => LastLogged.HasValue ? LastLogged.Value.ToString(DateTimeFormat) : string.Empty;

        public string LastTxn => LastTransferDate.HasValue ? LastTransferDate.Value.ToString(DateTimeFormat) : string.Empty;

        public string LastUpdateBAString => LastUpdateBA.HasValue ? LastUpdateBA.Value.ToString(DateTimeFormat) : string.Empty;

        public string LastUpdateSpecialLevelString => SpecialLevelUpdateDate.HasValue ? SpecialLevelUpdateDate.Value.ToString(DateTimeFormat) : string.Empty;
    }
}