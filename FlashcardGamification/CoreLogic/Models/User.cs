using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashcardGamification.CoreLogic.Models
{
    /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/User"/>
    public class User
    {
        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/User_UserId"/>
        public Guid UserId { get; set; }

        // Gamification Stats
        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/User_XP"/>
        public int XP { get; set; }
        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/User_Level"/>
        public int Level { get; set; }
        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/User_CurrentDailyStreak"/>
        public int CurrentDailyStreak { get; set; }
        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/User_LongestDailyStreak"/>
        public int LongestDailyStreak { get; set; }

        // Progress Tracking
        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/User_TotalCorrectAnswers"/>
        public int TotalCorrectAnswers { get; set; }
        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/User_TotalSessionsCompleted"/>
        public int TotalSessionsCompleted { get; set; }
        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/User_LastSessionDate"/>
        public DateTime LastSessionDate { get; set; }

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/User_ctor"/>
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
