using Einkaufslisten_Template10.Services.SettingsServices;
using System;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Einkaufslisten_Template10.Models.Objects
{    
    public class StyleController : INotifyPropertyChanged
    {
        public static String prefix;
        public StyleController()
        {
            loadStyle();
        }       
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void OnPropertyChanged(String propertyName)
        {
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));       
        }

        private SolidColorBrush relativePanelSolidColorBrush;
        public SolidColorBrush RelativePanelSolidColorBrush
        {
            get { return relativePanelSolidColorBrush; }
            set
            {
                this.relativePanelSolidColorBrush = value;
                OnPropertyChanged("RelativePanelSolidColorBrush");
            }
        }
        private Style btnStyle;
        public Style BtnStyle
        {
            get { return btnStyle; }
            set
            {
                this.btnStyle = value;
                OnPropertyChanged("BtnStyle");
            }
        }
        public static Style btn_style; 
        public void changeStyle()
        {
            loadPrefix();
            loadStyle();
        }
        public void loadStyle()
        {
            if (prefix == null)
            {
                loadPrefix();
            }    
            var res = Application.Current.Resources;
            this.BtnStyle = (Style) res[prefix + "_btn_style"];
            this.RelativePanelSolidColorBrush = (SolidColorBrush) res[prefix + "_solidColorBrush_RelativePanel"];
        }
        private void loadPrefix()
        {
            String sprache = SettingsService.Instance.Sprache;
            if (sprache.Equals("de-de"))
            {
                prefix = "de";
            }
            else
            {
                prefix = "ru";
            }
        }
    }
}