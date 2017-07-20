using Template10.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using System.Linq;
using System.Threading.Tasks;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using Einkaufslisten_Template10.Models.Objects;
using Einkaufslisten_Template10.Services.AzureServices;
using Microsoft.WindowsAzure.MobileServices;
using System.Diagnostics;
using Windows.UI.Xaml.Controls;

namespace Einkaufslisten_Template10.ViewModels
{
    public class EinkaufslistenViewModel : ViewModelBase 
    {       
        public MobileServiceCollection<Produkt, Produkt> Produkten_Collection;
        public ObservableCollection<Einkaufsliste> Einkaufslisten_Collection;
        public ObservableCollection<Produkt> All_Produkte;
    
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            if (await AuthService.AuthenticateAsync())
            {
#if DEBUG 
                Console.WriteLine(AuthService.user.ToString());
#endif
#if OFFLINE_SYNC_ENABLED
                await SyncService.InitLocalStoreAsync(); // offline sync
#endif

                Views.Busy.SetBusy(true, "Bitte warten. Daten werden geladen");
                await RefreshEinkaufslisten();
                Views.Busy.SetBusy(false);

                
            }
            // Datensätze eintragen (test)
            /*Produkt ProduktTest = new Produkt(15, "ok")
            {
                anzahl = 5,
                mengenbezeichnung = "Liter"
            };
            Produkt ProduktTest2 = new Produkt(111, "gut", 9, "Gramm");*/
            //Einkaufsliste EinkauflisteTest = new Einkaufsliste("einkaufsbereich", AuthService.user);


            //await Produkt.InsertAsync(ProduktTest2);
            //await SyncService.Einkaufsliste.InsertAsync(EinkauflisteTest);

            /*await SyncService.Einheit.InsertAsync(new Einheit("Liter"));
            await SyncService.Einheit.InsertAsync(new Einheit("Stück"));
            await SyncService.Einheit.InsertAsync(new Einheit("Gramm"));
            await SyncService.Einheit.InsertAsync(new Einheit("Kilogramm"));
            
            await SyncService.Produkt.InsertAsync(new Produkt("Wasser"));
            await SyncService.Produkt.InsertAsync(new Produkt("Bier"));
            await SyncService.Produkt.InsertAsync(new Produkt("Milch"));
            await SyncService.Produkt.InsertAsync(new Produkt("Reis"));*/

            /*await SyncService.Produkt_Einkaufsliste.InsertAsync(new Produkt_Einkaufsliste("02efe8f129d44cf69b27fd33c64c86b6", "1549890959e74bc3840028888fb063bc", "f7f891c859db4000a2fe5b2c6366530e", 2));
            await SyncService.Produkt_Einkaufsliste.InsertAsync(new Produkt_Einkaufsliste("02efe8f129d44cf69b27fd33c64c86b6", "741d5cf8a8254cbab96c7641a0ccd01c", "51ef44b1af074afa852efa2031bc072e", 1));
            await SyncService.Produkt_Einkaufsliste.InsertAsync(new Produkt_Einkaufsliste("02efe8f129d44cf69b27fd33c64c86b6", "7a494911eb964ff29d660727d709eb45", "51ef44b1af074afa852efa2031bc072e", 5));
            await SyncService.Produkt_Einkaufsliste.InsertAsync(new Produkt_Einkaufsliste("02efe8f129d44cf69b27fd33c64c86b6", "f959ec70245349adb51ea289bc3d3046", "51ef44b1af074afa852efa2031bc072e", 2));
            await SyncService.Produkt_Einkaufsliste.InsertAsync(new Produkt_Einkaufsliste("02efe8f129d44cf69b27fd33c64c86b6", "f959ec70245349adb51ea289bc3d3046", "51ef44b1af074afa852efa2031bc072e", 2));*/

#if OFFLINE_SYNC_ENABLED
            await SyncService.MobileService.SyncContext.PushAsync(); // offline sync + Push für die neuen Listen anpassen!
#endif

        }
        public async Task RefreshEinkaufslisten()
        {
            MobileServiceInvalidOperationException exception = null;
            try
            {
                /// <summary>
                /// This code refreshes the entries in the list view by querying the Einkaufsliste table
                /// https://docs.microsoft.com/de-de/azure/app-service-mobile/app-service-mobile-node-backend-how-to-use-server-sdk
                /// </summary> 
                Einkaufslisten_Collection = await SyncService.Einkaufsliste
                    //.Where(Einkaufsliste => Einkaufsliste.id_user == AuthService.user) im Tabelleskript
                    .OrderByDescending(einkaufslise => einkaufslise.name)
                    .ToCollectionAsync();
                All_Produkte = await SyncService.Produkt.ToCollectionAsync();
            }
            catch (MobileServiceInvalidOperationException e)
            {
                exception = e;
            }
            if (exception != null)
            {
                await new MessageDialog(exception.Message, "Error loading items").ShowAsync();
            }
        }
        public void CreateButtonClicked()
        {
            Einkaufsliste e = new Einkaufsliste("");
            GoEinkaufsbereich(e);
        }
        public void GoEinkaufsbereich(Einkaufsliste e)
        {
            SessionState.Add("einkaufsliste", e);
            NavigationService.Navigate(typeof(Views.Einkaufsbereich), "einkaufsliste");
        }
        public async Task CreateNewElement()
        {
            Einkaufsliste e = new Einkaufsliste("zzz", AuthService.user);
            e.updatedAt = DateTime.Now;
            Einkaufslisten_Collection.Add(e);
            await SyncService.Einkaufsliste.InsertAsync(e);
            //await SyncService.SyncAsync();
            //await RefreshEinkaufslisten();
        }
        public async Task CreateEinkaufslistenElement(Einkaufsliste e)
        {
            if (e == null)
            {
                e = new Einkaufsliste("zzz", AuthService.user);
            }
            e.updatedAt = DateTime.Now;
            Einkaufslisten_Collection.Add(e);
            await SyncService.Einkaufsliste.InsertAsync(e);
            //await SyncService.SyncAsync();
            //await RefreshEinkaufslisten();
        }
        public void EinkaufsBereich(object sender, ItemClickEventArgs e)
        {
            Einkaufsliste clickedItem = e.ClickedItem as Einkaufsliste;
            String id_einkaufsliste_clicked = clickedItem.id;
            NavigationService.Navigate(typeof(Views.Einkaufsbereich), id_einkaufsliste_clicked);
        }
        public void OrderListZToA()
        {
            Einkaufslisten_Collection.OrderByDescending(einkaufliste => einkaufliste.name);
        }
        public void SortByDate()
        {
            ObservableCollection <Einkaufsliste> temp = new ObservableCollection<Einkaufsliste>(Einkaufslisten_Collection.OrderBy(einkaufliste => einkaufliste.updatedAt));
            Einkaufslisten_Collection.Clear();
            foreach (Einkaufsliste e in temp)
            {
                Einkaufslisten_Collection.Add(e);
            }
        }
        public IEnumerable<Produkt> GetMatchingProducts(string query)
        {
            return All_Produkte
                .Where(c => c.name.IndexOf(query, StringComparison.CurrentCultureIgnoreCase) > -1)
                .OrderByDescending(c => c.name.StartsWith(query, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}