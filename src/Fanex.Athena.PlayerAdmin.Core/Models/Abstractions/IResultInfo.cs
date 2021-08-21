namespace Fanex.Athena.PlayerAdmin.Core.Models
{
    public interface IResultInfo
    {
        int Code { get; set; }

        bool IsSuccessful { get; }

        string Message { get; set; }

        string ExtendedData { get; set; }
    }
}