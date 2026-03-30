using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace TeamNut.Models
{
    // English: Complete UserData model matching all database columns used in UserRepository
    public partial class UserData : ObservableValidator
    {
        [ObservableProperty]
        [Key]
        private int _id;

        [ObservableProperty]
        private int _userId;

        [ObservableProperty]
        private double _weight;

        [ObservableProperty]
        private double _height;

        [ObservableProperty]
        private int _age;

        [ObservableProperty]
        private string _gender = string.Empty;

        // English: Added missing properties based on UserRepository requirements
        [ObservableProperty]
        private string _goal = string.Empty;

        [ObservableProperty]
        private double _bmi;

        [ObservableProperty]
        private double _calorieNeeds;

        [ObservableProperty]
        private double _proteinNeeds;

        [ObservableProperty]
        private double _carbNeeds;

        [ObservableProperty]
        private double _fatNeeds;

        [ObservableProperty]
        private string _activityLevel = string.Empty;

        public void CalculateAge(DateTimeOffset birthDate)
        {
            // English: Calculate age based on birthDate
            var today = DateTimeOffset.Now;
            var age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age).Date) age--;
            Age = age;
        }

        public System.Collections.Generic.List<string> GetValidationErrors()
        {
            ValidateAllProperties();
            var errors = new System.Collections.Generic.List<string>();
            foreach (var error in GetErrors())
            {
                if (error.ErrorMessage != null)
                {
                    errors.Add(error.ErrorMessage);
                }
            }
            return errors;
        }
    }
}