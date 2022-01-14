using Microsoft.EntityFrameworkCore.Migrations;

namespace SampleEndpointApp.DataAccess.Migrations;

public partial class Initial : Migration
{
  protected override void Up(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.CreateTable(
        name: "Authors",
        columns: table => new
        {
          Id = table.Column<int>(nullable: false)
                .Annotation("Sqlite:Autoincrement", true),
          Name = table.Column<string>(nullable: false),
          PluralsightUrl = table.Column<string>(nullable: false),
          TwitterAlias = table.Column<string>(nullable: true)
        },
        constraints: table =>
        {
          table.PrimaryKey("PK_Authors", x => x.Id);
        });

    migrationBuilder.InsertData(
        table: "Authors",
        columns: new[] { "Id", "Name", "PluralsightUrl", "TwitterAlias" },
        values: new object[] { 1, "Steve Smith", "", "ardalis" });

    migrationBuilder.InsertData(
        table: "Authors",
        columns: new[] { "Id", "Name", "PluralsightUrl", "TwitterAlias" },
        values: new object[] { 2, "Julie Lerman", "", "julialerman" });
  }

  protected override void Down(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.DropTable(
        name: "Authors");
  }
}
