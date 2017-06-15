using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Einkaufslisten_Template10.Models.Objects
{
    public class Produkt:BaseObject
    {
        private int anzahl;
        private string mengenBezeichnung;
                       
        public Produkt(int id, string name) : base(id, name)
        {
        }
        public Produkt(int id, string name, int anzahl, string mengenBezeichnung) : this(id, name)
        {
            this.anzahl = anzahl;
            this.mengenBezeichnung = mengenBezeichnung;
        }

        public int Anzahl
        {
            get => anzahl; 
            set => anzahl = value; 
        }
        public string MengenBezeichnung
        {
            get => mengenBezeichnung; 
            set => mengenBezeichnung = value;
        }
        public override string ToString()
        {
            return base.ToString() + " Anzahl = " + anzahl + " Mengenbezeichnung = " + mengenBezeichnung;
        }
    }
}
