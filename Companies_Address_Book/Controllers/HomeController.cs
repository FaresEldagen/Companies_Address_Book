using System.Diagnostics;
using Companies_Address_Book.Data;
using Companies_Address_Book.Models.Entities;
using Companies_Address_Book.Models.ViewModels;
using Companies_Address_Book.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Companies_Address_Book.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICompanyImportService _companyImportService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            AppDbContext context,
            ICompanyImportService companyImportService,
            ILogger<HomeController> logger)
        {
            _companyImportService = companyImportService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var companies = new List<Company>();

            return View(companies);
        }

        [HttpPost]
        public async Task<IActionResult> Import(IFormFile? file)
        {
            if (file == null || file.Length == 0)
            {
                _logger.LogWarning("Import action invoked without an uploaded file.");
                return BadRequest("Please select a file on your device to import.");
            }

            _logger.LogInformation("Import action triggered with uploaded file: {FileName}", file.FileName);
            var importedCompanies = await _companyImportService.ImportCompaniesFromFileAsync(file);
            return Json(importedCompanies);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

