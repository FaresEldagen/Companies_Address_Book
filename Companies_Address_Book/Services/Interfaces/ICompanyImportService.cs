using Companies_Address_Book.Models.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Companies_Address_Book.Services.Interfaces
{
    public interface ICompanyImportService
    {
        Task<List<ImportedCompanyViewModel>> ImportCompaniesFromFileAsync(IFormFile file);
    }
}
