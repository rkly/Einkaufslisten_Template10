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
        public static Boolean eingeloggt = false;
        /// <summary>
        /// Define a method that performs the authentication process using Facebook sign-in. 
        /// </summary>
        public static async Task<bool> AuthenticateAsync()
        {
            Boolean success = false;
            String message;
            if (_user != null)
            {
                success = true;
            }
            else
            {                
                try
                {
                    var provider = MobileServiceAuthenticationProvider.Facebook;
                    String uriScheme = "einkaufslisten-scheme";
                    _user = await AuthenticateFacebook(provider, uriScheme);
                    success = true;
                    eingeloggt = true;
                }
                catch (InvalidOperationException)
                {
                    message = new Windows.ApplicationModel.Resources.ResourceLoader().GetString("SieMuessenSichEinloggen");
                }   
            }
            message = string.Format(new Windows.ApplicationModel.Resources.ResourceLoader().GetString("SieSindEingeloggt")+" - {0} - {1} - {2} ", _user.Message.id, _user.Message.name, _user.Message.email);          
            var dialog = new MessageDialog(message);
            dialog.Commands.Add(new UICommand("OK"));
            Views.Busy.SetBusy(false);
            await dialog.ShowAsync();
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