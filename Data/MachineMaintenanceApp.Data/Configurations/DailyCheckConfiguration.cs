
namespace MachineMaintenanceApp.Data.Configurations
{
    using MachineMaintenanceApp.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class DailyCheckConfiguration : IEntityTypeConfiguration<DailyCheck>
    {
        public void Configure(EntityTypeBuilder<DailyCheck> builder)
        {
            builder.Property(x => x.Notes)
                .HasColumnType("Text");
        }
    }
}
