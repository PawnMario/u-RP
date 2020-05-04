using Microsoft.EntityFrameworkCore.Migrations;

namespace uRP.Migrations
{
    public partial class database1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "member_pass_hash",
                table: "Players",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "Players",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "member_pass_hash",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "name",
                table: "Players");
        }
    }
}
