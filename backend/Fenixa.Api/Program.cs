using Fenixa.Api;
using Fenixa.Api.Behaviors;
using Fenixa.Api.Infrastructure;
using Fenixa.Api.Services;
using Content;
using Identity;
using Identity.Features.LoginUser;
using Identity.Features.RefreshToken;
using Identity.Features.RegisterUser;
using Identity.Features.UpdateGeminiKey;
using Identity.Infrastructure;
using Profile;
using Profile.Features.GetUserSettings;
using Profile.Features.UpdateSettings;
using Shared.Abstractions;
using Study;
using Study.Features.GetStudySession;
using Study.Features.GetStudyStatistics;
using Study.Features.LogDailyActivity;
using Study.Features.ReviewCard;

var builder = WebApplication.CreateBuilder(args);

string privateKeyPath = Path.Combine(builder.Environment.ContentRootPath, "keys", "private.pem");
string publicKeyPath = Path.Combine(builder.Environment.ContentRootPath, "keys", "public.pem");

string privateKey = File.ReadAllText(privateKeyPath);
string publicKey = File.ReadAllText(publicKeyPath);

string issuer = builder.Configuration.GetValue<string>("Jwt:Issuer")
    ?? throw new InvalidOperationException("Jwt:Issuer is missing in appsettings.json");

string audience = builder.Configuration.GetValue<string>("Jwt:Audience")
    ?? throw new InvalidOperationException("Jwt:Audience is missing in appsettings.json");

var cryptoOptions = new CryptoOptions
{
    PrivateKey = privateKey,
    PublicKey = publicKey,
    Issuer = issuer,
    Audience = audience
};

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserContext, CurrentUserContext>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(
        typeof(RegisterUserCommand).Assembly,
        typeof(GetUserSettingsQuery).Assembly,
        typeof(Content.Features.CreateDeck.CreateDeckCommand).Assembly,
        typeof(ReviewCardCommand).Assembly);

    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services
    .AddIdentityModule(builder.Configuration, cryptoOptions)
    .AddProfileModule(builder.Configuration)
    .AddContentModule(builder.Configuration)
    .AddStudyModule(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

await app.ApplyDatabaseMigrationsAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapIdentityEndpoints();
app.MapProfileEndpoints();
app.MapContentEndpoints();
app.MapStudyEndpoints();

app.Run();
