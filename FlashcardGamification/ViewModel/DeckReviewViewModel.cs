using System.ComponentModel.Design;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlashcardGamification.CoreLogic.Interfaces;
using FlashcardGamification.CoreLogic.Models;
using FlashcardGamification.Views;


namespace FlashcardGamification.ViewModel
{
    [QueryProperty(nameof(DeckIdString), "DeckId")]
    public partial class DeckReviewViewModel : BaseViewModel
    {
        private readonly IDataService _dataService;
        private List<Card> _sessionCards;
        private Deck _currentDeck;
        private User _currentUser;
        private int _currentCardIndex;
        private int _sessionCorrectAnswers;
        private int _sessionTotalReviewed;
        private Guid _deckId;

        [ObservableProperty]
        string deckIdString;

        [ObservableProperty]
        Card currentCard;

        [ObservableProperty]
        string displayQuestion;

        [ObservableProperty]
        string displayAnswer;

        [ObservableProperty]
        bool isAnswerVisible;

        [ObservableProperty]
        string progressText;

        [ObservableProperty]
        bool isSessionComplete;

        [ObservableProperty]
        bool isLoading = true;

        public bool IsNotLoading => !IsLoading;

        public DeckReviewViewModel(IDataService dataService)
        {
            _dataService = dataService;
            Title = "Review Session";
            _sessionCards = new List<Card>();
        }

        async partial void OnDeckIdStringChanged(string value)
        {
            if (Guid.TryParse(value, out _deckId) && _deckId != Guid.Empty)
            {
                await LoadReviewSessionAsync();
            }
            else
            {
                Console.WriteLine($"Invalid DeckId received for review: {value}");
                await Shell.Current.DisplayAlert("Error", "Invalid Deck ID for review.", "OK");
                await Shell.Current.GoToAsync("..");
            }
        }

        private async Task LoadReviewSessionAsync()
        {
            if (_deckId == Guid.Empty) return;

            IsLoading = true;
            IsSessionComplete = false;
            _currentCardIndex = -1;
            _sessionCorrectAnswers = 0;
            _sessionTotalReviewed = 0;

            try
            {
                _currentDeck = await _dataService.GetDeckAsync(_deckId);
                _currentUser = await _dataService.GetUserAsync();

                if (_currentDeck == null || !_currentDeck.Cards.Any())
                {
                    await Shell.Current.DisplayAlert("Error", "Cannot start review. Deck is empty or not found.", "OK");
                    await Shell.Current.GoToAsync("..");
                    return;
                }

                // Pick random card
                var random = new Random();
                _sessionCards = _currentDeck.Cards.OrderBy(c => random.Next()).ToList();

                if (!_sessionCards.Any())
                {
                    await Shell.Current.DisplayAlert("Error", "No cards selected for review.", "OK");
                    await Shell.Current.GoToAsync("..");
                    return;
                }
                IsLoading = false;
                ShowNextCard();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading review session: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "Failed to load review session.", "OK");
                await Shell.Current.GoToAsync("..");
            }
            finally
            {
                IsLoading = false;
            }
        }


        [RelayCommand]
        private void FlipCard()
        {
            if (CurrentCard == null || IsSessionComplete) return;
            IsAnswerVisible = !IsAnswerVisible;
        }

        [RelayCommand]
        private async Task SubmitAnswerAsync(string wasCorrectString)
        {
            if (CurrentCard == null || IsSessionComplete || _deckId == Guid.Empty) return;
            bool wasCorrect = wasCorrectString.Equals("True") ? true : false;

            _sessionTotalReviewed++;
            if (wasCorrect)
            {
                _sessionCorrectAnswers++;
                _currentUser.XP += 10;
                _currentUser.TotalCorrectAnswers++;
                CurrentCard.CorrectStreak++;
            }
            else
            {
                _currentUser.XP += 1;
                CurrentCard.CorrectStreak = 0;
            }
            CurrentCard.LastReviewed = DateTime.UtcNow;

            // Save updated card stats back to the deck file
            try
            {
                await _dataService.UpdateCardInDeckAsync(_deckId, CurrentCard);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating card review stats: {ex.Message}");
            }

            ShowNextCard();
        }

        private async void ShowNextCard() 
        {
            _currentCardIndex++;
            if (_currentCardIndex < _sessionCards.Count)
            {
                CurrentCard = _sessionCards[_currentCardIndex];
                DisplayQuestion = CurrentCard.FrontContent;
                DisplayAnswer = CurrentCard.BackContent;
                IsAnswerVisible = false;
                ProgressText = $"Card {_currentCardIndex + 1} of {_sessionCards.Count}";
            }
            else
            {
                IsSessionComplete = true;
                CurrentCard = null;
                DisplayQuestion = "Session Complete!";
                DisplayAnswer = "";
                ProgressText = $"Finished: {_sessionCards.Count} cards";
                await CompleteSessionAsync(); 
            }
        }

        private async Task CompleteSessionAsync()
        {
            if (_currentUser == null) return;

            _currentUser.TotalSessionsCompleted++;
            DateTime today = DateTime.UtcNow.Date;
            DateTime lastSession = _currentUser.LastSessionDate.Date;
            if (lastSession == today.AddDays(-1)) _currentUser.CurrentDailyStreak++;
            else if (lastSession != today) _currentUser.CurrentDailyStreak = 1;
            if (_currentUser.CurrentDailyStreak > _currentUser.LongestDailyStreak) _currentUser.LongestDailyStreak = _currentUser.CurrentDailyStreak;
            _currentUser.LastSessionDate = DateTime.UtcNow;
            int newLevel = (_currentUser.XP / 100) + 1;
            if (newLevel > _currentUser.Level) _currentUser.Level = newLevel;

            try
            {
                await _dataService.SaveUserAsync(_currentUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving user stats after session: {ex.Message}");
                await Shell.Current.DisplayAlert("Warning", "Could not save session progress statistics.", "OK");
            }

            // Pass results as strings to ResultsPage
            var results = new Dictionary<string, object>
            {
                { "Score", _sessionCorrectAnswers.ToString() },
                { "TotalReviewed", _sessionTotalReviewed.ToString() },
                { "DeckName", _currentDeck?.Name ?? "Deck" },
                { "DeckId", _deckId.ToString() } // Pass DeckId for "Review Again"
            };
            // go to results
            await Shell.Current.GoToAsync(nameof(ResultsPage), results);
        }

        [RelayCommand]
        async Task AbortSessionAsync()
        {
            bool confirm = await Shell.Current.DisplayAlert("Abort Session", "Are you sure you want to end this review session early?", "Yes, Abort", "No, Continue");
            if (!confirm) return;
            await Shell.Current.GoToAsync(nameof(DeckListPage)); // Go back deck list
        }
    }
}
