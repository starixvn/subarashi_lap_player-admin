namespace Fanex.Athena.PlayerAdmin.Infrastructure.Models
{
    public interface IFunctionContractMetaModel
    {
        bool IsAllowedBA { get; set; }

        bool HasComm { get; set; }

        bool SystemIdVisible { get; set; }

        string FunctionKey { get; }
    }
}