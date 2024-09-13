using CommunityToolkit.Maui.Views;
using Sporttiporssi.Models;
using Sporttiporssi.ViewModels;
using Sporttiporssi.Views.Popups;

namespace Sporttiporssi.Views;

public partial class GroupPage : ContentPage
{
    private readonly IServiceProvider _serviceProvider;
    private readonly GroupViewModel _groupViewModel;
    public GroupPage(IServiceProvider serviceProvider, GroupViewModel groupViewModel)
	{
        _serviceProvider = serviceProvider;
		InitializeComponent();
        _groupViewModel = groupViewModel;
        BindingContext = _groupViewModel;
	}

    private async void CreateGroupToolbarItem_Clicked(object sender, EventArgs e)
    {
        var createGroupPage = _serviceProvider.GetRequiredService<CreateGroupPage>();
        await Shell.Current.Navigation.PushAsync(createGroupPage);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _groupViewModel.LoadAllFantasyGroupsBySerie();
    }

    private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if(e.Item is FantasyGroup selectedGroup)
        {
            var popup = new JoinGroupPopup(selectedGroup, _groupViewModel, _serviceProvider);
            this.ShowPopup(popup);
        }     
    }
}