using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlashcardGamification.CoreLogic.Interfaces;
using FlashcardGamification.CoreLogic.Models;

namespace FlashcardGamification.ViewModel
{
    [QueryProperty(nameof(DeckId), "DeckId")]
    [QueryProperty(nameof(CardId), "CardId")] // Optional CardId for editing
    public partial class CardEditViewModel : BaseViewModel
    {
        private readonly IDataService _dataService;
        private Card _originalCard; // Card being edited

        [ObservableProperty]
        Guid deckId; // Must be provided

        [ObservableProperty]
        Guid cardId; // Provided if editing

        [ObservableProperty]
        string cardFront;

        [ObservableProperty]
        string cardBack;

        [ObservableProperty]
        bool isEditing;

        public CardEditViewModel(IDataService dataService)
        {
            _dataService = dataService;
            Title = "New Card";
        }

        async partial void OnCardIdChanged(Guid value)
        {
            if (value != Guid.Empty)
            {
                IsEditing = true;
                Title = "Edit Card";
                await LoadCardAsync(value);
            }
            else
            {
                IsEditing = false;
                Title = "New Card";
                CardFront = string.Empty;
                CardBack = string.Empty;
                _originalCard = null;
            }
        }

        partial void OnDeckIdChanged(Guid value)
        {
            if (value == Guid.Empty)
            {
                Console.WriteLine("Error: DeckId not provided to CardEditViewModel");
            }
        }

        private async Task LoadCardAsync(Guid id)
        {
            if (DeckId == Guid.Empty || IsBusy) return; 
            IsBusy = true;
            try
            {
                // Need to load the deck to find the card
                var deck = await _dataService.GetDeckAsync(DeckId);
                _originalCard = deck?.Cards.FirstOrDefault(c => c.Id == id);

                if (_originalCard != null)
                {
                    CardFront = _originalCard.FrontContent;
                    CardBack = _originalCard.BackContent;
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Could not load card details.", "OK");
                    await Shell.Current.GoToAsync("..");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load card: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "Failed to load card.", "OK");
                await Shell.Current.GoToAsync("..");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        async Task SaveCardAsync()
        {
            if (DeckId == Guid.Empty)
            {
                await Shell.Current.DisplayAlert("Error", "Cannot save card without a Deck ID.", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(CardFront) || string.IsNullOrWhiteSpace(CardBack))
            {
                await Shell.Current.DisplayAlert("Validation Error", "Both Front and Back text are required.", "OK");
                return;
            }

            if (IsBusy) return;
            IsBusy = true;

            try
            {
                if (IsEditing && _originalCard != null)
                {
                    // Update existing card 
                    _originalCard.FrontContent = CardFront;
                    _originalCard.BackContent = CardBack;
                    await _dataService.UpdateCardInDeckAsync(DeckId, _originalCard);
                }
                else
                {
                    // Create new card
                    var newCard = new Card
                    {
                        FrontContent = CardFront,
                        BackContent = CardBack
                    };
                    await _dataService.AddCardToDeckAsync(DeckId, newCard);
                }
                await Shell.Current.GoToAsync(".."); // Navigate back
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving card: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "Failed to save card.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        async Task CancelAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
