using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlashcardGamification.CoreLogic.Interfaces;
using FlashcardGamification.CoreLogic.Models;
using FlashcardGamification.Views;


namespace FlashcardGamification.ViewModel
{
    [QueryProperty(nameof(ScoreString), "Score")]
    [QueryProperty(nameof(TotalReviewedString), "TotalReviewed")]
    [QueryProperty(nameof(DeckName), "DeckName")]
    [QueryProperty(nameof(DeckIdString), "DeckId")] 
    public partial class ResultsViewModel : BaseViewModel
    {
        [ObservableProperty]
        string scoreString;

        [ObservableProperty]
        string totalReviewedString;

        [ObservableProperty]
        string deckName;

        [ObservableProperty]
        string deckIdString; 

        [ObservableProperty]
        int score;

        [ObservableProperty]
        int totalReviewed;

        private Guid _deckId;

        [ObservableProperty]
        string resultSummary;

        public ResultsViewModel()
        {
            Title = "Session Results";
        }

        partial void OnScoreStringChanged(string value)
        {
            if (int.TryParse(value, out int parsedScore)) Score = parsedScore;
            else Score = 0;
            UpdateSummary();
        }

        partial void OnTotalReviewedStringChanged(string value)
        {
            if (int.TryParse(value, out int parsedTotal)) TotalReviewed = parsedTotal;
            else TotalReviewed = 0;
            UpdateSummary();
        }

        partial void OnDeckNameChanged(string value) => UpdateSummary();

        partial void OnDeckIdStringChanged(string value)
        {
            // Parse the received DeckId string
            if (Guid.TryParse(value, out Guid parsedId))
            {
                _deckId = parsedId;
            }
            else
            {
                _deckId = Guid.Empty;
                Console.WriteLine($"Warning: Invalid DeckId received on ResultsPage: {value}");
            }
            ReviewAgainCommand.NotifyCanExecuteChanged();
        }

        private void UpdateSummary()
        {
            if (TotalReviewed > 0)
            {
                double percentage = (double)Score / TotalReviewed * 100;
                ResultSummary = $"Deck: {DeckName}\nScore: {Score} / {TotalReviewed} ({percentage:F0}%)";
            }
            else
            {
                ResultSummary = $"Deck: {DeckName}\nNo cards were reviewed.";
            }
        }

        // Use CanExecute to enable/disable the button based on valid DeckId
        [RelayCommand(CanExecute = nameof(CanReviewAgain))]
        async Task ReviewAgainAsync()
        {
            // Navigate back to the ReviewPage for the same deck
            // Use ".." to go up one level from the root navigation we used to get here
            await Shell.Current.GoToAsync($"..?DeckId={_deckId}");
        }

        private bool CanReviewAgain()
        {
            // Only allow review again if we have a valid DeckId
            return _deckId != Guid.Empty;
        }

        [RelayCommand]
        async Task GoToDeckListAsync()
        {
            // Go back to the root Deck List page
            await Shell.Current.GoToAsync($"//{nameof(DeckListPage)}");
        }
    }
}
