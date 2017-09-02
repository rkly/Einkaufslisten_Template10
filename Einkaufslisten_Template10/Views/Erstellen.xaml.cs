using System.Linq;
using Windows.UI.Xaml.Controls;
using System.Text.RegularExpressions;
using Einkaufslisten_Template10.Models.Objects;
using System.Diagnostics;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Einkaufslisten_Template10.Views
{
    public sealed partial class Erstellen : Page
    {
        public Erstellen()
        {
            InitializeComponent();
        }
        private void TextBox_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            if (!Regex.IsMatch(sender.Text, "^\\d*\\.?\\d*$") && sender.Text != "")
            {
                int pos = sender.SelectionStart - 1;
                sender.Text = sender.Text.Remove(pos, 1);
                sender.SelectionStart = pos;
            }
        }
        private void ProduktBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // Only get results when it was a user typing,
            // otherwise assume the value got filled in by TextMemberPath
            // or the handler for SuggestionChosen.
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                //Set the ItemsSource to be your filtered dataset
                var matchingProdukte = ViewModel.GetMatchingProdukte(sender.Text);
                sender.ItemsSource = matchingProdukte.ToList();
            }
        }
        private void ProduktBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                //User selected an item, take an action on it here
                var produkt = args.ChosenSuggestion as Produkt;
#if DEBUG
                Debug.WriteLine("Ein Element wurde gewählt " + produkt.name);
#endif
            }
            else
            {
                //Do a fuzzy search on the query text
                var matchingProdukte = ViewModel.GetMatchingProdukte(args.QueryText);
#if DEBUG
                if (matchingProdukte.Count() >= 1)
                {
                    //Choose the first match
                    Debug.WriteLine("Ein Element wurde gewählt " + matchingProdukte.FirstOrDefault().name);
                }
                else
                {
                    Debug.WriteLine("Neues Element eingegeben: " + args.QueryText);
                }
#endif
            }
        }
        private void EinheitBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // Only get results when it was a user typing,
            // otherwise assume the value got filled in by TextMemberPath
            // or the handler for SuggestionChosen.
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                //Set the ItemsSource to be your filtered dataset
                var matchingEinheiten = ViewModel.GetMatchingEinheiten(sender.Text);
                sender.ItemsSource = matchingEinheiten.ToList();
            }
        }
        private void EinheitBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                //User selected an item, take an action on it here
                var einheit = args.ChosenSuggestion as Einheit;
#if DEBUG
                Debug.WriteLine("Ein Element wurde gewählt " + einheit.name);
#endif
            }
            else
            {
                //Do a fuzzy search on the query text
                var matchingEinheiten = ViewModel.GetMatchingEinheiten(args.QueryText);
#if DEBUG
                if (matchingEinheiten.Count() >= 1)
                {
                    //Choose the first match
                    Debug.WriteLine("Ein Element wurde gewählt " + matchingEinheiten.FirstOrDefault().name);
                }
                else
                {
                    Debug.WriteLine("Neues Element eingegeben: " + args.QueryText);
                }
#endif
            }
        }      
    }
}