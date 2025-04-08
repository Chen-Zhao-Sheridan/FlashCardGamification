using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashcardGamification.CoreLogic.Models;

namespace FlashcardGamification.CoreLogic.Interfaces
{
    public interface IDataService
    {
        // Deck Operations
        Task<IEnumerable<Deck>> GetAllDecksAsync();
        Task<Deck> GetDeckAsync(Guid deckId); 
        Task SaveDeckAsync(Deck deck);
        Task DeleteDeckAsync(Guid deckId);

        // Deck-Card Operations
        Task AddCardToDeckAsync(Guid deckId, Card card);
        Task UpdateCardInDeckAsync(Guid deckId, Card card);
        Task DeleteCardFromDeckAsync(Guid deckId, Guid cardId);

        // User Operations
        Task<User> GetUserAsync();
        Task SaveUserAsync(User user);
    }
}
