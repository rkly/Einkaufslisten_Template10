using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Template10.Mvvm;
using Einkaufslisten_Template10.Services.AzureServices;
using Einkaufslisten_Template10.Models.Objects;
using Microsoft.WindowsAzure.MobileServices;
using Windows.UI.Popups;

namespace Einkaufslisten_Template10.ViewModels
{
    public class EinkaufsbereichViewModel : ViewModelBase
    {
        public MobileServiceCollection<Produkt, Produkt> Produkten_Collection;
        public MobileServiceCollection<Produkt_Einkaufsliste, Produkt_Einkaufsliste> Produkt_Einkaufsliste_Collection;

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            if (await AuthService.AuthenticateAsync())
            {
                Views.Busy.SetBusy(true, "Bitte warten. Daten werden geladen");
                await RefreshEinkaufsbereich();
                Views.Busy.SetBusy(false);
            }

#if OFFLINE_SYNC_ENABLED
            //await SyncService.MobileService.SyncContext.PushAsync(); // offline sync + Push für die neuen Listen anpassen!
#endif
        }
        public async Task RefreshEinkaufsbereich()
        {
            MobileServiceInvalidOperationException exception = null;
            try
            {
                /// <summary>
                /// This code refreshes the entries in the list view by querying the Produkt_Einkaufsliste table
                /// </summary> 
               // Produkt_Einkaufsliste_Collection = await SyncService.Produkt_Einkaufsliste
               //     .Where(Produkt_Einkaufsliste => Produkt_Einkaufsliste.id_einkaufsliste == "025a18d8fc5b436b9c66b0cd505f72cd")
                    //.Where(Einkaufsliste => Einkaufsliste.Complete == false)
               //     .ToCollectionAsync();
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
    }
}
