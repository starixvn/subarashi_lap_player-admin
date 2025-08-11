namespace Fanex.Athena.PlayerAdmin.Core.Settings
{
    using System.Collections.Generic;

    public enum FunctionUMType : byte
    {
        Commission = 1,
        BetSetting = 2
    }

    public class FunctionUMSetting
    {
        public IEnumerable<FunctionUM> Functions { get; set; }
    }

    public class FunctionUM
    {
        public FunctionUMType Type { get; set; }
        public bool IsUM { get; set; }
        public string TimeFrom { get; set; }
        public string TimeTo { get; set; }
    }
}
