using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;

// https://gist.github.com/dj-nitehawk/4efe5ef70f813aec2c55fff3bbb833c0
// https://youtu.be/GrJJXixjR8M?si=vzCgKJXMP5FSnwy4
sealed class ApikeyAuth : AuthenticationHandler<AuthenticationSchemeOptions>
{
    internal const string SchemeName = "ApiKey";
    internal const string HeaderName = "x-api-key";
    private readonly string _apiKey;
    private readonly ILogger<ApikeyAuth> _logger;

    public ApikeyAuth(IOptionsMonitor<AuthenticationSchemeOptions> options,
                      ILoggerFactory loggerFactory,
                      UrlEncoder encoder,
                      IConfiguration config)
        : base(options, loggerFactory, encoder)
    {
        _apiKey = config["Auth:ApiKey"] ?? throw new InvalidOperationException("API key not set in appsettings.json");
        _logger = loggerFactory.CreateLogger<ApikeyAuth>();
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        Request.Headers.TryGetValue(HeaderName, out var extractedApiKey);

        if (string.IsNullOrEmpty(extractedApiKey))
        {
            _logger.LogWarning("API key missing in request.");
            return Task.FromResult(AuthenticateResult.Fail("API key is missing"));
        }

        if (!extractedApiKey.Equals(_apiKey))
        {
            _logger.LogWarning("Invalid API key attempted.");
            return Task.FromResult(AuthenticateResult.Fail("Invalid API credentials"));
        }

        // Skapa en identitet och auktorisera användaren
        var identity = new ClaimsIdentity(
             // Info om den som är inloggad här är det default då jag inte använder inloggningar
            claims: new[] { new Claim("ClientID", "Default") },
            authenticationType: Scheme.Name);

        var principal = new GenericPrincipal(identity, roles: null); // vem som är inloggad
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }

    bool IsPublicEndpoint()
        => Context.GetEndpoint()?.Metadata.OfType<AllowAnonymousAttribute>().Any() is null or true;
}

/*

// https://gist.github.com/dj-nitehawk/4efe5ef70f813aec2c55fff3bbb833c0

1. Klassen ApikeyAuth

Den här klassen är en implementering av autentisering baserad på en API-nyckel. 
Den ärver från AuthenticationHandler<AuthenticationSchemeOptions>, vilket innebär 
att den hanterar autentisering enligt det valda autentiseringssystemet.

    Konstruktorn:

        Tar in flera beroenden via injektion:

            IOptionsMonitor<AuthenticationSchemeOptions>: För att hantera autentiseringens inställningar.
            ILoggerFactory logger: För loggning av eventuella fel eller händelser.
            UrlEncoder encoder: För att hantera kodning (kan användas för att encodera/dekoda URL:er, om det behövs).
            IConfiguration config: För att hämta API-nyckeln från konfigurationen (t.ex. appsettings.json).

        _apiKey: Den här variabeln hämtar API-nyckeln från konfigurationen, vilket förväntas finnas under 
        "Auth:ApiKey" i appsettings.json. Om den inte finns, kastas ett undantag.

2. Metoden HandleAuthenticateAsync

Det här är huvuddelen av autentiseringstjänsten. Den hanterar begäran och utför autentiseringen.

    Hämtar API-nyckeln från headern:

        Den försöker hämta värdet från headern x-api-key i HTTP-begäran.

    Kontrollerar om begäran är för en offentlig endpoint:

        Om begäran inte är offentlig (dvs. den är inte markerad med [AllowAnonymous]), kontrolleras att den API-nyckel 
        som skickades med i begäran stämmer överens med den som är definierad i konfigurationen. Om API-nyckeln är 
        felaktig eller saknas returneras ett misslyckande (authenticate fail).

    Skapar en ClaimsIdentity och GenericPrincipal:

        Om autentiseringen är lyckad, skapas en ClaimsIdentity för den användare som autentiserades. 
        Den här identiteten innehåller ett claim som representerar en "ClientID" med värdet "Default".
        En GenericPrincipal skapas sedan med denna identitet, vilket gör att användaren kan autentiseras 
        för vidare begäran.

    Returnerar en autentiseringstillstånd (ticket):

        En AuthenticationTicket skapas som representerar den autentiserade användaren. 
        Den returneras som ett lyckat autentiseringstillstånd (AuthenticateResult.Success(ticket)).

3. Metoden IsPublicEndpoint

Den här metoden kontrollerar om den begärda endpointen är offentlig eller inte.

    Den söker igenom metadata för endpointen och kollar om den är markerad med [AllowAnonymous]. 
    Om en endpoint har denna attribut är den offentlig och kräver inte autentisering.

Sammanfattning:

Den här koden implementerar en anpassad autentisering som validerar att begäran innehåller en korrekt 
API-nyckel i headern (x-api-key). Om nyckeln är korrekt och om endpointen inte är offentlig, skapas en 
autentiseringstoken och begäran tillåts att fortsätta. Om nyckeln är felaktig eller saknas, 
returneras ett autentiseringsfel. Det är också viktigt att notera att autentiseringen bara tillämpas på 
icke-offentliga endpoints (de som inte har [AllowAnonymous] attributet).


*/