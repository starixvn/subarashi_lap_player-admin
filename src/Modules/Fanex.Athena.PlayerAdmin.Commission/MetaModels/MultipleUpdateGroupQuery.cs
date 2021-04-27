namespace Fanex.Athena.PlayerAdmin.Commission.MetaModels
{
    public class MultipleUpdateGroupQuery : BasePlayerCommissionUpdateQuery
    {
        public string[] Products { get; set; }

        public decimal[] Comms { get; set; }

        public override bool IsValid
            => base.IsValid && CommValidator.IsValid(Comms);
    }
}
