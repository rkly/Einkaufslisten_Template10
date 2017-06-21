using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Einkaufslisten_Template10.Models.Objects
{
    public class Einkaufsliste:BaseObject
    {
        private DateTime _aenderungsdatum;
        public Einkaufsliste(int id_item, string name) : base(id_item, name)
        {
        }
        public Einkaufsliste (int id_item, string name, DateTime aenderungsdatum) : this(id_item, name)
        {
            _aenderungsdatum = aenderungsdatum;
        }
        [JsonProperty(PropertyName = "aenderungsdatum")]
        public DateTime aenderungsdatum
        {
            get => _aenderungsdatum;
            set => _aenderungsdatum = value;
        }
        public override string ToString()
        {
            return base.ToString() + " Aenderungsdatum = " + _aenderungsdatum;
        }
    }
}
