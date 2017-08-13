using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Template10.Mvvm;
using Einkaufslisten_Template10.Services.AzureServices;
using Einkaufslisten_Template10.Models.Objects;
using Microsoft.WindowsAzure.MobileServices;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace Einkaufslisten_Template10.ViewModels
{
    public class EinkaufsbereichViewModel : ViewModelBase
    {
        public ObservableCollection<Produkt_Einkaufsliste_View_Einkaufsbereich> Produkt_Einkaufsliste_Collection = new ObservableCollection<Produkt_Einkaufsliste_View_Einkaufsbereich>();


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
                ObservableCollection<Produkt_Einkaufsliste_View> PList;
                PList = await SyncService.Produkt_Einkaufsliste_View
                    .WithParameters(parameters)
                    .OrderBy(produkt => produkt.produkt)
                    .ToCollectionAsync();
                foreach (Produkt_Einkaufsliste_View produkt in PList)
                {
                    Produkt_Einkaufsliste_Collection.Add(new Produkt_Einkaufsliste_View_Einkaufsbereich(produkt.id_einkaufsliste, produkt.produkt, produkt.einheit, produkt.menge, false));
                }

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
        public void ChangeStatusFromItem(object sender, ItemClickEventArgs e)
        {
            Produkt_Einkaufsliste_View_Einkaufsbereich clickedItem = e.ClickedItem as Produkt_Einkaufsliste_View_Einkaufsbereich;
            clickedItem.InCard = !clickedItem.InCard;
            Console.WriteLine(clickedItem.ToString());
            createNewList();
            //Produkt_Einkaufsliste_View_Einkaufsbereich oldElement = (Produkt_Einkaufsliste_View_Einkaufsbereich) Produkt_Einkaufsliste_Collection.Select(item => item.produkt.Equals(clickedItem.produkt));
            //oldElement.InCard = !oldElement.InCard;
        }
        private void createNewList()
        {
            ObservableCollection<Produkt_Einkaufsliste_View_Einkaufsbereich> temp = new ObservableCollection<Produkt_Einkaufsliste_View_Einkaufsbereich>(Produkt_Einkaufsliste_Collection.OrderBy(produkt => produkt.InCard));
            Produkt_Einkaufsliste_Collection.Clear();
            foreach (Produkt_Einkaufsliste_View_Einkaufsbereich p in temp)
            {
                Produkt_Einkaufsliste_Collection.Add(p);
            }
        }
    }
}