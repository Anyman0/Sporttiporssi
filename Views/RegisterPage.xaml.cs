using Sporttiporssi.ViewModels;

namespace Sporttiporssi.Views;

public partial class RegisterPage : ContentPage
{
	public RegisterPage(LoginViewModel loginViewModel)
	{
		InitializeComponent();
		BindingContext = loginViewModel;
	}
  
}