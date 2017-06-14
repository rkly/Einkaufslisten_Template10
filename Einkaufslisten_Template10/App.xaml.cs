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

namespace Einkaufslisten_Template10
{
    /// Documentation on APIs used in this page:
    /// https://github.com/Windows-XAML/Template10/wiki

    [Bindable]
    sealed partial class App : BootStrapper
    {
        /// <summary>
        /// Azure Backend
        /// </summary>
        public static MobileServiceClient MobileService = new MobileServiceClient("https://einkaufslisten.azurewebsites.net");
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
            await NavigationService.NavigateAsync(typeof(Views.MainPage));
            
            //Azure test
            /*TodoItem item = new TodoItem
            {
                Text = "Awesome item",
                Complete = false
            };
            await App.MobileService.GetTable<TodoItem>().InsertAsync(item);*/
            
        }
    }


    //Azure test
    public class TodoItem
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public bool Complete { get; set; }
    }
}
