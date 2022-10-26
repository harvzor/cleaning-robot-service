﻿// <auto-generated />
using System;
using CleaningRobotService.DataPersistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CleaningRobotService.DataPersistence.Migrations
{
    [DbContext(typeof(ServiceDbContext))]
    partial class ServiceDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CleaningRobotService.DataPersistence.Models.CommandRobot", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTimeOffset?>("DeletedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted_at");

                    b.Property<DateTimeOffset>("ModifiedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("modified_at");

                    b.Property<int>("StartPointX")
                        .HasColumnType("integer")
                        .HasColumnName("start_point_x");

                    b.Property<int>("StartPointY")
                        .HasColumnType("integer")
                        .HasColumnName("start_point_y");

                    b.HasKey("Id")
                        .HasName("pk_command_robots");

                    b.ToTable("command_robots", (string)null);
                });

            modelBuilder.Entity("CleaningRobotService.DataPersistence.Models.CommandRobotCommand", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid?>("CommandRobotId")
                        .HasColumnType("uuid")
                        .HasColumnName("command_robot_id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTimeOffset?>("DeletedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted_at");

                    b.Property<string>("Direction")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("direction");

                    b.Property<DateTimeOffset>("ModifiedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("modified_at");

                    b.Property<long>("Steps")
                        .HasColumnType("bigint")
                        .HasColumnName("steps");

                    b.HasKey("Id")
                        .HasName("pk_command_robots_commands");

                    b.HasIndex("CommandRobotId")
                        .HasDatabaseName("ix_command_robots_commands_command_robot_id");

                    b.ToTable("command_robots_commands", (string)null);
                });

            modelBuilder.Entity("CleaningRobotService.DataPersistence.Models.Execution", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("CommandRobotId")
                        .HasColumnType("uuid")
                        .HasColumnName("command_robot_id");

                    b.Property<int>("Commands")
                        .HasColumnType("integer")
                        .HasColumnName("commands");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTimeOffset?>("DeletedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted_at");

                    b.Property<TimeSpan?>("Duration")
                        .HasColumnType("interval")
                        .HasColumnName("duration");

                    b.Property<DateTimeOffset>("ModifiedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("modified_at");

                    b.Property<int?>("Result")
                        .HasColumnType("integer")
                        .HasColumnName("result");

                    b.HasKey("Id")
                        .HasName("pk_executions");

                    b.HasIndex("CommandRobotId")
                        .HasDatabaseName("ix_executions_command_robot_id");

                    b.ToTable("executions", (string)null);
                });

            modelBuilder.Entity("CleaningRobotService.DataPersistence.Models.CommandRobotCommand", b =>
                {
                    b.HasOne("CleaningRobotService.DataPersistence.Models.CommandRobot", null)
                        .WithMany("Commands")
                        .HasForeignKey("CommandRobotId")
                        .HasConstraintName("fk_command_robots_commands_command_robots_command_robot_id");
                });

            modelBuilder.Entity("CleaningRobotService.DataPersistence.Models.Execution", b =>
                {
                    b.HasOne("CleaningRobotService.DataPersistence.Models.CommandRobot", "CommandRobot")
                        .WithMany()
                        .HasForeignKey("CommandRobotId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_executions_command_robots_command_robot_id");

                    b.Navigation("CommandRobot");
                });

            modelBuilder.Entity("CleaningRobotService.DataPersistence.Models.CommandRobot", b =>
                {
                    b.Navigation("Commands");
                });
#pragma warning restore 612, 618
        }
    }
}
