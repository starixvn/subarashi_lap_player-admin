using Fanex.Athena.PlayerAdmin.API.MetaModel;
using Fanex.Athena.PlayerAdmin.API.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.Infrastructure.DataDomains;
using Fanex.Athena.PlayerAdmin.Infrastructure.Mediator;
using Fanex.Athena.PlayerAdmin.Infrastructure.Models;
using Fanex.Athena.PlayerAdmin.Infrastructure.Repositories;
using MediatR;
using System;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.API.Services
{
    public class PlayerOddsGroupService : IPlayerOddsGroupService
    {
        private readonly IDbFactory _dbFactory;

        private readonly IPublisher _publisher;

        public PlayerOddsGroupService(IDbFactory dbFactory, IPublisher publisher)
        {
            _dbFactory = dbFactory;
            _publisher = publisher;
        }

        public async Task<ResultViewModel> Update(OddsGroupUpdatingMetaModel metaModel)
        {
            const string UspName = "Lrf_CustomerUpdUgroup";
            ResultViewModel result;

            using (var db = _dbFactory.GetObjectDb(UspName))
            {
                db.SetParameterValue("UserId", metaModel.UserId);
                db.SetParameterValue("CustId", metaModel.CustId);
                db.SetParameterValue("OddsGroup", metaModel.OddsGroup);
                db.SetParameterValue("IP", metaModel.Ip);
                db.SetParameterValue("Langkey", metaModel.Language);

                db.SetParameterValue("ErrCode", null);
                db.SetParameterValue("ErrMsg", null);

                await db.ExecuteNonQueryAsync();

                var code = Convert.ToInt32(db.GetParameterValue("ErrCode"));
                var message = Convert.ToString(db.GetParameterValue("ErrMsg"));

                result = new ResultViewModel
                {
                    Success = code == 0,
                    Message = message,
                    ErrorCode = code
                };
            }

            var @event = new PlayerUpdatedOddsGroupEvent
            {
                LicenseeCustId = metaModel.LicenseeCustId,
                SiteName = metaModel.SiteName,
                UserId = metaModel.UserId,
                CustID = metaModel.CustId,
                Status = result.Success ? LogText.Success : LogText.False,
                NewValue = metaModel.OddsGroup,
                OldValue = metaModel.OldValue
            };

            await _publisher.Publish(@event);

            return result;
        }
    }
}