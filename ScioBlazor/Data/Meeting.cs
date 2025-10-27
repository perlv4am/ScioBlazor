using System.ComponentModel.DataAnnotations;

namespace ScioBlazor.Data
{
    public class Meeting
    {
        public int Id { get; set; }

        [Required]
        public string OwnerId { get; set; } = default!;

        public DateTime StartUtc { get; set; }

        // You can extend with EndUtc, Title, etc.
    }
}

