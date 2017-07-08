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

namespace Einkaufslisten_Template10.ViewModels
{
    public class EinkaufslistenViewModel : ViewModelBase 
    {       
        public MobileServiceCollection<Produkt, Produkt> Produkten_Collection;
        public MobileServiceCollection<Einkaufsliste, Einkaufsliste> Einkaufslisten_Collection;
    
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

                IOrderedEnumerable<Einkaufsliste> o = Einkaufslisten_Collection.OrderBy(einkaufliste => einkaufliste.name);
                //Einkaufslisten_Collection = o.ToList();
                //await RefreshEinkaufslisten();
            }
            // Datensätze eintragen (test)
            /*Produkt ProduktTest = new Produkt(15, "ok")
            {
                anzahl = 5,
                mengenbezeichnung = "Liter"
            };
            Produkt ProduktTest2 = new Produkt(111, "gut", 9, "Gramm");
            Einkaufsliste EinkauflisteTest = new Einkaufsliste(2, "facebooktest", DateTime.Now, AuthService.user);*/


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

            /*await SyncService.Produkt_Einkaufsliste.InsertAsync(new Produkt_Einkaufsliste("025a18d8fc5b436b9c66b0cd505f72cd", "385abccf996c4780a324e1557fca2f0a", "9daef6e198f743709c87d1f402c8e2fd", 2));
            await SyncService.Produkt_Einkaufsliste.InsertAsync(new Produkt_Einkaufsliste("025a18d8fc5b436b9c66b0cd505f72cd", "385abccf996c4780a324e1557fca2f0a", "9daef6e198f743709c87d1f402c8e2fd", 1));
            await SyncService.Produkt_Einkaufsliste.InsertAsync(new Produkt_Einkaufsliste("025a18d8fc5b436b9c66b0cd505f72cd", "f8aa78029a0245bba8f0e26f580e7aee", "198ec3fa5ed147f7aa99c86e46cab004", 5));
            await SyncService.Produkt_Einkaufsliste.InsertAsync(new Produkt_Einkaufsliste("025a18d8fc5b436b9c66b0cd505f72cd", "f0afa658417b4bfbb7c161f70c55a7f6", "9daef6e198f743709c87d1f402c8e2fd", 2));
            await SyncService.Produkt_Einkaufsliste.InsertAsync(new Produkt_Einkaufsliste("025a18d8fc5b436b9c66b0cd505f72cd", "683385893a434bbaa54d414017874420", "9daef6e198f743709c87d1f402c8e2fd", 2));*/

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
                /// </summary> 
                Einkaufslisten_Collection = await SyncService.Einkaufsliste
                    .Where(Einkaufsliste => Einkaufsliste.id_user == AuthService.user)
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
            Einkaufsliste e = new Einkaufsliste("bla",AuthService.user);
            e.updatedAt = DateTime.Now;
            Einkaufslisten_Collection.Add(e);
            await SyncService.Einkaufsliste.InsertAsync(e);
            //await SyncService.SyncAsync();
            //await RefreshEinkaufslisten();
        }
        public void EinkaufsBereich()
        {
            NavigationService.Navigate(typeof(Views.Einkaufsbereich));
        }

        public void OrderListZToA()
        {
            Einkaufslisten_Collection.OrderByDescending(einkaufliste => einkaufliste.name);
            
        }
    }
}