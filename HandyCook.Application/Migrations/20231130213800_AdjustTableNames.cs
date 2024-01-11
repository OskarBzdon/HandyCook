using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HandyCook.Application.Migrations
{
    /// <inheritdoc />
    public partial class AdjustTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredient_Recipes_RecipeNavigationId",
                table: "Ingredient");

            migrationBuilder.DropForeignKey(
                name: "FK_Ingredient_Step_StepNavigationId",
                table: "Ingredient");

            migrationBuilder.DropForeignKey(
                name: "FK_Step_File_MediaFileId",
                table: "Step");

            migrationBuilder.DropForeignKey(
                name: "FK_Step_Recipes_RecipeNavigationId",
                table: "Step");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Step",
                table: "Step");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ingredient",
                table: "Ingredient");

            migrationBuilder.RenameTable(
                name: "Step",
                newName: "Steps");

            migrationBuilder.RenameTable(
                name: "Ingredient",
                newName: "Ingredients");

            migrationBuilder.RenameIndex(
                name: "IX_Step_RecipeNavigationId",
                table: "Steps",
                newName: "IX_Steps_RecipeNavigationId");

            migrationBuilder.RenameIndex(
                name: "IX_Step_MediaFileId",
                table: "Steps",
                newName: "IX_Steps_MediaFileId");

            migrationBuilder.RenameIndex(
                name: "IX_Ingredient_StepNavigationId",
                table: "Ingredients",
                newName: "IX_Ingredients_StepNavigationId");

            migrationBuilder.RenameIndex(
                name: "IX_Ingredient_RecipeNavigationId",
                table: "Ingredients",
                newName: "IX_Ingredients_RecipeNavigationId");

            migrationBuilder.AlterColumn<double>(
                name: "Timer",
                table: "Steps",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "Temperature",
                table: "Steps",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "MediaFileId",
                table: "Steps",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Steps",
                table: "Steps",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ingredients",
                table: "Ingredients",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Recipes_RecipeNavigationId",
                table: "Ingredients",
                column: "RecipeNavigationId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Steps_StepNavigationId",
                table: "Ingredients",
                column: "StepNavigationId",
                principalTable: "Steps",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Steps_File_MediaFileId",
                table: "Steps",
                column: "MediaFileId",
                principalTable: "File",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Steps_Recipes_RecipeNavigationId",
                table: "Steps",
                column: "RecipeNavigationId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Recipes_RecipeNavigationId",
                table: "Ingredients");

            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Steps_StepNavigationId",
                table: "Ingredients");

            migrationBuilder.DropForeignKey(
                name: "FK_Steps_File_MediaFileId",
                table: "Steps");

            migrationBuilder.DropForeignKey(
                name: "FK_Steps_Recipes_RecipeNavigationId",
                table: "Steps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Steps",
                table: "Steps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ingredients",
                table: "Ingredients");

            migrationBuilder.RenameTable(
                name: "Steps",
                newName: "Step");

            migrationBuilder.RenameTable(
                name: "Ingredients",
                newName: "Ingredient");

            migrationBuilder.RenameIndex(
                name: "IX_Steps_RecipeNavigationId",
                table: "Step",
                newName: "IX_Step_RecipeNavigationId");

            migrationBuilder.RenameIndex(
                name: "IX_Steps_MediaFileId",
                table: "Step",
                newName: "IX_Step_MediaFileId");

            migrationBuilder.RenameIndex(
                name: "IX_Ingredients_StepNavigationId",
                table: "Ingredient",
                newName: "IX_Ingredient_StepNavigationId");

            migrationBuilder.RenameIndex(
                name: "IX_Ingredients_RecipeNavigationId",
                table: "Ingredient",
                newName: "IX_Ingredient_RecipeNavigationId");

            migrationBuilder.AlterColumn<double>(
                name: "Timer",
                table: "Step",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Temperature",
                table: "Step",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MediaFileId",
                table: "Step",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Step",
                table: "Step",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ingredient",
                table: "Ingredient",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredient_Recipes_RecipeNavigationId",
                table: "Ingredient",
                column: "RecipeNavigationId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredient_Step_StepNavigationId",
                table: "Ingredient",
                column: "StepNavigationId",
                principalTable: "Step",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Step_File_MediaFileId",
                table: "Step",
                column: "MediaFileId",
                principalTable: "File",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Step_Recipes_RecipeNavigationId",
                table: "Step",
                column: "RecipeNavigationId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
