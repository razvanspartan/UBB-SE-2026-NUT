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
<<<<<<< NUT-78-Create-Meal-Plan-Service
        public partial string statusMessage { get; set; } = string.Empty;
=======
        public partial string StatusMessage { get; set; }
>>>>>>> main

        [ObservableProperty]
        public partial bool IsBusy { get; set; }

        private readonly MealPlanService _mealPlanService;

        public MealPlanViewModel()
        {
            _mealPlanService = new MealPlanService();
        }

        [RelayCommand]
        private async void onGenerateMealPlan()
        {
            StatusMessage = string.Empty;
            IsBusy = true;

            // Calling our new service
            var result = await _mealPlanService.GenerateNewMealPlanAsync(1);

<<<<<<< NUT-78-Create-Meal-Plan-Service
            if (result)
=======
            try
            {
                // Simulate processing time for generation logic

                StatusMessage = "Meal plan generated successfully based on your pantry!";
            }
            catch (Exception ex)
>>>>>>> main
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