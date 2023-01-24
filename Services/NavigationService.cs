using MauiAppMVVM.Contracts.Services;
using MauiAppMVVM.View;
using MauiAppMVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppMVVM.Services
{
    public class NavigationService : INavigationService
    {
        readonly IServiceProvider _services;

        // Get the Navigation Property of the App's MainPage
        protected INavigation Navigation
        {
            get
            {
                INavigation? navigation = Application.Current?.MainPage?.Navigation;
                if (navigation != null)
                {
                    return navigation;
                }
                else
                {
                    if (Debugger.IsAttached)
                    {
                        Debugger.Break();
                    }
                    throw new Exception();
                }
            }
        }

        /* 
         * The IServiceProvider provides the the classes or instances which are
         * registered in the MauiProgram class. This ServiceProvider will get injected 
         * when an instance of this NavigationService is resolved. 
         */

        public NavigationService(IServiceProvider services) => _services = services;
        public Task NavigateBack()
        {
            if (Navigation.NavigationStack.Count > 1)
            {
                return Navigation.PopAsync();
            }
            throw new InvalidOperationException("No pages to navigate back to!");
        }

        public Task NavigateToMainPage()
            => NavigateToPage<MainPage>();

        public Task NavigateToSecondPage() 
            => NavigateToPage<SecondPage>();

        /*
         * This method navigates to a page of a specific type.
         * It accepts an optional parameter that can be passed from one view to another.
         * The 'where T : page' is a generic type constraint which specifies 
         * that the type parameter 'T' must be or derive from the 'Page' class. 
         * This ensures that only pages can be navigated to using this method.
         */
        private async Task NavigateToPage<T>(object? parameter = null) where T : Page
        {
            var toPage = ResolvePage<T>();

            if (toPage is not null)
            {
                // Subscribe to the toPage's NavigatedTo event
                toPage.NavigatedTo += Page_NavigatedTo;

                // Get VM of the toPage
                var toViewModel = GetPageViewModelBase(toPage);

                // Call navigatingTo on VM, passing in the paramter
                if (toViewModel is not null)
                    await toViewModel.OnNavigatingTo(parameter);

                // Navigate to requested page
                await Navigation.PushAsync(toPage, true);

                // Subscribe to the toPage's NavigatedFrom event
                toPage.NavigatedFrom += Page_NavigatedFrom;
            }
            else
                throw new InvalidOperationException($"Unable to resolve type {typeof(T).FullName}");
        }

        /*
         * To determine forward navigation, look at the 2nd to last item on the NavigationStack
         * If that entry equals the sender, it means we navigated forward from the sender to another page
         */
        private async void Page_NavigatedFrom(object? sender, NavigatedFromEventArgs e)
        {
            bool isForwardNavigation = Navigation.NavigationStack.Count > 1
                && Navigation.NavigationStack[^2] == sender;

            if (sender is Page thisPage)
            {
                if (!isForwardNavigation)
                {
                    thisPage.NavigatedTo -= Page_NavigatedTo;
                    thisPage.NavigatedFrom -= Page_NavigatedFrom;
                }

                await CallNavigatedFrom(thisPage, isForwardNavigation);
            }
        }

        /*
         * This NavigatedFrom is triggered by MAUI when the user navigates away
         * from a page. It works both back and forward from a page to another. 
         * Therefore, pass this 
         */
        private Task CallNavigatedFrom(Page p, bool isForward)
        {
            var fromViewModel = GetPageViewModelBase(p);

            if (fromViewModel is not null)
                return fromViewModel.OnNavigatedFrom(isForward);
            return Task.CompletedTask;
        }

        private async void Page_NavigatedTo(object? sender, NavigatedToEventArgs e)
            => await CallNavigatedTo(sender as Page);

        /*
         * When this is called, check if the page has a ViewModelBase.
         * If so, call the ViewModel's OnNavigatedTo method.
         */
        private Task CallNavigatedTo(Page? p)
        {
            var fromViewModel = GetPageViewModelBase(p);

            if (fromViewModel is not null)
                return fromViewModel.OnNavigatedTo();
            return Task.CompletedTask;
        }

        /*
         * The method uses the 'GetService' method of an object of the type 'IServiceProvider'
         * which is stored in the private field '_services' to try to resolve a service of 
         * type 'T'.
         */
        private T? ResolvePage<T>() where T : Page
            => _services.GetService<T>();

        /*
         * Get the page's ViewModel of type ViewModelBase.
         */
        private ViewModelBase? GetPageViewModelBase(Page? p)
            => p?.BindingContext as ViewModelBase;
    }
}
