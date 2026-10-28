using Companies_Address_Book.Data;
using Companies_Address_Book.Models.Entities;
using Companies_Address_Book.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Companies_Address_Book.Repositories.Implementation
{
    public class ContactsRepository : IContactsRepositories
    {
        private readonly AppDbContext _context;

        public ContactsRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<Contact> AddContactAsync(Contact contact)
        {
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();
            return contact;
        }

        public async Task<int> GetContactsCountByCompanyIdAsync(int companyId)
        {
            return await _context.Contacts.CountAsync(c => c.CompanyId == companyId);
        }
    }
}
