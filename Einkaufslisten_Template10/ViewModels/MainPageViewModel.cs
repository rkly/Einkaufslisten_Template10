using Template10.Mvvm;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;
using Einkaufslisten_Template10.Services.AzureServices;

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
        }
        private StyleController styleController = new StyleController();
        
        public StyleController StyleController
        {
            get { return styleController; }
        }
        string _Value = "";
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
        public void GotoSettings() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 0);
        public void GotoPrivacy() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 1);
        public void GotoAbout() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 2);
        public async Task Einloggen()
        {
            Views.Busy.SetBusy(true, new Windows.ApplicationModel.Resources.ResourceLoader().GetString("BitteWarten"));
            await AuthService.AuthenticateAsync();     
        }          
    }
}