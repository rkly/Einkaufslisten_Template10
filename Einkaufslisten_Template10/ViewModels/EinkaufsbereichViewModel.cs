using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        public String einkaufsbereich_titel = String.Empty;
        private StyleController styleController = new StyleController(); 
        public StyleController StyleController
        {
            get { return styleController; }
        }
        public override async Task OnNavigatedToAsync(Object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            this.StyleController.loadStyle();
            if (AuthService.eingeloggt)
            {
                Einkaufsliste parameter_casted = parameter as Einkaufsliste;
                Views.Busy.SetBusy(true, new Windows.ApplicationModel.Resources.ResourceLoader().GetString("DatenWerdenGeladen"));
                einkaufsbereich_titel = parameter_casted.name;
                await RefreshEinkaufsbereich(parameter_casted.id);
                Views.Busy.SetBusy(false);
            }
        }
        private async Task RefreshEinkaufsbereich(String id_einkaufsliste)
        {
            MobileServiceInvalidOperationException exception = null;
            try
            {
                /// <summary>
                /// This code refreshes the entries in the list view by querying the Produkt_Einkaufsliste table
                /// </summary> 
                var parameters = new Dictionary<string, string>();
                parameters.Add("id_einkaufsliste", id_einkaufsliste.ToString());
                ObservableCollection<Produkt_Einkaufsliste_View> PList = new ObservableCollection<Produkt_Einkaufsliste_View>();
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
                await new MessageDialog(exception.Message,
                    new Windows.ApplicationModel.Resources.ResourceLoader().GetString("Fehler")).ShowAsync();
            }
        }
        public void ChangeStatusFromItem(object sender, ItemClickEventArgs e)
        {
            Produkt_Einkaufsliste_View_Einkaufsbereich clickedItem = e.ClickedItem as Produkt_Einkaufsliste_View_Einkaufsbereich;
            clickedItem.InCard = !clickedItem.InCard;
            Console.WriteLine(clickedItem.ToString());
            createNewList();
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