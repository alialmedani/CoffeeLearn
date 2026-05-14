using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeeLearn.Infrastructure.Migrations
{
	/// <inheritdoc />
	public partial class AddAuditFieldsFromBaseEntity : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<DateTime>(
				name: "CreatedAt",
				table: "Products",
				type: "datetime2",
				nullable: false,
				defaultValueSql: "GETUTCDATE()");

			migrationBuilder.AddColumn<DateTime>(
				name: "UpdatedAt",
				table: "Products",
				type: "datetime2",
				nullable: true);

			migrationBuilder.AddColumn<DateTime>(
				name: "UpdatedAt",
				table: "Orders",
				type: "datetime2",
				nullable: true);

			migrationBuilder.AddColumn<DateTime>(
				name: "CreatedAt",
				table: "OrderItems",
				type: "datetime2",
				nullable: false,
				defaultValueSql: "GETUTCDATE()");

			migrationBuilder.AddColumn<DateTime>(
				name: "UpdatedAt",
				table: "OrderItems",
				type: "datetime2",
				nullable: true);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "CreatedAt",
				table: "Products");

			migrationBuilder.DropColumn(
				name: "UpdatedAt",
				table: "Products");

			migrationBuilder.DropColumn(
				name: "UpdatedAt",
				table: "Orders");

			migrationBuilder.DropColumn(
				name: "CreatedAt",
				table: "OrderItems");

			migrationBuilder.DropColumn(
				name: "UpdatedAt",
				table: "OrderItems");
		}
	}
}