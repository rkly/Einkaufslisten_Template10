using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Windows.UI.Popups;
using System.Net.Http;
using Einkaufslisten_Template10.Models.Objects;

namespace Einkaufslisten_Template10.Services.AzureServices
{
    public class AuthService
    { 
        /// <summary>
        /// Define a member variable for storing the signed-in user
        /// </summary>
        private static MobileServiceUser_Erweitert _user;
        /// <summary>
        /// Define a method that performs the authentication process using Facebook sign-in. 
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
                    string uriScheme = "einkaufslisten-scheme";
                    _user = await AuthenticateFacebook(provider, uriScheme);
                    message = string.Format("Sie sind eingeloggt - {0} - {1} - {2} ", _user.Message.id, _user.Message.name, _user.Message.email);
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
        private static async Task<MobileServiceUser_Erweitert> GetUserData()
        {
            return await SyncService.MobileService.InvokeApiAsync<MobileServiceUser_Erweitert>(
                    "getextrauserinfo",
                    HttpMethod.Get,
                    null);
        }
        public static async Task<MobileServiceUser_Erweitert> AuthenticateFacebook(MobileServiceAuthenticationProvider provider, String uriScheme)
        {
            await Authenticate(provider, uriScheme);
            return await GetUserData();
        }
        private static async Task<MobileServiceUser> Authenticate(MobileServiceAuthenticationProvider provider, String uriScheme)
        {
            try
            {
                return await SyncService.MobileService.LoginAsync(provider, uriScheme);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}