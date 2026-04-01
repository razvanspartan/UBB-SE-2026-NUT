using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

using TeamNut.Models;
using TeamNut.Services;

namespace TeamNut.ViewModels
{

    public partial class ShoppingListViewModel : ObservableObject
    {
        private readonly ShoppingListService _shoppingListService;

        [ObservableProperty]
        private ObservableCollection<ShoppingItem> items = new ObservableCollection<ShoppingItem>();

        public ShoppingListViewModel()
        {
            _shoppingListService = new ShoppingListService();
            _ = LoadItemsAsync();
        }

        public async Task LoadItemsAsync()
        {
            if (UserSession.UserId == null) return;

            var loadedItems = await _shoppingListService.GetShoppingItemsAsync(UserSession.UserId.Value);
            
            Items.Clear();
            foreach (var item in loadedItems)
            {
                // Subscribe to PropertyChanged to automatically update standard properties to the DB when checked
                item.PropertyChanged += async (s, e) =>
                {
                    if (e.PropertyName == nameof(ShoppingItem.IsChecked))
                    {
                        await _shoppingListService.UpdateItemAsync((ShoppingItem)s);
                    }
                };
                Items.Add(item);
            }
        }

        [RelayCommand]
        public async Task AddItem(string itemName)
        {
            if (!string.IsNullOrWhiteSpace(itemName) && UserSession.UserId != null)
            {
                var addedItem = await _shoppingListService.AddItemAsync(itemName.Trim(), UserSession.UserId.Value);
                if (addedItem != null)
                {
                    addedItem.PropertyChanged += async (s, e) =>
                    {
                        if (e.PropertyName == nameof(ShoppingItem.IsChecked))
                        {
                            await _shoppingListService.UpdateItemAsync((ShoppingItem)s);
                        }
                    };
                    Items.Add(addedItem);
                }
            }
        }

        [RelayCommand]
        public async Task DeleteItem(ShoppingItem item)
        {
            if (item != null && Items.Contains(item))
            {
                bool success = await _shoppingListService.RemoveItemAsync(item);
                if (success)
                {
                    Items.Remove(item);
                }
            }
        }
    }
}
