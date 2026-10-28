namespace Companies_Address_Book.Models.Entities
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int NumContacts { get; set; }
        public int? MaxContactsAge { get; set; }
    }
}
