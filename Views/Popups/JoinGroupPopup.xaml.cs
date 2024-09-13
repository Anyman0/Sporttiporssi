using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Views;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Sporttiporssi.Models;
using Sporttiporssi.Services;
using Sporttiporssi.ViewModels;
using System.Runtime.CompilerServices;

namespace Sporttiporssi.Views.Popups;

public partial class JoinGroupPopup : Popup
{
    private Label errorLabel;
    private Entry passwordEntry;
    private GroupViewModel _groupViewModel;
    private IServiceProvider _serviceProvider;

    public JoinGroupPopup(FantasyGroup fantasyGroup, GroupViewModel groupViewModel, IServiceProvider serviceProvider)
	{
        _groupViewModel = groupViewModel;
        _serviceProvider = serviceProvider;
        var layout = new Grid
        {
            Padding = 20,
            Margin = 20,
            BackgroundColor = Colors.White,
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Star },
                new RowDefinition { Height = GridLength.Auto }
            },
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star }
            }
        };

        var scrollView = new ScrollView
        {
            Content = new VerticalStackLayout
            {
                Children = 
                {
                    new Label { Text = $"{fantasyGroup.GroupName}", FontSize = 20, TextColor = Colors.Black, FontAttributes = FontAttributes.Bold, Padding = 5, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = $"Trades Per Phase:      {fantasyGroup.TradesPerPhase}", FontSize = 20, TextColor = Colors.Black, Padding = 5 },
                    new Label { Text = $"Starting Money:        {fantasyGroup.StartMoney}", FontSize = 20, TextColor = Colors.Black, Padding = 5 },
                    new Label { Text = $"Offence Pass FTP:      {fantasyGroup.OffencePassFTP}", FontSize = 20, TextColor = Colors.Black, Padding = 5 },
                    new Label { Text = $"Offence Goal FTP:      {fantasyGroup.OffenceGoalFTP}", FontSize = 20, TextColor = Colors.Black, Padding = 5 },
                    new Label { Text = $"Offence 2min Penalty FTP:      {fantasyGroup.OffencePenaltyFTP}", FontSize = 20, TextColor = Colors.Black, Padding = 5 },
                    new Label { Text = $"Offence 10min Penalty FTP:     {fantasyGroup.OffencePenalty10FTP}", FontSize = 20, TextColor = Colors.Black, Padding = 5 },
                    new Label { Text = $"Offence 20min Penalty FTP:     {fantasyGroup.OffencePenalty20FTP}", FontSize = 20, TextColor = Colors.Black, Padding = 5 },
                    new Label { Text = $"Offence +/- FTP:       {fantasyGroup.OffencePowerFTP}", FontSize = 20, TextColor = Colors.Black, Padding = 5 },
                    new Label { Text = $"Offence Shot FTP:      {fantasyGroup.OffenceShotFTP}", FontSize = 20, TextColor = Colors.Black, Padding = 5 },
                    new Label { Text = $"Defence Pass FTP:      {fantasyGroup.DefencePassFTP}", FontSize = 20, TextColor = Colors.Black, Padding = 5 },
                    new Label { Text = $"Defence Goal FTP:      {fantasyGroup.DefenceGoalFTP}", FontSize = 20, TextColor = Colors.Black, Padding = 5 },
                    new Label { Text = $"Defence Penalty FTP:       {fantasyGroup.DefencePenaltyFTP}", FontSize = 20, TextColor = Colors.Black, Padding = 5 },
                    new Label { Text = $"Defence 10min Penalty FTP:     {fantasyGroup.DefencePenalty10FTP}", FontSize = 20, TextColor = Colors.Black, Padding = 5 },
                    new Label { Text = $"Defence 20min Penalty FTP:     {fantasyGroup.DefencePenalty20FTP}", FontSize = 20, TextColor = Colors.Black, Padding = 5 },
                    new Label { Text = $"Defence +/- FTP:       {fantasyGroup.DefencePowerFTP}", FontSize = 20, TextColor = Colors.Black, Padding = 5 },
                    new Label { Text = $"Defence Shot FTP:      {fantasyGroup.DefenceShotFTP}", FontSize = 20, TextColor = Colors.Black, Padding = 5 },
                    new Label { Text = $"Goalie Pass FTP:       {fantasyGroup.GoaliePassFTP}", FontSize = 20, TextColor = Colors.Black, Padding = 5 },
                    new Label { Text = $"Goalie Goal FTP:       {fantasyGroup.GoalieGoalFTP}", FontSize = 20, TextColor = Colors.Black, Padding = 5 },
                    new Label { Text = $"Goalie Win FTP:        {fantasyGroup.GoalieWinFTP}", FontSize = 20, TextColor = Colors.Black, Padding = 5 },
                    new Label { Text = $"Faceoff FTP:       {fantasyGroup.FaceOffFTP}", FontSize = 20, TextColor = Colors.Black, Padding = 5 },
                    //new Entry { Placeholder = "Group password", IsPassword = true, PlaceholderColor = Colors.Black }
                    (passwordEntry = new Entry { Placeholder = "Group password", IsPassword = true, PlaceholderColor = Colors.Black }),
                    (errorLabel = new Label { Text = "", FontSize = 20, TextColor = Colors.Red, Padding = 5, HorizontalOptions = LayoutOptions.Center })
                }
            }
        };
        
        Grid.SetColumn(scrollView, 0);
        Grid.SetRow(scrollView, 0);
        layout.Children.Add(scrollView);
       
        var buttonGrid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Auto },
                new ColumnDefinition { Width = GridLength.Auto },
                new ColumnDefinition { Width = GridLength.Star }
            }
        };

        var closeButton = new Button
        {
            Text = "Close",
            Command = new Command(() => Close()),
            HorizontalOptions = LayoutOptions.Start,
            BackgroundColor = Colors.Red,
        };

        var joinbutton = new Button
        {
            Text = "Join",
            Command = new Command(() => JoinGroup(fantasyGroup)),
            HorizontalOptions = LayoutOptions.End,
            BackgroundColor = Colors.Green,
        };

        buttonGrid.Children.Add(closeButton);
        Grid.SetColumn(closeButton, 0);
        buttonGrid.Children.Add(joinbutton);
        Grid.SetColumn(joinbutton, 4);
        Grid.SetRow(buttonGrid, 1);
        Grid.SetColumn(buttonGrid, 2);

        layout.Children.Add(buttonGrid);

        Content = layout;
    }
    
    private async void JoinGroup(FantasyGroup group)
    {
        var teamId = Preferences.Get("chosen_team", string.Empty);
        if(!String.IsNullOrEmpty(passwordEntry.Text) && !string.IsNullOrEmpty(teamId))
        {
            bool success = await _groupViewModel.JoinGroup(group.GroupId, Guid.Parse(teamId), passwordEntry.Text);
            if(success)
            {                
                Close();
                var myteamPage = _serviceProvider.GetRequiredService<MyTeamPage>();
                await Shell.Current.Navigation.PushAsync(myteamPage);
            }            
        }
        else
        {
             
        }
    }
}