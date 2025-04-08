using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashcardGamification.CoreLogic.Models
{
    public class User
    {
        public Guid UserId { get; set; }

        // Gamification Stats
        public int XP { get; set; }
        public int Level { get; set; } 
        public int CurrentDailyStreak { get; set; }
        public int LongestDailyStreak { get; set; }

        // Progress Tracking
        public int TotalCorrectAnswers { get; set; }
        public int TotalSessionsCompleted { get; set; }
        public DateTime LastSessionDate { get; set; }

        public User()
        {
            UserId = Guid.Empty; 
            XP = 0;
            Level = 1; 
            CurrentDailyStreak = 0;
            LongestDailyStreak = 0;
            TotalCorrectAnswers = 0;
            TotalSessionsCompleted = 0;
            LastSessionDate = DateTime.MinValue;
        }
    }
}
