using System.Text;
using System.Text.RegularExpressions;
using Companies_Address_Book.Models.ViewModels;
using Companies_Address_Book.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Companies_Address_Book.Services.Implementation
{
    public class CompanyImportService : ICompanyImportService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<CompanyImportService> _logger;

        public CompanyImportService(IWebHostEnvironment environment, ILogger<CompanyImportService> logger)
        {
            _environment = environment;
            _logger = logger;
        }

        public async Task<List<ImportedCompanyViewModel>> ImportCompaniesFromFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                _logger.LogError("Uploaded file is null or empty.");
                throw new ArgumentException("Please provide a valid, non-empty file to import.", nameof(file));
            }

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (ext != ".txt")
            {
                throw new InvalidOperationException(($"Invalid file type: {ext}. Only .txt files are supported for import."));
            }

            _logger.LogInformation("Reading and parsing uploaded file: {FileName} ({Length} bytes)", file.FileName, file.Length);

            var lines = new List<string>();
            using (var stream = file.OpenReadStream())
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                while (await reader.ReadLineAsync() is { } line)
                {
                    lines.Add(line);
                }
            }

            return ParseLines(lines);
        }

        private List<ImportedCompanyViewModel> ParseLines(IEnumerable<string> lines)
        {
            var importedList = new List<ImportedCompanyViewModel>();

            foreach (var rawLine in lines)
            {
                if (string.IsNullOrWhiteSpace(rawLine))
                    continue;

                var trimmed = rawLine.Trim();

                // Skip header or comment rows
                if (trimmed.StartsWith("#"))
                    continue;

                string[] parts;
                if (trimmed.Contains('\t'))
                {
                    parts = trimmed.Split('\t', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                }
                else
                {
                    // Fallback if tabs are replaced by multiple spaces
                    parts = Regex.Split(trimmed, @"\s{2,}")
                        .Select(p => p.Trim())
                        .Where(p => !string.IsNullOrEmpty(p))
                        .ToArray();
                }

                if (parts.Length == 0)
                    continue;

                var companyName = parts[0];

                var numContacts = 0;
                if (parts.Length > 1 && int.TryParse(parts[1], out var parsedNum))
                {
                    numContacts = parsedNum;
                }

                int? maxContactAge = null;
                if (parts.Length > 2 && int.TryParse(parts[2], out var parsedAge))
                {
                    maxContactAge = parsedAge;
                }

                importedList.Add(new ImportedCompanyViewModel
                {
                    Name = companyName,
                    NumContacts = numContacts,
                    MaxContactAge = maxContactAge
                });
            }

            _logger.LogInformation("Successfully parsed {Count} companies from file.", importedList.Count);
            return importedList;
        }
    }
}
