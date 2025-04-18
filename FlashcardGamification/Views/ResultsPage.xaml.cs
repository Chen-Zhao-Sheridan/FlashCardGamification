using FlashcardGamification.ViewModel;

namespace FlashcardGamification.Views
{
    /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/ResultsPage"/>
    public partial class ResultsPage : ContentPage
    {
        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/ResultsPage_ctor"/>
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
