namespace Fanex.Athena.PlayerAdmin.Commission.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Extensions;
    using Core.Models;
    using Core.Repositories;
    using Criteria;
    using Data;
    using Interfaces;
    using ViewModels;

    public class PlayerCommissionRepository : IPlayerCommissionRepository
    {
        private readonly IDbFactory dbFactory;

        public PlayerCommissionRepository(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public async Task<IEnumerable<PlayerCommissionSettingViewModel>> GetAllCommSettingAsync(ProductAccess productAccess)
        {
            const string UspName = "Lrf_LicenseeCommProductList";

            var comSettings = new List<PlayerCommissionSettingViewModel>();

            using (var db = dbFactory.GetObjectDb(UspName))
            {
                using var reader = await db.ExecuteReaderAsync().ConfigureAwait(false);

                while (reader.Read())
                {
                    var productCode = Convert.ToString(reader["ProductCode"]);

                    comSettings.Add(new PlayerCommissionSettingViewModel
                    {
                        Sporttype = Convert.ToInt32(reader["Sporttype"]),
                        Bettype = Convert.ToInt32(reader["Bettype"]),
                        ProductCode = productCode,
                        ProductName = productAccess.GetProductName(productCode)
                    });
                }
            }

            return comSettings;
        }

        public async Task<(IEnumerable<PlayerCommissionViewModel>, IEnumerable<PlayerCommissionSettingViewModel>)> GetAllCommGroups(GetAllCommGroupsCriteria criteria, ProductAccess productAccess)
        {
            var comms = new List<PlayerCommissionViewModel>();
            var commValues = new List<PlayerCommissionSettingViewModel>();

            using (var db = dbFactory.GetObjectDb(criteria.SPName))
            {
                criteria.SetParameterValues(db);
                using var reader = await db.ExecuteReaderAsync();

                while (reader.Read())
                {
                    comms.Add(new PlayerCommissionViewModel
                    {
                        GroupName = Converter.ToString(reader["GroupName"]),
                        GroupCommId = reader.ToInt("GroupDiscountID")
                    });
                }

                reader.NextResult();

                while (comms.Any() && reader.Read())
                {
                    var productCode = Convert.ToString(reader["ProductCode"]);

                    commValues.Add(new PlayerCommissionSettingViewModel
                    {
                        Sporttype = Convert.ToInt32(reader["Sporttype"]),
                        Bettype = Convert.ToInt32(reader["Bettype"]),
                        ProductCode = productCode,
                        ProductName = productAccess.GetProductName(productCode),
                        Discount = Convert.ToDecimal(reader["Discount"]),
                        GroupCommId = Convert.ToInt32(reader["GroupDiscountID"])
                    });
                }
            }

            return (comms, commValues);
        }

        public async Task<(PlayerCommissionViewModel, IEnumerable<PlayerCommissionSettingViewModel>)> GetCommissionGroupById(GetCommissionGroupByIdCriteria criteria, ProductAccess productAccess)
        {
            var comm = new PlayerCommissionViewModel();
            var commValues = new List<PlayerCommissionSettingViewModel>();

            using (var db = dbFactory.GetObjectDb(criteria.SPName))
            {
                criteria.SetParameterValues(db);
                using var reader = await db.ExecuteReaderAsync();

                while (reader.Read())
                {
                    comm.GroupName = Converter.ToString(reader["GroupName"]);
                    comm.GroupCommId = reader.ToInt("GroupDiscountID");
                }

                reader.NextResult();

                while (reader.Read())
                {
                    var productCode = Convert.ToString(reader["ProductCode"]);

                    commValues.Add(new PlayerCommissionSettingViewModel
                    {
                        Sporttype = Convert.ToInt32(reader["Sporttype"]),
                        Bettype = Convert.ToInt32(reader["Bettype"]),
                        ProductCode = productCode,
                        ProductName = productAccess.GetProductName(productCode),
                        Discount = Convert.ToDecimal(reader["Discount"])
                    });
                }
            }

            return (comm, commValues);
        }

        public async Task<IResultInfo> MultipleMoveGroup(MultipleMoveGroupCriteria criteria)
        {
            var result = new ResultInfo();

            using var db = dbFactory.GetObjectDb(criteria.GetSPName());
            criteria.SetParameterValues(db);

            await db.ExecuteNonQueryAsync().ConfigureAwait(false);
            AssignResult(db, result);

            return result;
        }

        public async Task<IResultInfo> MultipleRemoveFromGroup(MultipleRemoveFromGroupCriteria criteria)
        {
            var result = new ResultInfo();

            using var db = dbFactory.GetObjectDb(criteria.GetSPName());
            criteria.SetParameterValues(db);

            await db.ExecuteNonQueryAsync().ConfigureAwait(false);
            AssignResult(db, result);

            return result;
        }

        public async Task<IResultInfo> MultipleUpdateGroup(MultipleUpdateGroupCriteria criteria)
        {
            var result = new ResultInfo();

            using var db = dbFactory.GetObjectDb(criteria.GetSPName());
            criteria.SetParameterValues(db);

            await db.ExecuteNonQueryAsync();
            AssignResult(db, result);

            return result;
        }

        public async Task<(PlayerCommissionViewModel, IEnumerable<PlayerCommissionSettingViewModel>)> GetPlayerCommissionGroup(GetPlayerCommissionGroupCriteria criteria, ProductAccess productAccess)
        {
            var comm = new PlayerCommissionViewModel();
            var commValues = new List<PlayerCommissionSettingViewModel>();

            using (var db = dbFactory.GetObjectDb(criteria.SPName))
            {
                criteria.SetParameterValues(db);
                using var reader = await db.ExecuteReaderAsync();
                while (reader.Read())
                {
                    comm.GroupCommId = reader.ToInt("GroupDiscountID");
                }

                reader.NextResult();

                while (reader.Read())
                {
                    var productCode = Convert.ToString(reader["ProductCode"]);

                    commValues.Add(new PlayerCommissionSettingViewModel
                    {
                        Sporttype = Convert.ToInt32(reader["Sporttype"]),
                        Bettype = Convert.ToInt32(reader["Bettype"]),
                        ProductCode = productCode,
                        ProductName = productAccess.GetProductName(productCode),
                        Discount = Convert.ToDecimal(reader["PlayerDiscount"])
                    });
                }
            }

            return (comm, commValues);
        }

        private static void AssignResult(IObjectDb db, IResultInfo result)
        {
            var errorCode = db.GetParameterValue("ErrCode");
            var errorMessage = db.GetParameterValue("ErrMsg");

            result.Code = Converter.ToInt(errorCode);
            result.Message = Converter.ToString(errorMessage);
        }
    }
}