using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Core.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AutoBindingMetaModelAttribute : TypeFilterAttribute
    {
        public AutoBindingMetaModelAttribute(
            bool bindAgent = false,
            bool bindSiteName = false,
            bool bindLanguage = false,
            bool bindClientIP = false,
            bool bindUserId = false,
            bool bindBalanceUp = false,
            bool bindLicenseeContract = false,
            bool bindLicenseeId = false,
            bool bindUsername = false
            )
            : base(typeof(AutoBindingMetaModelImplAttribute))
        {
            Arguments = new object[] { bindAgent, bindSiteName, bindLanguage, bindClientIP, bindUserId, bindBalanceUp, bindLicenseeContract, bindLicenseeId, bindUsername };
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AutoBindingMetaModelImplAttribute : ActionFilterAttribute
    {
        private readonly IMetaModelBindingService _bindingService;
        private readonly ILicenseeService _licenseeService;
        private readonly ISubsidiaryService _subsidiaryService;
        private readonly IContextService _contextService;

        private readonly bool _bindAgent;
        private readonly bool _bindSiteName;
        private readonly bool _bindLanguage;
        private readonly bool _bindClientIP;
        private readonly bool _bindUserId;
        private readonly bool _bindBalanceUp;
        private readonly bool _bindLicenseeContract;
        private readonly bool _bindLicenseeId;
        private readonly bool _bindUsername;

        public AutoBindingMetaModelImplAttribute(
            IMetaModelBindingService bindingService,
            ILicenseeService licenseeService,
            ISubsidiaryService subsidiaryService,
            IContextService contextService,
            bool bindAgent,
            bool bindSiteName,
            bool bindLanguage,
            bool bindClientIP,
            bool bindUserId,
            bool bindBalanceUp,
            bool bindFunctionContract,
            bool bindLicenseeId,
            bool bindUsername
         )
        {
            _bindingService = bindingService;
            _licenseeService = licenseeService;
            _subsidiaryService = subsidiaryService;
            _contextService = contextService;

            _bindAgent = bindAgent;
            _bindSiteName = bindSiteName;
            _bindLanguage = bindLanguage;
            _bindClientIP = bindClientIP;
            _bindUserId = bindUserId;
            _bindBalanceUp = bindBalanceUp;
            _bindLicenseeContract = bindFunctionContract;
            _bindLicenseeId = bindLicenseeId;
            _bindUsername = bindUsername;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (_bindAgent)
            {
                await BindAgent(context);
            }

            if (_bindSiteName && MetaModelHelper.IsMetaModel(context.ActionArguments, out ISiteNameMetaModel siteNameMetaModel))
            {
                siteNameMetaModel.SiteName = _contextService.SiteName;
            }

            if (_bindLanguage && MetaModelHelper.IsMetaModel(context.ActionArguments, out ILanguageMetaModel languageMetaModel))
            {
                languageMetaModel.Language = _contextService.Language;
            }

            if (_bindClientIP && MetaModelHelper.IsMetaModel(context.ActionArguments, out IClientIPMetaModel clientIPMetaModel))
            {
                clientIPMetaModel.Ip = _contextService.ClientIP;
            }

            if (_bindUserId && MetaModelHelper.IsMetaModel(context.ActionArguments, out IUserIdMetaModel userIdMetaModel))
            {
                userIdMetaModel.UserId = _contextService.UserId;
            }

            if (_bindLicenseeId && MetaModelHelper.IsMetaModel(context.ActionArguments, out ILicenseeIdMetaModel licenseeIdMetaModel))
            {
                licenseeIdMetaModel.LicenseeId = _contextService.LicenseeId;
            }

            if (_bindUsername && MetaModelHelper.IsMetaModel(context.ActionArguments, out IUsernameMetaModel userNameMetaModel))
            {
                userNameMetaModel.Username = _contextService.Username;
            }

            if (_bindBalanceUp && MetaModelHelper.IsMetaModel(context.ActionArguments, out IBalanceUpAndMoveBetMetaModel balanceUpAndMoveBetMetaModel))
            {
                await BindBalanceUp(balanceUpAndMoveBetMetaModel);
            }

            if (_bindLicenseeContract && MetaModelHelper.IsMetaModel(context.ActionArguments, out IFunctionContractMetaModel functionContractMetaModel))
            {
                await BindLicenseeContract(functionContractMetaModel);
            }

            await next();
        }

        private async Task BindBalanceUp(IBalanceUpAndMoveBetMetaModel balanceUpAndMoveBetMetaModel)
        {
            var balanceUp = await _licenseeService.GetBalanceUp();
            balanceUpAndMoveBetMetaModel.BalanceUpDay = balanceUp.BalanceUpDay;
            balanceUpAndMoveBetMetaModel.MoveBetsDay = balanceUp.MoveBetsDay;
        }

        private async Task BindLicenseeContract(IFunctionContractMetaModel functionContractMetaModel)
        {
            var productAccess = await _licenseeService.GetSingleWalletProductAccess(
                   _contextService.LicenseeId,
                   "en-US",
                   functionContractMetaModel.FunctionKey);

            functionContractMetaModel.HasComm = _contextService.IsAllowedCustomerComm;
            functionContractMetaModel.IsAllowedBA = productAccess.IsAllowBA;
            functionContractMetaModel.SystemIdVisible = !_licenseeService.IsFakeLicensee(_contextService.SiteName);
        }

        private async Task BindAgent(ActionExecutingContext context)
        {
            var agentList = _contextService.AgentList as IDictionary<int, int[]>;
            if (MetaModelHelper.IsMetaModel(context.ActionArguments, out ISubsidiarySearchMetaModel subsidiaryMetaModel)
                && !_subsidiaryService.IsIgnoredSubsidiarySearch(subsidiaryMetaModel.SubsidiarySearch, subsidiaryMetaModel.SubsidiaryList, _contextService.SubsidiaryViewType))
            {
                agentList = await _subsidiaryService.GetAgentList(_contextService.SiteName, _contextService.AgentList, subsidiaryMetaModel.SubsidiaryList);
            }
            _bindingService.BindAgentXml(context.ActionArguments, agentList);
        }
    }
}