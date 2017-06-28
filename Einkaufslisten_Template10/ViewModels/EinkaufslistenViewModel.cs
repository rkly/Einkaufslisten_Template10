using Template10.Mvvm;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;
using Einkaufslisten_Template10.Models.Objects;
using Einkaufslisten_Template10.Models.Source;
using System.Collections.ObjectModel;

namespace Einkaufslisten_Template10.ViewModels
{
    public class EinkaufslistenViewModel : ViewModelBase
    {
        private ISource<Einkaufsliste> source;
        private ObservableCollection<Einkaufsliste> einkaufsListe;

        public EinkaufslistenViewModel()
        {
            source = new FactoryISource().CreateEinkaufsListenSource();
            FillEinkaufsliste();
        }
        private async void FillEinkaufsliste()
        {
            einkaufsListe = new ObservableCollection<Einkaufsliste>(await source.GetRepo());
            if (einkaufsListe.Count() == 0)
            {
                Einkaufsliste e = new Einkaufsliste(-1, "Bitte neue Liste erzeugen");
                einkaufsListe.Add(e);
            }
        }

        public ObservableCollection<Einkaufsliste> EinkaufsListe
        {
            get => einkaufsListe; 
            set => einkaufsListe = value; 
        } 
    

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {

            await Task.CompletedTask;
        }

        public override async Task OnNavigatedFromAsync(IDictionary<string, object> suspensionState, bool suspending)
        {

        }

        public override async Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            args.Cancel = false;
            await Task.CompletedTask;
        }
        public void createButtonClicked()
        {
            Einkaufsliste e = new Einkaufsliste(-1, "");
            GoEinkaufsbereich(e);
        }

        public void GoEinkaufsbereich(Einkaufsliste e)
        {
            SessionState.Add("einkaufsliste", e);
            NavigationService.Navigate(typeof(Views.Einkaufsbereich), "einkaufsliste");
        }

    }
}
