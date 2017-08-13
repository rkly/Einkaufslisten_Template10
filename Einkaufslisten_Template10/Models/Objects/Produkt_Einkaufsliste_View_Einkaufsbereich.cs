using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Einkaufslisten_Template10.Models.Objects
{
    public class Produkt_Einkaufsliste_View_Einkaufsbereich:Produkt_Einkaufsliste_View
    {
        public Produkt_Einkaufsliste_View_Einkaufsbereich() : base()
        {
        }
        public Produkt_Einkaufsliste_View_Einkaufsbereich(String id_einkaufsliste, String produkt, String einheit, UInt16 menge) : base(id_einkaufsliste, produkt, einheit, menge)
        {
        }
        public Produkt_Einkaufsliste_View_Einkaufsbereich(String id_einkaufsliste, String produkt, String einheit, UInt16 menge, bool inCard) : base(id_einkaufsliste, produkt, einheit, menge)
        {
            InCard = inCard;
        }

        bool _inCard = false;
        [JsonProperty(PropertyName = "incard")]
        public bool InCard
        {
            get => _inCard;
            set => _inCard = value;
        }
        
    }
}
