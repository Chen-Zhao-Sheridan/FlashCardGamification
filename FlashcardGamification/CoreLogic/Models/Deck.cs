using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashcardGamification.CoreLogic.Models
{
    public class Deck
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Card> Cards { get; set; }

        public Deck()
        {
            Id = Guid.NewGuid();
            Name = "New Deck";
            Cards = new List<Card>();
        }
    }
}
