using FlashcardGamification.ViewModel;

namespace FlashcardGamification.Views
{
    /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/DeckDetailPage"/>
    public partial class DeckDetailPage : ContentPage
    {
        private readonly DeckDetailViewModel _viewModel;
        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/DeckDetailPage_ctor"/>
        public DeckDetailPage(DeckDetailViewModel viewModel) 
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/DeckDetailPage_OnAppearing"/>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}
