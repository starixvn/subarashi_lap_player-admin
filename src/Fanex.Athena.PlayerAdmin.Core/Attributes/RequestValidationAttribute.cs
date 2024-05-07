using Fanex.Athena.PlayerAdmin.Core.Extensions;
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
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class RequestValidationAttribute : TypeFilterAttribute
    {
        public RequestValidationAttribute(bool validateCustId = false, bool validateClientIP = false, int validateProductId = -99, params int[] limitedProductIds)
            : base(typeof(DataValidationImplAttribute))
        {
            Arguments = new object[] { validateCustId, validateClientIP, validateProductId, limitedProductIds };
        }
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class DataValidationImplAttribute : ActionFilterAttribute
    {
        private const int NoValidationProductId = -99;
        private const string ErrorMsg = "Invalid Params";

        private readonly IContextService _contextService;
        private readonly ILicenseeService _licenseeService;

        private readonly bool _validateCustId;
        private readonly bool _validateClientIP;
        private readonly int _validateProductId;
        private readonly int[] _limitedProductIds;

        public DataValidationImplAttribute(IContextService contextService, ILicenseeService licenseeService,
            bool validateCustId,
            bool validateClientIP,
            int validateProductId,
            int[] limitedProductIds
            )
        {
            _contextService = contextService;
            _licenseeService = licenseeService;

            _validateCustId = validateCustId;
            _validateClientIP = validateClientIP;
            _validateProductId = validateProductId;
            _limitedProductIds = limitedProductIds;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var isValid = await IsValid(context.ActionArguments);
            if (!isValid)
            {
                context.HandleInvalidRequest(ErrorMsg);
            }
            else
            {
                await next();
            }
        }

        private async Task<bool> IsValid(IDictionary<string, object> actionArguments)
        {
            var result = !MetaModelHelper.IsMetaModel(actionArguments, out IValidationMetaModel validationMetaModel) || validationMetaModel.IsValid;

            result &= !_validateCustId || (MetaModelHelper.IsMetaModel(actionArguments, out ICustomerIdentityMetaModel customerIdentityMetaModel)
                && await _licenseeService.CheckCustomerBelongTo(_contextService.AgentList, customerIdentityMetaModel.CustId, customerIdentityMetaModel.CurrencyId));

            //result &= !_validateClientIP || string.Equals(_contextService.CurrentRequestClientIP, _contextService.OwnerClientIP, StringComparison.InvariantCultureIgnoreCase);  

            result &= _validateProductId == NoValidationProductId 
                || await _licenseeService.CheckProductBelongsTo(_contextService.SiteName, GetProductId(actionArguments, _validateProductId), _limitedProductIds);

            return result;
        }

        private static int GetProductId(IDictionary<string, object> actionArguments, int validateProductId)
        {
            if (validateProductId >= 0)
            {
                return validateProductId;
            }
            return MetaModelHelper.IsMetaModel(actionArguments, out IProductMetaModel productMetaModel) ? productMetaModel.ProductId : validateProductId;
        }
    }
}