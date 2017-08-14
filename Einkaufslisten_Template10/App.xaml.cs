using Windows.UI.Xaml;
using System.Threading.Tasks;
using Einkaufslisten_Template10.Services.SettingsServices;
using Windows.ApplicationModel.Activation;
using Template10.Controls;
using Template10.Common;
using System;
using System.Linq;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Controls;
using System.IO;
using Template10.Utils;
using Microsoft.WindowsAzure.MobileServices;
using Einkaufslisten_Template10.Models.Objects;
using Windows.UI.Popups;
using Einkaufslisten_Template10.Services.AzureServices;
using System.Diagnostics;
using Einkaufslisten_Template10.Models.Enum;

namespace Einkaufslisten_Template10
{
    /// Documentation on APIs used in this page:
    /// https://github.com/Windows-XAML/Template10/wiki

    [Bindable]
    sealed partial class App : BootStrapper
    {
        public App()
        {
            InitializeComponent();
            SplashFactory = (e) => new Views.Splash(e);

#region app settings

            // some settings must be set in app.constructor
            var settings = SettingsService.Instance;
            RequestedTheme = settings.AppTheme;
            CacheMaxDuration = settings.CacheMaxDuration;
            ShowShellBackButton = settings.UseShellBackButton;

#endregion
        }
        
        public override UIElement CreateRootElement(IActivatedEventArgs e)
        {
            var service = NavigationServiceFactory(BackButton.Attach, ExistingContent.Exclude);
            return new ModalDialog
            {
                DisableBackButtonWhenModal = true,
                Content = new Views.Shell(service),
                ModalContent = new Views.Busy(),
            };
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            // TODO: add your long-running task here
            ///<summary>
            /// Rückgabe des Tokens durch URL 
            /// </summary>
            if (args.Kind == ActivationKind.Protocol)
            {
                ProtocolActivatedEventArgs protocolArgs = args as ProtocolActivatedEventArgs;
                SyncService.MobileService.ResumeWithURL(protocolArgs.Uri);
                await NavigationService.NavigateAsync(typeof(Views.Einkaufslisten), TargetView.LISTE);
            }
            else
            {
                await NavigationService.NavigateAsync(typeof(Views.MainPage));
            }     
        }
    }
}