using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Einkaufslisten_Template10.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Disabled;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var p = Template10.Services.SerializationService.SerializationService.Json.Deserialize<int>(e.Parameter?.ToString());
            base.OnNavigatedTo(e);            
        }
    }
}