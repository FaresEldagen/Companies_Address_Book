using Companies_Address_Book.Models.Entities;

namespace Companies_Address_Book.Repositories.Interfaces
{
    public interface ICompaniesRepository
    {
        Task<List<Company>> GetAllCompaniesAsync();
        Task<List<string>> GetAllCompaniesNamesAsync();
        Task AddRangeAsync(IEnumerable<Company> companies);
    }
}
