using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppMVVM.Contracts.Services
{
    public interface INavigationService
    {
        Task NavigateBack();
        Task NavigateToMainPage();
        Task NavigateToSecondPage();
    }
}
