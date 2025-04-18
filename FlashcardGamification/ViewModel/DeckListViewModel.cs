using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlashcardGamification.CoreLogic.Interfaces;
using FlashcardGamification.CoreLogic.Models;
using FlashcardGamification.Views;

namespace FlashcardGamification.ViewModel
{
    /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/DeckListViewModel"/>
    public partial class DeckListViewModel : BaseViewModel // Or ObservableObject
    {
        private readonly IDataService _dataService;

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/DeckListViewModel_Decks"/>
        [ObservableProperty]
        ObservableCollection<Deck> decks;

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/DeckListViewModel_IsLoading"/>
        [ObservableProperty]
        bool isLoading;

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/DeckListViewModel_IsNotLoading"/>
        public bool IsNotLoading => !IsBusy;

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/DeckListViewModel_ctor"/>
        public DeckListViewModel(IDataService dataService)
        {
            _dataService = dataService;
            Title = "Flashcard Decks";
            Decks = new ObservableCollection<Deck>();
        }

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/DeckListViewModel_LoadDecksAsync"/>
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

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/DeckListViewModel_GoToCreateDeckAsync"/>
        [RelayCommand]
        async Task GoToCreateDeckAsync()
        {
            // Navigate to the DeckEditPage for creating a new deck
            await Shell.Current.GoToAsync($"{nameof(DeckDetailPage)}?DeckId=");
        }

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/DeckListViewModel_GoToDeckDetailsAsync"/>
        [RelayCommand]
        async Task GoToDeckDetailsAsync(Deck deck)
        {
            if (deck == null) return;

            // Navigate to CardListPage, passing the DeckId
            await Shell.Current.GoToAsync($"{nameof(DeckDetailPage)}?DeckId={deck.Id}");
        }

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/DeckListViewModel_StartReviewAsync"/>
        [RelayCommand]
        async Task StartReviewAsync(Deck deck)
        {
            await Shell.Current.GoToAsync($"{nameof(DeckReviewPage)}?DeckId={deck.Id}");
        }


        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/DeckListViewModel_DeleteDeckAsync"/>
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


        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/DeckListViewModel_OnAppearing"/>
        public void OnAppearing()
        {
            if (!IsLoading)
            {
                LoadDecksCommand.Execute(null);
            }
        }
    }
}
