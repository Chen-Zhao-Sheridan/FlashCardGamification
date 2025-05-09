﻿
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlashcardGamification.CoreLogic.Interfaces;
using FlashcardGamification.CoreLogic.Models;
using FlashcardGamification.Views;

namespace FlashcardGamification.ViewModel
{
    // Update QueryProperty target name if needed (was DeckIdString)
    /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/DeckDetailViewModel"/>
    [QueryProperty(nameof(DeckIdString), "DeckId")]
    public partial class DeckDetailViewModel : BaseViewModel // Renamed class
    {
        private readonly IDataService _dataService;
        private Deck _currentDeck; // Holds the full deck including cards
        private Guid _deckId;

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/DeckDetailViewModel_DeckIdString"/>
        [ObservableProperty]
        string deckIdString;

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/DeckDetailViewModel_DeckName"/>
        [ObservableProperty]
        string deckName;

        // Add collection for cards
        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/DeckDetailViewModel_Cards"/>
        [ObservableProperty]
        ObservableCollection<Card> cards;

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/DeckDetailViewModel_IsEditing"/>
        [ObservableProperty]
        bool isEditing;

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/DeckDetailViewModel_IsLoading"/>
        [ObservableProperty]
        bool isLoading;

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/DeckDetailViewModel_IsNotLoading"/>
        public bool IsNotLoading => !IsBusy;

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/DeckDetailViewModel_ctor"/>
        public DeckDetailViewModel(IDataService dataService) 
        {
            _dataService = dataService;
            Cards = new ObservableCollection<Card>();
            Title = "Deck Details"; 
        }

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/DeckDetailViewModel_OnDeckIdStringChanged"/>
        async partial void OnDeckIdStringChanged(string value)
        {
            IsEditing = Guid.TryParse(value, out _deckId) && value != null && value != "";

            if (IsEditing) // Existing deck
            {
                Title = "Edit Deck"; 
                await LoadDeckAsync(_deckId);
            }
            else // New deck
            {
                Title = "New Deck";
                DeckName = string.Empty;
                Cards.Clear(); // Ensure card list is empty for new deck
                _currentDeck = new Deck(); // Create a placeholder new deck
                _deckId = _currentDeck.Id; // Assign ID for potential card adds before first save
                IsLoading = false; 
            }
        }

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/DeckDetailViewModel_LoadDeckAsync"/>
        private async Task LoadDeckAsync(Guid id)
        {
            if (IsLoading || id == Guid.Empty) return;
            IsLoading = true;
            Cards.Clear(); // Clear previous cards before loading
            try
            {
                _currentDeck = await _dataService.GetDeckAsync(id);
                if (_currentDeck != null)
                {
                    DeckName = _currentDeck.Name;
                    // Load cards into the observable collection
                    foreach (var card in _currentDeck.Cards.OrderBy(c => c.FrontContent)) 
                    {
                        Cards.Add(card);
                    }
                    Title = $"Deck: {DeckName}";
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Could not load deck details.", "OK");
                    await Shell.Current.GoToAsync("..");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load deck: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "Failed to load deck.", "OK");
                await Shell.Current.GoToAsync("..");
            }
            finally
            {
                IsLoading = false;
            }
        }


        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/DeckDetailViewModel_SaveDeckAsync"/>
        [RelayCommand]
        async Task SaveDeckAsync()
        {
            if (string.IsNullOrWhiteSpace(DeckName))
            {
                await Shell.Current.DisplayAlert("Validation Error", "Deck name cannot be empty.", "OK");
                return;
            }
            if (_currentDeck == null) return; // Should have a deck object by now

            if (IsLoading) return; 
            IsLoading = true;

            try
            {
                // Update the name on the current deck object
                _currentDeck.Name = DeckName;

                await _dataService.SaveDeckAsync(_currentDeck);
                await Shell.Current.DisplayAlert("Saved", "Deck details saved.", "OK");
                Title = $"Deck: {DeckName}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving deck: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "Failed to save deck.", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/DeckDetailViewModel_CancelAsync"/>
        [RelayCommand]
        async Task CancelAsync() 
        {
            await Shell.Current.GoToAsync("..");
        }

        // Card Management

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/DeckDetailViewModel_GoToAddCardAsync"/>
        [RelayCommand]
        async Task GoToAddCardAsync()
        {
            if (_currentDeck == null) return;
            await Shell.Current.GoToAsync($"{nameof(CardEditPage)}?DeckId={_deckId}");
        }

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/DeckDetailViewModel_GoToEditCardAsync"/>
        [RelayCommand]
        async Task GoToEditCardAsync(Card card)
        {
            if (card == null || _currentDeck == null) return;
            await Shell.Current.GoToAsync($"{nameof(CardEditPage)}?DeckId={_deckId}&CardId={card.Id}");
        }

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/DeckDetailViewModel_DeleteCardAsync"/>
        [RelayCommand]
        async Task DeleteCardAsync(Card card)
        {
            if (card == null || _currentDeck == null) return;

            bool confirm = await Shell.Current.DisplayAlert("Confirm Delete", $"Delete card starting with '" +
                $"{card.FrontContent.Substring(0, Math.Min(card.FrontContent.Length, 20))}...'?", "Yes", "No");
            if (!confirm) return;

            if (IsLoading) return;
            IsLoading = true; 

            try
            {
                await _dataService.DeleteCardFromDeckAsync(_deckId, card.Id);
                Cards.Remove(card);
                _currentDeck.Cards.RemoveAll(c => c.Id == card.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting card: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "Failed to delete card.", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/DeckDetailViewModel_OnAppearing"/>
        public async void OnAppearing()
        {

            if (_deckId != Guid.Empty && !IsLoading && DeckIdString != "")
            {
                Console.WriteLine("DeckDetailViewModel OnAppearing - Refreshing Deck");
                await LoadDeckAsync(_deckId);
            }

        }
    }
}
