using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Darts.Domain.DomainObjects
{
   public class PremierLeagueMatch
   {
      public long Id { get; set; }
      public long PremierLeagueId { get; set; }
      public long? HomePlayerId { get; set; }
      public long? GuestPlayerId { get; set; }
      public decimal HomeLegs { get; set; }
      public decimal HomeAverage { get; set; }
      public decimal GuestLegs { get; set; }
      public decimal GuestAverage { get; set; }
      public DateTimeOffset Date { get; set; }
      public Player HomePlayer { get; set; }
      public Player GuestPlayer { get; set; }
      public PremierLeague PremierLeague { get; set; }

      [NotMapped]
      public bool HomeWin => Win(HomePlayerId);
      [NotMapped]
      public string Result => $"{(int)HomeLegs} : {(int)GuestLegs}";
      [NotMapped]
      public bool IsComplete => HomePlayerId.HasValue && GuestPlayerId.HasValue;
      [NotMapped]
      public bool IsEmpty => !HomePlayerId.HasValue && !GuestPlayerId.HasValue;
      [NotMapped]
      public string Formatted
      {
         get
         {
            var home = HomePlayer != null ? HomePlayer.FullName : "Bye";
            var guest = GuestPlayer != null ? GuestPlayer.FullName : "Bye";

            return $"{home} : {guest}";
         }
      }

      public bool Win(long? playerId)
      {
         bool result = false;
         if (playerId.HasValue)
         {
            result = ResolveMatchResult(playerId.Value) == MatchResult.Win;
         }
         return result;
      }

      public MatchResult ResolveMatchResult(long playerId)
      {
         var result = HomePlayerId == playerId
            ? ResolveInternal(HomeLegs, GuestLegs)
            : ResolveInternal(GuestLegs, HomeLegs);

         return result;
      }

      private MatchResult ResolveInternal(decimal? homePoints, decimal? guestPoints)
      {
         MatchResult result = MatchResult.Empty;

         if (homePoints.HasValue && guestPoints.HasValue)
         {
            if (homePoints > guestPoints)
            {
               result = MatchResult.Win;
            }
            else if (homePoints < guestPoints)
            {
               result = MatchResult.Loss;
            }
            else
            {
               if (homePoints > 0 || guestPoints > 0)
               {
                  result = MatchResult.Draw;
               }
            }
         }
         else
         {
            if (homePoints.HasValue)
            {
               result = MatchResult.Win;
            }
            else if (guestPoints.HasValue)
            {
               result = MatchResult.Loss;
            }
         }

         return result;
      }
   }
}
