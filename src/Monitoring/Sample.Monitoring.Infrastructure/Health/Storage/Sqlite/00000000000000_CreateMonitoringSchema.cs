using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sample.Monitoring.Infrastructure.Health.Storage.Sqlite
{
  /// <inheritdoc />
  public partial class CreateMonitoringSchema : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
          name: "HealthChecks",
          columns: table => new
          {
            fldId = table.Column<Guid>(type: "TEXT", nullable: false),
            fldName = table.Column<String>(type: "TEXT", nullable: false),
            fldDescription = table.Column<String>(type: "TEXT", nullable: true),
            fldTags = table.Column<String>(type: "TEXT", nullable: true)
          },
          constraints: table => table.PrimaryKey("PK_HealthChecks", x => x.fldId));

      migrationBuilder.CreateTable(
          name: "HealthCheckEntries",
          columns: table => new
          {
            fldId = table.Column<Guid>(type: "TEXT", nullable: false),
            fldApplicationId = table.Column<Guid>(type: "TEXT", nullable: false),
            fldServerName = table.Column<String>(type: "TEXT", nullable: false),
            fldName = table.Column<String>(type: "TEXT", nullable: false),
            fldDescription = table.Column<String>(type: "TEXT", nullable: true),
            fldStatus = table.Column<Int32>(type: "INTEGER", nullable: false),
            fldLastExecution = table.Column<DateTime>(type: "TEXT", nullable: false),
            fldLastDuration = table.Column<TimeSpan>(type: "TEXT", nullable: false),
            fldHealthCheckId = table.Column<Guid>(type: "TEXT", nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_HealthCheckEntries", x => x.fldId);
            table.ForeignKey(
                      name: "FK_HealthCheckEntries_HealthChecks_fldHealthCheckId",
                      column: x => x.fldHealthCheckId,
                      principalTable: "HealthChecks",
                      principalColumn: "fldId");
          });

      migrationBuilder.CreateTable(
          name: "HealthCheckHistoryEntries",
          columns: table => new
          {
            fldId = table.Column<Guid>(type: "TEXT", nullable: false),
            fldEntryId = table.Column<Guid>(type: "TEXT", nullable: false),
            fldStatus = table.Column<Int32>(type: "INTEGER", nullable: false),
            fldStatusFrom = table.Column<DateTime>(type: "TEXT", nullable: false),
            fldLastExecution = table.Column<DateTime>(type: "TEXT", nullable: false),
            fldLastDuration = table.Column<TimeSpan>(type: "TEXT", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_HealthCheckHistoryEntries", x => x.fldId);
            table.ForeignKey(
                      name: "FK_HealthCheckHistoryEntry_HealthCheckEntry",
                      column: x => x.fldEntryId,
                      principalTable: "HealthCheckEntries",
                      principalColumn: "fldId");
          });

      migrationBuilder.CreateIndex(
          name: "IX_HealthCheckEntries_fldHealthCheckId",
          table: "HealthCheckEntries",
          column: "fldHealthCheckId");

      migrationBuilder.CreateIndex(
          name: "IX_HealthCheckHistoryEntries_fldEntryId",
          table: "HealthCheckHistoryEntries",
          column: "fldEntryId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "HealthCheckHistoryEntries");

      migrationBuilder.DropTable(
          name: "HealthCheckEntries");

      migrationBuilder.DropTable(
          name: "HealthChecks");
    }
  }
}
