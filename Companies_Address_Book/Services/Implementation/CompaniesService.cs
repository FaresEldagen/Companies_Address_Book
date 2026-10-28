using Companies_Address_Book.Models.Entities;
using Companies_Address_Book.Models.ViewModels;
using Companies_Address_Book.Repositories.Interfaces;
using Companies_Address_Book.Services.Interfaces;

namespace Companies_Address_Book.Services.Implementation
{
    public class CompaniesService : ICompaniesService
    {
        private readonly ICompaniesRepository _companiesRepository;
        private readonly ILogger<CompaniesService> _logger;

        public CompaniesService(
            ICompaniesRepository companiesRepository,
            ILogger<CompaniesService> logger)
        {
            _companiesRepository = companiesRepository;
            _logger = logger;
        }


        public async Task<List<Company>> GetAllCompaniesAsync()
        {
            return await _companiesRepository.GetAllCompaniesAsync();
        }

        public async Task<int> ImportCompaniesAsync(List<ImportedCompanyViewModel> importedCompanies)
        {
            if (importedCompanies == null || importedCompanies.Count == 0)
            {
                return 0;
            }

            var existingNames = new HashSet<string>(
                await _companiesRepository.GetAllCompaniesNamesAsync(),
                StringComparer.OrdinalIgnoreCase);

            var companiesToInsert = new List<Company>();

            foreach (var importedCompany in importedCompanies)
            {
                bool isValid = ValidateCompany(importedCompany);

                if (isValid && !existingNames.Contains(importedCompany.Name))
                {
                    companiesToInsert.Add(new Company
                    {
                        Name = importedCompany.Name,
                        NumContacts = importedCompany.NumContacts,
                        MaxContactsAge = importedCompany.MaxContactAge
                    });

                    existingNames.Add(importedCompany.Name);
                }
            }

            if (companiesToInsert.Count > 0)
            {
                await _companiesRepository.AddRangeAsync(companiesToInsert);
                _logger.LogInformation("Successfully inserted {Count} companies in a single batch.", companiesToInsert.Count);
            }

            return companiesToInsert.Count;
        }


        private static readonly char[] SpecialCharacters = { '§', '®', '™', '©', 'ʬ' };

        public bool ValidateCompany(ImportedCompanyViewModel company)
        {
            if (string.IsNullOrWhiteSpace(company.Name))
            {
                return false;
            }

            bool nameIsValid = false;
            for (int i = 1; i < company.Name.Length; i++)
            {
                if (SpecialCharacters.Contains(company.Name[i]) &&
                    char.IsDigit(company.Name[i - 1]))
                {
                    nameIsValid = true;
                    break;
                }
            }
            if (!nameIsValid)
            {
                return false;
            }

            if (company.NumContacts < 0)
            {
                return false;
            }
            if (company.MaxContactAge.HasValue && company.MaxContactAge.Value < 0)
            {
                return false;
            }
            return true;
        }
    }
}
