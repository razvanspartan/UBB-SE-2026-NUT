using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using TeamNut.Models;
using TeamNut.ViewModels;

namespace TeamNut.Views.CalorieLoggingView
{
    /// <summary>
    /// View responsible for logging daily meal intake.
    /// </summary>
    public sealed partial class CalorieLoggingPage : Page
    {
        private readonly DailyLogViewModel _viewModel;

        public CalorieLoggingPage()
        {
            this.InitializeComponent();

            // Initialize ViewModel and set DataContext for XAML binding
            _viewModel = new DailyLogViewModel();
            this.DataContext = _viewModel;

            LoadData();
        }

        private async void LoadData()
        {
            await _viewModel.LoadAsync();
        }

        #region Event Handlers

        private void MealSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (args.SelectedItem is Meal meal)
            {
                _viewModel.SelectedMeal = meal;
            }
        }

        private async void OnLogMealClicked(object sender, RoutedEventArgs e)
        {
            await _viewModel.LogSelectedMealAsync();
        }

        #endregion
    }
}