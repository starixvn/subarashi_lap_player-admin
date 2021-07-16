namespace Fanex.Athena.PlayerAdmin.Infrastructure.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CurrencyId { get; set; }
        public int AgentId { get; set; }
    }
}