using FlashcardGamification.Views;

namespace FlashcardGamification
{
    /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/AppShell"/>
    public partial class AppShell : Shell
    {
        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/AppShell_ctor"/>
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(DeckDetailPage), typeof(DeckDetailPage));
            Routing.RegisterRoute(nameof(CardEditPage), typeof(CardEditPage));
            Routing.RegisterRoute(nameof(DeckReviewPage), typeof(DeckReviewPage));
            Routing.RegisterRoute(nameof(DeckListPage), typeof(DeckListPage));
            Routing.RegisterRoute(nameof(ResultsPage), typeof(ResultsPage));
        }
    }
}
