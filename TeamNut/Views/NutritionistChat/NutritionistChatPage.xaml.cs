using Microsoft.UI.Xaml.Controls;
using TeamNut.ViewModels;
using TeamNut.Models;

namespace TeamNut.Views.NutritionistChat
{
    public sealed partial class NutritionistChatPage : Page
    {
        public NutritionistChatPage()
        {
            this.InitializeComponent();
            ViewModel = new NutritionistChatViewModel();
            this.DataContext = ViewModel;
            Loaded += NutritionistChatPage_Loaded;
        }

        public NutritionistChatViewModel ViewModel { get; }

        private async void NutritionistChatPage_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            await ViewModel.LoadConversations();
        }
    }
}
