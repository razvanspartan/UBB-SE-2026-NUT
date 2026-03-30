using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace TeamNut.Models
{
    public partial class Reminder : ObservableValidator
    {
        [ObservableProperty]
        [Key]
        private int _id;

        [ObservableProperty]
        [Required]
        private int _userId;

        [ObservableProperty]
        [Required]
        private string _name = string.Empty;

        [ObservableProperty]
        private bool _hasSound = false;

        [ObservableProperty]
        [Required]
        private TimeSpan _time;

        [ObservableProperty]
        private string _frequency = string.Empty;
    }
}