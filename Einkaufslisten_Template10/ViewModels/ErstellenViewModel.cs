using System;
using System.Collections.Generic;
using System.Linq;
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
        private StyleController styleController = new StyleController();

        public StyleController StyleController
        {
            get { return styleController; }
        }
        public override async Task OnNavigatedToAsync(Object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            this.StyleController.loadStyle();
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
            IDeleteProdukt = new DelegateCommand<Produkt_Einkaufsliste_View>(DeleteProdukt);
        }
        private async Task ListeBearbeiten(String id_einkaufsliste)
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
                Produkt_Einkaufsliste_Erstellen_Collection = await SyncService.Produkt_Einkaufsliste_View
                    .WithParameters(parameters)
                    .OrderBy(produkt => produkt.produkt)
                    .ToCollectionAsync();
            }
            catch (MobileServiceInvalidOperationException e)
            {
                exception = e;
            }
            if (exception != null) await new MessageDialog(exception.Message, "Error loading items").ShowAsync();
        }
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
            if (exception != null) await new MessageDialog(exception.Message, "Produkt DB Fehler").ShowAsync();
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
            if (exception != null) await new MessageDialog(exception.Message, "Einheit DB Fehler").ShowAsync();
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
                textBoxMenge.IsEnabled = false;
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
            textBoxMenge.IsEnabled = true;
        }
        public async Task ProduktHinzufuegen(object sender, RoutedEventArgs e)
        {
            if(String.IsNullOrEmpty(_id_produkt))
            {
                try
                {
                    var produkt_object = await Task.FromResult(Produkt_Collection.SingleOrDefault(a => a.name == _name_produkt)); //hier kann man nicht direkt id abrufen, Task wird nicht gewartet (?!?!?!)
                    if (produkt_object != null) _id_produkt = produkt_object.id;
                }
                catch(ArgumentNullException)
                {
                    await new MessageDialog("ArgumentNullException", "Fehler").ShowAsync();
                }
                
            }
            if (String.IsNullOrEmpty(_id_einheit))
            {
                try
                {
                    var einheit_object = await Task.FromResult(Einheit_Collection.SingleOrDefault(a => a.name == _name_einheit));
                    if (einheit_object != null) _id_einheit = einheit_object.id;
                }
                catch (ArgumentNullException)
                {
                    await new MessageDialog("ArgumentNullException", "Fehler").ShowAsync();
                }
            }
            if (String.IsNullOrEmpty(_id_produkt) || String.IsNullOrEmpty(_id_einheit)) await neueElementeEintragen();
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
            //db vorbereitung
            Produkt_Einkaufsliste NeuesProdukt = new Produkt_Einkaufsliste(id_produkt, id_einheit, menge);
            Produkt_Einkaufsliste_Collection.Add(NeuesProdukt);
        } 
        public async void DeleteProdukt(Produkt_Einkaufsliste_View delete_clicked)
        {
            if(delete_clicked.id != null)
            {
                var Produkt_Temp = Produkt_Einkaufsliste_Collection.FirstOrDefault(d => d.id == delete_clicked.id);
                Produkt_Einkaufsliste_Collection.Remove(Produkt_Temp);
                if(_Einkaufsliste.id != null) await SyncService.Produkt_Einkaufsliste.DeleteAsync(new Produkt_Einkaufsliste(delete_clicked.id));
            }
            else if(delete_clicked.id == null)
            {
                String id_produkt_temp = Produkt_Collection.FirstOrDefault(p => p.name == delete_clicked.produkt).id;
                String id_einheit_temp = Einheit_Collection.FirstOrDefault(e => e.name == delete_clicked.einheit).id;
                var Produkt_Temp = Produkt_Einkaufsliste_Collection.FirstOrDefault(d => d.id_produkt == id_produkt_temp && d.id_einheit == id_einheit_temp && d.menge == delete_clicked.menge);
                Produkt_Einkaufsliste_Collection.Remove(Produkt_Temp);
            }
            Produkt_Einkaufsliste_Erstellen_Collection.Remove(delete_clicked);
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
                catch (Exception)
                {
                    await new MessageDialog("Exception", "Fehler").ShowAsync();
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