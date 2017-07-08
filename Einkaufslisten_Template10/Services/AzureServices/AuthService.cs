using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Einkaufslisten_Template10.Services.AzureServices;
using Windows.UI.Popups;
using System.Diagnostics;

namespace Einkaufslisten_Template10.Services.AzureServices
{
    public class AuthService
    { 
        /// <summary>
        /// Define a member variable for storing the signed-in user
        /// </summary>
        private static MobileServiceUser _user;
        public static String user
        {
            get
            {
                return _user.UserId;
            }
        }
        /// <summary>
        /// Define a method that performs the authentication process using Azure Active Directory sign-in. 
        /// </summary>
        public static async Task<bool> AuthenticateAsync()
        {
            bool success = false;
            if (_user != null)
            {
                success = true;
            }
            else
            {
                string message;              
                try
                {
                    var provider = MobileServiceAuthenticationProvider.Facebook;
                    //bool singleSignOn = true;
                    string uriScheme = "einkaufslisten-scheme";
                    _user = await SyncService.MobileService.LoginAsync(provider, uriScheme);
                    message = string.Format("Sie sind eingeloggt - {0}", _user.UserId);
                    success = true;
                }
                catch (InvalidOperationException)
                {
                    message = "You must log in. Login Required";
                }
                var dialog = new MessageDialog(message);
                dialog.Commands.Add(new UICommand("OK"));
                await dialog.ShowAsync();
            }
            return success;
        }
    }
}