using System.ComponentModel.DataAnnotations;

namespace Fanex.Athena.PlayerAdmin.WebHost.Models
{
    public class LapTokenRequest
    {
        [Required]
        [StringLength(30)]
        public string Token { get; set; }
    }
}