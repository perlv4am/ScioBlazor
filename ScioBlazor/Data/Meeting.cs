using System.ComponentModel.DataAnnotations;

namespace ScioBlazor.Data
{
    public class Meeting
    {
        public int Id { get; set; }

        [Required]
        public string OwnerId { get; set; } = default!;

        public DateTime StartUtc { get; set; }

        public DateTime EndUtc { get; set; }

        [MaxLength(100)]
        public string? AttendeeName { get; set; }

        [MaxLength(256)]
        [EmailAddress]
        public string? AttendeeEmail { get; set; }

        // One meeting per share link (optional for legacy/owner-created meetings)
        public int? ShareLinkId { get; set; }

        // You can extend with EndUtc, Title, etc.
    }
}
