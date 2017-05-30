using Windows.UI.Xaml;
using System.Threading.Tasks;
using Einkaufslisten_Template10.Services.SettingsServices;
using Windows.ApplicationModel.Activation;
using Template10.Controls;
using Template10.Common;
using System;
using System.Linq;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Controls;
using Microsoft.Data.Sqlite;
using Microsoft.Data.Sqlite.Internal;
using System.IO;

namespace Einkaufslisten_Template10
{
    /// Documentation on APIs used in this page:
    /// https://github.com/Windows-XAML/Template10/wiki

    [Bindable]
    sealed partial class App : BootStrapper
    {
        public App()
        {
            InitializeComponent();
            SplashFactory = (e) => new Views.Splash(e);

            #region app settings

            // some settings must be set in app.constructor
            var settings = SettingsService.Instance;
            RequestedTheme = settings.AppTheme;
            CacheMaxDuration = settings.CacheMaxDuration;
            ShowShellBackButton = settings.UseShellBackButton;

            #endregion

            #region database
            SqliteEngine.UseWinSqlite3();
            using (SqliteConnection db = new SqliteConnection("Filename=sqliteSample.db"))
            {
                db.Open();
                String tableCommand = File.ReadAllText("einkaufslisteDeleteAndCreate.sql");
                SqliteCommand createTable = new SqliteCommand(tableCommand, db);
                try
                {
                    createTable.ExecuteReader();
                }
                catch (SqliteException e)
                {
                    //Do nothing
                }
            }
            #endregion database
        }

        public override UIElement CreateRootElement(IActivatedEventArgs e)
        {
            var service = NavigationServiceFactory(BackButton.Attach, ExistingContent.Exclude);
            return new ModalDialog
            {
                DisableBackButtonWhenModal = true,
                Content = new Views.Shell(service),
                ModalContent = new Views.Busy(),
            };
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            // TODO: add your long-running task here
            await NavigationService.NavigateAsync(typeof(Views.MainPage));
        }
    }
}
