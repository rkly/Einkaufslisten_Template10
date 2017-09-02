using System;

namespace Einkaufslisten_Template10.Models.Objects
{
    public class Einheit : BaseObject
    {
        public Einheit() : base() //leere Klasse für JSON Deserializer in RefreshEinkaufslisten()
        {
        }                 
        public Einheit(String name) : base(name)
        {
        }
        public override String ToString()
        {
            return name;
        }
    }
}
