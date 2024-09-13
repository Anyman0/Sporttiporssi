using Sporttiporssi.Models;
using Sporttiporssi.ViewModels;

namespace Sporttiporssi.Views;

public partial class TradesPage : ContentPage
{
    private PlayerViewModel _playerViewModel;
    private readonly IServiceProvider _serviceProvider;
    public TradesPage(PlayerViewModel playerViewModel, IServiceProvider serviceProvider)
    {
        Shell.SetNavBarIsVisible(this, false);
        InitializeComponent();
        _playerViewModel = playerViewModel;
        BindingContext = _playerViewModel;
        _serviceProvider = serviceProvider;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }
	protected override async void OnDisappearing() 
	{ 
		base.OnDisappearing();
    }

    private async void CompleteButton_Clicked(object sender, EventArgs e)
    {
        var tradePlayerPage = _serviceProvider.GetRequiredService<TradePlayerPage>();
        await Shell.Current.Navigation.PushAsync(tradePlayerPage);
        //await Shell.Current.GoToAsync(nameof(MyTeamPage));
    }
}