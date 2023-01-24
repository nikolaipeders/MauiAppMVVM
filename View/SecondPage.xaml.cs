using MauiAppMVVM.ViewModel;

namespace MauiAppMVVM.View;

public partial class SecondPage : ContentPage
{
    // Inject the SecondPageViewModel into the View using the constructor parameter.
    public SecondPage(SecondPageViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}