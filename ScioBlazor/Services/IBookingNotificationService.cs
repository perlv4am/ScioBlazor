using System.Threading;
using System.Threading.Tasks;
using ScioBlazor.Data;

namespace ScioBlazor.Services;

public interface IBookingNotificationService
{
    Task NotifyBookingCreated(Meeting meeting, ApplicationUser owner, CancellationToken ct = default);
    Task NotifyBookingRescheduled(Meeting meeting, ApplicationUser owner, DateTime oldStartUtc, DateTime oldEndUtc, CancellationToken ct = default);
    Task NotifyBookingCanceled(Meeting meeting, ApplicationUser owner, CancellationToken ct = default);
}
