using FlashcardGamification.ViewModel;

namespace FlashcardGamification.Views
{
    public partial class StatsPage : ContentPage
    {
        private readonly StatsViewModel _viewModel;
        public StatsPage(StatsViewModel viewModel)
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
