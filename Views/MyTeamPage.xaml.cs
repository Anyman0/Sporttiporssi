using Sporttiporssi.Models;

namespace Sporttiporssi.Views;

public partial class MyTeamPage : ContentPage
{
    private readonly IServiceProvider _serviceProvider;
	public MyTeamPage(IServiceProvider serviceProvider)
	{
        _serviceProvider = serviceProvider;
        Shell.SetNavBarIsVisible(this, false);
        InitializeComponent();
    }

    private async void TradeButton_Clicked(object sender, EventArgs e)
    {
        var tradesPage = _serviceProvider.GetRequiredService<TradesPage>();
        await Shell.Current.Navigation.PushAsync(tradesPage);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();       
    }

}