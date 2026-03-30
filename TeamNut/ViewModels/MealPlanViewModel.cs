using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamNut.Models;
using TeamNut.Services;

namespace TeamNut.ModelViews
{
    public partial class MealPlanViewModel : ObservableObject
    {
        [ObservableProperty]
        public partial string statusMessage { get; set; } = string.Empty;

        [ObservableProperty]
        public partial bool isBusy { get; set; }

        private readonly MealPlanService _mealPlanService;

        public MealPlanViewModel()
        {
            _mealPlanService = new MealPlanService();
        }

        [RelayCommand]
        private async void OnGenerateMealPlan()
        {
            StatusMessage = string.Empty;
            IsBusy = true;

            // Calling our new service
            var result = await _mealPlanService.GenerateNewMealPlanAsync(1);

            if (result)
            {
                StatusMessage = "Meal plan generated and saved successfully!";
            }
            else
            {
                StatusMessage = "Failed to generate meal plan. Please try again.";
            }

            IsBusy = false;
        }
    }
}