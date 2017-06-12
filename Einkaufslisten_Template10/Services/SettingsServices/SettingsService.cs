using System;
using Template10.Common;
using Template10.Utils;
using Windows.UI.Xaml;
using System.Globalization;
using Windows.UI.Popups; //TEST !

namespace Einkaufslisten_Template10.Services.SettingsServices
{
    public class SettingsService
    {
        public static SettingsService Instance { get; } = new SettingsService();
        Template10.Services.SettingsService.ISettingsHelper _helper;
        private SettingsService()
        {
            _helper = new Template10.Services.SettingsService.SettingsHelper();
        }

        public bool UseShellBackButton
        {
            get { return _helper.Read<bool>(nameof(UseShellBackButton), true); }
            set
            {
                _helper.Write(nameof(UseShellBackButton), value);
                BootStrapper.Current.NavigationService.GetDispatcherWrapper().Dispatch(() =>
                {
                    BootStrapper.Current.ShowShellBackButton = value;
                    BootStrapper.Current.UpdateShellBackButton();
                });
            }
        }

        public ApplicationTheme AppTheme
        {
            get
            {
                var theme = ApplicationTheme.Light;
                var value = _helper.Read<string>(nameof(AppTheme), theme.ToString());
                return Enum.TryParse<ApplicationTheme>(value, out theme) ? theme : ApplicationTheme.Dark;
            }
            set
            {
                _helper.Write(nameof(AppTheme), value.ToString());
                (Window.Current.Content as FrameworkElement).RequestedTheme = value.ToElementTheme();
                Views.Shell.HamburgerMenu.RefreshStyles(value, true);
            }
        }

        public TimeSpan CacheMaxDuration
        {
            get { return _helper.Read<TimeSpan>(nameof(CacheMaxDuration), TimeSpan.FromDays(2)); }
            set
            {
                _helper.Write(nameof(CacheMaxDuration), value);
                BootStrapper.Current.CacheMaxDuration = value;
            }
        }

        public bool ShowHamburgerButton
        {
            get { return _helper.Read<bool>(nameof(ShowHamburgerButton), true); }
            set
            {
                _helper.Write(nameof(ShowHamburgerButton), value);
                Views.Shell.HamburgerMenu.HamburgerButtonVisibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public bool IsFullScreen
        {
            get { return _helper.Read<bool>(nameof(IsFullScreen), false); }
            set
            {
                _helper.Write(nameof(IsFullScreen), value);
                Views.Shell.HamburgerMenu.IsFullScreen = value;
            }
        }

        public CultureInfo Culture
        {
            get { return _helper.Read<CultureInfo>(nameof(Culture), CultureInfo.CurrentCulture); }
            set
            {
                _helper.Write(nameof(Culture), value);
                //Frame.Navigate(this.GetType());
                /*
                 * var culture = new CultureInfo("fr-FR");
    Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = culture.Name;
    CultureInfo.DefaultThreadCurrentCulture = culture;
    CultureInfo.DefaultThreadCurrentUICulture = culture;*/
            }
        }
        public string test
        {
            get { return _helper.Read<String>(nameof(test), CultureInfo.CurrentCulture.ToString()); }
            set
            {
                _helper.Write(nameof(test), value);
                CultureInfo.CurrentCulture = new CultureInfo(value);
                //Template10.Common.BootStrapper.Current.NavigationService.Navigate(typeof(Views.SettingsPage));
                //Template10.Common.BootStrapper.Current.NavigationService.Navigate(typeof(Views.Einkaufsbereich));
            }
        }
    }
}
