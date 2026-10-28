using Companies_Address_Book.Models.Entities;

namespace Companies_Address_Book.Repositories.Interfaces
{
    public interface IContactsRepositories
    {
        Task<Contact> AddContactAsync(Contact contact);
        Task<int> GetContactsCountByCompanyIdAsync(int companyId);
    }
}
