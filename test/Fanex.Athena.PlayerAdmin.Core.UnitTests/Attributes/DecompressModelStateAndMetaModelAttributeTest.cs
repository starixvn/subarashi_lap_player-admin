using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using Fanex.Athena.PlayerAdmin.BatchUpdate.Controllers;
using Fanex.Athena.PlayerAdmin.BatchUpdate.MetaModels;
using Fanex.Athena.PlayerAdmin.BatchUpdate.Services;
using Fanex.Athena.PlayerAdmin.Core.Attributes;
using LZStringCSharp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Moq;
using Xunit;

namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Attributes
{
    public class DecompressModelStateAndMetaModelAttributeTest
    {
        private readonly IFixture fixture = new Fixture();

        private const string compressCustNameStringFromJS = "MYZwJgLgpiEAwEYA0pI3gJheas4GZs08AWI3eAViQEMA7OjOOMugSwagC8BPFACwBObWOjhZgQkRQIDho2AFkA4gGE509CtWINCiNubVJ8iobgA2PeeQmR5iVJDnCd52uZk3540-NWAOQAhVQBJABEAJV03GURkIIB7OgBXfV0ARxo4gHYADiRYGmEADzZ4HMKIYrYy+ABOKpq6hDgm0vKELCKOiARCHtrOsgo3JFGnXUG65mRp8uZu6t7mAeWh+Dhqec2rHdm2ihpgYF0jk-Fx9GPTshuZanuxKye8ApoAcwBVOkSI6OQnx+iQAKgAJIK6IG-SKKSGA76-L4AZXCugAgsoZMgsnEMJVcWJxAUHnAcmcxJQybJSeSyIS8ARCAzNvgCdkifgSZS4HlMhzGfhGkEAO4cf66UXiqKXACiwmANCQUAVAogGAsWyQLNWZGUiUSKLRyH1oIhFNgrRIljgjQoFCtNsa+2oKhBsuRIN0bo9XqwPs9sgDXrIwa1AE0weHYfCkABFPKKaO6BNGlN5cGxhOyr4A+N5dFfY35mPpiXIYKqJO6SsomshcH11Q5puFpuwjEAG07md0gmI6u26xKUI+FAAagFwrQx+gjTOKDHtWqMAUWTQALY8flHLeXfvYpAHsRYY94Qhn+BkS9bIA";
        private const string compressCustIdStringFromJS = "IwJmGYE5wDgBgDTjgVgOxoCwSajnxwFRgYVM4jhgA2cFOG4Y2mmTTEJENGCNKmRCQ0kZqRTDIIRBI7AsxMhw4olKcqC4TNMGuvLJtZQ3EgGCwczoJ0L4Gpns19pGiBAw+S+HDTUfUXB-BBlGSBEqaT5-V2jIOicrYT8BYmgUcGwo+mxgJIy6DHTYEBRvK1KMcUhMDQFZWvr6dLr0JnTHBzsrGkhULPT4PT8hpiMxhQihtDh8mHSIxzRtCITHfTCKFEdifBoVhYUsA93jzFO0PZOMWX2MNWPgijuBXko9vQ0ZT5odn4UX144l4Ln81l44JwgPBaFckPAWj2-RAF2MCjgYHEMUxZiUqHIZ3gGhQtCU1DKnnJB3g1j4B2B5PecKZXmAshgsERLNInEwEWMfIiSXYqIij1F-MgR1FDDgHOwYh57CswB2SkVtGxis82uSxV5+r1YkgEs1mQ1yQulukLhtIB6Kuk5XtGldxi8IxFmHgqo18Cy1hc5WC4mDnL6xHDwWMDgcMCDwrYV1o4poqGIsDhPG0WfcaQgMGzaEeeZ4rjLGCAA";
        private const string custNameStringFromJS = "csdtest01,csdtest02,csdtest03,csdtest04,csdtest05,ann2004,ninnezy,christest02,christest03,christestMGC,christestMGC01,christestMGC005,christestMGC006,chrisMGC001,chrisMGC002,chrisMGC003,chrisMGC004,chrisMGC005,chrisMGC006,NBCIDR01,christest0011,Bonustest01,qatest0078,starixit07,starixit09,starixit10,starixit12,starixit13,starixit14,testchris,testchris01,starixit001,starixit002,starixit003,starixit005,starixit006,starixit0010,testacc01,testacc02,testacc04,acctest05,acctest06,acctest08,agUnoIDR01,agUnoTHB01,agUnoRMB01,agUnoUSD01,AGtest01,qatest0027,qatest0028,test050701,test050703,test050704,qatest0033,qatest0037,qatest0038,test050801,qatest0039,BwinIDR01,BwinIDR02,Erica,ericatest2605,qatest0034,GooUSD01,GooTHB01,test1040609,testtest1040609,starixit05,MGTEST01,MGTEST02,MGTEST03,MGTEST04,MGTEST05,YHYRMB01,Q8MYR01,Q8USD01,Q8THB01,Q8EUR01,Q8AUD01,Q8RMB01,Q8IDR01,NBCMY01,NBCUS01,NBCTH01,NBCEU01,NBCAU01,NBCRM01,AllTHB01,rdtest25,starix01,agtestVND,agtestUSD,agtestRMB,qatest28,qatestamy01,qatestamy02,rdtest01,rdtest02,rdtest03,rdtest04,rdtest05";
        private const string custIdStringFromJS = "12139380,30577413,30577433,12185403,11635061,11668442,32781373,18529791,18529920,18544174,18544445,18554122,18554186,18554302,18554309,18554319,18554363,18554364,18554366,18622881,18800711,18879371,20069973,19281716,19289634,19290773,19353413,19354114,19356377,19382581,19382771,19455730,19455735,19455761,19643663,19690534,19808607,19861302,19861799,19870148,19996472,19996646,20040564,17746728,17746764,17746767,17746770,17746775,17737400,17737803,17865520,17865620,17865781,17866719,17871913,17871976,17873122,17902462,18170211,18170209,18055464,18055516,18112528,18167809,18167781,18178076,18178810,18383176,18424992,18424994,18424995,18424998,18425000,18419176,18419156,18419161,18419281,18419277,18419271,18419195,18419153,18419246,18419266,18419263,18419258,18419255,18419252,18888604,18480191,18480349,16658371,16658369,16658372,18363689,16994687,16995605,13876272,13876273,13876275,13876276,13876277";

        [Fact]
        public void DecompressParam_WhenCall_RunCorrectly()
        {
            // Arrange
            var argument = "metaModel";
            var metaModel = fixture.Build<PlayerBatchUpdateMetaModel>()
                .With(_ => _.CustIds, compressCustIdStringFromJS)
                .With(_ => _.CustNames, compressCustNameStringFromJS)
                .Create();
            var actionContext = CreateActionContext();
            var context = CreateContext(metaModel, actionContext, argument);
            var next = CreateActionExecutionDelegate(actionContext);

            var filter = new DecompressModelStateAndMetaModelAttribute(argument, new[] { "CustIds", "CustNames" });

            // Action
            filter.OnActionExecutionAsync(context, next);

            // Assert
            Assert.Equal(custIdStringFromJS, metaModel.CustIds);
            Assert.Equal(custNameStringFromJS, metaModel.CustNames);
            Assert.Equal(LZString.DecompressFromEncodedURIComponent(compressCustIdStringFromJS), metaModel.CustIds);
            Assert.Equal(LZString.DecompressFromEncodedURIComponent(compressCustNameStringFromJS), metaModel.CustNames);
        }

        private static ActionContext CreateActionContext()
        {
            var modelState = new ModelStateDictionary();
            var httpContext = new DefaultHttpContext();
            return new ActionContext(
                    httpContext: httpContext,
                    routeData: new RouteData(),
                    actionDescriptor: new ActionDescriptor(),
                    modelState: modelState
             );
        }

        private static ActionExecutingContext CreateContext(PlayerBatchUpdateMetaModel metaModel, ActionContext actionContext, string argument)
        {
            return new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object> { { argument, metaModel } },
                new Mock<Controller>().Object);
        }

        private static ActionExecutionDelegate CreateActionExecutionDelegate(ActionContext actionContext)
        {
            var service = new Mock<IPlayerBatchUpdateService>();
            var controller = new PlayerBatchUpdateController(service.Object);
            var metadata = new List<IFilterMetadata>();
            Task<ActionExecutedContext> next()
            {
                var ctx = new ActionExecutedContext(actionContext, metadata, controller);
                return Task.FromResult(ctx);
            }

            return next;
        }
    }
}