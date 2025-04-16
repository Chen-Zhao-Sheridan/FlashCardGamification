using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FlashcardGamification.CoreLogic.Interfaces;
using FlashcardGamification.CoreLogic.Models;

namespace FlashcardGamification.CoreLogic
{
    public class FileSystemDataService : IDataService
    {
        private readonly string _dataDirectory;
        private readonly JsonSerializerOptions _serializerOptions;
        private const string DeckFilePrefix = "deck_";
        private const string DeckFileExtension = ".json";
        private readonly string _userProfileFile;

        public FileSystemDataService()
        {
            _dataDirectory = FileSystem.AppDataDirectory;
            _userProfileFile = Path.Combine(_dataDirectory, "user_profile.json"); 
            _serializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };

            if (!Directory.Exists(_dataDirectory))
            {
                Directory.CreateDirectory(_dataDirectory);
            }
        }

        private string GetDeckFilePath(Guid deckId)
        {
            return Path.Combine(_dataDirectory, $"{DeckFilePrefix}{deckId}{DeckFileExtension}");
        }

        // User Operations

        public async Task<User> GetUserAsync()
        {
            if (!File.Exists(_userProfileFile))
            {
                Console.WriteLine("User profile not found. Creating a new one.");
                var newUser = new User
                {
                    UserId = Guid.NewGuid() 
                };
                await SaveUserAsync(newUser);
                return newUser;
            }

            try
            {
                string json = await File.ReadAllTextAsync(_userProfileFile);
                if (string.IsNullOrWhiteSpace(json))
                {
                    Console.WriteLine("User profile file is empty. Creating a new one.");
                    var newUser = new User { UserId = Guid.NewGuid() };
                    await SaveUserAsync(newUser);
                    return newUser;
                }
                var user = JsonSerializer.Deserialize<User>(json, _serializerOptions);

                if (user == null)
                {
                    Console.WriteLine("Warning: Failed to deserialize user profile. Creating a new one.");
                    user = new User { UserId = Guid.NewGuid() };
                    await SaveUserAsync(user);
                }
                else if (user.UserId == Guid.Empty)
                {
                    Console.WriteLine("Warning: Loaded user profile has empty UserId. Assigning a new one.");
                    user.UserId = Guid.NewGuid();
                    await SaveUserAsync(user); 
                }

                return user;
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"Error deserializing user profile: {jsonEx.Message}. Creating a new default profile.");
                var newUser = new User { UserId = Guid.NewGuid() };
                await SaveUserAsync(newUser); 
                return newUser;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading user profile file {_userProfileFile}: {ex.Message}. Returning a default profile.");
                return new User { UserId = Guid.NewGuid() };
            }
        }

        public async Task SaveUserAsync(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (user.UserId == Guid.Empty)
            {
                Console.WriteLine("Warning: Saving user profile with empty UserId. Assigning a new one.");
                user.UserId = Guid.NewGuid();
            }

            string json = JsonSerializer.Serialize(user, _serializerOptions);
            try
            {
                await File.WriteAllTextAsync(_userProfileFile, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing user profile file {_userProfileFile}: {ex.Message}");
                throw; 
            }
        }


        // Deck Operations

        public async Task<IEnumerable<Deck>> GetAllDecksAsync()
        {
            var deckFiles = Directory.EnumerateFiles(_dataDirectory, $"{DeckFilePrefix}*{DeckFileExtension}");
            var decks = new List<Deck>();

            foreach (var filePath in deckFiles)
            {
                try
                {
                    string json = await File.ReadAllTextAsync(filePath);
                    var deck = JsonSerializer.Deserialize<Deck>(json, _serializerOptions);
                    if (deck != null)
                    {
                        decks.Add(deck);
                    }
                    else
                    {
                        Console.WriteLine($"Warning: Failed to deserialize deck file (or file was empty): {filePath}");
                    }
                }
                catch (JsonException jsonEx)
                {
                    Console.WriteLine($"Error deserializing deck file {filePath}: {jsonEx.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading deck file {filePath}: {ex.Message}");
                }
            }
            return decks.OrderBy(d => d.Name); // Example sorting
        }

        public async Task<Deck> GetDeckAsync(Guid deckId)
        {
            string filePath = GetDeckFilePath(deckId);
            if (!File.Exists(filePath)) return null;
            try
            {
                string json = await File.ReadAllTextAsync(filePath);
                return JsonSerializer.Deserialize<Deck>(json, _serializerOptions);
            }
            catch (Exception ex) { Console.WriteLine($"Error reading deck file {filePath}: {ex.Message}"); return null; }
        }

        public async Task SaveDeckAsync(Deck deck)
        {
            if (deck == null) throw new ArgumentNullException(nameof(deck));
            foreach (var card in deck.Cards.Where(c => c.Id == Guid.Empty)) card.Id = Guid.NewGuid();

            string filePath = GetDeckFilePath(deck.Id);
            string json = JsonSerializer.Serialize(deck, _serializerOptions);
            try
            {
                await File.WriteAllTextAsync(filePath, json);
            }
            catch (Exception ex) { Console.WriteLine($"Error writing deck file {filePath}: {ex.Message}"); throw; }
        }

        public Task DeleteDeckAsync(Guid deckId)
        {
            string filePath = GetDeckFilePath(deckId);
            try
            {
                if (File.Exists(filePath)) File.Delete(filePath);
                else Console.WriteLine($"Warning: Attempted to delete non-existent deck file: {filePath}");
            }
            catch (Exception ex) { Console.WriteLine($"Error deleting deck file {filePath}: {ex.Message}"); throw; }
            return Task.CompletedTask;
        }

        // Card Operations
        public async Task AddCardToDeckAsync(Guid deckId, Card card)
        {
            var deck = await GetDeckAsync(deckId);
            if (deck != null)
            {
                if (card.Id == Guid.Empty) card.Id = Guid.NewGuid();
                deck.Cards.Add(card);
                await SaveDeckAsync(deck); // Save the entire deck
            }
            else
            {
                Console.WriteLine($"Error adding card: Deck {deckId} not found.");
                throw new KeyNotFoundException($"Deck with ID {deckId} not found."); 
            }
        }

        public async Task UpdateCardInDeckAsync(Guid deckId, Card card)
        {
            var deck = await GetDeckAsync(deckId);
            if (deck != null)
            {
                var existingCard = deck.Cards.FirstOrDefault(c => c.Id == card.Id);
                if (existingCard != null)
                {
                    existingCard.FrontContent = card.FrontContent;
                    existingCard.BackContent = card.BackContent;
                    existingCard.LastReviewed = card.LastReviewed;
                    existingCard.CorrectStreak = card.CorrectStreak;
                    await SaveDeckAsync(deck);
                }
                else
                {
                    Console.WriteLine($"Error updating card: Card {card.Id} not found in Deck {deckId}.");
                    throw new KeyNotFoundException($"Card with ID {card.Id} not found in deck {deckId}.");
                }
            }
            else
            {
                Console.WriteLine($"Error updating card: Deck {deckId} not found.");
                throw new KeyNotFoundException($"Deck with ID {deckId} not found.");
            }
        }

        public async Task DeleteCardFromDeckAsync(Guid deckId, Guid cardId)
        {
            var deck = await GetDeckAsync(deckId);
            if (deck != null)
            {
                var cardToRemove = deck.Cards.FirstOrDefault(c => c.Id == cardId);
                if (cardToRemove != null)
                {
                    deck.Cards.Remove(cardToRemove);
                    await SaveDeckAsync(deck);
                }
            }
            else
            {
                Console.WriteLine($"Error deleting card: Deck {deckId} not found.");
                throw new KeyNotFoundException($"Deck with ID {deckId} not found.");
            }
        }
    }
}
