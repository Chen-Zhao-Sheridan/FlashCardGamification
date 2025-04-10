using FlashcardGamification.ViewModel;

namespace FlashcardGamification.Views
{
    public partial class ResultsPage : ContentPage
    {
        public ResultsPage(ResultsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;

            // Prevent user from using hardware/shell back button
            // to go back to the Review page after session ends.
            Shell.SetBackButtonBehavior(this, new BackButtonBehavior
            {
                IsEnabled = false
            });
        }
    }
}
