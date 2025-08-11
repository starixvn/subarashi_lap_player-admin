namespace Fanex.Athena.PlayerAdmin.WebHost.Models
{
    using System.ComponentModel.DataAnnotations;

    public class LapTokenRequest
    {
        const int MaxLengthToken = 30;

        [Required]
        [StringLength(MaxLengthToken)]
        public string Token { get; set; }
    }
}