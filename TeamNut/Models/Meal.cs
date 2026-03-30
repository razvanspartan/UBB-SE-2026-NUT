using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TeamNut.Models
{
    // English: Updated Meal model to include all necessary properties (IsFavorite, ImageUrl, dietary flags)
    public partial class Meal : ObservableObject
    {
        [ObservableProperty]
        [Key]
        private int _id;

        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private string _imageUrl = string.Empty;

        [ObservableProperty]
        private double _calories;

        [ObservableProperty]
        private string _description = string.Empty;

        [ObservableProperty]
        private bool _isFavorite;

        [ObservableProperty]
        private bool _isKeto;

        [ObservableProperty]
        private bool _isVegan;

        [ObservableProperty]
        private bool _isNutFree;

        [ObservableProperty]
        private bool _isLactoseFree;

        [ObservableProperty]
        private bool _isGlutenFree;
    }
}