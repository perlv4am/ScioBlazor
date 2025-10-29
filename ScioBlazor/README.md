# ScioBlazor — Scheduling & Calendar (Job Interview Task)

## Tech Stack

- .NET 9, Blazor Server, ASP.NET Core Identity
- EF Core + SQL Server (LocalDB in development)
- SendGrid (via `IBookingNotificationService` abstraction)
- Declension (via www.sklonovani-jmen.cz)
- External auth: Google
- Configuration: `appsettings.json` + user‑secrets / env vars

## Project Structure

- `Components/Pages/` — Blazor pages
  - `Calendar.razor` — monthly owner view with event actions
  - `Schedule.razor` — single‑use booking link for external attendees
  - `Reschedule.razor` — owner‑only rescheduling flow
- `Components/Account/Pages/` — Identity UI (Google‑only sign‑in)
- `Services/` — abstractions and integrations
  - `INameDeclensionService`, `SklonovaniJmenService`
  - `IBookingNotificationService`, `SendGridBookingNotificationService`
- `Data/` — EF Core context, entities, migrations
  - `ApplicationUser` (adds First/Last name + instrumental first name)
- `Program.cs` — service registration, culture, auth, and migrations

## Getting Started

### Prerequisites

- .NET 9 SDK
- SQL Server LocalDB (or a reachable SQL Server instance)
- A Google OAuth client (Web) for external login
- A SendGrid API key (optional for local)
- An API key for www.sklonovani-jmen.cz
- A sklonovani‑jmen API key (optional; app gracefully falls back)

### Configuration

1) Database
- `appsettings.json` has a default LocalDB connection string. Adjust if needed.

2) Google Auth (required for sign‑in)
- Set via user‑secrets or environment variables:
  - `Authentication:Google:ClientId`
  - `Authentication:Google:ClientSecret`

3) SendGrid (optional but recommended)
- User‑secrets or env vars under `Email:SendGrid`:
  - `Email:SendGrid:ApiKey`
  - `Email:SendGrid:FromAddress`
  - `Email:SendGrid:FromName`

4) Czech Name Declension API (optional)
- Move the key to user‑secrets (do not commit it):
  - `Names:SklonovaniJmen:ApiKey`
  - `Names:SklonovaniJmen:BaseUrl` (defaults to `https://www.sklonovani-jmen.cz/api`)

### User‑Secrets Quickstart

From the project directory:

```
dotnet user-secrets init

# Google
dotnet user-secrets set "Authentication:Google:ClientId" "<your-client-id>"
dotnet user-secrets set "Authentication:Google:ClientSecret" "<your-client-secret>"

# SendGrid (optional)
dotnet user-secrets set "Email:SendGrid:ApiKey" "<your-sendgrid-key>"
dotnet user-secrets set "Email:SendGrid:FromAddress" "noreply@example.com"
dotnet user-secrets set "Email:SendGrid:FromName" "ScioBlazor"

# Name declension (optional)
dotnet user-secrets set "Names:SklonovaniJmen:ApiKey" "<your-sklonovani-key>"
```

### Run

```
dotnet run
```

- App applies EF Core migrations at startup.
- Default culture is forced to `cs-CZ` for consistent formatting.
- Navigate to `/` and use Google sign‑in to provision your user.
- Calendar is at `/calendar`. Share a link from your own flow, or navigate to `/s/{token}` directly to test booking.

## Development Notes

- Encoding & Diacritics
  - All Razor/C# files are stored as UTF‑8; an `.editorconfig` is included to enforce this.
  - If you see mojibake in a terminal, open files in an editor/browser to confirm.
- Migrations
  - Migrations are applied automatically on startup. In development, failures surface in the console; in production, the app logs and continues.
- Testing
  - The app was structured for practical manual testing during interviews. If you want an automated test harness, a lightweight bUnit + EFCore.InMemory setup can be added.

## Extensibility Ideas (Future Work)

- i18n switcher with satellite resources (EN/CZ)
- Attendee‑requested reschedule flow (owner approves)
- ICS calendar attachments in e‑mails
- Per‑user working hours / capacity / holidays
- Rate limits / CAPTCHA on booking
- Owner dashboard for managing share links

## Security Considerations

- The reschedule and delete actions are owner‑only and rechecked server‑side.
- Share links are single‑use; schedule page checks if a link has already been used.
- External name declension API is optional and failure‑tolerant.

## License

No license has been specified for the interview task. Please obtain permission before reusing code outside the interview context.

