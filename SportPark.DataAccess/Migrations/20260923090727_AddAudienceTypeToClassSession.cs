using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportPark.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddAudienceTypeToClassSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AudienceType",
                table: "ClassSessions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AudienceType",
                table: "ClassSessions");
        }
    }
}
