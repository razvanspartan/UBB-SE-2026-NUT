using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using TeamNut.Models;
using TeamNut.Services; 
using TeamNut.Views;

namespace TeamNut.Views
{
    public sealed partial class MainPage : Page
    {
        private bool mealsLoaded = false;
        private bool chatLoaded = false;
        private bool shoppingListLoaded = false;
        private bool remindersLoaded = false; 

        private readonly ReminderService _reminderService = new();
        public MainViewModel ViewModel { get; } = new();
        public TeamNut.ViewModels.RemindersViewModel RemindersViewModel { get; } = new();
        public MainPage()
        {
            this.InitializeComponent();
            _ = ViewModel.LoadHeaderData();
            LoadTopReminder();
        }

        
        private void MainTabView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var selectedItem = MainTabView.SelectedItem as TabViewItem;
                if (selectedItem == null) return;

                if (selectedItem == MealsTab && !mealsLoaded)
                {
                    MealsFrame.Navigate(typeof(TeamNut.MealsPage));
                    mealsLoaded = true;
                }
                else if (selectedItem == MealPlanTab)
                {
                    MealPlanFrame.Navigate(typeof(TeamNut.Views.MealPlanView.MealPlanPage));
                }
                else if (selectedItem == DailyLogTab)
                {
                    DailyLogFrame.Navigate(typeof(TeamNut.Views.CalorieLoggingView.CalorieLoggingPage));
                }
                else if (selectedItem == InventoryTab)
                {
                    InventoryFrame.Navigate(typeof(TeamNut.Views.InventoryView.InventoryPage));
                }
                else if (selectedItem == ChatTab && !chatLoaded)
                {
                    ChatFrame.Navigate(typeof(TeamNut.Views.NutritionistChat.NutritionistChatPage));
                    chatLoaded = true;
                }
                else if (selectedItem == ShoppingListTab && !shoppingListLoaded)
                {
                    ShoppingListFrame.Navigate(typeof(TeamNut.Views.ShoppingListView.ShoppingListPage));
                    shoppingListLoaded = true;
                }
                
                
                 else if (selectedItem == RemindersTab && !remindersLoaded)
                 {
                    try
                    {
                        RemindersFrame.Navigate(typeof(TeamNut.Views.RemindersView.RemindersPage));
                        remindersLoaded = true;
                    }
                    catch (Exception ex)
                    {
                          
                        System.Diagnostics.Debug.WriteLine($"NAVIGATION ERROR: {ex.Message}");
                    }
                }
                
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in tab selection: {ex.Message}");
            }
        }
        private async void LoadTopReminder()
        {
            try
            {
                int userId = UserSession.UserId ?? 0;
                if (userId == 0) return;

                var next = await _reminderService.GetNextReminder(userId);
                var text = next != null ? $"{next.Name} at {next.Time:hh\\:mm}" : "No upcoming meals";

                // Update UI element if available
                if (MainNextReminderText != null)
                {
                    MainNextReminderText.Text = text;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading top reminder: {ex.Message}");
            }
        }
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            UserSession.Logout();

            if (Application.Current is App app && app._window != null)
            {
                app._window.Content = new TeamNut.Views.UserView.UserView();
            }
        }

        private async void MainNextReminderDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int userId = UserSession.UserId ?? 0;
                if (userId == 0) return;

                var reminder = await _reminderService.GetNextReminder(userId);
                if (reminder == null)
                {
                    var noDialog = new ContentDialog
                    {
                        Title = "Reminder Details",
                        Content = "No upcoming reminders.",
                        CloseButtonText = "Close",
                        XamlRoot = this.XamlRoot
                    };
                    await noDialog.ShowAsync();
                    return;
                }

                var panel = new StackPanel { Spacing = 8 };
                panel.Children.Add(new TextBlock { Text = "Name", FontWeight = Microsoft.UI.Text.FontWeights.SemiBold });
                panel.Children.Add(new TextBlock { Text = reminder.Name ?? string.Empty, TextWrapping = Microsoft.UI.Xaml.TextWrapping.Wrap });

                panel.Children.Add(new TextBlock { Text = "Date", FontWeight = Microsoft.UI.Text.FontWeights.SemiBold });
                panel.Children.Add(new TextBlock { Text = reminder.ReminderDate ?? string.Empty });

                panel.Children.Add(new TextBlock { Text = "Time", FontWeight = Microsoft.UI.Text.FontWeights.SemiBold });
                panel.Children.Add(new TextBlock { Text = reminder.Time.ToString(@"hh\:mm") });

                panel.Children.Add(new TextBlock { Text = "Sound", FontWeight = Microsoft.UI.Text.FontWeights.SemiBold });
                panel.Children.Add(new TextBlock { Text = reminder.HasSound ? "On" : "Off" });

                panel.Children.Add(new TextBlock { Text = "Frequency", FontWeight = Microsoft.UI.Text.FontWeights.SemiBold });
                panel.Children.Add(new TextBlock { Text = reminder.Frequency ?? string.Empty });

                var dialog = new ContentDialog
                {
                    Title = "Reminder Details",
                    Content = new ScrollViewer
                    {
                        Content = panel,
                        VerticalScrollMode = Microsoft.UI.Xaml.Controls.ScrollMode.Auto,
                        VerticalScrollBarVisibility = Microsoft.UI.Xaml.Controls.ScrollBarVisibility.Auto
                    },
                    CloseButtonText = "Close",
                    XamlRoot = this.XamlRoot
                };

                await dialog.ShowAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error showing reminder details: {ex.Message}");
            }
        }
    }
}




/*using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using TeamNut.Models;
using TeamNut.Views;

namespace TeamNut.Views
{
    public sealed partial class MainPage : Page
    {
        private bool mealsLoaded = false;
        private bool mealPlanLoaded = false;
        private bool chatLoaded = false;
        private bool shoppingListLoaded = false;

        public MainPage()
        {
            this.InitializeComponent();

            // If current user is a nutritionist, restrict UI to chat only
            try
            {
                if (TeamNut.Models.UserSession.Role == "Nutritionist")
                {
                    MealsTab.Visibility = Visibility.Collapsed;
                    MealPlanTab.Visibility = Visibility.Collapsed;
                    DailyLogTab.Visibility = Visibility.Collapsed;
                    InventoryTab.Visibility = Visibility.Collapsed;
                    ShoppingListTab.Visibility = Visibility.Collapsed;

                    // Select and load chat tab
                    if (!chatLoaded && ChatFrame != null)
                    {
                        ChatFrame.Navigate(typeof(TeamNut.Views.NutritionistChat.NutritionistChatPage));
                        chatLoaded = true;
                    }
                    MainTabView.SelectedItem = ChatTab;
                }
            }
            catch { }
        }

        private void MealsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!mealsLoaded && MealsFrame != null)
                {
                    MealsFrame.Navigate(typeof(TeamNut.MealsPage));
                    mealsLoaded = true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading initial content: {ex.Message}");
            }
        }

        private void MainTabView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var selectedItem = MainTabView.SelectedItem as TabViewItem;
                if (selectedItem == null) return;

                if (selectedItem == MealsTab && !mealsLoaded)
                {
                    MealsFrame.Navigate(typeof(TeamNut.MealsPage));
                    mealsLoaded = true;
                }
                else if (selectedItem == MealPlanTab && !mealPlanLoaded)
                {
                    MealPlanFrame.Navigate(typeof(TeamNut.Views.MealPlanView.MealPlanPage));
                    mealPlanLoaded = true;
                }
                else if (selectedItem == DailyLogTab)
                {
                    DailyLogFrame.Navigate(typeof(TeamNut.Views.CalorieLoggingView.CalorieLoggingPage));
                }
                else if (selectedItem == InventoryTab)
                {
                    InventoryFrame.Navigate(typeof(TeamNut.Views.InventoryView.InventoryPage));
                }
                else if (selectedItem == ChatTab && !chatLoaded)
                {
                    ChatFrame.Navigate(typeof(TeamNut.Views.NutritionistChat.NutritionistChatPage));
                    chatLoaded = true;
                }
                else if (selectedItem == ShoppingListTab && !shoppingListLoaded)
                {
                    ShoppingListFrame.Navigate(typeof(TeamNut.Views.ShoppingListView.ShoppingListPage));
                    shoppingListLoaded = true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in tab selection: {ex.Message}");
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            UserSession.Logout();

            if (Application.Current is App app && app._window != null)
            {
                app._window.Content = new TeamNut.Views.UserView.UserView();
            }
        }
    }
}*/
