namespace MachineMaintenanceApp.Data.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using MachineMaintenanceApp.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class PlannedRepairConfiguration : IEntityTypeConfiguration<PlannedRepair>
    {
        public void Configure(EntityTypeBuilder<PlannedRepair> builder)
        {
            builder.Property(x => x.Description)
                .HasColumnType("Text");
        }
    }
}
