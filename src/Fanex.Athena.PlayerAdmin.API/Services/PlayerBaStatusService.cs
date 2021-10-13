using Fanex.Athena.PlayerAdmin.API.MetaModel;
using Fanex.Athena.PlayerAdmin.API.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.Core.DataDomains;
using Fanex.Athena.PlayerAdmin.Core.Mediator;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Repositories;
using MediatR;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.API.Services
{
    public class PlayerBaStatusService : IPlayerBaStatusService
    {
        private readonly IDbFactory _dbFactory;
        private readonly IPublisher _publisher;

        public PlayerBaStatusService(IDbFactory dbFactory, IPublisher publisher)
        {
            _dbFactory = dbFactory;
            _publisher = publisher;
        }

        public async Task<ResultViewModel> Update(BaStatusUpdatingMetaModel metaModel)
        {
            const string UspName = "Lrf_CustomerUpdMemberBAStatus";
            ResultViewModel result;

            using (var db = _dbFactory.GetObjectDb(UspName))
            {
                db.SetParameterValue("LicenseeName", metaModel.SiteName);
                db.SetParameterValue("CustID", metaModel.CustId);
                db.SetParameterValue("ModifyBy", metaModel.UserId);
                db.SetParameterValue("IsBA", metaModel.IsBa);
                db.SetParameterValue("langkey", metaModel.Language);
                db.SetParameterValue("IP", metaModel.Ip);
                db.SetParameterValue("OddsGroup", null);
                db.SetParameterValue("ErrCode", null);
                db.SetParameterValue("ErrMsg", null);

                await db.ExecuteNonQueryAsync();

                var code = Convert.ToInt32(db.GetParameterValue("ErrCode"));
                var message = Convert.ToString(db.GetParameterValue("ErrMsg"));
                var oddsGroup = db.GetParameterValue("OddsGroup");

                result = new ResultViewModel
                {
                    Success = code == 0,
                    Message = message,
                    ErrorCode = code,
                    ExtendedData = Convert.ToString(oddsGroup).ToUpper(CultureInfo.CurrentUICulture)
                };
            }

            var @event = new PlayerUpdatedBAEvent
            {
                LicenseeCustId = metaModel.LicenseeCustId,
                SiteName = metaModel.SiteName,
                UserId = metaModel.UserId,
                CustID = metaModel.CustId,
                Status = result.Success ? LogText.Success : LogText.False,
                NewValue = metaModel.IsBa ? LogText.On : LogText.Off,
                OldValue = !metaModel.IsBa ? LogText.On : LogText.Off
            };

            await _publisher.Publish(@event);

            return result;
        }
    }
}