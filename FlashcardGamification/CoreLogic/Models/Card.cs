using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashcardGamification.CoreLogic.Models
{
    /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/Card"/>
    public class Card
    {
        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/Card_Id"/>
        public Guid Id { get; set; }
        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/Card_FrontContent"/>
        public string FrontContent { get; set; }
        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/Card_BackContent"/>
        public string BackContent { get; set; }
        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/Card_LastReviewed"/>
        public DateTime LastReviewed { get; set; }
        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/Card_CorrectStreak"/>
        public int CorrectStreak { get; set; }

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/Card_ctor"/>
        public Card()
        {
            Id = Guid.NewGuid(); // Ensure new cards get an ID
            LastReviewed = DateTime.MinValue; // Default value
            FrontContent = "";
            BackContent = "";
        }
    }
}
