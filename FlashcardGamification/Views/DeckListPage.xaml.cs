
using FlashcardGamification.ViewModel;

namespace FlashcardGamification.Views
{
    /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/DeckListPage"/>
    public partial class DeckListPage : ContentPage
    {
        private readonly DeckListViewModel _viewModel;

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/DeckListPage_ctor"/>
        public DeckListPage(DeckListViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/DeckListPage_OnAppearing"/>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}
