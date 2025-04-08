using FlashcardGamification.ViewModel;

namespace FlashcardGamification.Views
{
    public partial class CardEditPage : ContentPage
    {
        public CardEditPage(CardEditViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
