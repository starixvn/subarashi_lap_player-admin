using System;

namespace Fanex.Athena.PlayerAdmin.WebHost.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public int Code { get; set; }
    }
}
