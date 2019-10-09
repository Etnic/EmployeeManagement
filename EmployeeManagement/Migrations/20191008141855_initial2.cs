using Microsoft.EntityFrameworkCore.Migrations;

namespace EmployeeManagement.Migrations
{
    public partial class initial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "employee_id_department_fkey",
                table: "employee");

            migrationBuilder.AlterColumn<string>(
                name: "photopath",
                table: "employee",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_id_department",
                table: "employee",
                column: "IdDepartment",
                principalTable: "department",
                principalColumn: "id_department",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_id_department",
                table: "employee");

            migrationBuilder.AlterColumn<string>(
                name: "photopath",
                table: "employee",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "employee_id_department_fkey",
                table: "employee",
                column: "IdDepartment",
                principalTable: "department",
                principalColumn: "id_department",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
