using Sporttiporssi.Models;
using Sporttiporssi.Models.DTOs;
using Sporttiporssi.ViewModels;

namespace Sporttiporssi.Views;

public partial class CreateTeamPage : ContentPage
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TeamViewModel _teamViewModel;
    public CreateTeamPage(IServiceProvider serviceProvider, TeamViewModel teamViewModel)
	{
		InitializeComponent();
		_serviceProvider = serviceProvider;
		_teamViewModel = teamViewModel;
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
		var teamName = TeamNameEntry.Text;

        var teamRegisterDto = new TeamRegisterDto
		{
			TeamName = teamName,
			TradesPerPhase = 20,
            FundsLeft = 2000000
        };

		var success = await _teamViewModel.SaveNewTeam(teamRegisterDto);
        if (success)
        {
			await DisplayAlert("Success", "Team created successfully!", "OK");
        }
		else
		{
			await DisplayAlert("Failed", "Failed to create new team..", "OK");
		}
    }
}