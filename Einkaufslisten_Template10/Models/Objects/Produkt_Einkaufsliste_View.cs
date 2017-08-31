using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using System.Windows.Input;

namespace Einkaufslisten_Template10.Models.Objects
{
    [DataTable("Einkaufsbereich_View")]
    public class Produkt_Einkaufsliste_View : IdObject
    {
        public Produkt_Einkaufsliste_View() : base()
        {
        }
        public Produkt_Einkaufsliste_View(String produkt, String einheit, UInt16 menge)
        {
            _einheit = einheit;
            _produkt = produkt;
            _menge = menge;
        }
        public Produkt_Einkaufsliste_View(String id_einkaufsliste, String produkt, String einheit, UInt16 menge)
        {
            _id_einkaufsliste = id_einkaufsliste;
            _einheit = einheit;
            _produkt = produkt;
            _menge = menge;
        }
        private String _id_einkaufsliste;
        private String _produkt;
        private String _einheit;
        private UInt16 _menge; //0..65535 
        public String id_einkaufsliste
        {
            get => _id_einkaufsliste;
            set => _id_einkaufsliste = value;
        }
        public String produkt
        {
            get => _produkt;
            set => _produkt = value;
        }
        public String einheit
        {
            get => _einheit;
            set => _einheit = value;
        }
        public UInt16 menge
        {
            get => _menge;
            set => _menge = value;
        }
    }
}