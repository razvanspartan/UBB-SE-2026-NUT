using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;
using TeamNut.Models;
using TeamNut.Services;

namespace TeamNut.ModelViews
{
    // English: ViewModel for Meal Planning, using private fields for source generation
    public partial class MealPlanViewModel : ObservableObject
    {
        private readonly MealPlanService _mealPlanService;

        [ObservableProperty]
        private string _statusMessage = "Ready to create your plan!";

        [ObservableProperty]
        private bool _isBusy;

        public MealPlanViewModel()
        {
            // Initializing the service to handle data logic
            _mealPlanService = new MealPlanService();
        }

        [RelayCommand]
        private async Task OnGenerateMealPlan()
        {
            if (IsBusy) return;

            IsBusy = true;
            StatusMessage = "Fetching today's plan...";

            try
            {
                // English: Attempt to fetch today's plan using the service
                var todaysPlan = await _mealPlanService.GetTodaysMealPlanAsync(1);

                if (todaysPlan != null)
                {
                    StatusMessage = $"Today's plan: {todaysPlan.GoalType}";
                }
                else
                {
                    StatusMessage = "No plan for today. Generating...";
                    var success = await _mealPlanService.GenerateNewMealPlanAsync(1);

                    if (success)
                        StatusMessage = "New plan for today created!";
                    else
                        StatusMessage = "Could not generate a new plan.";
                }
            }
            catch (Exception)
            {
                StatusMessage = "An error occurred while fetching your plan.";
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}