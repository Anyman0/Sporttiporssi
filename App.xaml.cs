using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using Sporttiporssi.Services;
using Sporttiporssi.ViewModels;
using Sporttiporssi.Views;
using System.Security.Claims;

namespace Sporttiporssi
{
    public partial class App : Application
    {
        private static LocalDatabaseService _databaseService;
        private readonly IServiceProvider _serviceProvider;
        private LoginService _loginService;

        public static string CurrentSerie { get; set; }
        public static string CurrentUser { get; set; }
        public App(IServiceProvider serviceProvider, LoginService loginService)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _databaseService = _serviceProvider.GetRequiredService<LocalDatabaseService>();
            _loginService = _serviceProvider.GetRequiredService<LoginService>();            
        }

        protected override async void OnStart()
        {
            base.OnStart();
            if(await IsUserLoggedIn())
            {
                MainPage = new AppShell();
            }
            else
            {
                MainPage = _serviceProvider.GetRequiredService<LoginPage>();
            }
            if(string.IsNullOrEmpty(Preferences.Get("currentserie", string.Empty)))
            {
                CurrentSerie = "Liiga"; // Setting default to liiga if none is set
                //await SecureStorage.SetAsync("currentserie", CurrentSerie);
                Preferences.Set("currentserie", CurrentSerie);
            }
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

        private async Task<bool> IsUserLoggedIn()
        {
            //SecureStorage.Remove("auth_token");            
            var token = await SecureStorage.GetAsync("auth_token");          
            if (!string.IsNullOrEmpty(token))
            {
                var status = await _loginService.ValidateTokenAsync(token);
                return status;
                //return true;
            }
            else
            {
                return false;
            }
        }

        public void NavigateToMainPage()
        {
            MainPage = new AppShell();
        }

        public void NavigateToRegisterPage()
        {
            MainPage = _serviceProvider.GetRequiredService<RegisterPage>();
        }
        public void NavigateToLoginPage()
        {
            MainPage = _serviceProvider.GetRequiredService<LoginPage>();
        }

    }
}
