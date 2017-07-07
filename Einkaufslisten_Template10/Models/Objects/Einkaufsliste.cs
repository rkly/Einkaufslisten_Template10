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
        private String _id_user;
        public Einkaufsliste() : base() //leere Klasse für JSON Deserializer in RefreshEinkaufslisten()
        {
        }
        public Einkaufsliste(int id_item, string name) : base(id_item, name)
        {
        }
        public Einkaufsliste (int id_item, string name, DateTime aenderungsdatum, String id_user) : this(id_item, name)
        {
            _aenderungsdatum = aenderungsdatum;
            _id_user = id_user;
        }
        [JsonProperty(PropertyName = "aenderungsdatum")]
        public DateTime aenderungsdatum
        {
            get => _aenderungsdatum;
            set => _aenderungsdatum = value;
        }
        public String id_user
        {
            get => _id_user;
            set => _id_user = value;
        }
        public override string ToString()
        {
            return base.ToString() + " Aenderungsdatum = " + _aenderungsdatum;
        }
    }
}
