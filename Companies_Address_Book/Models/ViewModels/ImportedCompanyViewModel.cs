namespace Companies_Address_Book.Models.ViewModels
{
    public class ImportedCompanyViewModel
    {
        public string Name { get; set; } = string.Empty;
        public int NumContacts { get; set; }
        public int? MaxContactAge { get; set; }
    }
}
