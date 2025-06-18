using Sample.Monitoring.Model.Health;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sample.Monitoring.Infrastructure.Health.Configuration
{
  /// <summary>
  /// Konfiguriert die Datenbankzuordnung für die <see cref="HealthCheck"/>-Entität.
  /// </summary>
  internal class HealthCheckMap : IEntityTypeConfiguration<HealthCheck>
  {
    #region " öffentliche Methoden                        "

    #region " --> Configure                               "
    /// <summary>
    /// Konfiguriert die Eigenschaften und Zuordnungen der <see cref="HealthCheck"/>-Entität für Entity Framework Core.
    /// </summary>
    /// <param name="builder">Der <see cref="EntityTypeBuilder{HealthCheck}"/> zum Konfigurieren der Entität.</param>
    public void Configure(EntityTypeBuilder<HealthCheck> builder)
    {
      _ = builder.HasKey(e => e.Id);
      _ = builder.Property(e => e.Id).HasColumnName("fldId").ValueGeneratedNever().IsRequired();
      _ = builder.Property(e => e.Name).HasColumnName("fldName").IsRequired();
      _ = builder.Property(e => e.Description).HasColumnName("fldDescription");
      _ = builder.Property(e => e.Tags).HasColumnName("fldTags");
    }
    #endregion

    #endregion
  }
}