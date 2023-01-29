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
                name: "commands",
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
                    table.PrimaryKey("pk_commands", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "direction_steps",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    direction = table.Column<string>(type: "text", nullable: false),
                    steps = table.Column<long>(type: "bigint", nullable: false),
                    command_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    modified_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_direction_steps", x => x.id);
                    table.ForeignKey(
                        name: "fk_direction_steps_commands_command_id",
                        column: x => x.command_id,
                        principalTable: "commands",
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
                    command_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    modified_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_executions", x => x.id);
                    table.ForeignKey(
                        name: "fk_executions_commands_command_id",
                        column: x => x.command_id,
                        principalTable: "commands",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_direction_steps_command_id",
                table: "direction_steps",
                column: "command_id");

            migrationBuilder.CreateIndex(
                name: "ix_executions_command_id",
                table: "executions",
                column: "command_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "direction_steps");

            migrationBuilder.DropTable(
                name: "executions");

            migrationBuilder.DropTable(
                name: "commands");
        }
    }
}
