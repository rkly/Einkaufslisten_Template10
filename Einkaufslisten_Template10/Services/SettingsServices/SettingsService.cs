using System;
using Template10.Common;
using Template10.Utils;
using Windows.UI.Xaml;
using System.Globalization;
using System.Threading;
using Einkaufslisten_Template10.Models.Objects;

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
        /// <summary>
        /// Sprache setzen und speicehrn, https://github.com/Windows-XAML/Template10/issues/261
        /// </summary>
        public string Sprache
        {
            get { return _helper.Read<String>(nameof(Sprache), CultureInfo.CurrentCulture.ToString()); }
            set
            {
                _helper.Write(nameof(Sprache), value);
                /*CultureInfo.CurrentCulture = new CultureInfo(value);
                CultureInfo.CurrentUICulture = new CultureInfo(value);
                CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(value);
                CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(value);*/
                CultureInfo culture = CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(value);
                Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = culture.Name;
                //BootStrapper.Current.NavigationService.Refresh();
                //BootStrapper.Current.NavigationService.Navigate(typeof(Views.SettingsPage), 0);
            }
        }
    }
}
