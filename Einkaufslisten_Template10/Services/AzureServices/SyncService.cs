using Microsoft.WindowsAzure.MobileServices;
using Einkaufslisten_Template10.Models.Objects;

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
            get => _MobileService;       
        }
        /// <summary>
        /// Tabellen in Azure (Easy Tables), Modellen sind in Models.Objects
        /// </summary>
        private static IMobileServiceTable<Produkt> _Produkt = _MobileService.GetTable<Produkt>();
        private static IMobileServiceTable<Einkaufsliste> _Einkaufsliste = _MobileService.GetTable<Einkaufsliste>();
        private static IMobileServiceTable<Einheit> _Einheit = _MobileService.GetTable<Einheit>();
        private static IMobileServiceTable<Produkt_Einkaufsliste> _Produkt_Einkaufsliste = _MobileService.GetTable<Produkt_Einkaufsliste>();
        private static IMobileServiceTable<Produkt_Einkaufsliste_View> _Produkt_Einkaufsliste_View = _MobileService.GetTable<Produkt_Einkaufsliste_View>();
        public static IMobileServiceTable<Einkaufsliste> Einkaufsliste
        {
            get => _Einkaufsliste;
        }
        public static IMobileServiceTable<Produkt_Einkaufsliste> Produkt_Einkaufsliste
        {
            get => _Produkt_Einkaufsliste;
        }
        public static IMobileServiceTable<Produkt_Einkaufsliste_View> Produkt_Einkaufsliste_View
        {
            get => _Produkt_Einkaufsliste_View;
        }
        public static IMobileServiceTable<Einheit> Einheit
        {
            get => _Einheit;
        }
        public static IMobileServiceTable<Produkt> Produkt
        {
            get => _Produkt;
        }
    }
}