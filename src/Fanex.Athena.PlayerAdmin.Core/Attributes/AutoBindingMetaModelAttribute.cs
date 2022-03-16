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
#pragma warning disable S107 // Methods should not have too many parameters

        public AutoBindingMetaModelAttribute(
            bool bindAgent = false,
            bool bindSiteName = false,
            bool bindLanguage = false,
            bool bindClientIP = false,
            bool bindUserId = false,
            bool bindBalanceUp = false,
            bool bindLicenseeId = false,
            bool bindUsername = false,
            bool bindProductList = false
            )
#pragma warning restore S107 // Methods should not have too many parameters
            : base(typeof(AutoBindingMetaModelImplAttribute))
        {
            Arguments = new object[] { bindAgent, bindSiteName, bindLanguage, bindClientIP, bindUserId, bindBalanceUp, bindLicenseeId, bindUsername, bindProductList };
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
        private readonly bool _bindLicenseeId;
        private readonly bool _bindUsername;
        private readonly bool _bindProductList;

#pragma warning disable S107 // Methods should not have too many parameters

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
            bool bindLicenseeId,
            bool bindUsername,
            bool bindProductList
         )
#pragma warning restore S107 // Methods should not have too many parameters
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
            _bindLicenseeId = bindLicenseeId;
            _bindUsername = bindUsername;
            _bindProductList = bindProductList;
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
                clientIPMetaModel.Ip = _contextService.CurrentRequestClientIP;
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

            if(_bindProductList)
            {
                await BindProductList(context);
            }

            await next();
        }

        private async Task BindBalanceUp(IBalanceUpAndMoveBetMetaModel balanceUpAndMoveBetMetaModel)
        {
            var balanceUp = await _licenseeService.GetBalanceUp();
            balanceUpAndMoveBetMetaModel.BalanceUpDay = balanceUp.BalanceUpDay;
            balanceUpAndMoveBetMetaModel.MoveBetsDay = balanceUp.MoveBetsDay;
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

        private async Task BindProductList(ActionExecutingContext context)
        {
            var functionKey = MetaModelHelper.IsMetaModel(context.ActionArguments, out IFunctionKeyMetaModel functionKeyMetaModel) ? functionKeyMetaModel.FunctionKey : "Default";

            if (MetaModelHelper.IsMetaModel(context.ActionArguments, out IProductCodeListMetaModel productCodeListMetaModel))
            {
                await BindProductCodeList(productCodeListMetaModel, functionKey);
            } 
            else
            {
                //nothing
            }
        }

        private async Task BindProductCodeList(IProductCodeListMetaModel productCodeListMetaModel, string functionKey)
        {
            var productCodeListFromFilter = productCodeListMetaModel.ProductCodeList;
            if (string.IsNullOrEmpty(productCodeListFromFilter))
            {
                var productAccess = await _licenseeService.GetSingleWalletProductAccess(_contextService.LicenseeId, functionKey);
                productCodeListFromFilter = productAccess.ProductCodeListString;
            }

            productCodeListMetaModel.OriginalProductCodeList = productCodeListFromFilter;
            productCodeListMetaModel.ProductCodeList = _licenseeService.GetAllowedSubProductCodeList(_contextService.OfferedSubProductList, productCodeListFromFilter);
        }
    }
}