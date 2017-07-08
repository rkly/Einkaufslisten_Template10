using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Einkaufslisten_Template10.Models.Objects
{
    public abstract class BaseObject : IdObject
    {
        protected string _name;
        public BaseObject() //leer Klasse für JSON Deserializer in RefreshEinkaufslisten()
        {
        }
        public BaseObject(string name)
        {
            _name = name;
        }
        //[JsonProperty(PropertyName = "name")]
        public string name
        {
            get => _name;
            set => _name = value;
        }
        public DateTime updatedAt { get; set; }
    }
}
