using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Volo.Abp.Identity.Pro.DemoApp.Migrations;

public partial class Added_IsActive_To_IdentityUser : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "IsActive",
            table: "AbpUsers",
            type: "bit",
            nullable: false,
            defaultValue: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "IsActive",
            table: "AbpUsers");
    }
}
