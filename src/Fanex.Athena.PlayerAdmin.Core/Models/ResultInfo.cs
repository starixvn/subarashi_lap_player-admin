namespace Fanex.Athena.PlayerAdmin.Core.Models
{
    public class ResultInfo : IResultInfo
    {
        public bool IsSuccessful
        {
            get { return string.IsNullOrEmpty(Message) || (Code == 0); }
        }

        public int Code { get; set; }

        public string Message { get; set; }

        public string ExtendedData { get; set; }
    }

    public class ResultInfo<T> : ResultInfo, IResultInfo<T>
    {
        public T SpecificExtendedData { get; set; }
    }
}