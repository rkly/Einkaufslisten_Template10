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
using Microsoft.Data.Sqlite;
using Microsoft.Data.Sqlite.Internal;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Einkaufslisten_Template10.Views
{
    public sealed partial class Einkaufslisten : Page
    {
        private List<String> Grab_Entries()
        {
            List<String> entries = new List<string>();
            using (SqliteConnection db = new SqliteConnection("Filename=sqliteSample.db"))
            {
                db.Open();
                SqliteCommand selectCommand = new SqliteCommand("SELECT name from einkaufsliste", db);
                SqliteDataReader query;
                try
                {
                    query = selectCommand.ExecuteReader();
                }
                catch (SqliteException error)
                {
                    //Handle error
                    return entries;
                }
                while (query.Read())
                {
                    entries.Add(query.GetString(0));
                }
                db.Close();
            }
            return entries;
        }

        public Einkaufslisten()
        {
            InitializeComponent();
            // cached todo
            using (SqliteConnection db = new SqliteConnection("Filename=sqliteSample.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "INSERT INTO einkaufsliste (user_id, name) VALUES (1, 'Liste 1');INSERT INTO einkaufsliste (user_id, name) VALUES (1, 'Liste 2');INSERT INTO einkaufsliste (user_id, name) VALUES (1, 'Liste 3');";
                //insertCommand.Parameters.AddWithValue("@Entry", Input_Box.Text);

                try
                {
                    insertCommand.ExecuteReader();
                }
                catch (SqliteException error)
                {
                    //Handle error
                    return;
                }
                db.Close();
            }
            Output.ItemsSource = Grab_Entries();
            //Console.WriteLine(Grab_Entries());
        }
    }
}
