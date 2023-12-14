using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Medical_Appointments_API.Migrations
{
    public partial class nullableNotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Appointments",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "18d53b99-6a51-4404-9556-d9b3f2aa75e7");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "d52a7ee4-f984-434f-b8f9-f5c9be3134d2");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "312fc76c-b862-4ee5-b20f-dcad38d7e396");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "6b704758-a41b-44b3-a18e-4c1b482dd8e2", "AQAAAAEAACcQAAAAEEff9zb3t3sG4WxhncoAh32MK0K6j9e+JF1mYezDwY1K52CXPB6bQlSkLWG57ySkfw==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Appointments",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "26167322-2f45-4e9b-be8f-baad7bacb0d5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "71d91539-f14b-4db1-8c3b-feae0d80d1af");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "5e9aa65c-39a3-4f07-bde9-ea9e44e27ce1");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "ef18d970-6275-4fb0-b31a-415a1d77b59b", "AQAAAAEAACcQAAAAEEkgdLc7p7UePUrkcbOO66P+Qso23Km9k+Xhkb63ImB6/s10hEFXYpvcJ05rhh3cwQ==" });
        }
    }
}
