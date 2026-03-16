using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FilmeApis.Migrations
{
    /// <inheritdoc />
    public partial class RelacionamentoCinemaEndereco : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cinemas_Endereços_EndereçoID",
                table: "Cinemas");

            migrationBuilder.RenameColumn(
                name: "EndereçoID",
                table: "Cinemas",
                newName: "EnderecoId");

            migrationBuilder.RenameIndex(
                name: "IX_Cinemas_EndereçoID",
                table: "Cinemas",
                newName: "IX_Cinemas_EnderecoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cinemas_Endereços_EnderecoId",
                table: "Cinemas",
                column: "EnderecoId",
                principalTable: "Endereços",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cinemas_Endereços_EnderecoId",
                table: "Cinemas");

            migrationBuilder.RenameColumn(
                name: "EnderecoId",
                table: "Cinemas",
                newName: "EndereçoID");

            migrationBuilder.RenameIndex(
                name: "IX_Cinemas_EnderecoId",
                table: "Cinemas",
                newName: "IX_Cinemas_EndereçoID");

            migrationBuilder.AddForeignKey(
                name: "FK_Cinemas_Endereços_EndereçoID",
                table: "Cinemas",
                column: "EndereçoID",
                principalTable: "Endereços",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
