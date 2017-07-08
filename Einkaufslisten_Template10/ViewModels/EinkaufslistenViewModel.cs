using Template10.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using System.Linq;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using Einkaufslisten_Template10.Models.Objects;
using System.Collections.ObjectModel;
using Einkaufslisten_Template10.Services.AzureServices;
using Windows.UI.Popups;
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
                Einkaufslisten_Collection = o.ToList();
                //await RefreshEinkaufslisten();
            }
            // Datensätze eintragen (test)
            /*Produkt ProduktTest = new Produkt(15, "ok")
            {
                anzahl = 5,
                mengenbezeichnung = "Liter"
            };
            Produkt ProduktTest2 = new Produkt(111, "gut", 9, "Gramm");
            Einkaufsliste EinkauflisteTest = new Einkaufsliste(1, "list1", DateTime.Now);
            */
            //await Produkt.InsertAsync(ProduktTest);
            //await Produkt.InsertAsync(ProduktTest2);
            //await SyncService.Einkaufsliste.InsertAsync(EinkauflisteTest);

#if OFFLINE_SYNC_ENABLED
            //await App.MobileService.SyncContext.PushAsync(); // offline sync + Push für die neuen Listen anpassen!
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
            Einkaufsliste e = new Einkaufsliste(-1,"");
            GoEinkaufsbereich(e);
        }
        public void GoEinkaufsbereich(Einkaufsliste e)
        {
            SessionState.Add("einkaufsliste", e);
            NavigationService.Navigate(typeof(Views.Einkaufsbereich), "einkaufsliste");
        }
        public void CreateNewElement()
        {
            Einkaufslisten_Collection.Add(new Einkaufsliste(3, "fadksjf"));
        }

        public void OrderListZToA()
        {
            Einkaufslisten_Collection.OrderByDescending(einkaufliste => einkaufliste.name);
            
        }
    }
}
