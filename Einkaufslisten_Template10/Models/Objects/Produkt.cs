using System;

namespace Einkaufslisten_Template10.Models.Objects
{
    public class Produkt:BaseObject
    {   
        public Produkt() : base() //leere Klasse für JSON Deserializer in RefreshEinkaufslisten()
        {
        }                    
        public Produkt(String name) : base(name)
        {
        }
        public override String ToString()
        {
            return name;
        }
    }
}
