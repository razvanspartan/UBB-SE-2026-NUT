using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamNut.Models;
using TeamNut.Services;

namespace TeamNut.ViewModels
{
    // English: ViewModel handling user registration, login and profile data
    public partial class UserViewModel : ObservableObject
    {
        [ObservableProperty]
        private User _currentUser = new();

        [ObservableProperty]
        private UserData _currentUserData = new();

        [ObservableProperty]
        private bool _isNutritionistChecked;

        [ObservableProperty]
        private string _statusMessage = string.Empty;

        [ObservableProperty]
        private DateTimeOffset _selectedDate = DateTimeOffset.Now;

        public event EventHandler RegistrationValid;
        public event EventHandler LoginSuccess;
        public event EventHandler SaveDataSuccess;

        private readonly UserService _userService;

        public UserViewModel()
        {
            _userService = new UserService();
        }

        [RelayCommand]
        private async Task OnRegister() // English: Changed from 'void' to 'Task' for better async handling
        {
            StatusMessage = string.Empty;

            // English: Set the role based on the checkbox value
            if (IsNutritionistChecked)
            {
                CurrentUser.Role = "Nutritionist";
            }
            else
            {
                CurrentUser.Role = "User";
            }

            List<string> errors = CurrentUser.ValidateAndReturnErrors();
            if (errors.Any())
            {
                StatusMessage = string.Join(Environment.NewLine, errors);
                return;
            }

            if (await _userService.CheckIfUsernameExistsAsync(CurrentUser.Username))
            {
                StatusMessage = "Username already exists. Please choose another one.";
                return;
            }

            if (CurrentUser.Role == "User")
            {
                RegistrationValid?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                var registeredUser = await _userService.RegisterUserAsync(CurrentUser);
                if (registeredUser != null)
                {
                    UserSession.Login(registeredUser.Username, registeredUser.Role);
                    LoginSuccess?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        [RelayCommand]
        private async Task OnSaveData()
        {
            StatusMessage = string.Empty;

            // English: Check for validation errors in UserData before proceeding
            List<string> errors = CurrentUserData.GetValidationErrors();
            if (errors.Any())
            {
                StatusMessage = string.Join(Environment.NewLine, errors);
                return;
            }

            CurrentUserData.CalculateAge(SelectedDate);
            var registeredUser = await _userService.RegisterUserAsync(CurrentUser);

            if (registeredUser == null)
            {
                StatusMessage = "Registration failed. Username might already exist.";
                return;
            }

            CurrentUserData.UserId = registeredUser.Id;
            await _userService.AddUserDataAsync(CurrentUserData);

            UserSession.Login(registeredUser.Username, registeredUser.Role);
            SaveDataSuccess?.Invoke(this, EventArgs.Empty);
        }

        [RelayCommand]
        private async Task OnLogin()
        {
            StatusMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(CurrentUser.Username) || string.IsNullOrWhiteSpace(CurrentUser.Password))
            {
                StatusMessage = "Username and Password are required.";
                return;
            }

            var user = await _userService.LoginAsync(CurrentUser.Username, CurrentUser.Password);
            if (user != null)
            {
                UserSession.Login(user.Username, user.Role);
                LoginSuccess?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                StatusMessage = "Invalid username or password.";
            }
        }
    }
}