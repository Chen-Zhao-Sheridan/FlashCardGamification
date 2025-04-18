using FlashcardGamification.ViewModel;

namespace FlashcardGamification.Views
{
    /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/StatsPage"/>
    public partial class StatsPage : ContentPage
    {
        private readonly StatsViewModel _viewModel;
        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/StatsPage_ctor"/>
        public StatsPage(StatsViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/StatsPage_OnAppearing"/>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}
