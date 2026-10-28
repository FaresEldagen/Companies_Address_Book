using Companies_Address_Book.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Companies_Address_Book.Data.Configurations
{
    public class ContactConfigurations : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.DateOfBirth)
                .IsRequired();

            builder.Property(c => c.PhoneNumber)
                .IsRequired()
                .HasMaxLength(15);

            builder.Property(c => c.CompanyId)
                .IsRequired();
        }
    }
}
