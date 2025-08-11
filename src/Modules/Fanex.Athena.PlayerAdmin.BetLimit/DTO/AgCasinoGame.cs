namespace Fanex.Athena.PlayerAdmin.BetLimit.DTO
{
    using System.Collections.Generic;
    using BetLimits;

    public class AgCasinoGame
    {
        public string GameType { get; set; }

        public string Name { get; set; }

        public IList<AgCasinoGroup> Groups { get; set; }
    }
}
