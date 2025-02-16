using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Exercise8.Migrations
{
    /// <inheritdoc />
    public partial class AddSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Doctors",
                columns: new[] { "IdDoctor", "Email", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1, "koko123@gmail.pl", "Kamil", "Koko" },
                    { 2, "Kali13@onet.pl", "Maciek", "Kali" }
                });

            migrationBuilder.InsertData(
                table: "Medicaments",
                columns: new[] { "IdMedicament", "Description", "Name", "Type" },
                values: new object[,]
                {
                    { 1, "Na ból głowy", "Apap", "Tabletka" },
                    { 2, "Gorączka", "Gripex", "Proszek" }
                });

            migrationBuilder.InsertData(
                table: "Patiens",
                columns: new[] { "IdPatient", "Birthdate", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 5, 17, 22, 49, 12, 513, DateTimeKind.Local).AddTicks(8077), "Maciek", "Musiała" },
                    { 2, new DateTime(2023, 5, 17, 22, 49, 12, 513, DateTimeKind.Local).AddTicks(8116), "Maja", "Gabur" }
                });

            migrationBuilder.InsertData(
                table: "Prescription",
                columns: new[] { "IdPrescription", "Date", "DueDate", "IdDoctor", "IdPatient" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 5, 17, 22, 49, 12, 516, DateTimeKind.Local).AddTicks(984), new DateTime(2023, 5, 17, 22, 49, 12, 516, DateTimeKind.Local).AddTicks(1000), 1, 2 },
                    { 2, new DateTime(2023, 5, 17, 22, 49, 12, 516, DateTimeKind.Local).AddTicks(1006), new DateTime(2023, 5, 17, 22, 49, 12, 516, DateTimeKind.Local).AddTicks(1007), 2, 1 }
                });

            migrationBuilder.InsertData(
                table: "Prescription_Medicament",
                columns: new[] { "IdMedicament", "IdPrescription", "Details", "Dose" },
                values: new object[,]
                {
                    { 1, 1, "Czytaj ulotke", 30 },
                    { 2, 2, "Nie dla ciężarnych kobiet", 50 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Prescription_Medicament",
                keyColumns: new[] { "IdMedicament", "IdPrescription" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "Prescription_Medicament",
                keyColumns: new[] { "IdMedicament", "IdPrescription" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "Medicaments",
                keyColumn: "IdMedicament",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Medicaments",
                keyColumn: "IdMedicament",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Prescription",
                keyColumn: "IdPrescription",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Prescription",
                keyColumn: "IdPrescription",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "IdDoctor",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "IdDoctor",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Patiens",
                keyColumn: "IdPatient",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Patiens",
                keyColumn: "IdPatient",
                keyValue: 2);
        }
    }
}
