using Template10.Mvvm;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;
using Einkaufslisten_Template10.Models.Objects;
using System.Collections.ObjectModel;
using Einkaufslisten_Template10.Services.AzureServices;
using Windows.UI.Popups;
using Microsoft.WindowsAzure.MobileServices;

namespace Einkaufslisten_Template10.ViewModels
{
    public class EinkaufslistenViewModel : ViewModelBase
    {       
        public MobileServiceCollection<Produkt, Produkt> Produkten_Collection;
        public MobileServiceCollection<Einkaufsliste, Einkaufsliste> Einkaufslisten_Collection;
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {

#if OFFLINE_SYNC_ENABLED
            await SyncService.InitLocalStoreAsync(); // offline sync
#endif
            DelegateCommand ShowBusyCommand;
            ShowBusyCommand = new DelegateCommand(async () =>
            {
                Views.Busy.SetBusy(true, "Bitte warten. Daten werden geladen");
                await RefreshEinkaufslisten();
                // damit die View richtig lädt
                await Task.Delay(50);
                Views.Busy.SetBusy(false);
            });
            ShowBusyCommand.Execute();
            await RefreshEinkaufslisten();
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
    }
}
