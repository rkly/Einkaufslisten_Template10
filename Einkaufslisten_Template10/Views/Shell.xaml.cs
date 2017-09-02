using System;
using System.Threading.Tasks;
using Template10.Controls;
using Template10.Services.NavigationService;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Einkaufslisten_Template10.Views
{
    public sealed partial class Shell : Page
    {
        public static Shell Instance { get; set; }
        public static HamburgerMenu HamburgerMenu => Instance.MyHamburgerMenu;
        Services.SettingsServices.SettingsService _settings;
        public Shell()
        {
            Instance = this;
            InitializeComponent();
            _settings = Services.SettingsServices.SettingsService.Instance;
        }   
        public Shell(INavigationService navigationService) : this()
        {
            SetNavigationService(navigationService);
        }
        public void SetNavigationService(INavigationService navigationService)
        {
            MyHamburgerMenu.NavigationService = navigationService;
            HamburgerMenu.RefreshStyles(_settings.AppTheme, true);
            HamburgerMenu.IsFullScreen = _settings.IsFullScreen;
            HamburgerMenu.HamburgerButtonVisibility = _settings.ShowHamburgerButton ? Visibility.Visible : Visibility.Collapsed;
        }
        public static async Task UpdateTextShell(String login, String erstellen, String einkaufen, String einstellungen)
        {
            Instance.Menu_Login.Text = login;
            Instance.Menu_Erstellen.Text = erstellen;
            Instance.Menu_Einkaufen.Text = einkaufen;
            Instance.Menu_Einstellungen.Text = einstellungen;
            await Task.CompletedTask;
        }
    }
}