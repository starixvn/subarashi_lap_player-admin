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

    public class PlayerSpecialLevelService : IPlayerSpecialLevelService
    {
        private readonly IDbFactory _dbFactory;
        private readonly IPublisher _publisher;

        public PlayerSpecialLevelService(IDbFactory dbFactory, IPublisher publisher)
        {
            _dbFactory = dbFactory;
            _publisher = publisher;
        }

        public async Task<ResultViewModel> Update(SpecialLevelUpdatingMetaModel metaModel)
        {
            const string UspName = "Lrf_CustomerUpdSpecialLevel";
            ResultViewModel result;

            using (var db = _dbFactory.GetObjectDb(UspName))
            {
                db.SetParameterValue("custid", metaModel.CustId);
                db.SetParameterValue("userid", metaModel.UserId);
                db.SetParameterValue("speciallevel", metaModel.SpecialLevel);
                db.SetParameterValue("ip", metaModel.Ip);
                db.SetParameterValue("langkey", metaModel.Language);
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

            var @event = new PlayerUpdatedSpecialLevelEvent
            {
                LicenseeCustId = metaModel.LicenseeCustId,
                SiteName = metaModel.SiteName,
                UserId = metaModel.UserId,
                CustID = metaModel.CustId,
                Status = result.Success ? LogText.Success : LogText.False,
                NewValue = metaModel.SpecialLevel.ToString(),
                OldValue = metaModel.OldValue
            };

            await _publisher.Publish(@event);

            return result;
        }
    }
}