using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Einkaufslisten_Template10.Models.Objects
{
    public class Einheit : BaseObject
    {
        public Einheit() : base() //leere Klasse für JSON Deserializer in RefreshEinkaufslisten()
        {
        }                 
        public Einheit(string name) : base(name)
        {
        }
    }
}
