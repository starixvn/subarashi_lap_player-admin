namespace Fanex.Athena.PlayerAdmin.Core.Models
{
    public class ResultViewModel
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public object ExtendedData { get; set; }
    }
}