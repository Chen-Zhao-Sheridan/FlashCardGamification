using FlashcardGamification.CoreLogic;
using FlashcardGamification.CoreLogic.Interfaces;
using FlashcardGamification.ViewModel;
using FlashcardGamification.Views;
using Microsoft.Extensions.Logging;

namespace FlashcardGamification
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Redister Backend Data
            builder.Services.AddSingleton<IDataService, FileSystemDataService>();

            // Register ViewModels 
            builder.Services.AddTransient<DeckListViewModel>();
            builder.Services.AddTransient<DeckDetailViewModel>();
            builder.Services.AddTransient<DeckReviewViewModel>();
            builder.Services.AddTransient<CardEditViewModel>();
            builder.Services.AddTransient<StatsViewModel>();
            builder.Services.AddTransient<ResultsViewModel>();

            // Register Views
            builder.Services.AddTransient<DeckListPage>();
            builder.Services.AddTransient<DeckDetailPage>();
            builder.Services.AddTransient<DeckReviewPage>();
            builder.Services.AddTransient<CardEditPage>();
            builder.Services.AddTransient<StatsPage>();
            builder.Services.AddTransient<ResultsPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
