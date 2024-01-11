using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HandyCook.Application.Migrations
{
    /// <inheritdoc />
    public partial class AddStepsIngredients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Step",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MediaFileId = table.Column<int>(type: "int", nullable: false),
                    Temperature = table.Column<double>(type: "float", nullable: false),
                    Timer = table.Column<double>(type: "float", nullable: false),
                    RecipeNavigationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Step", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Step_File_MediaFileId",
                        column: x => x.MediaFileId,
                        principalTable: "File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Step_Recipes_RecipeNavigationId",
                        column: x => x.RecipeNavigationId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ingredient",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecipeNavigationId = table.Column<int>(type: "int", nullable: false),
                    StepNavigationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ingredient_Recipes_RecipeNavigationId",
                        column: x => x.RecipeNavigationId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ingredient_Step_StepNavigationId",
                        column: x => x.StepNavigationId,
                        principalTable: "Step",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ingredient_RecipeNavigationId",
                table: "Ingredient",
                column: "RecipeNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredient_StepNavigationId",
                table: "Ingredient",
                column: "StepNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_Step_MediaFileId",
                table: "Step",
                column: "MediaFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Step_RecipeNavigationId",
                table: "Step",
                column: "RecipeNavigationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ingredient");

            migrationBuilder.DropTable(
                name: "Step");
        }
    }
}
