using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Kcd.Identity.Migrations
{
    /// <inheritdoc />
    public partial class changedvalidationforuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "0cf7e196-eae2-4959-adaf-ac7faac388aa");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "4ca1a567-1830-453d-a3dc-a60a5f2abe22");

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "0cf7e196-eae2-4959-adaf-ac7faac388aa", "c627d672-a80a-486f-98f4-4d6b5ec0e535" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0cf7e196-eae2-4959-adaf-ac7faac388aa");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4ca1a567-1830-453d-a3dc-a60a5f2abe22");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "c627d672-a80a-486f-98f4-4d6b5ec0e535");

            migrationBuilder.AlterColumn<string>(
                name: "Referral",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Company",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "AvatarId",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "baef5bf9-5189-42d2-8d20-cc79ad317556", null, "Admin", "ADMIN" },
                    { "fa828355-eff9-41f2-8c12-2d3a1fd1fd20", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "AvatarId", "Company", "ConcurrencyStamp", "Country", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Referral", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "d0a48569-9bff-499d-8811-2fb0301cfeeb", 0, "", "KCD", "ac2db4cb-0881-4947-9eaf-22636bacb68d", "N/A", "admin@admin.com", true, false, null, "Admin User", "ADMIN@ADMIN.COM", "ADMIN", "AQAAAAIAAYagAAAAEC0tUVwNqfl/pGRRoVcRCQrcF/61gjx22Omr2W8Gv+Er9NY4QlmWI+8AFtdKG9lB6Q==", null, false, "Internal", "fd4342f4-f1c5-4c5b-8d75-327109e42acf", false, "admin" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Description" },
                values: new object[,]
                {
                    { "baef5bf9-5189-42d2-8d20-cc79ad317556", "Admin" },
                    { "fa828355-eff9-41f2-8c12-2d3a1fd1fd20", "User" }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "baef5bf9-5189-42d2-8d20-cc79ad317556", "d0a48569-9bff-499d-8811-2fb0301cfeeb" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "baef5bf9-5189-42d2-8d20-cc79ad317556");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "fa828355-eff9-41f2-8c12-2d3a1fd1fd20");

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "baef5bf9-5189-42d2-8d20-cc79ad317556", "d0a48569-9bff-499d-8811-2fb0301cfeeb" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "baef5bf9-5189-42d2-8d20-cc79ad317556");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fa828355-eff9-41f2-8c12-2d3a1fd1fd20");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "d0a48569-9bff-499d-8811-2fb0301cfeeb");

            migrationBuilder.AlterColumn<string>(
                name: "Referral",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Company",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AvatarId",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0cf7e196-eae2-4959-adaf-ac7faac388aa", null, "Admin", "ADMIN" },
                    { "4ca1a567-1830-453d-a3dc-a60a5f2abe22", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "AvatarId", "Company", "ConcurrencyStamp", "Country", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Referral", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "c627d672-a80a-486f-98f4-4d6b5ec0e535", 0, "", "KCD", "dc33976f-e32f-41f1-9732-1717c7db2d5e", "N/A", "admin@admin.com", true, false, null, "Admin User", "ADMIN@ADMIN.COM", "ADMIN", "AQAAAAIAAYagAAAAEHKS+em86Zi3wR3ioU+F/dwGrbEgMteTFEKGl+Y0uWMQ5xW92okqfBGBnQHiE0FkhA==", null, false, "Internal", "1a021ac6-b7a1-4fa3-9305-22841a2e3308", false, "admin" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Description" },
                values: new object[,]
                {
                    { "0cf7e196-eae2-4959-adaf-ac7faac388aa", "Admin" },
                    { "4ca1a567-1830-453d-a3dc-a60a5f2abe22", "User" }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "0cf7e196-eae2-4959-adaf-ac7faac388aa", "c627d672-a80a-486f-98f4-4d6b5ec0e535" });
        }
    }
}
