using Microsoft.EntityFrameworkCore.Migrations;

namespace InstaAPI.Migrations
{
    public partial class modifiedWhoLiked : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WhoLiked_Posts_PostID",
                table: "WhoLiked");

            migrationBuilder.RenameColumn(
                name: "LikedID",
                table: "WhoLiked",
                newName: "UserID");

            migrationBuilder.AlterColumn<int>(
                name: "PostID",
                table: "WhoLiked",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WhoLiked_Posts_PostID",
                table: "WhoLiked",
                column: "PostID",
                principalTable: "Posts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WhoLiked_Posts_PostID",
                table: "WhoLiked");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "WhoLiked",
                newName: "LikedID");

            migrationBuilder.AlterColumn<int>(
                name: "PostID",
                table: "WhoLiked",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_WhoLiked_Posts_PostID",
                table: "WhoLiked",
                column: "PostID",
                principalTable: "Posts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
