using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FilmeApis.Migrations
{
    /// <inheritdoc />
    public partial class ProtecaoRelacionamentos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cinemas_Endereços_EnderecoId",
                table: "Cinemas");

            migrationBuilder.AddForeignKey(
                name: "FK_Cinemas_Endereços_EnderecoId",
                table: "Cinemas",
                column: "EnderecoId",
                principalTable: "Endereços",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cinemas_Endereços_EnderecoId",
                table: "Cinemas");

            migrationBuilder.AddForeignKey(
                name: "FK_Cinemas_Endereços_EnderecoId",
                table: "Cinemas",
                column: "EnderecoId",
                principalTable: "Endereços",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
