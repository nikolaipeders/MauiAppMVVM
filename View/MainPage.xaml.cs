using MauiAppMVVM.ViewModel;

namespace MauiAppMVVM.View;

public partial class MainPage : ContentPage
{

	// Inject the MainPageViewModel into the View using the constructor parameter.
	public MainPage(MainPageViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}

