using Microsoft.EntityFrameworkCore;
using Darts.Domain.DomainObjects;

namespace Darts.Api.Data
{
   public class DartsContext : DbContext
   {
      public DartsContext(DbContextOptions options)
         : base(options)
      {
      }

      public DartsContext()
      {

      }

      public DbSet<PremierLeague> PremierLeagues { get; set; }
      public DbSet<PremierLeagueMatch> PremierLeagueMatches { get; set; }

      public DbSet<League> Leagues { get; set; }
      public DbSet<Match> Matches { get; set; }

      public DbSet<Player> Players { get; set; }
      public DbSet<Stats> PlayerStats { get; set; }
      public DbSet<Team> Teams { get; set; }
      public DbSet<User> Users { get; set; }

      public DbSet<LeagueTeam> LeagueTeams { get; set; }
      public DbSet<LeagueTeamPlayer> LeagueTeamsPlayers { get; set; }
      
      public DbSet<Enrollment> Enrollments { get; set; }
      public DbSet<EditPlayerRecord> EditPlayerRecords { get; set; }
      public DbSet<EditTeamRecord> EditTeamRecords { get; set; }
      public DbSet<RequestRegistration> RequestRegistrations { get; set; }

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
         modelBuilder.Entity<Enrollment>().HasKey(x => x.Id);

         //Premier league match
         modelBuilder.Entity<PremierLeagueMatch>().HasKey(x => x.Id);
         modelBuilder.Entity<PremierLeagueMatch>().Property(c => c.HomeLegs).HasColumnType("decimal(13,4)");
         modelBuilder.Entity<PremierLeagueMatch>().Property(c => c.HomeAverage).HasColumnType("decimal(13,4)");
         modelBuilder.Entity<PremierLeagueMatch>().Property(c => c.GuestLegs).HasColumnType("decimal(13,4)");
         modelBuilder.Entity<PremierLeagueMatch>().Property(c => c.GuestAverage).HasColumnType("decimal(13,4)");
         modelBuilder.Entity<PremierLeagueMatch>().HasOne(x => x.PremierLeague).WithMany(x => x.PremierLeagueMatches);

         //Match
         modelBuilder.Entity<Match>().HasKey(x => x.Id);
         modelBuilder.Entity<Match>().Property(c => c.HomeLegs).HasColumnType("decimal(13,4)");
         modelBuilder.Entity<Match>().Property(c => c.HomePoints).HasColumnType("decimal(13,4)");
         modelBuilder.Entity<Match>().Property(c => c.GuestLegs).HasColumnType("decimal(13,4)");
         modelBuilder.Entity<Match>().Property(c => c.GuestPoints).HasColumnType("decimal(13,4)");
         modelBuilder.Entity<Match>().HasOne(x => x.HomeTeam);
         modelBuilder.Entity<Match>().HasOne(x => x.GuestTeam);
         modelBuilder.Entity<Match>().HasOne(x => x.League).WithMany(x => x.Matches).OnDelete(DeleteBehavior.NoAction);

         //Stats
         modelBuilder.Entity<Stats>().HasKey(x => x.Id);
         modelBuilder.Entity<Stats>().Property(c => c.Points).HasColumnType("decimal(13,4)");
         modelBuilder.Entity<Stats>().Property(c => c.Games).HasColumnType("decimal(13,4)");
         modelBuilder.Entity<Stats>().Property(c => c.WinLegs).HasColumnType("decimal(13,4)");
         modelBuilder.Entity<Stats>().Property(c => c.LooseLegs).HasColumnType("decimal(13,4)");
         modelBuilder.Entity<Stats>().HasOne(x => x.Player).WithMany(x => x.Stats).OnDelete(DeleteBehavior.NoAction);
         modelBuilder.Entity<Stats>().HasOne(x => x.Match).WithMany(x => x.Stats).OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<PremierLeaguePlayer>()
           .HasKey(bc => new { bc.PlayerId, bc.PremierLeagueId });

        modelBuilder.Entity<PremierLeaguePlayer>()
           .HasOne(plp => plp.Player).WithMany(x => x.PremierLeaguePlayers).OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<LeagueTeam>()
           .HasKey(bc => new { bc.LeagueId, bc.TeamId });

        modelBuilder.Entity<LeagueTeam>()
           .HasOne(bc => bc.Team).WithMany(x => x.LeagueTeams).OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<EditPlayerRecord>().HasKey(e => e.Id);
        modelBuilder.Entity<EditPlayerRecord>().HasOne(e => e.User).WithMany(x => x.EditPlayerRecords).OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<EditTeamRecord>().HasKey(e => e.Id);
        modelBuilder.Entity<EditTeamRecord>().HasOne(e => e.User).WithMany(x => x.EditTeamRecords).OnDelete(DeleteBehavior.NoAction);
            
         modelBuilder.Entity<LeagueTeam>()
             .HasOne(bc => bc.Team)
             .WithMany(c => c.LeagueTeams)
             .HasForeignKey(bc => bc.TeamId);


        modelBuilder.Entity<LeagueTeamPlayer>()
            .HasKey(bc => new { bc.LeagueId, bc.TeamId, bc.PlayerId });

        modelBuilder.Entity<LeagueTeamPlayer>()
             .HasOne(bc => bc.Team)
             .WithMany(c => c.LeagueTeamPlayers)
             .HasForeignKey(bc => bc.TeamId)
             .OnDelete(DeleteBehavior.NoAction);

         modelBuilder.Entity<LeagueTeamPlayer>()
             .HasOne(bc => bc.Player)
             .WithMany(c => c.LeagueTeamPlayers)
             .HasForeignKey(bc => bc.PlayerId)
             .OnDelete(DeleteBehavior.NoAction); 

         modelBuilder.Entity<LeagueTeamPlayer>()
             .HasOne(bc => bc.League)
             .WithMany(c => c.LeagueTeamPlayers)
             .HasForeignKey(bc => bc.LeagueId)
             .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<RequestRegistration>()
                .HasKey(bc => new { bc.Id });

        }
   }
}