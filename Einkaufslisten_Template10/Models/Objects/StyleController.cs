using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
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
        private String suffix;
        public String Suffix
        {
            get { return suffix; }
            set
            {
                this.suffix = value;
                OnPropertyChanged("Suffix");
            }
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
        public static Style btn_style = (Style)Application.Current.Resources[App.stylePrefix + "_btn_style"];

        public void changeStyle()
        {
            if (prefix == "ru")
            {
                prefix = "de";
            } else
            {
                prefix = "ru";
            }
            loadStyle();
           
        }
        public void loadStyle()
        {
            if (prefix == null)
            {
                prefix = "ru";
            }
            this.Suffix = prefix;
            this.BtnStyle = (Style) Application.Current.Resources[prefix + "_btn_style"];
            this.RelativePanelSolidColorBrush = (SolidColorBrush) Application.Current.Resources[prefix + "_solidColorBrush_RelativePanel"];
        }


    }

}
