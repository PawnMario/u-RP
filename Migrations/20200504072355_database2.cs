using Microsoft.EntityFrameworkCore.Migrations;

namespace uRP.Migrations
{
    public partial class database2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "gid",
                table: "Characters",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "Characters",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "surname",
                table: "Characters",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "gid",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "name",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "surname",
                table: "Characters");
        }
    }
}
