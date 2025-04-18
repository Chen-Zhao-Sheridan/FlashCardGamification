using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashcardGamification.CoreLogic.Models
{
    /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/Deck"/>
    public class Deck
    {
        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/Deck_Id"/>
        public Guid Id { get; set; }
        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/Deck_Name"/>
        public string Name { get; set; }
        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/Deck_Cards"/>
        public List<Card> Cards { get; set; }

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/Deck_ctor"/>
        public Deck()
        {
            Id = Guid.NewGuid();
            Name = "New Deck";
            Cards = new List<Card>();
        }
    }
}
