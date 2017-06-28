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

namespace Einkaufslisten_Template10
{
    /// Documentation on APIs used in this page:
    /// https://github.com/Windows-XAML/Template10/wiki

    [Bindable]
    sealed partial class App : BootStrapper
    {
        /// <summary>
        /// MobileServiceClient für Azure Backend
        /// </summary>
        private static MobileServiceClient MobileService = new MobileServiceClient(
            "https://einkaufslisten.azurewebsites.net"
        );
        /// <summary>
        /// Tabellen in Azure (Easy Tables), Modellen sind in Models.Objects
        /// </summary>
        private IMobileServiceTable<Produkt> Produkt = App.MobileService.GetTable<Produkt>();
        private IMobileServiceTable<Einkaufsliste> Einkaufsliste = App.MobileService.GetTable<Einkaufsliste>();
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
            
            // Datensätze eintragen (test)
            Produkt ProduktTest = new Produkt(15,"ok")
            {
                anzahl = 5,
                mengenbezeichnung = "Liter"
            };
            Produkt ProduktTest2 = new Produkt(111, "gut", 9, "Gramm");
            Einkaufsliste EinkauflisteTest = new Einkaufsliste(1, "list1", DateTime.Now);
            await Produkt.InsertAsync(ProduktTest);
            await Produkt.InsertAsync(ProduktTest2);
            await Einkaufsliste.InsertAsync(EinkauflisteTest);
            
        }
    }
}
