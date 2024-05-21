using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RL.Data.Migrations
{
    public partial class AddPlanProcedureUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlanProcedureUser",
                columns: table => new
                {
                    PlanProcedureUsersUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserPlanProceduresPlanId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserPlanProceduresProcedureId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanProcedureUser", x => new { x.PlanProcedureUsersUserId, x.UserPlanProceduresPlanId, x.UserPlanProceduresProcedureId });
                    table.ForeignKey(
                        name: "FK_PlanProcedureUser_PlanProcedures_UserPlanProceduresPlanId_UserPlanProceduresProcedureId",
                        columns: x => new { x.UserPlanProceduresPlanId, x.UserPlanProceduresProcedureId },
                        principalTable: "PlanProcedures",
                        principalColumns: new[] { "PlanId", "ProcedureId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanProcedureUser_Users_PlanProcedureUsersUserId",
                        column: x => x.PlanProcedureUsersUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlanProcedureUser_UserPlanProceduresPlanId_UserPlanProceduresProcedureId",
                table: "PlanProcedureUser",
                columns: new[] { "UserPlanProceduresPlanId", "UserPlanProceduresProcedureId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlanProcedureUser");
        }
    }
}
