using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmpAnalytics.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserJobs_DateTimeCreated_UserId",
                table: "UserJobs",
                columns: new[] { "DateTimeCreated", "UserId" },
                descending: new[] { true, false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserJobs_DateTimeCreated_UserId",
                table: "UserJobs");
        }
    }
}
