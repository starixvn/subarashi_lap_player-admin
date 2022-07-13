namespace Fanex.Athena.PlayerAdmin.Core.Settings
{
    public class CustomerClassApiSetting
    {
        public string BaseAddress { get; set; }

        public CustomerClassEndpoint EndPoints { get; set; }
    }

    public class CustomerClassEndpoint
    {
        public string AddLicBA { get; set; }

        public string RemoveLicBA { get; set; }

        public string GetCustomerClassCategory { get; set; }
    }
}
