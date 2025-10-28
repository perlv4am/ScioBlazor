using System.Globalization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ScioBlazor.Data;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ScioBlazor.Services;

public class SendGridBookingNotificationService : IBookingNotificationService
{
    private readonly ILogger<SendGridBookingNotificationService> _logger;
    private readonly SendGridOptions _options;

    public SendGridBookingNotificationService(ILogger<SendGridBookingNotificationService> logger, IOptions<SendGridOptions> options)
    {
        _logger = logger;
        _options = options.Value;
    }

    public async Task NotifyBookingCreated(Meeting meeting, ApplicationUser owner, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(_options.ApiKey) || string.IsNullOrWhiteSpace(_options.FromAddress))
        {
            _logger.LogWarning("SendGrid not configured; skipping email notifications.");
            return;
        }

        var client = new SendGridClient(_options.ApiKey);
        var from = new EmailAddress(_options.FromAddress, _options.FromName);

        // Compose common details
        var culture = CultureInfo.CurrentCulture;
        var startLocal = meeting.StartUtc.ToLocalTime();
        var endLocal = meeting.EndUtc.ToLocalTime();
        var whenText = $"{startLocal.ToString("dddd d. MMMM yyyy HH:mm", culture)} – {endLocal:HH:mm}";
        var ownerShort = OwnerShort(owner);

        // Attendee confirmation
        if (!string.IsNullOrWhiteSpace(meeting.AttendeeEmail))
        {
            var attendeeTo = new EmailAddress(meeting.AttendeeEmail!, meeting.AttendeeName);
            var attendeeSubject = "Potvrzení schůzky";
            var attendeeText = $"Dobrý den,\n\nVaše schůzka s {ownerShort} byla potvrzena.\n\nKdy: {whenText}\n";
            var attendeeHtml = $"<p>Dobrý den,</p><p>Vaše schůzka s <strong>{ownerShort}</strong> byla potvrzena.</p><p><strong>Kdy:</strong> {whenText}</p>";
            var aMsg = MailHelper.CreateSingleEmail(from, attendeeTo, attendeeSubject, attendeeText, attendeeHtml);
            await SafeSend(client, aMsg, "attendee", ct);
        }

        // Owner notification
        if (!string.IsNullOrWhiteSpace(owner.Email))
        {
            var ownerTo = new EmailAddress(owner.Email!, owner.UserName);
            var ownerSubject = "Nová rezervace schůzky";
            var who = meeting.AttendeeName ?? "(nezadáno)";
            var ownerText = $"Dobrý den,\n\nByla vytvořena nová rezervace.\n\nKdy: {whenText}\nKdo: {who}\n";
            var ownerHtml = $"<p>Dobrý den,</p><p>Byla vytvořena nová rezervace.</p><p><strong>Kdy:</strong> {whenText}<br/><strong>Kdo:</strong> {who}</p>";
            var oMsg = MailHelper.CreateSingleEmail(from, ownerTo, ownerSubject, ownerText, ownerHtml);
            await SafeSend(client, oMsg, "owner", ct);
        }
    }

    private static string OwnerShort(ApplicationUser owner)
    {
        if (!string.IsNullOrWhiteSpace(owner.FirstName) || !string.IsNullOrWhiteSpace(owner.LastName))
        {
            var first = owner.FirstName ?? string.Empty;
            var initial = !string.IsNullOrWhiteSpace(owner.LastName) ? $" {owner.LastName![0].ToString().ToUpperInvariant()}." : string.Empty;
            var composed = (first + initial).Trim();
            if (!string.IsNullOrEmpty(composed)) return composed;
        }
        return owner.UserName ?? "uživatel";
    }

    private async Task SafeSend(SendGridClient client, SendGridMessage message, string tag, CancellationToken ct)
    {
        try
        {
            var response = await client.SendEmailAsync(message, ct);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("SendGrid send failed for {Tag} with status {Status}", tag, response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SendGrid exception for {Tag}", tag);
        }
    }
}

