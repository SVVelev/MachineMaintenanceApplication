namespace MachineMaintenanceApp.Data.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using MachineMaintenanceApp.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class UnplannedRepairConfiguration : IEntityTypeConfiguration<UnplannedRepair>
    {
        public void Configure(EntityTypeBuilder<UnplannedRepair> builder)
        {
            builder.Property(x => x.Description)
                .HasColumnType("Text");
        }
    }
}
