using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dokumanYonetim.Migrations
{
    public partial class LastMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // İlk olarak, kısıtlamanın var olup olmadığını kontrol etmeden kaldırmaya çalışıyoruz
            migrationBuilder.Sql("IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Documents_Users_UserId') " +
                                 "ALTER TABLE Documents DROP CONSTRAINT FK_Documents_Users_UserId");

            // UserId sütununu nullable olarak değiştiriyoruz
            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Documents",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            // Yeni foreign key'i ekliyoruz
            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Users_UserId",
                table: "Documents",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict); // onDelete davranışını "Restrict" olarak ayarladık
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Yeni foreign key'i kaldırıyoruz
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Users_UserId",
                table: "Documents");

            // UserId sütununu tekrar nullable olmayan hale getiriyoruz
            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Documents",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            // Eski foreign key'i yeniden ekliyoruz
            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Users_UserId",
                table: "Documents",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
