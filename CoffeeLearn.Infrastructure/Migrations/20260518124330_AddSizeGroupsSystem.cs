using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeeLearn.Infrastructure.Migrations
{
	public partial class AddSizeGroupsSystem : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<int>(
				name: "SizeGroupId",
				table: "Products",
				type: "int",
				nullable: true);

			migrationBuilder.AddColumn<int>(
				name: "SizeOptionId",
				table: "ProductVariants",
				type: "int",
				nullable: true);

			migrationBuilder.CreateTable(
				name: "SizeGroups",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					CategoryId = table.Column<int>(type: "int", nullable: false),
					Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
					Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
					IsActive = table.Column<bool>(type: "bit", nullable: false),
					CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
					UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
					IsDeleted = table.Column<bool>(type: "bit", nullable: false),
					DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_SizeGroups", x => x.Id);
					table.ForeignKey(
						name: "FK_SizeGroups_Categories_CategoryId",
						column: x => x.CategoryId,
						principalTable: "Categories",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "SizeOptions",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					SizeGroupId = table.Column<int>(type: "int", nullable: false),
					Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
					SortOrder = table.Column<int>(type: "int", nullable: false),
					IsActive = table.Column<bool>(type: "bit", nullable: false),
					CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
					UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
					IsDeleted = table.Column<bool>(type: "bit", nullable: false),
					DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_SizeOptions", x => x.Id);
					table.ForeignKey(
						name: "FK_SizeOptions_SizeGroups_SizeGroupId",
						column: x => x.SizeGroupId,
						principalTable: "SizeGroups",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_Products_SizeGroupId",
				table: "Products",
				column: "SizeGroupId");

			migrationBuilder.CreateIndex(
				name: "IX_ProductVariants_SizeOptionId",
				table: "ProductVariants",
				column: "SizeOptionId");

			migrationBuilder.CreateIndex(
				name: "IX_SizeGroups_CategoryId_Name",
				table: "SizeGroups",
				columns: new[] { "CategoryId", "Name" },
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_SizeOptions_SizeGroupId_Name",
				table: "SizeOptions",
				columns: new[] { "SizeGroupId", "Name" },
				unique: true);

			migrationBuilder.AddForeignKey(
				name: "FK_Products_SizeGroups_SizeGroupId",
				table: "Products",
				column: "SizeGroupId",
				principalTable: "SizeGroups",
				principalColumn: "Id",
				onDelete: ReferentialAction.NoAction);

			migrationBuilder.AddForeignKey(
				name: "FK_ProductVariants_SizeOptions_SizeOptionId",
				table: "ProductVariants",
				column: "SizeOptionId",
				principalTable: "SizeOptions",
				principalColumn: "Id",
				onDelete: ReferentialAction.NoAction);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_ProductVariants_SizeOptions_SizeOptionId",
				table: "ProductVariants");

			migrationBuilder.DropForeignKey(
				name: "FK_Products_SizeGroups_SizeGroupId",
				table: "Products");

			migrationBuilder.DropTable(
				name: "SizeOptions");

			migrationBuilder.DropTable(
				name: "SizeGroups");

			migrationBuilder.DropIndex(
				name: "IX_ProductVariants_SizeOptionId",
				table: "ProductVariants");

			migrationBuilder.DropIndex(
				name: "IX_Products_SizeGroupId",
				table: "Products");

			migrationBuilder.DropColumn(
				name: "SizeOptionId",
				table: "ProductVariants");

			migrationBuilder.DropColumn(
				name: "SizeGroupId",
				table: "Products");
		}
	}
}