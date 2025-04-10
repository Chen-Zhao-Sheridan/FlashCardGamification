using FlashcardGamification.Views;

namespace FlashcardGamification
{
    public partial class AppShell : Shell
    {
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
