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
using Windows.UI.Xaml.Controls;
using Einkaufslisten_Template10.Views;
using Windows.UI.Xaml;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Einkaufslisten_Template10.ViewModels
{
    public class ErstellenViewModel : ViewModelBase
    {
        public ObservableCollection<Produkt> Produkt_Collection;
        public ObservableCollection<Einheit> Einheit_Collection;
        public ObservableCollection<Produkt_Einkaufsliste_View> Produkt_Einkaufsliste_Erstellen_Collection = new ObservableCollection<Produkt_Einkaufsliste_View>();
        private ObservableCollection<Produkt_Einkaufsliste> Temp_Produkten_Collection = new ObservableCollection<Produkt_Einkaufsliste>();
        private Einkaufsliste _NeueEinkaufsliste;
        private String name = String.Empty;
        private String _id_produkt = String.Empty;
        private String _id_einheit = String.Empty;
        private UInt16 _menge = 0;
        private String _name_produkt = String.Empty;
        private String _name_einheit = String.Empty;
        public ICommand IDeleteProdukt { get; set; }
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            await ProduktenZiehen();
            await EinheitenZiehen();
            _NeueEinkaufsliste = new Einkaufsliste("Liste", AuthService.user);
        }
        /*veraltet public override async Task OnNavigatedFromAsync(IDictionary<string, object> suspensionState, bool suspending)
        {
            if (!_NeueEinkaufsliste.Equals(null))
            {
                try
                {
                    foreach (Produkt_Einkaufsliste item in Temp_Produkten_Collection)
                    {
                        await SyncService.Produkt_Einkaufsliste.DeleteAsync(item);
                    }
                }
                catch
                {

                }
                Temp_Produkten_Collection = null;
                await SyncService.Einkaufsliste.DeleteAsync(_NeueEinkaufsliste);
            }  
        }*/
        private async Task ProduktenZiehen()
        {
            MobileServiceInvalidOperationException exception = null;
            try
            {
                /// <summary>
                /// Die Produkten aus der Tabelle ziehen
                /// </summary> 
                Produkt_Collection = await SyncService.Produkt.ToCollectionAsync();
            }
            catch (MobileServiceInvalidOperationException e)
            {
                exception = e;
            }
            if (exception != null)
            {
                await new MessageDialog(exception.Message, "Produkt DB Fehler").ShowAsync();
            }
        }
        private async Task EinheitenZiehen()
        {
            MobileServiceInvalidOperationException exception = null;
            try
            {
                /// <summary>
                /// Die Einheiten aus der Tabelle ziehen
                /// </summary> 
                Einheit_Collection = await SyncService.Einheit.ToCollectionAsync();
            }
            catch (MobileServiceInvalidOperationException e)
            {
                exception = e;
            }
            if (exception != null)
            {
                await new MessageDialog(exception.Message, "Einheit DB Fehler").ShowAsync();
            }
        }
        /*veraltet private async Task ElementenZiehen(String id_einkaufsliste)
        {
            MobileServiceInvalidOperationException exception = null;
            try
            {
                /// <summary>
                /// Die Produkten der Liste ziehen
                /// </summary> 
                var parameters = new Dictionary<string, string>();
                parameters.Add("id_einkaufsliste", id_einkaufsliste);
                Produkt_Einkaufsliste_Erstellen_Collection = await SyncService.Produkt_Einkaufsliste_View
                    .WithParameters(parameters)
                    .ToCollectionAsync();
            }
            catch (MobileServiceInvalidOperationException e)
            {
                exception = e;
            }
            if (exception != null)
            {
                await new MessageDialog(exception.Message, "Produkt_Einkaufsliste_View DB Fehler").ShowAsync();
            }
        }*/
        public void GetName(object sender, TextChangedEventArgs e)
        {
            TextBox textBoxName = sender as TextBox;
            name = textBoxName.Text;
            _NeueEinkaufsliste.name = name;
        }
        public void ProduktBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            var produkt = args.SelectedItem as Produkt;
            sender.Text = string.Format("{0}", produkt.name);
            _id_produkt = produkt.id;
            _name_produkt = produkt.name;
            }
        public void EinheitBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            var einheit = args.SelectedItem as Einheit;
            sender.Text = string.Format("{0}", einheit.name);
            _id_einheit = einheit.id;
            _name_einheit = einheit.name;
        }
        /*veraltet
        public void GetProdukt()
        {
            //ListBox listBoxProdukt = sender as ListBox;
            //Produkt selectedProdukt = listBoxProdukt.SelectedItem as Produkt;
            //_id_produkt = selectedProdukt.id;
        }*/
        /*public void GetEinheit(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBoxEinheit = sender as ListBox;
            Einheit selectedEinheit = listBoxEinheit.SelectedItem as Einheit;
            _id_einheit = selectedEinheit.id;
        }*/
        public async void GetMenge(object sender, TextChangedEventArgs e)
        {
            TextBox textBoxMenge = sender as TextBox;
            String selectedMenge = textBoxMenge.Text;
            try
            {
                _menge = Convert.ToUInt16(selectedMenge);
            }
            catch (Exception exception)
            {
                String Fehlermeldung = String.Empty;
                if (exception is OverflowException)
                {
                    Fehlermeldung = "Die Menge ist zu groß";
                }
                else if(exception is FormatException)
                {
                    Fehlermeldung = "Ungültige Zahl";
                }
                await new MessageDialog(exception.Message, Fehlermeldung).ShowAsync();
            }
        }
        public async Task ProduktHinzufuegen(object sender, RoutedEventArgs e)
        {
            if(!String.IsNullOrEmpty(_id_produkt) && !String.IsNullOrEmpty(_id_einheit) && _menge > 0)
            {
                AddProdukt(_id_produkt, _id_einheit, _menge);
            }
            else
            {
                await new MessageDialog("Geben Sie bitte Produkte vollständig an"+_id_produkt + _id_einheit+ _menge.ToString(), "Fehler").ShowAsync();
            }
        }
        private void AddProdukt(String id_produkt, String id_einheit, UInt16 menge)
        {
            //local
            Produkt_Einkaufsliste_View NeuesProduktView = new Produkt_Einkaufsliste_View(_name_produkt, _name_einheit, menge);
            Produkt_Einkaufsliste_Erstellen_Collection.Add(NeuesProduktView);
            //db
            Produkt_Einkaufsliste NeuesProdukt = new Produkt_Einkaufsliste(id_produkt, id_einheit, menge);
            Temp_Produkten_Collection.Add(NeuesProdukt);
            IDeleteProdukt = new DelegateCommand<Produkt_Einkaufsliste>(DeleteProdukt);
                //new RelayCommand<Produkt_Einkaufsliste>(DeleteProdukt);
        }
        private void DeleteProdukt(Produkt_Einkaufsliste NeuesProdukt)
        {
            
        }
        public async Task ListeSpeichern(object sender, RoutedEventArgs e)
        {
            if (Temp_Produkten_Collection.Any())
            {
                await SyncService.Einkaufsliste.InsertAsync(_NeueEinkaufsliste);
                String id_einkaufsliste = _NeueEinkaufsliste.id;
                try
                {
                    foreach (Produkt_Einkaufsliste item in Temp_Produkten_Collection)
                    {
                        item.id_einkaufsliste = id_einkaufsliste;
                        await SyncService.Produkt_Einkaufsliste.InsertAsync(item);
                    }
                }
                catch
                {
                }
                NavigationService.Navigate(typeof(Einkaufslisten));
            }
            else
            {
                await new MessageDialog("Geben Sie bitte mindestens ein Produkt an", "Fehler").ShowAsync();
            }         
        }
        public IEnumerable<Produkt> GetMatchingProdukte(string query)
        {
            return Produkt_Collection
                .Where(c => c.name.IndexOf(query, StringComparison.CurrentCultureIgnoreCase) > -1)
                .OrderByDescending(c => c.name.StartsWith(query, StringComparison.CurrentCultureIgnoreCase));
        }
        public IEnumerable<Einheit> GetMatchingEinheiten(string query)
        {
            return Einheit_Collection
                .Where(c => c.name.IndexOf(query, StringComparison.CurrentCultureIgnoreCase) > -1)
                .OrderByDescending(c => c.name.StartsWith(query, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}