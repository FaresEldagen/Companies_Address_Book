using System.Text.RegularExpressions;
using Companies_Address_Book.Models.Entities;
using Companies_Address_Book.Models.ViewModels;
using Companies_Address_Book.Repositories.Interfaces;
using Companies_Address_Book.Services.Interfaces;

namespace Companies_Address_Book.Services.Implementation
{
    public class ContactsService : IContactsService
    {
        private readonly IContactsRepositories _contactsRepository;
        private readonly ICompaniesRepository _companiesRepository;
        private readonly ILogger<ContactsService> _logger;

        public ContactsService(
            IContactsRepositories contactsRepository,
            ICompaniesRepository companiesRepository,
            ILogger<ContactsService> logger)
        {
            _contactsRepository = contactsRepository;
            _companiesRepository = companiesRepository;
            _logger = logger;
        }


        private static readonly Regex PhoneRegex = new(
            @"^(\+?[0-9]{1,4}[\s-]?)?([0-9]{6,14})$",
            RegexOptions.Compiled);

        public async Task<Contact> CreateContactAsync(CreateContactViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
                //return (false, "Contact data must not be empty.", null);
            }

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                throw new ArgumentNullException("model");

                //return (false, "Contact name is required.", null);
            }

            // Verify company exists
            var company = await _companiesRepository.GetCompanyByIdAsync(model.CompanyId);
            if (company == null)
            {
                throw new ArgumentNullException("model");
                //return (false, "Selected company was not found.", null);
            }

            // Rule 1: Validate contact age <= maxcontacts age
            // Reject future birthdays
            var today = DateOnly.FromDateTime(DateTime.Today);
            if (model.DateOfBirth > today)
            {
                throw new ArgumentOutOfRangeException(nameof(model.DateOfBirth), "Date of birth cannot be in the future.");
            }

            // Calculate age in years from birthdate to today
            int age = today.Year - model.DateOfBirth.Year;
            if (model.DateOfBirth > today.AddYears(-age))
            {
                age--;
            }

            // Allow equality: age <= maxContactsAge is valid
            if (company.MaxContactsAge.HasValue && age > company.MaxContactsAge.Value)
            {
                throw new InvalidOperationException($"Contact age ({age} years) exceeds the company '{company.Name}' maximum allowed contact age ({company.MaxContactsAge.Value} years).");
            }

            // Rule 2: Contact count <= numcontacts for the selected company
            int currentContactsCount = await _contactsRepository.GetContactsCountByCompanyIdAsync(company.Id);
            if (currentContactsCount >= company.NumContacts)
            {
                throw new InvalidOperationException($"Company '{company.Name}' has reached its maximum allowed contact limit ({company.NumContacts}). Current count is {currentContactsCount}.");
            }

            // Rule 3: Phone number in correct format
            if (string.IsNullOrWhiteSpace(model.PhoneNumber) || !PhoneRegex.IsMatch(model.PhoneNumber.Trim()))
            {
                throw new ArgumentException("Please provide a valid phone number format (e.g., +1234567890, +201012345678, or 01012345678).", nameof(model.PhoneNumber));
            }

            var contact = new Contact
            {
                Name = model.Name.Trim(),
                DateOfBirth = model.DateOfBirth,
                PhoneNumber = model.PhoneNumber.Trim(),
                CompanyId = model.CompanyId
            };

            var created = await _contactsRepository.AddContactAsync(contact);
            _logger.LogInformation("Contact '{ContactName}' created successfully with ID {ContactId} for Company '{CompanyName}' ({CompanyId}).",
                created.Name, created.Id, company.Name, company.Id);

            return contact;
        }
    }
}
