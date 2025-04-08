using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlashcardGamification.CoreLogic.Interfaces;
using FlashcardGamification.CoreLogic.Models;
using FlashcardGamification.Views;

namespace FlashcardGamification.ViewModel
{
    public partial class DeckListViewModel : BaseViewModel // Or ObservableObject
    {
        private readonly IDataService _dataService;

        [ObservableProperty]
        ObservableCollection<Deck> decks;

        [ObservableProperty]
        bool isLoading; 

        public DeckListViewModel(IDataService dataService)
        {
            _dataService = dataService;
            Title = "Flashcard Decks";
            Decks = new ObservableCollection<Deck>();
        }

        [RelayCommand]
        async Task LoadDecksAsync()
        {
            if (IsLoading) return;

            IsLoading = true;
            try
            {
                Decks.Clear();
                var loadedDecks = await _dataService.GetAllDecksAsync();
                foreach (var deck in loadedDecks)
                {
                    Decks.Add(deck);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading decks: {ex.Message}");
                // TODO: Display error message to user
                await Shell.Current.DisplayAlert("Error", "Failed to load decks.", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        async Task GoToCreateDeckAsync()
        {
            // Navigate to the DeckEditPage for creating a new deck
            await Shell.Current.GoToAsync(nameof(DeckEditPage));
        }

        [RelayCommand]
        async Task GoToDeckDetailsAsync(Deck deck)
        {
            if (deck == null) return;

            // Navigate to CardListPage, passing the DeckId
            await Shell.Current.GoToAsync($"{nameof(CardListPage)}?DeckId={deck.Id}");
        }

        [RelayCommand]
        async Task GoToEditDeckAsync(Deck deck)
        {
            if (deck == null) return;

            // Navigate to DeckEditPage, passing the DeckId for editing
            await Shell.Current.GoToAsync($"{nameof(DeckEditPage)}?DeckId={deck.Id}");
        }


        [RelayCommand]
        async Task DeleteDeckAsync(Deck deck)
        {
            if (deck == null) return;

            // Optional: Confirmation dialog
            bool confirm = await Shell.Current.DisplayAlert("Confirm Delete", $"Are you sure you want to delete the deck '{deck.Name}'?", "Yes", "No");
            if (!confirm) return;

            try
            {
                await _dataService.DeleteDeckAsync(deck.Id);
                Decks.Remove(deck); // Update the UI
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting deck: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "Failed to delete deck.", "OK");
            }
        }


        public void OnAppearing()
        {
            if (!IsLoading && (Decks == null || !Decks.Any()))
            {
                Task.Run(async () => await LoadDecksAsync());
            }
        }
    }
}
