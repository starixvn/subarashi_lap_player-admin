using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.BetLimit.DTO.BetLimits;
using Fanex.Athena.PlayerAdmin.BetLimit.Helpers;
using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.BetLimit.Repositories.Interfaces;
using Fanex.Athena.PlayerAdmin.BetLimit.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.Core.Helpers;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Settings;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Fanex.Athena.PlayerAdmin.BetLimit.Services
{
    public class AGCasinoBetLimitService : IAGCasinoBetLimitService
    {
        private readonly ILogger<AGCasinoBetLimitService> _logger;
        private readonly IPublisher _publisher;
        private readonly BetLimitApiSetting _appSetting;
        private readonly IAGCasinoBetLimitRepository _repository;
        private readonly HttpClient _httpClient;

        public AGCasinoBetLimitService(
            ILogger<AGCasinoBetLimitService> logger,
            IOptions<BetLimitApiSetting> appSetting,
            IAGCasinoBetLimitRepository repository,
            HttpClient httpClient,
            IPublisher publisher)
        {
            _logger = logger;
            _appSetting = appSetting.Value;
            _repository = repository;
            _publisher = publisher;
            _httpClient = httpClient;
        }

        public async Task<IList<AgCasinoGame>> GetGroupBettingsAsync(string siteName, int currency)
        {
            var games = new List<AgCasinoGame>();
            try
            {
                using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, new Uri(_appSetting.AGCasino)))
                {
                    httpRequest.Content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>(nameof(siteName), siteName),
                        new KeyValuePair<string, string>(nameof(currency), currency.ToString())
                    });

                    var response = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
                    var stringResult = await response.Content.ReadAsStringAsync();
                    var xml = XDocument.Parse(stringResult);

                    foreach (var xmlGame in xml.Descendants("Game"))
                    {
                        var game = new AgCasinoGame
                        {
                            GameType = xmlGame.Attribute("GameType").Value,
                            Name = xmlGame.Attribute("Name").Value,
                            Groups = new List<AgCasinoGroup>()
                        };

                        foreach (var xmlGroup in xmlGame.Descendants("Group"))
                        {
                            game.Groups.Add(new AgCasinoGroup
                            {
                                GroupType = xmlGroup.Attribute("GroupType").Value,
                                Min = Convert.ToInt32(xmlGroup.Element("Min").Value),
                                Max = Convert.ToInt32(xmlGroup.Element("Max").Value)
                            });
                        }

                        games.Add(game);
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, ErrorMessageHelper.GenerateMessage(exception));
            }

            return games;
        }

        public async Task<AgCasinoBetSetting> GetBetSettingAsync(int custId)
        {
            return await _repository.GetBetSettingAsync(custId);
        }

        public async Task<IResultInfo> UpdateSettingAsync(PlayerBetLimitUpdateMetaModel<AgCasinoBetLimitMetaModel> metaModel)
        {
            var result = await _repository.UpdateSettingAsync(metaModel);
            var @event = BetLimitLogHelper.CreateLogEvent(metaModel, result.Code == 0);
            await _publisher.Publish(@event);
            return result;
        }
    }
}