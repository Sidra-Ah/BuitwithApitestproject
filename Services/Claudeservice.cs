using System.Text;
using System.Text.Json;

namespace BuitwithApitestproject.Services;

public class ClaudeService
{
    private static readonly HttpClient _http = new()
    {
        BaseAddress = new Uri("https://api.anthropic.com/"),
        Timeout = TimeSpan.FromSeconds(60)
    };

    private const string Model = "claude-haiku-4-5-20251001";
    private readonly string _apiKey;

    public ClaudeService(string apiKey)
    {
        _apiKey = apiKey;
    }

    public async Task<string> AnalyzeDataAsync(string prompt)
    {
        var body = JsonSerializer.Serialize(new
        {
            model = Model,
            max_tokens = 1024,
            messages = new[] { new { role = "user", content = prompt } }
        });

        using var req = new HttpRequestMessage(HttpMethod.Post, "v1/messages")
        {
            Content = new StringContent(body, Encoding.UTF8, "application/json")
        };
        req.Headers.Add("x-api-key", _apiKey);
        req.Headers.Add("anthropic-version", "2023-06-01");

        using var res = await _http.SendAsync(req);
        var json = await res.Content.ReadAsStringAsync();

        if (!res.IsSuccessStatusCode)
            throw new HttpRequestException($"Claude API {(int)res.StatusCode}: {json}");

        using var doc = JsonDocument.Parse(json);
        var sb = new StringBuilder();
        foreach (var block in doc.RootElement.GetProperty("content").EnumerateArray())
            if (block.GetProperty("type").GetString() == "text")
                sb.Append(block.GetProperty("text").GetString());

        return sb.ToString();
    }
}