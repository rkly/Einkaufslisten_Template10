using System;
using Einkaufslisten_Template10.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Einkaufslisten_Template10.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var p = Template10.Services.SerializationService.SerializationService.Json.Deserialize<int>(e.Parameter?.ToString());
            base.OnNavigatedTo(e);
        }   
    }
}