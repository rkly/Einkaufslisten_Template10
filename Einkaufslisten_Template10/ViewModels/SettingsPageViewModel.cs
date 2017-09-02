using System;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Mvvm;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Einkaufslisten_Template10.Models.Objects;
using Einkaufslisten_Template10.Services.SettingsServices;

namespace Einkaufslisten_Template10.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        public SettingsPartViewModel SettingsPartViewModel { get; } = new SettingsPartViewModel();       
    }
    public class SettingsPartViewModel : ViewModelBase
    {
        SettingsService _settings;

        public SettingsPartViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                // designtime
            }
            else
            {
                _settings = Services.SettingsServices.SettingsService.Instance;
            }
			 this.styleController = new StyleController();
        }
        public bool ShowHamburgerButton
        {
            get { return _settings.ShowHamburgerButton; }
            set { _settings.ShowHamburgerButton = value; base.RaisePropertyChanged(); }
        }
        public bool IsFullScreen
        {
            get { return _settings.IsFullScreen; }
            set
            {
                _settings.IsFullScreen = value;
                base.RaisePropertyChanged();
                if (value)
                {
                    ShowHamburgerButton = false;
                }
                else
                {
                    ShowHamburgerButton = true;
                }
            }
        }
        public bool UseShellBackButton
        {
            get { return _settings.UseShellBackButton; }
            set { _settings.UseShellBackButton = value; base.RaisePropertyChanged(); }
        }
        public bool UseLightThemeButton
        {
            get { return _settings.AppTheme.Equals(ApplicationTheme.Light); }
            set { _settings.AppTheme = value ? ApplicationTheme.Light : ApplicationTheme.Dark; base.RaisePropertyChanged(); }
        }
        private StyleController styleController;

        public StyleController StyleController

        {
            get { return styleController; }
		}
        private string Sprache
        {
            set
            {
                _settings.Sprache = value;
				this.StyleController.changeStyle();
                base.RaisePropertyChanged();
            }
        }
        public async Task SpracheAendern(object sender, RoutedEventArgs e)
        {
            String sprache_clicked = (sender as MenuFlyoutItem).Tag.ToString();
            Sprache = sprache_clicked;
            try
            {
                BootStrapper.Current.NavigationService.ClearCache();
                var sprachen = new Windows.ApplicationModel.Resources.ResourceLoader();
                String login = sprachen.GetString("Menu_Login_");
                String erstellen = sprachen.GetString("Menu_Erstellen_");
                String einkaufen = sprachen.GetString("Menu_Einkaufen_");
                String einstellungen = sprachen.GetString("Menu_Einstellungen_");
                await Views.Shell.UpdateTextShell(login, erstellen, einkaufen, einstellungen);                
                await Task.Delay(100);
            }
            finally
            {
                Windows.ApplicationModel.Resources.Core.ResourceContext.GetForCurrentView().Reset();
                Windows.ApplicationModel.Resources.Core.ResourceContext.GetForViewIndependentUse().Reset();
                BootStrapper.Current.NavigationService.Refresh();
            }
        }
    }
}