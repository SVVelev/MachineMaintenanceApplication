namespace MachineMaintenanceApp.Data.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using MachineMaintenanceApp.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class SparePartConfiguration : IEntityTypeConfiguration<SparePart>
    {
        public void Configure(EntityTypeBuilder<SparePart> builder)
        {

            builder.Property(x => x.Description)
                   .HasColumnType("Text");

            builder
                .HasIndex(x => x.InventoryNumber)
                .IsUnique();
        }
    }
}
