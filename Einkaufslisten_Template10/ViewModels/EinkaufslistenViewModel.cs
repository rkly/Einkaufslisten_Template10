using Template10.Mvvm;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;
using Einkaufslisten_Template10.Models.Objects;
using Einkaufslisten_Template10.Models.Source;
using System.Collections.ObjectModel;
using Einkaufslisten_Template10.Services.AzureServices;
using Windows.UI.Popups;
#if OFFLINE_SYNC_ENABLED
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;  // offline sync
using Microsoft.WindowsAzure.MobileServices.Sync;         // offline sync
#endif
using Microsoft.WindowsAzure.MobileServices;

namespace Einkaufslisten_Template10.ViewModels
{
    public class EinkaufslistenViewModel : ViewModelBase
    {       
        /*private ISource<Einkaufsliste> source;
        private ObservableCollection<Einkaufsliste> einkaufsListe;

        public EinkaufslistenViewModel()
        {
            source = new FactoryISource().CreateEinkaufsListenSource();
            FillEinkaufsliste();
        }
        private async void FillEinkaufsliste()
        {
            einkaufsListe = new ObservableCollection<Einkaufsliste>(await source.GetRepo());
            if (einkaufsListe.Count() == 0)
            {
                Einkaufsliste e = new Einkaufsliste(-1, "Bitte neue Liste erzeugen");
                einkaufsListe.Add(e);
            }
        }

        public ObservableCollection<Einkaufsliste> EinkaufsListe
        {
            get => einkaufsListe; 
            set => einkaufsListe = value; 
        } 
    

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {

            await Task.CompletedTask;
        }

        public override async Task OnNavigatedFromAsync(IDictionary<string, object> suspensionState, bool suspending)
        {

        }

        public override async Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            args.Cancel = false;
            await Task.CompletedTask;
        }*/

        ////////////////////////////////////// 
        /// <summary>
        /// MobileServiceClient für Azure Backend
        /// </summary>
        private static MobileServiceClient MobileService = new MobileServiceClient(
            "https://einkaufslisten.azurewebsites.net"
        );

        /// <summary>
        /// Tabellen in Azure (Easy Tables), Modellen sind in Models.Objects
        /// </summary>
        private MobileServiceCollection<Produkt, Produkt> Produkten_Collection;
        public MobileServiceCollection<Einkaufsliste, Einkaufsliste> Einkaufslisten_Collection;
#if OFFLINE_SYNC_ENABLED
        private IMobileServiceSyncTable<Produkt> Produkt = SyncService.MobileService.GetSyncTable<Produkt>();
        private IMobileServiceSyncTable<Einkaufsliste> Einkaufsliste = SyncService.MobileService.GetSyncTable<Einkaufsliste>();
#else
        private IMobileServiceTable<Produkt> Produkt = MobileService.GetTable<Produkt>();
        private IMobileServiceTable<Einkaufsliste> Einkaufsliste = MobileService.GetTable<Einkaufsliste>();
#endif

        #region Offline sync
#if OFFLINE_SYNC_ENABLED
        private async Task InitLocalStoreAsync()
        {
            if (!SyncService.MobileService.SyncContext.IsInitialized)
            {
                var store = new MobileServiceSQLiteStore("localstore.db");
                store.DefineTable<Produkt>();
                store.DefineTable<Einkaufsliste>();
                await SyncService.MobileService.SyncContext.InitializeAsync(store);
            }
        }
        public void createButtonClicked()
        {
            Einkaufsliste e = new Einkaufsliste(-1, "");
            GoEinkaufsbereich(e);
        }

            await SyncAsync();
        }
        private async Task SyncAsync()
        {
            await SyncService.MobileService.SyncContext.PushAsync();
            await Produkt.PullAsync("todoItems", Produkt.CreateQuery());
            await Einkaufsliste.PullAsync("todoItems", Einkaufsliste.CreateQuery());
        }
#endif
        #endregion
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            await RefreshEinkaufslisten();
#if OFFLINE_SYNC_ENABLED
            await InitLocalStoreAsync(); // offline sync
#endif

            // Datensätze eintragen (test)
            Produkt ProduktTest = new Produkt(15, "ok")
            {
                anzahl = 5,
                mengenbezeichnung = "Liter"
            };
            Produkt ProduktTest2 = new Produkt(111, "gut", 9, "Gramm");
            Einkaufsliste EinkauflisteTest = new Einkaufsliste(1, "list1", DateTime.Now);
            
            await Produkt.InsertAsync(ProduktTest);
            await Produkt.InsertAsync(ProduktTest2);
            await Einkaufsliste.InsertAsync(EinkauflisteTest);

#if OFFLINE_SYNC_ENABLED
            await App.MobileService.SyncContext.PushAsync(); // offline sync
#endif

        }
        public async Task RefreshEinkaufslisten()
        {
            MobileServiceInvalidOperationException exception = null;
            try
            {
                // This code refreshes the entries in the list view by querying the TodoItems table.
                // The query excludes completed TodoItems.
                Einkaufslisten_Collection = await Einkaufsliste
                    //.Where(Einkaufsliste => Einkaufsliste.Complete == false)
                    .ToCollectionAsync();
            }
            catch (MobileServiceInvalidOperationException e)
            {
                exception = e;
            }

            if (exception != null)
            {
                await new MessageDialog(exception.Message, "Error loading items").ShowAsync();
            }
            else
            {
                //Views.Einkaufslisten.Einkaufslisten_View.ItemsSource = Einkaufslisten;
                //this.ButtonSave.IsEnabled = true;
            }
        }
        public void createButtonClicked()
        {
            Einkaufsliste e = new Einkaufsliste(-1,"");
            GoEinkaufsbereich(e);
        }
        public void GoEinkaufsbereich(Einkaufsliste e)
        {
            SessionState.Add("einkaufsliste", e);
            NavigationService.Navigate(typeof(Views.Einkaufsbereich), "einkaufsliste");
        }

    }
}
