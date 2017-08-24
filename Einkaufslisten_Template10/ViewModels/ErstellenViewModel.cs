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
using Einkaufslisten_Template10.Models.Enum;

namespace Einkaufslisten_Template10.ViewModels
{
    public class ErstellenViewModel : ViewModelBase
    {
        public ObservableCollection<Produkt> Produkt_Collection;
        public ObservableCollection<Einheit> Einheit_Collection;
        public ObservableCollection<Produkt_Einkaufsliste_View> Produkt_Einkaufsliste_Erstellen_Collection = new ObservableCollection<Produkt_Einkaufsliste_View>();
        private ObservableCollection<Produkt_Einkaufsliste> Produkt_Einkaufsliste_Collection = new ObservableCollection<Produkt_Einkaufsliste>();
        private Einkaufsliste _Einkaufsliste = new Einkaufsliste();   
        private String _id_produkt = String.Empty;
        private String _id_einheit = String.Empty;
        private UInt16 _menge = 0;
        private String _name_produkt = String.Empty;
        private String _name_einheit = String.Empty;
        public String name = String.Empty;
        public String erstellen_titel = "Neue Liste erstellen";
        public ICommand IDeleteProdukt { get; set; }
        public override async Task OnNavigatedToAsync(Object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            await ProduktenZiehen();
            await EinheitenZiehen();         
            if (parameter != null)
            {
                Einkaufsliste parameter_casted = parameter as Einkaufsliste;
                erstellen_titel = "Liste bearbeiten";
                name = parameter_casted.name;
                _Einkaufsliste.name = name;
                await ListeBearbeiten(parameter_casted.id);
            }   
        }
        public async Task ListeBearbeiten(String id_einkaufsliste)
        {
            MobileServiceInvalidOperationException exception = null;
            try
            {
                _Einkaufsliste.id = id_einkaufsliste;
                /// <summary>
                /// This code refreshes the entries in the list view by querying the Produkt_Einkaufsliste table
                /// </summary> 
                var parameters = new Dictionary<string, string>();
                parameters.Add("id_einkaufsliste", id_einkaufsliste.ToString());
                //ObservableCollection<Produkt_Einkaufsliste_View> PList = new ObservableCollection<Produkt_Einkaufsliste_View>();
                Produkt_Einkaufsliste_Erstellen_Collection = await SyncService.Produkt_Einkaufsliste_View
                    .WithParameters(parameters)
                    .OrderBy(produkt => produkt.produkt)
                    .ToCollectionAsync();
                /*foreach (Produkt_Einkaufsliste_View produkt in PList)
                {
                    Produkt_Einkaufsliste_Erstellen_Collection.Add(new Produkt_Einkaufsliste_View(produkt.id_einkaufsliste, produkt.produkt, produkt.einheit, produkt.menge));
                }*/
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
        public void GetName(object sender, TextChangedEventArgs e)
        {
            TextBox textBoxName = sender as TextBox;
            name = textBoxName.Text;
            _Einkaufsliste.name = name;
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
            if(String.IsNullOrEmpty(_id_produkt))
            {
                _id_produkt = Produkt_Collection.First(a => a.name == _name_produkt).id;
            }
            if (String.IsNullOrEmpty(_id_einheit))
            {
                _id_einheit = Einheit_Collection.First(a => a.name == _name_einheit).id;
            }
            if (String.IsNullOrEmpty(_id_produkt) || String.IsNullOrEmpty(_id_einheit))
            {
                await neueElementeEintragen();
            }
            if(!String.IsNullOrEmpty(_id_produkt) && !String.IsNullOrEmpty(_id_einheit) && _menge > 0)
            {
                AddProdukt(_id_produkt, _id_einheit, _menge);
            }
            else
            {
                await new MessageDialog("Geben Sie bitte Produkte vollständig an"+_id_produkt + _id_einheit+ _menge.ToString(), "Fehler").ShowAsync();
            }
        }
        private async Task neueElementeEintragen()
        {
            if(!String.IsNullOrEmpty(_name_produkt) && String.IsNullOrEmpty(_id_produkt))
            {
                Produkt neuesProdukt = new Produkt(_name_produkt);
                await SyncService.Produkt.InsertAsync(neuesProdukt);
                Produkt_Collection.Add(neuesProdukt);
                _id_produkt = neuesProdukt.id;
            }
            if (!String.IsNullOrEmpty(_name_einheit) && String.IsNullOrEmpty(_id_einheit))
            {
                Einheit neueEinheit = new Einheit(_name_einheit);
                await SyncService.Einheit.InsertAsync(neueEinheit);
                Einheit_Collection.Add(neueEinheit);
                _id_einheit = neueEinheit.id;
            }
        }
        private void AddProdukt(String id_produkt, String id_einheit, UInt16 menge)
        {
            //local
            Produkt_Einkaufsliste_View NeuesProduktView = new Produkt_Einkaufsliste_View(_name_produkt, _name_einheit, menge);
            Produkt_Einkaufsliste_Erstellen_Collection.Add(NeuesProduktView);
            //db
            Produkt_Einkaufsliste NeuesProdukt = new Produkt_Einkaufsliste(id_produkt, id_einheit, menge);
            Produkt_Einkaufsliste_Collection.Add(NeuesProdukt);
            IDeleteProdukt = new DelegateCommand<Produkt_Einkaufsliste>(DeleteProdukt);
            //new RelayCommand<Produkt_Einkaufsliste>(DeleteProdukt);
        }
        private void DeleteProdukt(Produkt_Einkaufsliste NeuesProdukt)
        {
            
        }
        public async Task ListeSpeichern(object sender, RoutedEventArgs e)
        {
            if (Produkt_Einkaufsliste_Erstellen_Collection.Any()) //beim Bearbeiten sind die Produkte erstmal nur in View
            {
                if (String.IsNullOrEmpty(_Einkaufsliste.id))
                {
                    await SyncService.Einkaufsliste.InsertAsync(_Einkaufsliste);
                }
                else
                {
                    await SyncService.Einkaufsliste.UpdateAsync(_Einkaufsliste);
                }

                String id_einkaufsliste = _Einkaufsliste.id;
                try
                {
                    foreach (Produkt_Einkaufsliste item in Produkt_Einkaufsliste_Collection)
                    {
                        item.id_einkaufsliste = id_einkaufsliste;
                        await SyncService.Produkt_Einkaufsliste.InsertAsync(item);
                    }
                }
                catch
                {
                }
                NavigationService.Navigate(typeof(Einkaufslisten), TargetView.LISTE);
            }
            else
            {
                await new MessageDialog("Geben Sie bitte mindestens ein Produkt an", "Fehler").ShowAsync();
            }         
        }
        public IEnumerable<Produkt> GetMatchingProdukte(string query)
        {
            _name_produkt = query;
            _id_produkt = null;
            return Produkt_Collection
                .Where(c => c.name.IndexOf(query, StringComparison.CurrentCultureIgnoreCase) > -1)
                .OrderByDescending(c => c.name.StartsWith(query, StringComparison.CurrentCultureIgnoreCase));
        }
        public IEnumerable<Einheit> GetMatchingEinheiten(string query)
        {
            _name_einheit = query;
            _id_einheit = null;
            return Einheit_Collection
                .Where(c => c.name.IndexOf(query, StringComparison.CurrentCultureIgnoreCase) > -1)
                .OrderByDescending(c => c.name.StartsWith(query, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}