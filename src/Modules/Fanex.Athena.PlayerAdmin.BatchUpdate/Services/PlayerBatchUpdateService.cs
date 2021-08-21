using Fanex.Athena.PlayerAdmin.BatchUpdate.Criteria;
using Fanex.Athena.PlayerAdmin.BatchUpdate.MetaModels;
using Fanex.Athena.PlayerAdmin.BatchUpdate.Repositories;
using Fanex.Athena.PlayerAdmin.Core.DataDomains;
using Fanex.Athena.PlayerAdmin.Core.Mediator;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Resources;
using Fanex.Athena.PlayerAdmin.Core.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BatchUpdate.Services
{
    public class PlayerBatchUpdateService : IPlayerBatchUpdateService
    {
        private const int TurnOffVIPErrorCode = 105;

        private readonly IPublisher _publisher;
        private readonly IPlayerBatchUpdateRepository _repository;
        private readonly ILicenseeService _licenseeService;
        private readonly IContextService _contextService;

        public PlayerBatchUpdateService(IPublisher publisher, IContextService contextService,
            ILicenseeService licenseeService,
            IPlayerBatchUpdateRepository repository)
        {
            _publisher = publisher;
            _contextService = contextService;
            _licenseeService = licenseeService;
            _repository = repository;
        }

        public async Task<IResultInfo> UpdateBAStatus(UpdateBAStatusApiMetaModel metaModel)
        {
            var result = new ResultInfo();
            var errors = new List<string>();

            var customers = ToCustomerList(metaModel.CustIds, metaModel.CustNames);
            var validCustIds = await _licenseeService.GetCustomersBelongTo(_contextService.AgentList, metaModel.CustIds);
            var oldValues = await _repository.GetOldValues(validCustIds, GetOldValuesCriteria.BAStatusType);
            var addTurnOffVIPError = false;
            var hasError = false;
            var extendedData = new List<string>();

            foreach (var cust in customers)
            {
                var custId = cust.Id.ToString();
                var custName = cust.Name;

                if (!validCustIds.Contains(cust.Id))
                {
                    errors.Add($"Access Denied. ({custName})");
                    continue;
                }

                var oldValue = oldValues.ContainsKey(custId) ? oldValues[custId] : string.Empty;
                if (string.Equals(metaModel.BAStatusId, oldValue, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var criteria = new UpdateBAStatusCriteria(metaModel, custId);
                var tempResult = await _repository.Update(criteria);
                if (tempResult.Code == TurnOffVIPErrorCode)
                {
                    addTurnOffVIPError = true;
                    continue;
                }
                else if (!tempResult.IsSuccessful)
                {
                    hasError = true;
                    errors.Add($"{tempResult.Message} ({custName})");
                }
                else
                {
                    extendedData.Add(custId);
                }

                var @event = new PlayerUpdatedBAEvent
                {
                    LicenseeCustId = metaModel.LicenseeId,
                    SiteName = metaModel.SiteName,
                    UserId = metaModel.UserId,
                    CustID = cust.Id,
                    Status = tempResult.IsSuccessful ? LogText.Success : LogText.False,
                    NewValue = metaModel.BAStatusId == "1" ? LogText.On : LogText.Off,
                    OldValue = oldValue
                };

                await _publisher.Publish(@event);
            }

            result.Code = hasError ? 1 : 0;
            var mssg = errors.Any() ? string.Join("<br />", errors) : ViewResources.UpdateSuccess;
            result.Message = addTurnOffVIPError ? $"{ViewResources.MessageTurnOffVIPError}<br />{mssg}" : mssg;
            result.ExtendedData = string.Join(",", extendedData);
            return result;
        }

        public async Task<IResultInfo> UpdateOddsGroup(UpdateOddsGroupApiMetaModel metaModel)
        {
            var result = new ResultInfo();
            var errors = new List<string>();
            var extendedData = new List<string>();

            var customers = ToCustomerList(metaModel.CustIds, metaModel.CustNames);
            var validCustIds = await _licenseeService.GetCustomersBelongTo(_contextService.AgentList, metaModel.CustIds);
            var oldValues = await _repository.GetOldValues(validCustIds, GetOldValuesCriteria.OddsGroupType);
            var oldBAs = await _repository.GetOldValues(validCustIds, GetOldValuesCriteria.BAStatusType);
            var addBAError = false;

            foreach (var cust in customers)
            {
                var custId = cust.Id.ToString();
                var custName = cust.Name;
                if (!validCustIds.Contains(cust.Id))
                {
                    errors.Add($"Access Denied. ({custName})");
                    continue;
                }

                var oldValue = oldValues.ContainsKey(custId) ? oldValues[custId] : string.Empty;
                if (string.Equals(metaModel.OddsGroupId, oldValue, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (oldBAs[custId] == "1")
                {
                    addBAError = true;
                    continue;
                }

                var criteria = new UpdateOddsGroupCriteria(metaModel, custId);
                var tempResult = await _repository.Update(criteria);
                if (!tempResult.IsSuccessful)
                {
                    errors.Add($"{tempResult.Message} ({custName})");
                }
                else
                {
                    extendedData.Add(custId);
                }

                var @event = new PlayerUpdatedOddsGroupEvent
                {
                    LicenseeCustId = metaModel.LicenseeId,
                    SiteName = metaModel.SiteName,
                    UserId = metaModel.UserId,
                    CustID = cust.Id,
                    Status = tempResult.IsSuccessful ? LogText.Success : LogText.False,
                    NewValue = metaModel.OddsGroupId,
                    OldValue = oldValue
                };

                await _publisher.Publish(@event);
            }

            result.Code = errors.Any() ? 1 : 0;
            var msg = errors.Any() ? string.Join("<br />", errors) : ViewResources.UpdateSuccess;
            result.Message = addBAError ? $"{ViewResources.MessageUpdateOddsGroupForBAPlayers}<br />{msg}" : msg;
            result.ExtendedData = string.Join(",", extendedData);
            return result;
        }

        public async Task<IResultInfo> UpdateSpecialLevel(UpdateSpecialLevelApiMetaModel metaModel)
        {
            var result = new ResultInfo();
            var errors = new List<string>();
            var extendedData = new List<string>();

            var customers = ToCustomerList(metaModel.CustIds, metaModel.CustNames);
            var validCustIds = await _licenseeService.GetCustomersBelongTo(_contextService.AgentList, metaModel.CustIds);
            var oldValues = await _repository.GetOldValues(validCustIds, GetOldValuesCriteria.SpecialLevelType);

            foreach (var cust in customers)
            {
                var custId = cust.Id.ToString();
                var custName = cust.Name;
                if (!validCustIds.Contains(cust.Id))
                {
                    errors.Add($"Access Denied. ({custName})");
                    continue;
                }

                var oldValue = oldValues.ContainsKey(custId) ? oldValues[custId] : string.Empty;
                if (string.Equals(metaModel.SpecialLevelId, oldValue, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var criteria = new UpdateSpecialLevelCriteria(metaModel, custId);
                var tempResult = await _repository.Update(criteria);
                if (!tempResult.IsSuccessful)
                {
                    errors.Add($"{tempResult.Message} ({custName})");
                }
                else
                {
                    extendedData.Add(custId);
                }

                var @event = new PlayerUpdatedSpecialLevelEvent
                {
                    LicenseeCustId = metaModel.LicenseeId,
                    SiteName = metaModel.SiteName,
                    UserId = metaModel.UserId,
                    CustID = cust.Id,
                    Status = tempResult.IsSuccessful ? LogText.Success : LogText.False,
                    NewValue = metaModel.SpecialLevelId,
                    OldValue = oldValue
                };

                await _publisher.Publish(@event);
            }

            result.Code = errors.Any() ? 1 : 0;
            result.Message = errors.Any() ? string.Join("<br />", errors) : ViewResources.UpdateSuccess;
            result.ExtendedData = string.Join(",", extendedData);
            return result;
        }

        private static IEnumerable<Customer> ToCustomerList(string custIds, string custNames)
        {
            var result = new List<Customer>();
            var ids = custIds.Split(new[] { ',' });
            var names = custNames.Split(new[] { ',' });
            for (var i = 0; i < ids.Length; i++)
            {
                var id = ids[i];
                var name = names[i];

                if (!int.TryParse(id, out _))
                {
                    continue;
                }

                result.Add(new Customer
                {
                    Id = int.Parse(id),
                    Name = name
                });
            }

            return result;
        }
    }
}