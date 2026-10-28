using Companies_Address_Book.Models.ViewModels;
using Companies_Address_Book.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Companies_Address_Book.Controllers
{
    public class ContactsController : Controller
    {
        private readonly IContactsService _contactsService;
        private readonly ICompaniesService _companiesService;
        private readonly ILogger<ContactsController> _logger;

        public ContactsController(
            IContactsService contactsService,
            ICompaniesService companiesService,
            ILogger<ContactsController> logger)
        {
            _contactsService = contactsService;
            _companiesService = companiesService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction(nameof(Create));
        }

        [HttpGet]
        public async Task<IActionResult> Create(int? companyId)
        {
            var model = new CreateContactViewModel();
            if (companyId.HasValue)
            {
                model.CompanyId = companyId.Value;
            }

            await PopulateCompaniesAsync(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateContactViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateCompaniesAsync(model);
                return View(model);
            }

            try
            {
                var contact = await _contactsService.CreateContactAsync(model);
                TempData["SuccessMessage"] = $"Contact '{contact.Name}' was successfully created!";
                return RedirectToAction(nameof(Create));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await PopulateCompaniesAsync(model);
                return View(model);
            }
        }

        private async Task PopulateCompaniesAsync(CreateContactViewModel model)
        {
            var companies = await _companiesService.GetAllCompaniesAsync();
            model.CompaniesList = companies.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name,
                Selected = c.Id == model.CompanyId
            }).ToList();
        }
    }
}
