using Fanex.Athena.PlayerAdmin.Commission.Criteria;
using Fanex.Athena.PlayerAdmin.Commission.Helpers;
using Fanex.Athena.PlayerAdmin.Commission.Repositories.Interfaces;
using Fanex.Athena.PlayerAdmin.Commission.ViewModels;
using Fanex.Athena.PlayerAdmin.Core.Extensions;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Repositories;
using Fanex.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Commission.Repositories
{
    public class PlayerCommissionRepository : IPlayerCommissionRepository
    {
        private readonly IDbFactory dbFactory;

        public PlayerCommissionRepository(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public async Task<IEnumerable<PlayerCommissionViewModel>> GetAllCommGroups(GetAllCommGroupsCriteria criteria)
        {
            var comms = new List<PlayerCommissionViewModel>();
            var hardcodedColumns = CommissionGroupHelper.MapHardcodedNameFromDB(criteria.Permissions, isUpdate: false);

            using (var db = dbFactory.GetObjectDb(criteria.GetSPName()))
            {
                criteria.SetParameterValues(db);
                using (var reader = await db.ExecuteReaderAsync())
                {
                    var visibility = CommissionGroupHelper.GetProductColumns(reader, hardcodedColumns);

                    while (reader.Read())
                    {
                        var comm = new PlayerCommissionViewModel();
                        comm.GroupName = Converter.ToString(reader["GroupName"]);
                        comm.MapData(reader, hardcodedColumns, visibility);
                        comms.Add(comm);
                    }
                }
            }

            return comms;
        }

        public async Task<PlayerCommissionViewModel> GetCommissionGroupById(GetCommissionGroupByIdCriteria criteria)
        {
            var comm = new PlayerCommissionViewModel();
            var hardcodedColumns = CommissionGroupHelper.MapHardcodedNameFromDB(criteria.Permissions, isUpdate: false);

            using (var db = dbFactory.GetObjectDb(criteria.GetSPName()))
            {
                criteria.SetParameterValues(db);
                using (var reader = await db.ExecuteReaderAsync())
                {
                    var visibility = CommissionGroupHelper.GetProductColumns(reader, hardcodedColumns);

                    while (reader.Read())
                    {
                        comm.GroupName = Converter.ToString(reader["GroupName"]);
                        comm.MapData(reader, hardcodedColumns, visibility);
                    }
                }
            }

            return comm;
        }

        public async Task<IResultInfo> MultipleMoveGroup(MultipleMoveGroupCriteria criteria)
        {
            var result = new ResultInfo();

            using (var db = dbFactory.GetObjectDb(criteria.GetSPName()))
            {
                criteria.SetParameterValues(db);
                await db.ExecuteNonQueryAsync().ConfigureAwait(false);
                AssignResult(db, result);
                return result;
            }
        }

        public async Task<IResultInfo> MultipleRemoveFromGroup(MultipleRemoveFromGroupCriteria criteria)
        {
            var result = new ResultInfo();

            using (var db = dbFactory.GetObjectDb(criteria.GetSPName()))
            {
                criteria.SetParameterValues(db);
                await db.ExecuteNonQueryAsync().ConfigureAwait(false);
                AssignResult(db, result);
                return result;
            }
        }

        public async Task<IResultInfo> MultipleUpdateGroup(MultipleUpdateGroupCriteria criteria)
        {
            var result = new ResultInfo();

            using (var db = dbFactory.GetObjectDb(criteria.GetSPName()))
            {
                criteria.SetParameterValues(db);
                await db.ExecuteNonQueryAsync();
                AssignResult(db, result);
                return result;
            }
        }

        public async Task<PlayerCommissionViewModel> GetPlayerCommissionGroup(GetPlayerCommissionGroupCriteria criteria)
        {
            var comm = new PlayerCommissionViewModel();
            var hardcodedColumns = CommissionGroupHelper.MapHardcodedNameFromDB(criteria.Permissions, isUpdate: false);

            using (var db = dbFactory.GetObjectDb(criteria.GetSPName()))
            {
                criteria.SetParameterValues(db);
                using (var reader = await db.ExecuteReaderAsync())
                {
                    var visibility = CommissionGroupHelper.GetProductColumns(reader, hardcodedColumns);

                    while (reader.Read())
                    {
                        comm.MapData(reader, hardcodedColumns, visibility);
                    }
                }
            }

            return comm;
        }

        private static void AssignResult(IObjectDb db, IResultInfo result)
        {
            var errorCode = db.GetParameterValue("ErrCode");
            var errorMessage = db.GetParameterValue("ErrMsg");

            result.Code = Converter.ToInt(errorCode);
            result.Message = Converter.ToString(errorMessage);
            result.ExtendedData = Converter.ToString(db.GetParameterValue("strCustId"));
        }
    }
}