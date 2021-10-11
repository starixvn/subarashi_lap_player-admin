using System.ComponentModel.DataAnnotations;

namespace Fanex.Athena.PlayerAdmin.WebHost.Models
{
    public class LapTokenRequest
    {
        const int MaxLengthToken = 30;

        [Required]
        [StringLength(MaxLengthToken)]
        public string Token { get; set; }
    }
}