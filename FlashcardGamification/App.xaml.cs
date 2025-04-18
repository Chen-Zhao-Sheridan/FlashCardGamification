namespace FlashcardGamification
{
    /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/App"/>
    public partial class App : Application
    {
        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/App_ctor"/>
        public App()
        {
            InitializeComponent();
        }

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/App_CreateWindow"/>
        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}