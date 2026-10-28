using Companies_Address_Book.Models.Entities;
using Companies_Address_Book.Models.ViewModels;

namespace Companies_Address_Book.Services.Interfaces
{
    public interface IContactsService
    {
        Task<Contact> CreateContactAsync(CreateContactViewModel model);
    }
}
