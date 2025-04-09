using FlashcardGamification.ViewModel;

namespace FlashcardGamification.Views
{
    public partial class DeckDetailPage : ContentPage
    {
        private readonly DeckDetailViewModel _viewModel;
        public DeckDetailPage(DeckDetailViewModel viewModel) 
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
