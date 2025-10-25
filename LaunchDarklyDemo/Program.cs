
using LaunchDarkly.Sdk;
using LaunchDarkly.Sdk.Server;

var builder = WebApplication.CreateBuilder(args);

// Read SDK key and flag key from environment variables
var sdkKey = Environment.GetEnvironmentVariable("LAUNCHDARKLY_SDK_KEY");
var flagKey = Environment.GetEnvironmentVariable("LAUNCHDARKLY_FLAG_KEY") ?? "showNewUI";

if (string.IsNullOrEmpty(sdkKey))
{
    throw new Exception("LAUNCHDARKLY_SDK_KEY is not set.");
}

// Configure LaunchDarkly client
var config = Configuration.Default(sdkKey);
var ldClient = new LdClient(config);

// Register the client in DI
builder.Services.AddSingleton(ldClient);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Test endpoint
app.MapGet("/test-flag", (LdClient client) =>
{
    var context = Context.Builder("example-user").Build();

    // Check if client is ready
    if (!client.Initialized)
    {
        return Results.Problem("⚠ LaunchDarkly client is not initialized yet.");
    }

    bool isEnabled = client.BoolVariation(flagKey, context, false);

    return Results.Ok(new
    {
        Flag = flagKey,
        Enabled = isEnabled,
        Message = isEnabled ? "✅ New UI is enabled!" : "🚀 Using classic UI."
    });
});


// Dispose client properly on shutdown
app.Lifetime.ApplicationStopping.Register(() => ldClient.Dispose());

app.Run();
