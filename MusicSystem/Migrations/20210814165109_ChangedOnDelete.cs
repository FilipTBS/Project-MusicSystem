using Microsoft.EntityFrameworkCore.Migrations;

namespace MusicSystem.Migrations
{
    public partial class ChangedOnDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Curators_AspNetUsers_UserId",
                table: "Curators");

            migrationBuilder.DropForeignKey(
                name: "FK_Songs_Curators_CuratorId",
                table: "Songs");

            migrationBuilder.AddForeignKey(
                name: "FK_Curators_AspNetUsers_UserId",
                table: "Curators",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_Curators_CuratorId",
                table: "Songs",
                column: "CuratorId",
                principalTable: "Curators",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Curators_AspNetUsers_UserId",
                table: "Curators");

            migrationBuilder.DropForeignKey(
                name: "FK_Songs_Curators_CuratorId",
                table: "Songs");

            migrationBuilder.AddForeignKey(
                name: "FK_Curators_AspNetUsers_UserId",
                table: "Curators",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_Curators_CuratorId",
                table: "Songs",
                column: "CuratorId",
                principalTable: "Curators",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
