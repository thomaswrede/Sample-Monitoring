using Sample.Monitoring.Model.Health;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Sample.Monitoring.Infrastructure.Health.Configuration
{
  /// <summary>
  /// Stellt die Entity Framework Core-Konfiguration für <see cref="HealthCheckHistoryEntry"/> bereit.
  /// </summary>
  internal class HealthCheckHistoryEntryMap : IEntityTypeConfiguration<HealthCheckHistoryEntry>
  {
    #region " öffentliche Methoden                        "

    #region " --> Configure                               "
    /// <summary>
    /// Konfiguriert die Datenbankzuordnung für die <see cref="HealthCheckHistoryEntry"/>-Entität.
    /// </summary>
    /// <param name="builder">
    /// Der <see cref="EntityTypeBuilder{HealthCheckHistoryEntry}"/> zum Konfigurieren der Entität.
    /// </param>
    public void Configure(EntityTypeBuilder<HealthCheckHistoryEntry> builder)
    {
      _ = builder.HasKey(e => e.Id);
      _ = builder.Property(e => e.Id).HasColumnName("fldId").ValueGeneratedNever();
      _ = builder.Property(e => e.EntryId).HasColumnName("fldEntryId");
      _ = builder.Property(e => e.StatusFrom).HasColumnName("fldStatusFrom");
      _ = builder.Property(e => e.Status).HasColumnName("fldStatus").HasConversion<HealthStatusConverter>();
      _ = builder.Property(e => e.LastExecution).HasColumnName("fldLastExecution");
      _ = builder.Property(e => e.LastDuration).HasColumnName("fldLastDuration");
      _ = builder.HasOne(d => d.HealthCheckEntry)
        .WithMany(p => p.HistoryEntries)
        .HasForeignKey(d => d.EntryId)
        .OnDelete(DeleteBehavior.ClientSetNull)
        .HasConstraintName("FK_HealthCheckHistoryEntry_HealthCheckEntry");
    }
    #endregion

    #endregion

  }
}