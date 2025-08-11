namespace Fanex.Athena.PlayerAdmin.API.Services
{
    using System;
    using System.Threading.Tasks;
    using Core.DataDomains;
    using Core.Mediator;
    using Core.Models;
    using Core.Repositories;
    using Interfaces;
    using MediatR;
    using MetaModel;

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