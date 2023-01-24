using MauiAppMVVM.Contracts.Services;
using MauiAppMVVM.Services;
using MauiAppMVVM.View;
using MauiAppMVVM.ViewModel;

namespace MauiAppMVVM;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
        /* 
         * Dependency injection in .NET contains three major lifetimes.
		 * Transient, Scoped, and Transient. 
		 */
        var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		/* 
		 * Transient lifetime services are created each time they are requested.
		 */
		builder.Services.AddTransient<MainPage>();
		builder.Services.AddTransient<SecondPage>();

        // Make sure to register all ViewModels in the DI container.
        builder.Services.AddTransient<MainPageViewModel>();
        builder.Services.AddTransient<SecondPageViewModel>();

        /*
		 * Singleton creates a single instance throughout the application. 
		 * It creates the instance for the first time and reuses the same object in the all calls.
		 */
        builder.Services.AddSingleton<IDataService, DataService>();
		builder.Services.AddSingleton<INavigationService, NavigationService>();

		return builder.Build();
	}
}
