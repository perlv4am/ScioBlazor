using System.Threading;
using System.Threading.Tasks;
using ScioBlazor.Data;

namespace ScioBlazor.Services;

public interface IBookingNotificationService
{
    Task NotifyBookingCreated(Meeting meeting, ApplicationUser owner, CancellationToken ct = default);
}

