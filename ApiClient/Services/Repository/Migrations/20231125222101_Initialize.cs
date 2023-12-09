using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable


namespace ApiClient.Services.Repository.Migrations
{
    /// <summary>
    /// https://stackoverflow.com/questions/59234655/apply-ef-migrations-in-github-workflow
    /// </summary>
    public partial class Initialize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Securities",
                columns: table => new
                {
                    isin = table.Column<string>(type: "varchar(12)", maxLength: 12, nullable: false),
                    price = table.Column<decimal>(type: "decimal(15,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Securities", x => x.isin);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Securities");
        }
    }
}
