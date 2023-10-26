using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Darts.Api.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Enrollments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MatchId = table.Column<long>(type: "bigint", nullable: true),
                    PremierLeagueMatchId = table.Column<long>(type: "bigint", nullable: true),
                    Data = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Uploaded = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequestRegistrations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Processed = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    BanReason = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestRegistrations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UserRole = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EditTeamRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeamId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Diff = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EditTeamRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EditTeamRecords_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EditTeamRecords_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Leagues",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortCut = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    PointsPerWin = table.Column<int>(type: "int", nullable: false),
                    PointsPerLoose = table.Column<int>(type: "int", nullable: false),
                    PointsPerOvertimeWin = table.Column<int>(type: "int", nullable: false),
                    PointsPerOvertimeLoose = table.Column<int>(type: "int", nullable: false),
                    PointsForDraw = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leagues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Leagues_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Identifier = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Players_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PremierLeagues",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortCut = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PointsPerWin = table.Column<int>(type: "int", nullable: false),
                    PointsPerLoose = table.Column<int>(type: "int", nullable: false),
                    PointsForDraw = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PremierLeagues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PremierLeagues_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeagueTeams",
                columns: table => new
                {
                    TeamId = table.Column<long>(type: "bigint", nullable: false),
                    LeagueId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeagueTeams", x => new { x.LeagueId, x.TeamId });
                    table.ForeignKey(
                        name: "FK_LeagueTeams_Leagues_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "Leagues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeagueTeams_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HomeLegs = table.Column<decimal>(type: "decimal(13,4)", nullable: true),
                    HomePoints = table.Column<decimal>(type: "decimal(13,4)", nullable: true),
                    GuestLegs = table.Column<decimal>(type: "decimal(13,4)", nullable: true),
                    GuestPoints = table.Column<decimal>(type: "decimal(13,4)", nullable: true),
                    IsOvertime = table.Column<bool>(type: "bit", nullable: false),
                    GuestTeamId = table.Column<long>(type: "bigint", nullable: true),
                    HomeTeamId = table.Column<long>(type: "bigint", nullable: true),
                    LeagueId = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_Leagues_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "Leagues",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Matches_Teams_GuestTeamId",
                        column: x => x.GuestTeamId,
                        principalTable: "Teams",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Matches_Teams_HomeTeamId",
                        column: x => x.HomeTeamId,
                        principalTable: "Teams",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EditPlayerRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Diff = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EditPlayerRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EditPlayerRecords_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EditPlayerRecords_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LeagueTeamsPlayers",
                columns: table => new
                {
                    LeagueId = table.Column<long>(type: "bigint", nullable: false),
                    TeamId = table.Column<long>(type: "bigint", nullable: false),
                    PlayerId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeagueTeamsPlayers", x => new { x.LeagueId, x.TeamId, x.PlayerId });
                    table.ForeignKey(
                        name: "FK_LeagueTeamsPlayers_Leagues_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "Leagues",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LeagueTeamsPlayers_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LeagueTeamsPlayers_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PremierLeagueMatches",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PremierLeagueId = table.Column<long>(type: "bigint", nullable: false),
                    HomePlayerId = table.Column<long>(type: "bigint", nullable: true),
                    GuestPlayerId = table.Column<long>(type: "bigint", nullable: true),
                    HomeLegs = table.Column<decimal>(type: "decimal(13,4)", nullable: false),
                    HomeAverage = table.Column<decimal>(type: "decimal(13,4)", nullable: false),
                    GuestLegs = table.Column<decimal>(type: "decimal(13,4)", nullable: false),
                    GuestAverage = table.Column<decimal>(type: "decimal(13,4)", nullable: false),
                    Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PremierLeagueMatches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PremierLeagueMatches_Players_GuestPlayerId",
                        column: x => x.GuestPlayerId,
                        principalTable: "Players",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PremierLeagueMatches_Players_HomePlayerId",
                        column: x => x.HomePlayerId,
                        principalTable: "Players",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PremierLeagueMatches_PremierLeagues_PremierLeagueId",
                        column: x => x.PremierLeagueId,
                        principalTable: "PremierLeagues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PremierLeaguePlayer",
                columns: table => new
                {
                    PlayerId = table.Column<long>(type: "bigint", nullable: false),
                    PremierLeagueId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PremierLeaguePlayer", x => new { x.PlayerId, x.PremierLeagueId });
                    table.ForeignKey(
                        name: "FK_PremierLeaguePlayer_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PremierLeaguePlayer_PremierLeagues_PremierLeagueId",
                        column: x => x.PremierLeagueId,
                        principalTable: "PremierLeagues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerStats",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Points = table.Column<decimal>(type: "decimal(13,4)", nullable: false),
                    Games = table.Column<decimal>(type: "decimal(13,4)", nullable: false),
                    WinLegs = table.Column<decimal>(type: "decimal(13,4)", nullable: false),
                    LooseLegs = table.Column<decimal>(type: "decimal(13,4)", nullable: false),
                    PlayerId = table.Column<long>(type: "bigint", nullable: false),
                    MatchId = table.Column<long>(type: "bigint", nullable: false),
                    LeagueId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerStats_Leagues_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "Leagues",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlayerStats_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlayerStats_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EditPlayerRecords_PlayerId",
                table: "EditPlayerRecords",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_EditPlayerRecords_UserId",
                table: "EditPlayerRecords",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EditTeamRecords_TeamId",
                table: "EditTeamRecords",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_EditTeamRecords_UserId",
                table: "EditTeamRecords",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Leagues_UserId",
                table: "Leagues",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LeagueTeams_TeamId",
                table: "LeagueTeams",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_LeagueTeamsPlayers_PlayerId",
                table: "LeagueTeamsPlayers",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_LeagueTeamsPlayers_TeamId",
                table: "LeagueTeamsPlayers",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_GuestTeamId",
                table: "Matches",
                column: "GuestTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_HomeTeamId",
                table: "Matches",
                column: "HomeTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_LeagueId",
                table: "Matches",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_UserId",
                table: "Players",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerStats_LeagueId",
                table: "PlayerStats",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerStats_MatchId",
                table: "PlayerStats",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerStats_PlayerId",
                table: "PlayerStats",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PremierLeagueMatches_GuestPlayerId",
                table: "PremierLeagueMatches",
                column: "GuestPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PremierLeagueMatches_HomePlayerId",
                table: "PremierLeagueMatches",
                column: "HomePlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PremierLeagueMatches_PremierLeagueId",
                table: "PremierLeagueMatches",
                column: "PremierLeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_PremierLeaguePlayer_PremierLeagueId",
                table: "PremierLeaguePlayer",
                column: "PremierLeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_PremierLeagues_UserId",
                table: "PremierLeagues",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EditPlayerRecords");

            migrationBuilder.DropTable(
                name: "EditTeamRecords");

            migrationBuilder.DropTable(
                name: "Enrollments");

            migrationBuilder.DropTable(
                name: "LeagueTeams");

            migrationBuilder.DropTable(
                name: "LeagueTeamsPlayers");

            migrationBuilder.DropTable(
                name: "PlayerStats");

            migrationBuilder.DropTable(
                name: "PremierLeagueMatches");

            migrationBuilder.DropTable(
                name: "PremierLeaguePlayer");

            migrationBuilder.DropTable(
                name: "RequestRegistrations");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "PremierLeagues");

            migrationBuilder.DropTable(
                name: "Leagues");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
