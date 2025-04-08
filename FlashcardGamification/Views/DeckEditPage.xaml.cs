using FlashcardGamification.ViewModel;

namespace FlashcardGamification.Views
{
    public partial class DeckEditPage : ContentPage
    {
        public DeckEditPage(DeckEditViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
