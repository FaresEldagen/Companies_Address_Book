using Companies_Address_Book.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Companies_Address_Book.Data.Configurations
{
    public class CompanyConfigurations : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.NumContacts)
                .IsRequired();

            builder.Property(c => c.MaxContactsAge)
                .IsRequired(false);
        }
    }
}
