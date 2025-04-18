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
    /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/CardEditViewModel"/>
    [QueryProperty(nameof(DeckIdString), "DeckId")]
    [QueryProperty(nameof(CardIdString), "CardId")]
    public partial class CardEditViewModel : BaseViewModel
    {
        private readonly IDataService _dataService;
        private Card _originalCard; 
        private Guid _deckId;
        private Guid _cardId;

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/CardEditViewModel_DeckIdString"/>
        [ObservableProperty]
        string deckIdString;

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/CardEditViewModel_CardIdString"/>
        [ObservableProperty]
        string cardIdString;

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/CardEditViewModel_CardFront"/>
        [ObservableProperty]
        string cardFront;

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/CardEditViewModel_CardBack"/>
        [ObservableProperty]
        string cardBack;

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/CardEditViewModel_IsEditing"/>
        [ObservableProperty]
        bool isEditing;

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/CardEditViewModel_ctor"/>
        public CardEditViewModel(IDataService dataService)
        {
            _dataService = dataService;
            Title = "New Card";
        }

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/CardEditViewModel_OnDeckIdStringChanged"/>
        partial void OnDeckIdStringChanged(string value)
        {
            if (!Guid.TryParse(value, out _deckId) || _deckId == Guid.Empty)
            {
                Console.WriteLine($"Error: Invalid or missing DeckId passed to CardEditViewModel: {value}");
                Shell.Current.DisplayAlert("Error", "Invalid Deck ID received.", "OK");
                Shell.Current.GoToAsync("..");
            }

            else if (!string.IsNullOrEmpty(CardIdString))
            {
                CheckAndLoadCard();
            }
        }

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/CardEditViewModel_OnCardIdStringChanged"/>
        partial void OnCardIdStringChanged(string value)
        {
            IsEditing = Guid.TryParse(value, out _cardId) && _cardId != Guid.Empty;

            if (IsEditing)
            {
                Title = "Edit Card";
                if (_deckId != Guid.Empty)
                {
                    CheckAndLoadCard();
                }
            }
            else
            {
                Title = "New Card";
                CardFront = string.Empty;
                CardBack = string.Empty;
                _originalCard = null;
                _cardId = Guid.Empty; 
            }
        }

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/CardEditViewModel_CheckAndLoadCard"/>
        private async void CheckAndLoadCard()
        {
            if (IsEditing && _deckId != Guid.Empty && _cardId != Guid.Empty)
            {
                await LoadCardAsync(_deckId, _cardId);
            }
        }

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/CardEditViewModel_LoadCardAsync"/>
        private async Task LoadCardAsync(Guid deckId, Guid cardId)
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var deck = await _dataService.GetDeckAsync(deckId);
                _originalCard = deck?.Cards.FirstOrDefault(c => c.Id == cardId);

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

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/CardEditViewModel_SaveCardAsync"/>
        [RelayCommand]
        async Task SaveCardAsync()
        {
            if (_deckId == Guid.Empty)
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
                    _originalCard.FrontContent = CardFront;
                    _originalCard.BackContent = CardBack;
                    await _dataService.UpdateCardInDeckAsync(_deckId, _originalCard);
                }
                else
                {
                    var newCard = new Card
                    {
                        FrontContent = CardFront,
                        BackContent = CardBack
                    };
                    await _dataService.AddCardToDeckAsync(_deckId, newCard);
                }
                await Shell.Current.GoToAsync("..");
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

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/CardEditViewModel_CancelAsync"/>
        [RelayCommand]
        async Task CancelAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
