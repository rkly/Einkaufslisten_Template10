using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Einkaufslisten_Template10.ViewModels;
using Template10.Services.NavigationService;
using System.Diagnostics;
using Einkaufslisten_Template10.Models.Objects;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Einkaufslisten_Template10.Views
{
    public sealed partial class Einkaufslisten : Page
    {
        public Einkaufslisten()
        {
            InitializeComponent();
        }
        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // Only get results when it was a user typing,
            // otherwise assume the value got filled in by TextMemberPath
            // or the handler for SuggestionChosen.
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                //sender.ItemsSource = 
                //Set the ItemsSource to be your filtered dataset
                var matchingProdukte = ViewModel.GetMatchingProducts(sender.Text);
                sender.ItemsSource = matchingProdukte.ToList();
            }
        }


        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            var produkt = args.SelectedItem as Produkt;

            sender.Text = string.Format("{0}", produkt.name);
        }


        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                //User selected an item, take an action on it here
                var produkt = args.ChosenSuggestion as Produkt;
                Debug.WriteLine("Ein Element wurde gewählt " + produkt.name);
            }
            else
            {
                //Do a fuzzy search on the query text
                var matchingProdukte = ViewModel.GetMatchingProducts(args.QueryText);

                if (matchingProdukte.Count() >= 1)
                {
                    //Choose the first match
                    Debug.WriteLine("Ein Element wurde gewählt " + matchingProdukte.FirstOrDefault().name);
                    
                }
                else
                {
                    Debug.WriteLine("Neues Element eingegeben: " + args.QueryText);
                }
            }
        }
    }
}
