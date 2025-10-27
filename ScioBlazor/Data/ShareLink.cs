using System.ComponentModel.DataAnnotations;

namespace ScioBlazor.Data
{
    public class ShareLink
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(64)]
        public string Token { get; set; } = default!;

        [Required]
        public string OwnerId { get; set; } = default!;

        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    }
}

