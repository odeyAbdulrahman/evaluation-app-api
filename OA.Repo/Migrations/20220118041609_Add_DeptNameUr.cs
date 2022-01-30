using Microsoft.EntityFrameworkCore.Migrations;

namespace OA.Repo.Migrations
{
    public partial class Add_DeptNameUr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameUr",
                table: "Departments",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameUr",
                table: "Departments");
        }
    }
}
