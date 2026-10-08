using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;

namespace BuitwithApitestproject.Services;

public class DatasetService
{
    public List<Dictionary<string, string>> Rows { get; } = new();
    public List<string> Columns { get; } = new();

    public DatasetService(IWebHostEnvironment env)
    {
        var path = Path.Combine(env.ContentRootPath, "data", "dataset.csv");
        if (!File.Exists(path)) return;

        var config = new CsvConfiguration(CultureInfo.InvariantCulture) { MissingFieldFound = null };
        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, config);

        csv.Read();
        csv.ReadHeader();
        Columns.AddRange(csv.HeaderRecord ?? Array.Empty<string>());

        while (csv.Read())
        {
            var row = new Dictionary<string, string>();
            foreach (var c in Columns) row[c] = csv.GetField(c) ?? "";
            Rows.Add(row);
        }
    }

    public string ToPromptContext(int maxRows = 200)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Total rows: {Rows.Count} (showing up to {maxRows})");
        sb.AppendLine(string.Join(" | ", Columns));
        foreach (var r in Rows.Take(maxRows))
            sb.AppendLine(string.Join(" | ", Columns.Select(c => r[c])));
        return sb.ToString();
    }
}