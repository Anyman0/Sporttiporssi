using CommunityToolkit.Maui;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sporttiporssi.Models;
using Sporttiporssi.Services;
using Sporttiporssi.ViewModels;
using Sporttiporssi.Views;
using Microsoft.Extensions.DependencyInjection;
using Sporttiporssi.Helpers;
using System.Net.Http.Headers;
using Sporttiporssi.Configurations;

namespace Sporttiporssi
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.UseMauiCommunityToolkit();          

            // Register the database file path
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "localData.db3");
            builder.Services.AddSingleton(dbPath);

            // Register LocalDatabaseService with the file path
            builder.Services.AddTransient<LocalDatabaseService>(provider =>
            {
                string path = provider.GetRequiredService<string>();
                return new LocalDatabaseService(path);
            });          
            // PROD API BASE
            //Preferences.Set("ApiBaseAddress", "https://sporttiporssi.azurewebsites.net/api/");
            // DEV API BASE
            Preferences.Set("ApiBaseAddress", "https://10.0.2.2:7092/api/");

            builder.Services.AddHttpClient();
           
            // Register services
            builder.Services.AddTransient<LeagueGamesService>();
            builder.Services.AddTransient<LoginService>();
            builder.Services.AddTransient<LeagueStandingsService>();
            builder.Services.AddTransient<LeaguePlayersService>();
            builder.Services.AddTransient<GroupService>();
            builder.Services.AddTransient<TeamService>();
            // Register viewmodels
            builder.Services.AddTransient<PlayerViewModel>();
            builder.Services.AddTransient<GamesViewModel>();
            builder.Services.AddTransient<LeagueViewModel>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<GroupViewModel>();
            builder.Services.AddTransient<TeamViewModel>();
            // Register pages
            builder.Services.AddTransient<LeaguePage>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MyTeamPage>();
            builder.Services.AddTransient<TeamsPage>();
            builder.Services.AddTransient<TradesPage>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddTransient<GroupPage>();
            builder.Services.AddTransient<CreateGroupPage>();
            builder.Services.AddTransient<CreateTeamPage>();
            builder.Services.AddTransient<TradePlayerPage>();
            builder.Services.AddTransient<ChangeTeamPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
