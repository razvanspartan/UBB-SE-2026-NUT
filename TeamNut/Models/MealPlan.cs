using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace TeamNut.Models
{
    // English: Updated MealPlan model to include GoalType as used in the repository
    public partial class MealPlan : ObservableObject
    {
        [ObservableProperty]
        private int _id;

        [ObservableProperty]
        private int _userId;

        [ObservableProperty]
        private string _goalType = string.Empty;

        [ObservableProperty]
        private DateTime _createdAt = DateTime.Now;
    }
}