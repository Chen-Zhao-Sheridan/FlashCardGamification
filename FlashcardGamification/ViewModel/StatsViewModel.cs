using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlashcardGamification.CoreLogic.Interfaces;
using FlashcardGamification.CoreLogic.Models;

namespace FlashcardGamification.ViewModel
{
    public partial class StatsViewModel : BaseViewModel
    {
        private readonly IDataService _dataService;

        [ObservableProperty]
        User currentUser;

        [ObservableProperty]
        bool isLoading;

        public StatsViewModel(IDataService dataService)
        {
            _dataService = dataService;
            Title = "My Progress";
            CurrentUser = new User();
        }

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

        public void OnAppearing()
        {
            Task.Run(async () => await LoadStatsAsync());
        }
    }
}
