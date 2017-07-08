using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Einkaufslisten_Template10.Models.Objects
{
    public class Produkt_Einkaufsliste : IdObject
    {
        public String id_einkaufsliste { get; set; }
        public String id_produkt { get; set; }
        public String id_einheit { get; set; }
        public int menge { get; set; }
        public Produkt_Einkaufsliste()
        {

        }
    }
}
