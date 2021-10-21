﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace PortalR.API.Migrations
{
    public partial class CreateDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(

                name: "Values",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                     .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Values", x => x.Id);
                });
               
;
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Valuse"
                );
        }
    }
}
