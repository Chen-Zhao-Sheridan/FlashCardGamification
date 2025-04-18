using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlashcardGamification.CoreLogic.Interfaces;
using FlashcardGamification.CoreLogic.Models;
using FlashcardGamification.Views;


namespace FlashcardGamification.ViewModel
{
    /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/ResultsViewModel"/>
    [QueryProperty(nameof(ScoreString), "Score")]
    [QueryProperty(nameof(TotalReviewedString), "TotalReviewed")]
    [QueryProperty(nameof(DeckName), "DeckName")]
    [QueryProperty(nameof(DeckIdString), "DeckId")] 
    public partial class ResultsViewModel : BaseViewModel
    {
        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/ResultsViewModel_ScoreString"/>
        [ObservableProperty]
        string scoreString;

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/ResultsViewModel_TotalReviewedString"/>
        [ObservableProperty]
        string totalReviewedString;

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/ResultsViewModel_DeckName"/>
        [ObservableProperty]
        string deckName;

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/ResultsViewModel_DeckIdString"/>
        [ObservableProperty]
        string deckIdString;

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/ResultsViewModel_Score"/>
        [ObservableProperty]
        int score;

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/ResultsViewModel_TotalReviewed"/>
        [ObservableProperty]
        int totalReviewed;

        private Guid _deckId;

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/ResultsViewModel_ResultSummary"/>
        [ObservableProperty]
        string resultSummary;

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/ResultsViewModel_ctor"/>
        public ResultsViewModel()
        {
            Title = "Session Results";
        }

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/ResultsViewModel_OnScoreStringChanged"/>
        partial void OnScoreStringChanged(string value)
        {
            if (int.TryParse(value, out int parsedScore)) Score = parsedScore;
            else Score = 0;
            UpdateSummary();
        }

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/ResultsViewModel_OnTotalReviewedStringChanged"/>
        partial void OnTotalReviewedStringChanged(string value)
        {
            if (int.TryParse(value, out int parsedTotal)) TotalReviewed = parsedTotal;
            else TotalReviewed = 0;
            UpdateSummary();
        }

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/ResultsViewModel_OnDeckNameChanged"/>
        partial void OnDeckNameChanged(string value) => UpdateSummary();

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/ResultsViewModel_OnDeckIdStringChanged"/>
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

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/ResultsViewModel_UpdateSummary"/>
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
        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/ResultsViewModel_ReviewAgainAsync"/>
        [RelayCommand(CanExecute = nameof(CanReviewAgain))]
        async Task ReviewAgainAsync()
        {
            // Navigate back to the ReviewPage for the same deck
            // Use ".." to go up one level from the root navigation we used to get here
            await Shell.Current.GoToAsync($"..?DeckId={_deckId}");
        }

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/ResultsViewModel_CanReviewAgain"/>
        private bool CanReviewAgain()
        {
            // Only allow review again if we have a valid DeckId
            return _deckId != Guid.Empty;
        }

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/ResultsViewModel_GoToDeckListAsync"/>
        [RelayCommand]
        async Task GoToDeckListAsync()
        {
            // Go back to the root Deck List page
            await Shell.Current.GoToAsync($"//{nameof(DeckListPage)}");
        }
    }
}
