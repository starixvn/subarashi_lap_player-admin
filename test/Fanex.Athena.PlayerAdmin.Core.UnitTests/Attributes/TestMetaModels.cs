namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Attributes
{
    using System;
    using Fanex.Core.Utils;
    using Models;

    // Test interfaces based on the actual interface properties I found
    public interface IAgentMetaModel
    {
        string AgentListXml { get; set; }
    }

    public interface ICurrencyIdListMetaModel
    {
        string CurrencyIdList { get; set; }
    }


    // Test meta model implementations
    public class TestValidationMetaModel : IValidationMetaModel
    {
        public bool IsValid { get; set; } = true;
    }

    public class TestCustomerIdentityMetaModel : ICustomerIdentityMetaModel
    {
        public int CustId { get; set; }
        public int CurrencyId { get; set; }
    }

    public class TestProductMetaModel : IProductMetaModel
    {
        public int ProductId { get; set; }
    }

    public class TestSiteNameMetaModel : ISiteNameMetaModel
    {
        public string SiteName { get; set; }
    }

    public class TestLanguageMetaModel : ILanguageMetaModel
    {
        public string Language { get; set; }
    }

    public class TestClientIPMetaModel : IClientIPMetaModel
    {
        public string Ip { get; set; }
    }

    public class TestUserIdMetaModel : IUserIdMetaModel
    {
        public int UserId { get; set; }
    }

    public class TestLicenseeIdMetaModel : ILicenseeIdMetaModel
    {
        public int LicenseeId { get; set; }
    }

    public class TestUsernameMetaModel : IUsernameMetaModel
    {
        public string Username { get; set; }
    }

    public class TestBalanceUpAndMoveBetMetaModel : IBalanceUpAndMoveBetMetaModel
    {
        public DateTime MoveBetsDay { get; set; }
        public DateTime BalanceUpDay { get; set; }
    }

    public class TestSubsidiarySearchMetaModel : ISubsidiarySearchMetaModel
    {
        public string SubsidiaryList { get; set; }
        public bool SubsidiarySearch { get; set; }
    }

    public class TestFunctionKeyMetaModel : IFunctionKeyMetaModel
    {
        public string FunctionKey { get; }
    }

    public class TestProductCodeListMetaModel : IProductCodeListMetaModel
    {
        public string ProductCodeList { get; set; }
        public string OriginalProductCodeList { get; set; }
    }

    public class TestAgentMetaModel : IAgentMetaModel
    {
        public string AgentListXml { get; set; }
    }

    public class TestCombinedMetaModel : IValidationMetaModel, ICustomerIdentityMetaModel, IProductMetaModel
    {
        public int CustId { get; set; }
        public int CurrencyId { get; set; }
        public int ProductId { get; set; }
        public bool IsValid { get; set; } = true;
    }
}