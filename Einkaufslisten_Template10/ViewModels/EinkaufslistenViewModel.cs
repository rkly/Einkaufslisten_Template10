using Template10.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using Einkaufslisten_Template10.Models.Objects;
using Einkaufslisten_Template10.Services.AzureServices;
using Microsoft.WindowsAzure.MobileServices;
using Windows.UI.Xaml.Controls;
using Einkaufslisten_Template10.Models.Enum;
using Windows.UI.Xaml;
using System.Windows.Input;

namespace Einkaufslisten_Template10.ViewModels
{
    public class EinkaufslistenViewModel : ViewModelBase
    {
        public ObservableCollection<Einkaufsliste> Einkaufslisten_Collection;
        public Byte targetView;
        public ICommand IDeleteListe { get; set; }
        private StyleController styleController = new StyleController();

        public StyleController StyleController
        {
            get { return styleController; }
        }
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {                  
            if (AuthService.eingeloggt && parameter != null)
            {
                StyleController.loadStyle();
                targetView = (Byte)parameter;

                Views.Busy.SetBusy(true, new Windows.ApplicationModel.Resources.ResourceLoader().GetString("DatenWerdenGeladen"));
                await RefreshEinkaufslisten();
                Views.Busy.SetBusy(false);
                IDeleteListe = new DelegateCommand<Einkaufsliste>(DeleteListe);
            }
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
                    .OrderBy(einkaufslise => einkaufslise.name)
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
        public async void DeleteListe(Einkaufsliste delete_clicked)
        {
            if (delete_clicked.id != null)
            {
                await SyncService.Einkaufsliste.DeleteAsync(delete_clicked); //Produkt_Einkaufsliste werden in nodejs gelöscht! 
            }
            Einkaufslisten_Collection.Remove(delete_clicked);
        }
        public void NeueListe()
        {
            NavigationService.Navigate(typeof(Views.Erstellen));
        }
        public void GoToNextView(object sender, ItemClickEventArgs e)
        {
            Einkaufsliste clickedItem = e.ClickedItem as Einkaufsliste;
            var next_view_parameters = new Einkaufsliste(); //NavigationService serializes objects automatically
            next_view_parameters.id = clickedItem.id;
            next_view_parameters.name = clickedItem.name;
            if (targetView == (Byte)TargetView.EINKAUFEN)
            {
                NavigationService.Navigate(typeof(Views.Einkaufsbereich), next_view_parameters);
            }
            else
            {
                NavigationService.Navigate(typeof(Views.Erstellen), next_view_parameters);
            }
        }
        public async void Sort(object sender, RoutedEventArgs e)
        {
            try
            {
                Byte art = (Byte)((AppBarButton)sender).Tag;
                ObservableCollection<Einkaufsliste> temp = null;
                if (art == (Byte)SortArt.NAME)
                {
                    temp = new ObservableCollection<Einkaufsliste>(Einkaufslisten_Collection.OrderBy(einkaufliste => einkaufliste.name));
                }
                else if (art == (Byte)SortArt.DATUM)
                {
                    temp = new ObservableCollection<Einkaufsliste>(Einkaufslisten_Collection.OrderBy(einkaufliste => einkaufliste.updatedAt));
                }
                Einkaufslisten_Collection.Clear();
                foreach (Einkaufsliste elem in temp)
                {
                    Einkaufslisten_Collection.Add(elem);
                }
            }
            catch(Exception sort_e)
            {
                await new MessageDialog(sort_e.Message, "Sort Error").ShowAsync();
            }
        }
    }
}