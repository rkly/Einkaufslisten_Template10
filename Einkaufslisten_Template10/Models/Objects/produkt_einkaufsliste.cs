using System;

namespace Einkaufslisten_Template10.Models.Objects
{
    public class Produkt_Einkaufsliste : IdObject
    {
        public Produkt_Einkaufsliste() : base()
        {
        }
        public Produkt_Einkaufsliste(String id_einkaufsliste, String id_produkt, String id_einheit, UInt16 menge)
        {
            _id_einkaufsliste = id_einkaufsliste;
            _id_einheit = id_einheit;
            _id_produkt = id_produkt;
            _menge = menge;
        }
        private String _id_einkaufsliste;
        private String _id_produkt;
        private String _id_einheit;
        private UInt16 _menge; //0..65535 
        public String id_einkaufsliste
        {
            get => _id_einkaufsliste;
            set => _id_einkaufsliste = value;
        }
        public String id_produkt
        {
            get => _id_produkt;
            set => _id_produkt = value;
        }
        public String id_einheit
        {
            get => _id_einheit;
            set => _id_einheit = value;
        }
        public UInt16 menge
        {
            get => _menge;
            set => _menge = value;
        }
    }
}