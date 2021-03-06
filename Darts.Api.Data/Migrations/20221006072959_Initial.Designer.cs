﻿// <auto-generated />
using System;
using Darts.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Darts.Api.Data.Migrations
{
    [DbContext(typeof(DartsContext))]
    [Migration("20221006072959_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Darts.Domain.DomainObjects.EditPlayerRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTimeOffset>("Date")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Diff")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("PlayerId")
                        .HasColumnType("bigint");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.HasIndex("UserId");

                    b.ToTable("EditPlayerRecords");
                });

            modelBuilder.Entity("Darts.Domain.DomainObjects.EditTeamRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTimeOffset>("Date")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Diff")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("TeamId")
                        .HasColumnType("bigint");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.HasIndex("UserId");

                    b.ToTable("EditTeamRecords");
                });

            modelBuilder.Entity("Darts.Domain.DomainObjects.Enrollment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<byte[]>("Data")
                        .HasColumnType("varbinary(max)");

                    b.Property<long?>("MatchId")
                        .HasColumnType("bigint");

                    b.Property<long?>("PremierLeagueMatchId")
                        .HasColumnType("bigint");

                    b.Property<DateTimeOffset>("Uploaded")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("Enrollments");
                });

            modelBuilder.Entity("Darts.Domain.DomainObjects.League", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<byte[]>("Image")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PointsForDraw")
                        .HasColumnType("int");

                    b.Property<int>("PointsPerLoose")
                        .HasColumnType("int");

                    b.Property<int>("PointsPerOvertimeLoose")
                        .HasColumnType("int");

                    b.Property<int>("PointsPerOvertimeWin")
                        .HasColumnType("int");

                    b.Property<int>("PointsPerWin")
                        .HasColumnType("int");

                    b.Property<string>("ShortCut")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Leagues");
                });

            modelBuilder.Entity("Darts.Domain.DomainObjects.LeagueTeam", b =>
                {
                    b.Property<long>("LeagueId")
                        .HasColumnType("bigint");

                    b.Property<long>("TeamId")
                        .HasColumnType("bigint");

                    b.HasKey("LeagueId", "TeamId");

                    b.HasIndex("TeamId");

                    b.ToTable("LeagueTeams");
                });

            modelBuilder.Entity("Darts.Domain.DomainObjects.LeagueTeamPlayer", b =>
                {
                    b.Property<long>("LeagueId")
                        .HasColumnType("bigint");

                    b.Property<long>("TeamId")
                        .HasColumnType("bigint");

                    b.Property<long?>("PlayerId")
                        .HasColumnType("bigint");

                    b.HasKey("LeagueId", "TeamId", "PlayerId");

                    b.HasIndex("PlayerId");

                    b.HasIndex("TeamId");

                    b.ToTable("LeagueTeamsPlayers");
                });

            modelBuilder.Entity("Darts.Domain.DomainObjects.Match", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTimeOffset>("Date")
                        .HasColumnType("datetimeoffset");

                    b.Property<decimal?>("GuestLegs")
                        .HasColumnType("decimal(13,4)");

                    b.Property<decimal?>("GuestPoints")
                        .HasColumnType("decimal(13,4)");

                    b.Property<long?>("GuestTeamId")
                        .HasColumnType("bigint");

                    b.Property<decimal?>("HomeLegs")
                        .HasColumnType("decimal(13,4)");

                    b.Property<decimal?>("HomePoints")
                        .HasColumnType("decimal(13,4)");

                    b.Property<long?>("HomeTeamId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsOvertime")
                        .HasColumnType("bit");

                    b.Property<long>("LeagueId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("GuestTeamId");

                    b.HasIndex("HomeTeamId");

                    b.HasIndex("LeagueId");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("Darts.Domain.DomainObjects.Player", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Identifier")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("Darts.Domain.DomainObjects.PremierLeague", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PointsForDraw")
                        .HasColumnType("int");

                    b.Property<int>("PointsPerLoose")
                        .HasColumnType("int");

                    b.Property<int>("PointsPerWin")
                        .HasColumnType("int");

                    b.Property<string>("ShortCut")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("PremierLeagues");
                });

            modelBuilder.Entity("Darts.Domain.DomainObjects.PremierLeagueMatch", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTimeOffset>("Date")
                        .HasColumnType("datetimeoffset");

                    b.Property<decimal>("GuestAverage")
                        .HasColumnType("decimal(13,4)");

                    b.Property<decimal>("GuestLegs")
                        .HasColumnType("decimal(13,4)");

                    b.Property<long?>("GuestPlayerId")
                        .HasColumnType("bigint");

                    b.Property<decimal>("HomeAverage")
                        .HasColumnType("decimal(13,4)");

                    b.Property<decimal>("HomeLegs")
                        .HasColumnType("decimal(13,4)");

                    b.Property<long?>("HomePlayerId")
                        .HasColumnType("bigint");

                    b.Property<long>("PremierLeagueId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("GuestPlayerId");

                    b.HasIndex("HomePlayerId");

                    b.HasIndex("PremierLeagueId");

                    b.ToTable("PremierLeagueMatches");
                });

            modelBuilder.Entity("Darts.Domain.DomainObjects.PremierLeaguePlayer", b =>
                {
                    b.Property<long>("PlayerId")
                        .HasColumnType("bigint");

                    b.Property<long>("PremierLeagueId")
                        .HasColumnType("bigint");

                    b.HasKey("PlayerId", "PremierLeagueId");

                    b.HasIndex("PremierLeagueId");

                    b.ToTable("PremierLeaguePlayer");
                });

            modelBuilder.Entity("Darts.Domain.DomainObjects.RequestRegistration", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("BanReason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("Processed")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("RequestRegistrations");
                });

            modelBuilder.Entity("Darts.Domain.DomainObjects.Stats", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<decimal>("Games")
                        .HasColumnType("decimal(13,4)");

                    b.Property<long?>("LeagueId")
                        .HasColumnType("bigint");

                    b.Property<decimal>("LooseLegs")
                        .HasColumnType("decimal(13,4)");

                    b.Property<long>("MatchId")
                        .HasColumnType("bigint");

                    b.Property<long>("PlayerId")
                        .HasColumnType("bigint");

                    b.Property<decimal>("Points")
                        .HasColumnType("decimal(13,4)");

                    b.Property<decimal>("WinLegs")
                        .HasColumnType("decimal(13,4)");

                    b.HasKey("Id");

                    b.HasIndex("LeagueId");

                    b.HasIndex("MatchId");

                    b.HasIndex("PlayerId");

                    b.ToTable("PlayerStats");
                });

            modelBuilder.Entity("Darts.Domain.DomainObjects.Team", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("Darts.Domain.DomainObjects.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Mobile")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserRole")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Darts.Domain.DomainObjects.EditPlayerRecord", b =>
                {
                    b.HasOne("Darts.Domain.DomainObjects.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Darts.Domain.DomainObjects.User", "User")
                        .WithMany("EditPlayerRecords")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Player");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Darts.Domain.DomainObjects.EditTeamRecord", b =>
                {
                    b.HasOne("Darts.Domain.DomainObjects.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Darts.Domain.DomainObjects.User", "User")
                        .WithMany("EditTeamRecords")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Team");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Darts.Domain.DomainObjects.League", b =>
                {
                    b.HasOne("Darts.Domain.DomainObjects.User", "User")
                        .WithMany("League")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Darts.Domain.DomainObjects.LeagueTeam", b =>
                {
                    b.HasOne("Darts.Domain.DomainObjects.League", "League")
                        .WithMany("LeagueTeams")
                        .HasForeignKey("LeagueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Darts.Domain.DomainObjects.Team", "Team")
                        .WithMany("LeagueTeams")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("League");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("Darts.Domain.DomainObjects.LeagueTeamPlayer", b =>
                {
                    b.HasOne("Darts.Domain.DomainObjects.League", "League")
                        .WithMany("LeagueTeamPlayers")
                        .HasForeignKey("LeagueId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Darts.Domain.DomainObjects.Player", "Player")
                        .WithMany("LeagueTeamPlayers")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Darts.Domain.DomainObjects.Team", "Team")
                        .WithMany("LeagueTeamPlayers")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("League");

                    b.Navigation("Player");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("Darts.Domain.DomainObjects.Match", b =>
                {
                    b.HasOne("Darts.Domain.DomainObjects.Team", "GuestTeam")
                        .WithMany()
                        .HasForeignKey("GuestTeamId");

                    b.HasOne("Darts.Domain.DomainObjects.Team", "HomeTeam")
                        .WithMany()
                        .HasForeignKey("HomeTeamId");

                    b.HasOne("Darts.Domain.DomainObjects.League", "League")
                        .WithMany("Matches")
                        .HasForeignKey("LeagueId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("GuestTeam");

                    b.Navigation("HomeTeam");

                    b.Navigation("League");
                });

            modelBuilder.Entity("Darts.Domain.DomainObjects.Player", b =>
                {
                    b.HasOne("Darts.Domain.DomainObjects.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Darts.Domain.DomainObjects.PremierLeague", b =>
                {
                    b.HasOne("Darts.Domain.DomainObjects.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Darts.Domain.DomainObjects.PremierLeagueMatch", b =>
                {
                    b.HasOne("Darts.Domain.DomainObjects.Player", "GuestPlayer")
                        .WithMany()
                        .HasForeignKey("GuestPlayerId");

                    b.HasOne("Darts.Domain.DomainObjects.Player", "HomePlayer")
                        .WithMany()
                        .HasForeignKey("HomePlayerId");

                    b.HasOne("Darts.Domain.DomainObjects.PremierLeague", "PremierLeague")
                        .WithMany("PremierLeagueMatches")
                        .HasForeignKey("PremierLeagueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GuestPlayer");

                    b.Navigation("HomePlayer");

                    b.Navigation("PremierLeague");
                });

            modelBuilder.Entity("Darts.Domain.DomainObjects.PremierLeaguePlayer", b =>
                {
                    b.HasOne("Darts.Domain.DomainObjects.Player", "Player")
                        .WithMany("PremierLeaguePlayers")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Darts.Domain.DomainObjects.PremierLeague", "PremierLeague")
                        .WithMany("PremierLeaguePlayers")
                        .HasForeignKey("PremierLeagueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");

                    b.Navigation("PremierLeague");
                });

            modelBuilder.Entity("Darts.Domain.DomainObjects.Stats", b =>
                {
                    b.HasOne("Darts.Domain.DomainObjects.League", null)
                        .WithMany("Stats")
                        .HasForeignKey("LeagueId");

                    b.HasOne("Darts.Domain.DomainObjects.Match", "Match")
                        .WithMany("Stats")
                        .HasForeignKey("MatchId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Darts.Domain.DomainObjects.Player", "Player")
                        .WithMany("Stats")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Match");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("Darts.Domain.DomainObjects.League", b =>
                {
                    b.Navigation("LeagueTeamPlayers");

                    b.Navigation("LeagueTeams");

                    b.Navigation("Matches");

                    b.Navigation("Stats");
                });

            modelBuilder.Entity("Darts.Domain.DomainObjects.Match", b =>
                {
                    b.Navigation("Stats");
                });

            modelBuilder.Entity("Darts.Domain.DomainObjects.Player", b =>
                {
                    b.Navigation("LeagueTeamPlayers");

                    b.Navigation("PremierLeaguePlayers");

                    b.Navigation("Stats");
                });

            modelBuilder.Entity("Darts.Domain.DomainObjects.PremierLeague", b =>
                {
                    b.Navigation("PremierLeagueMatches");

                    b.Navigation("PremierLeaguePlayers");
                });

            modelBuilder.Entity("Darts.Domain.DomainObjects.Team", b =>
                {
                    b.Navigation("LeagueTeamPlayers");

                    b.Navigation("LeagueTeams");
                });

            modelBuilder.Entity("Darts.Domain.DomainObjects.User", b =>
                {
                    b.Navigation("EditPlayerRecords");

                    b.Navigation("EditTeamRecords");

                    b.Navigation("League");
                });
#pragma warning restore 612, 618
        }
    }
}
