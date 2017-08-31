﻿using Template10.Mvvm;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;

using Einkaufslisten_Template10.Services.AzureServices;
using Windows.UI.Xaml;
using Einkaufslisten_Template10.Models.Objects;

namespace Einkaufslisten_Template10.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                Value = "Designtime value";
            }
            this.styleController = new StyleController();
            
        }
        private StyleController styleController;
        
        public StyleController StyleController
        {
            get { return styleController; }
        }

        string _Value = "bla_Test";
        public string Value { get { return _Value; } set { Set(ref _Value, value); } }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            if (suspensionState.Any())
            {
                Value = suspensionState[nameof(Value)]?.ToString();
            }
            this.StyleController.loadStyle();
            await Task.CompletedTask;
        }

        public override async Task OnNavigatedFromAsync(IDictionary<string, object> suspensionState, bool suspending)
        {
            if (suspending)
            {
                suspensionState[nameof(Value)] = Value;
            }
            await Task.CompletedTask;
        }

        public async Task OnNavigatingToAsync(NavigatingEventArgs args)
        {
            args.Cancel = false;
            await Task.CompletedTask;
        }

        public void GotoDetailsPage() =>
            NavigationService.Navigate(typeof(Views.DetailPage), Value);

        public void GotoSettings() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 0);

        public void GotoPrivacy() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 1);

        public void GotoAbout() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 2);

        public async Task Einloggen()
        {
            Views.Busy.SetBusy(true, "Bitte warten");
            await AuthService.AuthenticateAsync();     
        }          
        public void Refresh()
        {
            //NavigationService.Navigate(typeof(Views.MainPage), 0);
            this.StyleController.changeStyle();
        }
    }
}
