namespace Fanex.Athena.PlayerAdmin.Core.Models
{
    public class LicenseeData
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public bool IsAllowedCustomerComm { get; set; }

        public bool IsEnableBA { get; set; }
    }
}