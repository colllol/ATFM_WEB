using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Options;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddOptions<FlightTrackingOptions>()
    .Bind(builder.Configuration.GetSection(FlightTrackingOptions.SectionName))
    .Validate(options => !string.IsNullOrWhiteSpace(options.ApiKey)
            && options.ApiKey.Length >= 16
            && !string.Equals(options.ApiKey, "CHANGE_ME_MINIMUM_16_CHARACTERS", StringComparison.Ordinal),
        "FlightTracking:ApiKey phải có ít nhất 16 ký tự và không được giữ giá trị mẫu.")
    .ValidateOnStart();

builder.Services.AddSingleton(serviceProvider =>
{
    var connectionString = builder.Configuration.GetConnectionString("TracksPostgres");
    if (string.IsNullOrWhiteSpace(connectionString))
        throw new InvalidOperationException("Thiếu ConnectionStrings:TracksPostgres.");

    return NpgsqlDataSource.Create(connectionString);
});
builder.Services.AddSingleton<TrackRepository>();

var app = builder.Build();

app.UseExceptionHandler(errorApp => errorApp.Run(async context =>
{
    var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
    app.Logger.LogError(exception, "Lỗi chưa xử lý khi phục vụ API flight tracking.");
    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
    await Results.Problem(
        statusCode: StatusCodes.Status500InternalServerError,
        title: "Không thể đọc dữ liệu flight tracking.")
        .ExecuteAsync(context);
}));

app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/health"))
    {
        await next();
        return;
    }

    var configuredKey = context.RequestServices.GetRequiredService<IOptions<FlightTrackingOptions>>().Value.ApiKey;
    var suppliedKey = context.Request.Headers["X-API-Key"].ToString();
    if (!SecureEquals(configuredKey, suppliedKey))
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await Results.Problem(
            statusCode: StatusCodes.Status401Unauthorized,
            title: "API key không hợp lệ.")
            .ExecuteAsync(context);
        return;
    }

    await next();
});

app.MapGet("/health/live", () => Results.Ok(new { status = "ok" }));

app.MapGet("/health/ready", async (NpgsqlDataSource dataSource, CancellationToken cancellationToken) =>
{
    try
    {
        await using var command = dataSource.CreateCommand("SELECT 1");
        await command.ExecuteScalarAsync(cancellationToken);
        return Results.Ok(new { status = "ready" });
    }
    catch
    {
        return Results.Problem(statusCode: StatusCodes.Status503ServiceUnavailable, title: "PostgreSQL chưa sẵn sàng.");
    }
});

app.MapGet("/api/v1/tracks", async (
    string? date,
    TrackRepository repository,
    IOptions<FlightTrackingOptions> configuredOptions,
    CancellationToken cancellationToken) =>
{
    DateOnly requestedDate;
    if (string.IsNullOrWhiteSpace(date))
    {
        requestedDate = DateOnly.FromDateTime(DateTime.UtcNow);
    }
    else if (!DateOnly.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out requestedDate))
    {
        return Results.BadRequest(new { error = "Tham số date phải có định dạng yyyy-MM-dd." });
    }

    var options = configuredOptions.Value;
    var today = DateOnly.FromDateTime(DateTime.UtcNow);
    if (requestedDate > today || requestedDate < today.AddDays(-options.MaxLookbackDays))
    {
        return Results.BadRequest(new
        {
            error = $"Chỉ được truy vấn từ {today.AddDays(-options.MaxLookbackDays):yyyy-MM-dd} đến {today:yyyy-MM-dd}."
        });
    }

    var tracks = await repository.GetLatestTracksAsync(requestedDate, cancellationToken);
    return Results.Ok(new TrackResponse(
        requestedDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
        DateTime.UtcNow.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'", CultureInfo.InvariantCulture),
        tracks.Count,
        tracks));
});

app.Run();

static bool SecureEquals(string expected, string supplied)
{
    var expectedBytes = Encoding.UTF8.GetBytes(expected ?? string.Empty);
    var suppliedBytes = Encoding.UTF8.GetBytes(supplied ?? string.Empty);
    return expectedBytes.Length == suppliedBytes.Length
        && CryptographicOperations.FixedTimeEquals(expectedBytes, suppliedBytes);
}

internal sealed class FlightTrackingOptions
{
    public const string SectionName = "FlightTracking";
    public string ApiKey { get; init; } = string.Empty;
    public int MaxLookbackDays { get; init; } = 7;
}

internal sealed class TrackRepository(NpgsqlDataSource dataSource)
{
    private const string LatestTracksSql = """
        SELECT DISTINCT ON (normalized_callsign)
               flight_id_current,
               normalized_callsign,
               updated_at_timestamp,
               last_lat,
               last_lon,
               COALESCE(last_track_deg, 0)
        FROM (
            SELECT flight_id_current,
                   UPPER(TRIM(SPLIT_PART(COALESCE(flight_id_current, ''), '-', 1))) normalized_callsign,
                   NULLIF(TRIM(updated_at_utc), '')::timestamp updated_at_timestamp,
                   last_lat,
                   last_lon,
                   last_track_deg
            FROM public.tracks
            WHERE flight_id_current IS NOT NULL
              AND updated_at_utc IS NOT NULL
              AND last_lat IS NOT NULL
              AND last_lon IS NOT NULL
              AND NULLIF(TRIM(updated_at_utc), '')::timestamp >= $1
              AND NULLIF(TRIM(updated_at_utc), '')::timestamp < $2
        ) source
        WHERE normalized_callsign <> ''
        ORDER BY normalized_callsign, updated_at_timestamp DESC
        """;

    public async Task<List<TrackItem>> GetLatestTracksAsync(DateOnly date, CancellationToken cancellationToken)
    {
        var fromDate = DateTime.SpecifyKind(date.ToDateTime(TimeOnly.MinValue), DateTimeKind.Unspecified);
        var result = new List<TrackItem>();

        await using var command = dataSource.CreateCommand(LatestTracksSql);
        command.CommandTimeout = 30;
        command.Parameters.AddWithValue(fromDate);
        command.Parameters.AddWithValue(fromDate.AddDays(1));

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            result.Add(new TrackItem(
                reader.GetString(0),
                reader.GetString(1),
                reader.GetDateTime(2).ToString("yyyy-MM-dd'T'HH:mm:ss", CultureInfo.InvariantCulture),
                Convert.ToDouble(reader.GetValue(3), CultureInfo.InvariantCulture),
                Convert.ToDouble(reader.GetValue(4), CultureInfo.InvariantCulture),
                Convert.ToDouble(reader.GetValue(5), CultureInfo.InvariantCulture)));
        }

        return result;
    }
}

internal sealed record TrackItem(
    string FlightIdCurrent,
    string Callsign,
    string UpdatedAtUtc,
    double Latitude,
    double Longitude,
    double Heading);

internal sealed record TrackResponse(
    string Day,
    string ServerTimeUtc,
    int Count,
    IReadOnlyList<TrackItem> Flights);

public partial class Program;
