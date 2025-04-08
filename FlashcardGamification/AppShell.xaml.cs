using FlashcardGamification.Views;

namespace FlashcardGamification
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(DeckEditPage), typeof(DeckEditPage));
            Routing.RegisterRoute(nameof(CardListPage), typeof(CardListPage));
            Routing.RegisterRoute(nameof(CardEditPage), typeof(CardEditPage));
        }
    }
}
