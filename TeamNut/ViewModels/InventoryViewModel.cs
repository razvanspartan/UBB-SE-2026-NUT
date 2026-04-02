using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TeamNut.Models;
using TeamNut.Services;
using System;

namespace TeamNut.ViewModels
{
    public partial class InventoryViewModel : ObservableObject
    {
        private readonly InventoryService _inventoryService;
        private readonly int _currentUserId;

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private string _emptyListMessage = "Your pantry is empty. Start adding items!";

        [ObservableProperty]
        private string _statusMessage = string.Empty;

        
        public ObservableCollection<Inventory> Items { get; } = new();

        public InventoryViewModel(int userId)
        {
            _inventoryService = new InventoryService();
            _currentUserId = userId;

            _ = LoadInventoryAsync();
        }

       
        [RelayCommand]
        public async Task LoadInventoryAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                var inventoryItems = await _inventoryService.GetUserInventory(_currentUserId);

                Items.Clear();
                foreach (var item in inventoryItems)
                {
                    Items.Add(item);
                }

                OnPropertyChanged(nameof(IsListEmpty));
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error loading inventory: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        
        [RelayCommand]
        private async Task RemoveItemAsync(Inventory item)
        {
            if (item == null) return;

            try
            {
                await _inventoryService.RemoveItem(item.Id);
                Items.Remove(item);
                OnPropertyChanged(nameof(IsListEmpty));
            }
            catch (Exception ex)
            {
                StatusMessage = $"Could not delete item: {ex.Message}";
            }
        }

        
        [RelayCommand]
        private void AddNewIngredient()
        {
            // Navigation to add-ingredient screen isn't implemented here.
            StatusMessage = "Redirecting to Add Ingredient screen...";
        }

        
        public bool IsListEmpty => !Items.Any();
    }
}