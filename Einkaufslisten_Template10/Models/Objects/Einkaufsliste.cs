using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Einkaufslisten_Template10.Models.Objects
{
    public class Einkaufsliste:BaseObject
    {
        private DateTime aenderungsdatum;

        public Einkaufsliste(int id, string name) : base(id, name)
        {
        }
        public Einkaufsliste (int id, string name, DateTime aenderungsdatum) : this(id, name)
        {
            this.aenderungsdatum = aenderungsdatum;
        }
        public DateTime Aenderungsdatum
        {
            get => aenderungsdatum;
            set => aenderungsdatum = value;
        }
        public override string ToString()
        {
            return base.ToString() + " Aenderungsdatum = " + aenderungsdatum;
        }
    }
}
