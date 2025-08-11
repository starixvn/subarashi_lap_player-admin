namespace Fanex.Athena.PlayerAdmin.BatchUpdate.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Extensions;
    using Core.Models;
    using Core.Repositories;
    using Criteria;
    using Data;

    public class PlayerBatchUpdateRepository : IPlayerBatchUpdateRepository
    {
        private readonly IDbFactory _dbFactory;

        public PlayerBatchUpdateRepository(IDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<IDictionary<string, string>> GetOldValues(IEnumerable<int> custIds, int type)
        {
            var criteria = new GetOldValuesCriteria(custIds, type);
            var result = new Dictionary<string, string>();

            using (var db = _dbFactory.GetObjectDb(criteria.GetSPName()))
            {
                criteria.SetParameterValues(db);
                var valueCoumn = criteria.GetValueColumnName();

                using (var reader = await db.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var custId = reader.ToString("custid");
                        result[custId] = reader.ToString(valueCoumn);
                    }
                }
            }

            return result;
        }

        public async Task<IResultInfo> Update(BasePlayerBatchUpdateCriteria criteria)
        {
            var result = new ResultInfo();

            using (var db = _dbFactory.GetObjectDb(criteria.GetSPName()))
            {
                criteria.SetParameterValues(db);
                await db.ExecuteNonQueryAsync();
                AssignOutputResult(db, result);
                return result;
            }
        }

        private static void AssignOutputResult(IObjectDb db, IResultInfo result)
        {
            var errorCode = db.GetParameterValue("ErrCode");
            var errorMessage = db.GetParameterValue("ErrMsg");

            result.Code = Converter.ToInt(errorCode);
            result.Message = Converter.ToString(errorMessage);
        }
    }
}