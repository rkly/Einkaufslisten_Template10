using Microsoft.WindowsAzure.MobileServices;
using Einkaufslisten_Template10.Models.Objects;
using System.Threading.Tasks;
#if OFFLINE_SYNC_ENABLED
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;  // offline sync
using Microsoft.WindowsAzure.MobileServices.Sync;         // offline sync
#endif

namespace Einkaufslisten_Template10.Services.AzureServices
{
    public class SyncService
    {
        /// <summary>
        /// MobileServiceClient für Azure Backend
        /// </summary>
        private static MobileServiceClient _MobileService = new MobileServiceClient(
            "https://einkaufslisten.azurewebsites.net"
        );
        public static MobileServiceClient MobileService
        {
            get
            {
                return _MobileService;
            }
        }
        /// <summary>
        /// Tabellen in Azure (Easy Tables), Modellen sind in Models.Objects
        /// </summary>
#if OFFLINE_SYNC_ENABLED
        private static IMobileServiceSyncTable<Produkt> _Produkt = _MobileService.GetSyncTable<Produkt>();
        private static IMobileServiceSyncTable<Einkaufsliste> _Einkaufsliste = _MobileService.GetSyncTable<Einkaufsliste>();
        private static IMobileServiceSyncTable<Einheit> _Einheit = _MobileService.GetSyncTable<Einheit>();
        private static IMobileServiceSyncTable<Produkt_Einkaufsliste> _Produkt_Einkaufsliste = _MobileService.GetSyncTable<Produkt_Einkaufsliste>();
        public static IMobileServiceSyncTable<Einkaufsliste> Einkaufsliste
        {
            get
            {
                return _Einkaufsliste;
            }
        }
#else
        private static IMobileServiceTable<Produkt> _Produkt = _MobileService.GetTable<Produkt>();
        private static IMobileServiceTable<Einkaufsliste> _Einkaufsliste = _MobileService.GetTable<Einkaufsliste>();
        public static IMobileServiceTable<Einkaufsliste> Einkaufsliste
        {
            get
            {
                return _Einkaufsliste;
            }
        }
#endif 
        #region Offline sync
#if OFFLINE_SYNC_ENABLED
        public static async Task InitLocalStoreAsync()
        {
            if (!_MobileService.SyncContext.IsInitialized)
            {
                var store = new MobileServiceSQLiteStore("localstore.db");
                store.DefineTable<Produkt>();
                store.DefineTable<Einkaufsliste>();
                store.DefineTable<Einheit>();
                store.DefineTable<Produkt_Einkaufsliste>();
                await _MobileService.SyncContext.InitializeAsync(store);
            }
            await SyncAsync();
        }
        public static async Task SyncAsync()
        {
            await _MobileService.SyncContext.PushAsync();
            await _Produkt.PullAsync(null, _Produkt.CreateQuery());
            await _Einkaufsliste.PullAsync(null, _Einkaufsliste.CreateQuery());
            await _Einheit.PullAsync(null, _Einheit.CreateQuery());
            await _Produkt_Einkaufsliste.PullAsync(null, _Produkt_Einkaufsliste.CreateQuery());
        }
#endif
        #endregion
    }
}
