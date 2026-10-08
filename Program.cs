//using BuitwithApitestproject.Services;
//DotNetEnv.Env.Load();
//var builder = WebApplication.CreateBuilder(args);

//var apiKey = Environment.GetEnvironmentVariable("ANTHROPIC_API_KEY")
//    ?? throw new InvalidOperationException("ANTHROPIC_API_KEY is not set.");
//builder.Services.AddSingleton(new ClaudeService(apiKey));

//var app = builder.Build();
//app.UseDefaultFiles();
//app.UseStaticFiles();
//app.MapPost("/api/analyze", async (AnalyzeRequest req, ClaudeService claude) =>
//{
//    try { return Results.Ok(new { answer = await claude.AnalyzeDataAsync(req.Question) }); }
//    catch (Exception ex) { return Results.Problem(ex.Message); }
//});

//app.Run();

//record AnalyzeRequest(string Question);
using BuitwithApitestproject.Services;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

var apiKey = Environment.GetEnvironmentVariable("ANTHROPIC_API_KEY")
    ?? throw new InvalidOperationException("ANTHROPIC_API_KEY is not set.");
builder.Services.AddSingleton(new ClaudeService(apiKey));
builder.Services.AddSingleton<DatasetService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapGet("/api/dataset", (DatasetService ds) =>
    Results.Ok(new { count = ds.Rows.Count, columns = ds.Columns, sample = ds.Rows.Take(5) }));

app.MapPost("/api/analyze", async (AnalyzeRequest req, DatasetService ds, ClaudeService claude) =>
{
    var prompt = $"Dataset:\n{ds.ToPromptContext()}\n\nQuestion: {req.Question}\n\n" +
                 "Answer only from the dataset. If it cannot answer, say so.";
    try { return Results.Ok(new { answer = await claude.AnalyzeDataAsync(prompt) }); }
    catch (Exception ex) { return Results.Problem(ex.Message); }
});

app.Run();

record AnalyzeRequest(string Question);