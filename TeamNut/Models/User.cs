using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace TeamNut.Models
{
    public partial class User : ObservableValidator
    {
        [ObservableProperty]
        [Key]
        private int _id;

        [ObservableProperty]
        [Required(ErrorMessage = "Username is mandatory")]
        [StringLength(30, MinimumLength = 3)]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Username must be alphanumeric")]
        private string _username = string.Empty;

        [ObservableProperty]
        [Required(ErrorMessage = "Password is mandatory")]
        [StringLength(30, MinimumLength = 8)]
        private string _password = string.Empty;

        [ObservableProperty]
        [Required(ErrorMessage = "Role is mandatory")]
        [RegularExpression(@"^(User|Nutritionist)$", ErrorMessage = "Role must be 'User' or 'Nutritionist'")]
        private string _role = "User";

        public List<string> ValidateAndReturnErrors()
        {
            ValidateAllProperties();
            return GetErrors().Select(e => e.ErrorMessage!).Where(msg => msg != null).ToList();
        }
    }
}