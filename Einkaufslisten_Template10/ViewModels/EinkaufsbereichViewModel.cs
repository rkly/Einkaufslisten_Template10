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
        public MobileServiceCollection<Produkt_Einkaufsliste_View, Produkt_Einkaufsliste_View> Produkt_Einkaufsliste_Collection;

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            if (await AuthService.AuthenticateAsync())
            {
                Views.Busy.SetBusy(true, "Bitte warten. Daten werden geladen");
                await RefreshEinkaufsbereich(parameter);
                Views.Busy.SetBusy(false);
            }
            //await SyncService.Produkt_Einkaufsliste_View.InsertAsync(new Produkt_Einkaufsliste_View("02efe8f129d44cf69b27fd33c64c86b6", "Milch_string", "Liter_string", 2));
#if OFFLINE_SYNC_ENABLED
            await SyncService.MobileService.SyncContext.PushAsync(); // offline sync + Push für die neuen Listen anpassen!
#endif
        }
        public async Task RefreshEinkaufsbereich(object id_einkaufsliste)
        {
            MobileServiceInvalidOperationException exception = null;
            try
            {
                /// <summary>
                /// This code refreshes the entries in the list view by querying the Produkt_Einkaufsliste table
                /// </summary> 
                var parameters = new Dictionary<string, string>();
                parameters.Add("id_einkaufsliste", id_einkaufsliste.ToString());
                Produkt_Einkaufsliste_Collection = await SyncService.Produkt_Einkaufsliste_View
                    .WithParameters(parameters)
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
    }
}