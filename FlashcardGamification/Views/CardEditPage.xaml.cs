using FlashcardGamification.ViewModel;

namespace FlashcardGamification.Views
{
    /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/CardEditPage"/>
    public partial class CardEditPage : ContentPage
    {
        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/CardEditPage_ctor"/>
        public CardEditPage(CardEditViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
