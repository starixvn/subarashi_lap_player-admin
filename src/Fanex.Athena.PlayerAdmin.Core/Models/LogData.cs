namespace Fanex.Athena.PlayerAdmin.Core.Models
{
    public class LogData
    {
        public int UserId { get; set; }

        public int CustID { get; set; }

        public int ActionID { get; set; }

        public string Doing { get; set; }

        public byte Duration { get; set; }

        public byte TypeId { get; set; }
    }
}