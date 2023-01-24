using MauiAppMVVM.Contracts.Services;

namespace MauiAppMVVM;

public partial class App : Application
{
    /* If we now add a parameter of type MainPage to the constructor of the
	 * App class, the DI container will inject an instance when an the Builder
	 * resolves an instance of the App class
	 */
    public App(INavigationService navigationService )
	{
		InitializeComponent();
		MainPage = new AppShell();
		//navigationService.NavigateToMainPage();
	}
}
