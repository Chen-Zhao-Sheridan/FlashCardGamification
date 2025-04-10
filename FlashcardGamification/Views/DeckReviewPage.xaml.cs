
using FlashcardGamification.ViewModel;

namespace FlashcardGamification.Views
{
    public partial class DeckReviewPage : ContentPage
    {
        private readonly DeckReviewViewModel _viewModel;
        public DeckReviewPage(DeckReviewViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override bool OnBackButtonPressed()
        {
            // If the session is not complete, ask for confirmation
            if (BindingContext is DeckReviewViewModel vm && !vm.IsSessionComplete && vm.IsNotLoading)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    bool confirm = await Shell.Current.DisplayAlert(
                        "Abort Session?",
                        "Are you sure you want to end this review session early?",
                        "Yes, Abort",
                        "No, Continue");

                    if (confirm)
                    {
                        // If confirmed, execute the ViewModel's abort command
                        if (vm.AbortSessionCommand.CanExecute(null))
                        {
                            vm.AbortSessionCommand.Execute(null);
                        }
                        else // Fallback if command cannot execute (e.g., already navigating)
                        {
                            await Shell.Current.GoToAsync(".."); // Basic back navigation
                        }
                    }
                    // If not confirmed, do nothing (stay on page)
                });

                return true; // Prevent default back button behavior
            }

            // Allow default back button behavior if session is complete or loading
            return base.OnBackButtonPressed();
        }
    }
}
