namespace MachineMaintenanceApp.Data.Configurations
{

    using MachineMaintenanceApp.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder
                .HasIndex(x => x.Name)
                .IsUnique();

            builder.Property(x => x.Description)
                .HasColumnType("Text");
        }
    }
}
