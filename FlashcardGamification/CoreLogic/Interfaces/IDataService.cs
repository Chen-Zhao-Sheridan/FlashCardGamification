using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashcardGamification.CoreLogic.Models;

namespace FlashcardGamification.CoreLogic.Interfaces
{
    /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/IDataService"/>
    public interface IDataService
    {
        // Deck Operations
        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/IDataService_GetAllDecksAsync"/>
        Task<IEnumerable<Deck>> GetAllDecksAsync();


        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/IDataService_GetDeckAsync"/>
        Task<Deck> GetDeckAsync(Guid deckId);


        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/IDataService_SaveDeckAsync"/>
        Task SaveDeckAsync(Deck deck);


        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/IDataService_DeleteDeckAsync"/>
        Task DeleteDeckAsync(Guid deckId);

        // Deck-Card Operations
        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/IDataService_AddCardToDeckAsync"/>
        Task AddCardToDeckAsync(Guid deckId, Card card);


        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/IDataService_UpdateCardInDeckAsync"/>
        Task UpdateCardInDeckAsync(Guid deckId, Card card);


        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/IDataService_DeleteCardFromDeckAsync"/>
        Task DeleteCardFromDeckAsync(Guid deckId, Guid cardId);

        // User Operations
        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/IDataService_GetUserAsync"/>
        Task<User> GetUserAsync();


        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/IDataService_SaveUserAsync"/>
        Task SaveUserAsync(User user);
    }
}
