using LZStringCSharp;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class DecompressModelStateAndMetaModelAttribute : ActionFilterAttribute
    {
        private readonly string[] _paramArray;
        private readonly string _actionArgument;

        public DecompressModelStateAndMetaModelAttribute(string actionArgument, params string[] paramArray)
        {
            _paramArray = paramArray;
            _actionArgument = actionArgument;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            context.ActionArguments.TryGetValue(_actionArgument, out object metaModel);

            if (metaModel != null)
            {
                var modelState = context.ModelState;

                foreach (var property in metaModel.GetType().GetProperties())
                {
                    if (_paramArray.Contains(property.Name) && property.CanWrite)
                    {
                        DecompressPropertyValue(property, modelState, metaModel);
                    }
                }
            }

            await next();
        }

        private static void DecompressPropertyValue(PropertyInfo property, ModelStateDictionary modelState, object metaModel)
        {
            var compressValue = property.GetValue(metaModel, null);

            if (compressValue != null && !string.IsNullOrEmpty(compressValue.ToString()))
            {
                var decompressValue = LZString.DecompressFromEncodedURIComponent(compressValue.ToString());

                property.SetValue(metaModel, decompressValue);
                modelState.SetModelValue(property.Name, new ValueProviderResult(decompressValue));
            }
        }
    }
}