using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlashcardGamification.CoreLogic.Interfaces;
using FlashcardGamification.CoreLogic.Models;

namespace FlashcardGamification.ViewModel
{
    /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/StatsViewModel"/>
    public partial class StatsViewModel : BaseViewModel
    {
        private readonly IDataService _dataService;

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/StatsViewModel_CurrentUser"/>
        [ObservableProperty]
        User currentUser;

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/StatsViewModel_IsLoading"/>
        [ObservableProperty]
        bool isLoading;

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/StatsViewModel_IsNotLoading"/>
        public bool IsNotLoading => !IsBusy;

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/StatsViewModel_ctor"/>
        public StatsViewModel(IDataService dataService)
        {
            _dataService = dataService;
            Title = "My Progress";
            CurrentUser = new User();
        }

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/StatsViewModel_LoadStatsAsync"/>
        [RelayCommand]
        async Task LoadStatsAsync()
        {
            if (IsLoading) return;
            IsLoading = true;
            try
            {
                CurrentUser = await _dataService.GetUserAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading user stats: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "Failed to load user statistics.", "OK");
                CurrentUser = new User();
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <include file="Docs.xml" path="docs/members[@name='FlashcardGamification']/StatsViewModel_OnAppearing"/>
        public void OnAppearing()
        {
            Task.Run(async () => await LoadStatsAsync());
        }
    }
}
