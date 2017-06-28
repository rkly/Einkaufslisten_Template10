#define OFFLINE_SYNC_ENABLED

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
using Einkaufslisten_Template10.Views;
using Einkaufslisten_Template10;
#if OFFLINE_SYNC_ENABLED
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;  // offline sync
using Microsoft.WindowsAzure.MobileServices.Sync;         // offline sync
#endif

namespace Einkaufslisten_Template10.Services.AzureServices
{
    public class SyncService
    {
        /*/// <summary>
        /// MobileServiceClient für Azure Backend
        /// </summary>
        private static MobileServiceClient MobileService = new MobileServiceClient(
            "https://einkaufslisten.azurewebsites.net"
        );
        /// <summary>
        /// Tabellen in Azure (Easy Tables), Modellen sind in Models.Objects
        /// </summary>
        public MobileServiceCollection<Produkt, Produkt> Produkten;
        public MobileServiceCollection<Einkaufsliste, Einkaufsliste> Einkaufslisten;
#if OFFLINE_SYNC_ENABLED
        public IMobileServiceSyncTable<Produkt> Produkt = SyncService.MobileService.GetSyncTable<Produkt>();
        public IMobileServiceSyncTable<Einkaufsliste> Einkaufsliste = SyncService.MobileService.GetSyncTable<Einkaufsliste>();
#else
        private IMobileServiceTable<Produkt> Produkt = App.MobileService.GetTable<Produkt>();
        private IMobileServiceTable<Einkaufsliste> Einkaufsliste = App.MobileService.GetTable<Einkaufsliste>();
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

    */
    }
}
