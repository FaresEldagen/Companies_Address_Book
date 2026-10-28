namespace Companies_Address_Book.Models.Entities
{
    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public int CompanyId { get; set; }
    }
}
