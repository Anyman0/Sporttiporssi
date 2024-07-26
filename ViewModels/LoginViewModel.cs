using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows.Input;
using Sporttiporssi.Services;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Sporttiporssi.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _email;
        private string _password;
        private string _confirmPassword;
        private string _invalidPasswordMessage;
        private string _invalidEmailMessage;
        private bool _isRegisterEnabled;
        private LoginService _loginService;

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
                ValidateInputs();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
                ValidateInputs();
            }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
                ValidateInputs();
            }
        }

        public string InvalidPasswordMessage
        {
            get => _invalidPasswordMessage;
            set
            {
                _invalidPasswordMessage = value;
                OnPropertyChanged(nameof(InvalidPasswordMessage));
            }
        }

        public string InvalidEmailMessage
        {
            get => _invalidEmailMessage;
            set
            {
                _invalidEmailMessage = value;
                OnPropertyChanged(nameof(InvalidEmailMessage));
            }
        }

        public bool IsRegisterEnabled
        {
            get => _isRegisterEnabled;
            set
            {
                _isRegisterEnabled = value;
                OnPropertyChanged(nameof(IsRegisterEnabled));
            }
        }


        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }

        public ICommand LoginNavigateCommand { get; }
        public ICommand RegisterNavigateCommand { get; }

        public LoginViewModel(LoginService loginService)
        {
            _loginService = loginService;
            LoginCommand = new Command(OnLogin);
            RegisterCommand = new Command(async () => await OnRegister(), CanRegister);
            LoginNavigateCommand = new Command(OnNavigateLogin);
            RegisterNavigateCommand = new Command(OnNavigateRegister);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void OnLogin()
        {
            // Validate user
            if (await ValidateUser(Email, Password))
            {
                if(Application.Current is App app)
                {
                    app.NavigateToMainPage();
                }
            }
            else
            {
               
            }
        }

        private async Task OnRegister()
        {           
            var statusCode = await _loginService.RegisterUserAsync(Email, Password);
            if(((int)statusCode) == 200)
            {
                // Register successful, inform user and move to login
                await Application.Current.MainPage.DisplayAlert("Success", "Registration successful. Press OK to proceed to login.", "OK");
                if(Application.Current is App app)
                {
                    app.NavigateToLoginPage();
                }
            }
            else if(((int)statusCode) == 205)
            {
                await Application.Current.MainPage.DisplayAlert("Failed.", "This email is already registered.", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Registration failed. Please try again.", "OK");
            }
        }

        private void OnNavigateLogin()
        {
            if (Application.Current is App app)
            {
                app.NavigateToLoginPage();
            }
        }
        private void OnNavigateRegister()
        {
            if(Application.Current is App app)
            {
                app.NavigateToRegisterPage();
            }
        }

        private async Task<bool> ValidateUser(string email, string password)
        {
            var response = await _loginService.Login(email, password);
            return response;
        }

        private bool CanRegister()
        {
            return IsRegisterEnabled;
        }

        private void ValidateInputs()
        {
            bool isEmailValid = true;
            bool isPasswordValid = true;
            // email validation
            if (string.IsNullOrWhiteSpace(Email))
            {
                InvalidEmailMessage = "Email field cannot be empty";
                isEmailValid = false;
            }
            else if (!IsValidEmail(Email))
            {
                InvalidEmailMessage = "Invalid email format";
                isEmailValid = false;
            }
            else
            {
                InvalidEmailMessage = string.Empty;
                isEmailValid = true;
            }
            // password validation
            if (string.IsNullOrWhiteSpace(Password))
            {
                InvalidPasswordMessage = "Password field cannot be empty";
                isPasswordValid = false;
            }
            else if (string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                InvalidPasswordMessage = "Confirm Password field cannot be empty";
                isPasswordValid = false;
            }
            else if (Password != ConfirmPassword)
            {
                InvalidPasswordMessage = "Passwords do not match";
                isPasswordValid = false;
            }
            else
            {
                InvalidPasswordMessage = string.Empty;
                isPasswordValid = true;
            }
            IsRegisterEnabled = isEmailValid && isPasswordValid;
            ((Command)RegisterCommand).ChangeCanExecute();
        }
        private bool IsValidEmail(string email)
        {
            // Simple regex for email validation
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return emailRegex.IsMatch(email);
        }

    }
}
