
using FlashcardGamification.ViewModel;

namespace FlashcardGamification.Views
{
    public partial class DeckListPage : ContentPage
    {
        private readonly DeckListViewModel _viewModel;

        public DeckListPage(DeckListViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}
