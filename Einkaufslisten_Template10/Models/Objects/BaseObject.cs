using System;

namespace Einkaufslisten_Template10.Models.Objects
{
    public abstract class BaseObject : IdObject
    {
        protected String _name;
        protected DateTime _updatedAt;
        public BaseObject() //leer Klasse für JSON Deserializer in RefreshEinkaufslisten()
        {
        }
        public BaseObject(String name)
        {
            _name = name;
        }
        public String name
        {
            get => _name;
            set => _name = value;
        }
        public DateTime updatedAt {
            get => _updatedAt;
            set => _updatedAt = value;
        }
    }
}