using BuitwithApitestproject.Services;
DotNetEnv.Env.Load();
var builder = WebApplication.CreateBuilder(args);

var apiKey = Environment.GetEnvironmentVariable("ANTHROPIC_API_KEY")
    ?? throw new InvalidOperationException("ANTHROPIC_API_KEY is not set.");
builder.Services.AddSingleton(new ClaudeService(apiKey));

var app = builder.Build();

app.MapPost("/api/analyze", async (AnalyzeRequest req, ClaudeService claude) =>
{
    try { return Results.Ok(new { answer = await claude.AnalyzeDataAsync(req.Question) }); }
    catch (Exception ex) { return Results.Problem(ex.Message); }
});

app.Run();

record AnalyzeRequest(string Question);