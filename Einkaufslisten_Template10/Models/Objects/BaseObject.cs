using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Einkaufslisten_Template10.Models.Objects
{
    public abstract class BaseObject
    {
        protected int _id;
        protected string _name;
        public BaseObject() //leer Klasse für JSON Deserializer in RefreshEinkaufslisten()
        {
        }
        public BaseObject(int id_item, string name)
        {
            _id = id_item;
            _name = name;
        }
        [JsonProperty(PropertyName = "id_item")]
        public int id_item
        {
            get => _id;
            set => _id = value;
        }
        [JsonProperty(PropertyName = "name")]
        public string name
        {
            get => _name;
            set => _name = value;
        }
        public override string ToString()
        {
            return "Id = " + _id + " Name = " + _name;
        }
        public string id { get; set; }
    }
}
