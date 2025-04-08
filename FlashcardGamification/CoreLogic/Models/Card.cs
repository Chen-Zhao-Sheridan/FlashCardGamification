using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashcardGamification.CoreLogic.Models
{
    public class Card
    {
        public Guid Id { get; set; }
        public string FrontContent { get; set; }
        public string BackContent { get; set; }
        public DateTime LastReviewed { get; set; }
        public int CorrectStreak { get; set; }

        public Card()
        {
            Id = Guid.NewGuid(); // Ensure new cards get an ID
            LastReviewed = DateTime.MinValue; // Default value
            FrontContent = "";
            BackContent = "";
        }
    }
}
