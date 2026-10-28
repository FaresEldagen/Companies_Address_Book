using Companies_Address_Book.Data;
using Companies_Address_Book.Models.Entities;
using Companies_Address_Book.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Companies_Address_Book.Repositories.Implementation
{
    public class CompaniesRepository : ICompaniesRepository
    {
        private readonly AppDbContext _context;

        public CompaniesRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<List<Company>> GetAllCompaniesAsync()
        {
            return await _context.Companies.AsNoTracking().ToListAsync();
        }

        public async Task<List<string>> GetAllCompaniesNamesAsync()
        {
            return await _context.Companies.Select(c => c.Name).AsNoTracking().ToListAsync();
        }

        public async Task AddRangeAsync(IEnumerable<Company> companies)
        {
            await _context.Companies.AddRangeAsync(companies);
            await _context.SaveChangesAsync();
        }
    }
}
