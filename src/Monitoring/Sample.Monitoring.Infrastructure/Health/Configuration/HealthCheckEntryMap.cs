using Sample.Monitoring.Model.Health;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Sample.Monitoring.Infrastructure.Health.Configuration
{
  /// <summary>
  /// Stellt die Entity Framework Core-Konfiguration für <see cref="HealthCheckEntry"/> bereit.
  /// </summary>
  internal class HealthCheckEntryMap : IEntityTypeConfiguration<HealthCheckEntry>
  {
    #region " öffentliche Methoden                        "

    #region " --> Configure                               "
    /// <summary>
    /// Konfiguriert die Datenbankzuordnung für die <see cref="HealthCheckEntry"/>-Entität.
    /// </summary>
    /// <param name="builder">Der <see cref="EntityTypeBuilder{HealthCheckEntry}"/> zum Konfigurieren der Entität.</param>
    public void Configure(EntityTypeBuilder<HealthCheckEntry> builder)
    {
      _ = builder.HasKey(e => e.Id);
      _ = builder.Property(e => e.Id).HasColumnName("fldId").ValueGeneratedNever().IsRequired();
      _ = builder.Property(e => e.ApplicationId).HasColumnName("fldApplicationId").IsRequired();
      _ = builder.Property(e => e.HealthCheckId).HasColumnName("fldHealthCheckId");
      _ = builder.Property(e => e.ServerName).HasColumnName("fldServerName").IsRequired();
      _ = builder.Property(e => e.Name).HasColumnName("fldName").IsRequired();
      _ = builder.Property(e => e.Description).HasColumnName("fldDescription");
      _ = builder.Property(e => e.Status).HasColumnName("fldStatus").HasConversion<HealthStatusConverter>();
      _ = builder.Property(e => e.LastExecution).HasColumnName("fldLastExecution");
      _ = builder.Property(e => e.LastDuration).HasColumnName("fldLastDuration");

      _ = builder.HasOne(e => e.HealthCheck)
        .WithMany()
        .HasForeignKey(e => e.HealthCheckId)
        .OnDelete(DeleteBehavior.ClientSetNull);
    }
    #endregion

    #endregion

  }
}