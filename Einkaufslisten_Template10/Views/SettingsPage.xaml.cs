using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Einkaufslisten_Template10.Views
{
    public sealed partial class SettingsPage : Page
    {
        Template10.Services.SerializationService.ISerializationService _SerializationService;

        public SettingsPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
            _SerializationService = Template10.Services.SerializationService.SerializationService.Json;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var index = int.Parse(_SerializationService.Deserialize(e.Parameter?.ToString()).ToString());
            MyPivot.SelectedIndex = index;
        }
        /// <summary>
        ///  Nach dem Click-Ereignis die Sprache ersetzen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuFlyoutItem_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var sprache = (sender as MenuFlyoutItem).Tag.ToString();
            ViewModel.SettingsPartViewModel.Sprache = sprache;
        }
    }
}