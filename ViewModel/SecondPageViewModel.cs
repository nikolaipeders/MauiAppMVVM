using MauiAppMVVM.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppMVVM.ViewModel
{
    public class SecondPageViewModel
    {
        /* 
         * Following code adds a dependency of the interfaces IDataService and INavigationService
         * to the MainPageViewModel.
         */
        readonly IDataService _dataService;
        readonly INavigationService _navigationService;
        public SecondPageViewModel(IDataService dataService, INavigationService navigationService)
        {
            _dataService = dataService;
            _navigationService = navigationService;
        }
        public Command CMDGoBack => new Command(async () => await _navigationService.NavigateBack());
    }
}
