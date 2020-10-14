namespace MachineMaintenanceApp.Data.Configurations
{
    using MachineMaintenanceApp.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class WeeklyCheckConfiguration : IEntityTypeConfiguration<WeeklyCheck>
    {
        public void Configure(EntityTypeBuilder<WeeklyCheck> builder)
        {
            builder.Property(x => x.Notes)
                .HasColumnType("Text");
        }
    }
}
