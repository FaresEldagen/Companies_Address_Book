using Companies_Address_Book.Models.Entities;
using Companies_Address_Book.Models.ViewModels;

namespace Companies_Address_Book.Services.Interfaces
{
    public interface ICompaniesService
    {
        Task<List<Company>> GetAllCompaniesAsync();
        Task<int> ImportCompaniesAsync(List<ImportedCompanyViewModel> companies);
    }
}
