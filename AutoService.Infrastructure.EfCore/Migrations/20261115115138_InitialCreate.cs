using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AutoService.Infrastructure.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "clients",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    full_name = table.Column<string>(type: "longtext", nullable: false),
                    phone = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clients", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mechanics",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    passport_number = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    full_name = table.Column<string>(type: "longtext", nullable: false),
                    specialization = table.Column<int>(type: "int", nullable: false),
                    experience = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mechanics", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "work_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: false),
                    category = table.Column<int>(type: "int", nullable: false),
                    cost = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    duration = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_work_types", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "cars",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    license_plate = table.Column<string>(type: "longtext", nullable: false),
                    brand = table.Column<string>(type: "longtext", nullable: false),
                    model = table.Column<string>(type: "longtext", nullable: false),
                    year = table.Column<int>(type: "int", nullable: true),
                    client_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cars", x => x.id);
                    table.ForeignKey(
                        name: "FK_cars_clients_client_id",
                        column: x => x.client_id,
                        principalTable: "clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "repair_orders",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    client_id = table.Column<int>(type: "int", nullable: false),
                    car_id = table.Column<int>(type: "int", nullable: false),
                    admission_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    release_date = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_repair_orders", x => x.id);
                    table.ForeignKey(
                        name: "FK_repair_orders_cars_car_id",
                        column: x => x.car_id,
                        principalTable: "cars",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_repair_orders_clients_client_id",
                        column: x => x.client_id,
                        principalTable: "clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "order_mechanics",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    repair_order_id = table.Column<int>(type: "int", nullable: false),
                    mechanic_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_mechanics", x => x.id);
                    table.ForeignKey(
                        name: "FK_order_mechanics_mechanics_mechanic_id",
                        column: x => x.mechanic_id,
                        principalTable: "mechanics",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_order_mechanics_repair_orders_repair_order_id",
                        column: x => x.repair_order_id,
                        principalTable: "repair_orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "order_works",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    repair_order_id = table.Column<int>(type: "int", nullable: false),
                    work_type_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_works", x => x.id);
                    table.ForeignKey(
                        name: "FK_order_works_repair_orders_repair_order_id",
                        column: x => x.repair_order_id,
                        principalTable: "repair_orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_order_works_work_types_work_type_id",
                        column: x => x.work_type_id,
                        principalTable: "work_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.InsertData(
                table: "work_types",
                columns: new[] { "id", "category", "cost", "description", "duration", "name" },
                values: new object[,]
                {
                    { 1, 5, 2500m, "Engine Oil And Oil Filter Replacement", 60, "Oil And Filter Change" },
                    { 2, 3, 5000m, "Computerized Engine Diagnostic Testing", 120, "Engine Diagnostics" },
                    { 3, 0, 15000m, "Repair Of Engine Components And Systems", 360, "Engine Repair" },
                    { 4, 1, 20000m, "Clutch Assembly Replacement", 360, "Clutch Replacement" },
                    { 5, 2, 4000m, "Electrical System Diagnostic And Troubleshooting", 120, "Electrical Diagnostics" },
                    { 6, 2, 7000m, "Repair And Restoration Of Electrical Wiring", 180, "Electrical Wiring Repair" },
                    { 7, 4, 25000m, "Repair Of Vehicle Body Damage", 480, "Body Repair" },
                    { 8, 5, 8000m, "Scheduled Vehicle Maintenance And Inspection", 180, "Routine Vehicle Maintenance" },
                    { 9, 0, 50000m, "Complete Engine Rebuild And Overhaul", 720, "Engine Overhaul" },
                    { 10, 1, 6000m, "Transmission System Diagnostic And Inspection", 120, "Transmission Diagnostics" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_cars_client_id",
                table: "cars",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_mechanics_mechanic_id",
                table: "order_mechanics",
                column: "mechanic_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_mechanics_repair_order_id",
                table: "order_mechanics",
                column: "repair_order_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_works_repair_order_id",
                table: "order_works",
                column: "repair_order_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_works_work_type_id",
                table: "order_works",
                column: "work_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_repair_orders_car_id",
                table: "repair_orders",
                column: "car_id");

            migrationBuilder.CreateIndex(
                name: "IX_repair_orders_client_id",
                table: "repair_orders",
                column: "client_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "order_mechanics");

            migrationBuilder.DropTable(
                name: "order_works");

            migrationBuilder.DropTable(
                name: "mechanics");

            migrationBuilder.DropTable(
                name: "repair_orders");

            migrationBuilder.DropTable(
                name: "work_types");

            migrationBuilder.DropTable(
                name: "cars");

            migrationBuilder.DropTable(
                name: "clients");
        }
    }
}
