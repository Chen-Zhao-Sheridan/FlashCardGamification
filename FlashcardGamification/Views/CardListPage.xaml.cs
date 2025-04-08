using FlashcardGamification.ViewModel;

namespace FlashcardGamification.Views
{
    public partial class CardListPage : ContentPage
    {
        private readonly CardListViewModel _viewModel;

        public CardListPage(CardListViewModel viewModel)
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
