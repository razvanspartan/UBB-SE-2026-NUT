using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using TeamNut.Models;
using TeamNut.ViewModels;

namespace TeamNut.Views.InventoryView
{
    /// <summary>
    /// View responsible for managing the user's food inventory.
    /// </summary>
    public sealed partial class InventoryPage : Page
    {
        // Property-based ViewModel initialization for better XAML binding access
        public InventoryViewModel ViewModel { get; } = new InventoryViewModel(UserSession.UserId ?? 0);

        public InventoryPage()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel;
        }

        /// <summary>
        /// Trigger data loading when the user navigates to this page.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Fire and forget the async load operation
            _ = ViewModel.LoadInventoryAsync();
        }

        #region Event Handlers

        private void IngredientSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (args.SelectedItem is Ingredient ingredient)
            {
                ViewModel.SelectedIngredient = ingredient;
            }
        }

        #endregion
    }
}