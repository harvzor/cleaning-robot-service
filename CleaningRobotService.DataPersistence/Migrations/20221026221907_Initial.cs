using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleaningRobotService.DataPersistence.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "command_robots",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    start_point_x = table.Column<int>(type: "integer", nullable: false),
                    start_point_y = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    modified_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_command_robots", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "command_robots_commands",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    direction = table.Column<string>(type: "text", nullable: false),
                    steps = table.Column<long>(type: "bigint", nullable: false),
                    command_robot_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    modified_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_command_robots_commands", x => x.id);
                    table.ForeignKey(
                        name: "fk_command_robots_commands_command_robots_command_robot_id",
                        column: x => x.command_robot_id,
                        principalTable: "command_robots",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "executions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    commands = table.Column<int>(type: "integer", nullable: false),
                    result = table.Column<int>(type: "integer", nullable: true),
                    duration = table.Column<TimeSpan>(type: "interval", nullable: true),
                    command_robot_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    modified_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_executions", x => x.id);
                    table.ForeignKey(
                        name: "fk_executions_command_robots_command_robot_id",
                        column: x => x.command_robot_id,
                        principalTable: "command_robots",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_command_robots_commands_command_robot_id",
                table: "command_robots_commands",
                column: "command_robot_id");

            migrationBuilder.CreateIndex(
                name: "ix_executions_command_robot_id",
                table: "executions",
                column: "command_robot_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "command_robots_commands");

            migrationBuilder.DropTable(
                name: "executions");

            migrationBuilder.DropTable(
                name: "command_robots");
        }
    }
}
