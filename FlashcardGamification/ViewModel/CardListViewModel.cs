
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlashcardGamification.CoreLogic.Interfaces;
using FlashcardGamification.CoreLogic.Models;
using FlashcardGamification.Views;

namespace FlashcardGamification.ViewModel
{
    [QueryProperty(nameof(DeckId), "DeckId")]
    public partial class CardListViewModel : BaseViewModel
    {
        private readonly IDataService _dataService;

        [ObservableProperty]
        Guid deckId; // Set via navigation

        [ObservableProperty]
        Deck currentDeck; 

        [ObservableProperty]
        ObservableCollection<Card> cards;

        [ObservableProperty]
        bool isLoading;

        public CardListViewModel(IDataService dataService)
        {
            _dataService = dataService;
            Cards = new ObservableCollection<Card>();
            Title = "Cards"; // Default, will update
        }

        // Triggered when DeckId is set via navigation
        async partial void OnDeckIdChanged(Guid value)
        {
            if (value != Guid.Empty)
            {
                await LoadCardsAsync();
            }
        }

        [RelayCommand]
        async Task LoadCardsAsync()
        {
            if (DeckId == Guid.Empty || IsLoading) return;

            IsLoading = true;
            try
            {
                CurrentDeck = await _dataService.GetDeckAsync(DeckId);
                if (CurrentDeck != null)
                {
                    Title = $"Deck: {CurrentDeck.Name}"; // Update title
                    Cards.Clear();
                    foreach (var card in CurrentDeck.Cards.OrderBy(c => c.FrontContent))
                    {
                        Cards.Add(card);
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Could not load deck.", "OK");
                    await Shell.Current.GoToAsync(".."); // Go back if deck not found
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading cards: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "Failed to load cards.", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        async Task GoToCreateCardAsync()
        {
            if (CurrentDeck == null) return;
            // Navigate to CardEditPage, passing the DeckId
            await Shell.Current.GoToAsync($"{nameof(CardEditPage)}?DeckId={CurrentDeck.Id}");
        }

        [RelayCommand]
        async Task GoToEditCardAsync(Card card)
        {
            if (card == null || CurrentDeck == null) return;
            await Shell.Current.GoToAsync($"{nameof(CardEditPage)}?DeckId={CurrentDeck.Id}&CardId={card.Id}");
        }

        [RelayCommand]
        async Task DeleteCardAsync(Card card)
        {
            if (card == null || CurrentDeck == null) return;

            bool confirm = await Shell.Current.DisplayAlert("Confirm Delete", $"Delete card starting with '{card.FrontContent.Substring(0, Math.Min(card.FrontContent.Length, 20))}...'?", "Yes", "No");
            if (!confirm) return;

            try
            {
                await _dataService.DeleteCardFromDeckAsync(CurrentDeck.Id, card.Id);
                Cards.Remove(card);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting card: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "Failed to delete card.", "OK");
            }
        }

        [RelayCommand]
        async Task StartReviewAsync()
        {
            if (CurrentDeck == null || !Cards.Any())
            {
                await Shell.Current.DisplayAlert("Cannot Start", "There are no cards in this deck to review.", "OK");
                return;
            }
            //await Shell.Current.GoToAsync($"{nameof(ReviewPage)}?DeckId={CurrentDeck.Id}");
        }

        public void OnAppearing()
        {
            if (CurrentDeck != null && !IsLoading)
            {
                Task.Run(async () => await LoadCardsAsync()); // Refresh the list
            }
        }
    }
}
