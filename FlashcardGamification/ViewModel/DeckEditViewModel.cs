
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlashcardGamification.CoreLogic.Interfaces;
using FlashcardGamification.CoreLogic.Models;

namespace FlashcardGamification.ViewModel
{
    // This attribute links the "DeckId" query parameter from navigation
    // to the DeckId property in this ViewModel.
    [QueryProperty(nameof(DeckId), "DeckId")]
    public partial class DeckEditViewModel : BaseViewModel
    {
        private readonly IDataService _dataService;
        private Deck _originalDeck; // To hold the deck being edited

        [ObservableProperty]
        string deckName;

        [ObservableProperty]
        Guid deckId; // Will be set automaticaly if editing

        [ObservableProperty]
        bool isEditing; // To adjust UI/Title

        public DeckEditViewModel(IDataService dataService)
        {
            _dataService = dataService;
            Title = "New Deck"; // Default title
        }

        // This method is automatically called when the DeckId property changes
        async partial void OnDeckIdChanged(Guid value)
        {
            if (value != Guid.Empty)
            {
                IsEditing = true;
                Title = "Edit Deck";
                await LoadDeckAsync(value);
            }
            else
            {
                IsEditing = false;
                Title = "New Deck";
                DeckName = string.Empty; // Reset name for new deck
                _originalDeck = null;
            }
        }

        private async Task LoadDeckAsync(Guid id)
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                _originalDeck = await _dataService.GetDeckAsync(id);
                if (_originalDeck != null)
                {
                    DeckName = _originalDeck.Name;
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Could not load deck details.", "OK");
                    await Shell.Current.GoToAsync(".."); // Go back if deck not found
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
                IsBusy = false;
            }
        }

        [RelayCommand]
        async Task SaveDeckAsync()
        {
            if (string.IsNullOrWhiteSpace(DeckName))
            {
                await Shell.Current.DisplayAlert("Validation Error", "Deck name cannot be empty.", "OK");
                return;
            }

            if (IsBusy) return;
            IsBusy = true;

            try
            {
                Deck deckToSave;
                if (IsEditing && _originalDeck != null)
                {
                    // Update existing deck
                    _originalDeck.Name = DeckName;
                    // LastModifiedDate updated in SaveDeckAsync service method
                    deckToSave = _originalDeck;
                }
                else
                {
                    // Create new deck
                    deckToSave = new Deck
                    {
                        Name = DeckName
                        // ID, Dates, and empty Cards list set by Deck constructor/SaveDeckAsync
                    };
                }

                await _dataService.SaveDeckAsync(deckToSave);
                await Shell.Current.GoToAsync(".."); // Navigate back after saving
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving deck: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "Failed to save deck.", "OK");
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
