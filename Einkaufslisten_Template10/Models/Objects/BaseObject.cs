using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Einkaufslisten_Template10.Models.Objects
{
    public abstract class BaseObject
    {
        protected int id;
        protected string name;

        public BaseObject(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
        public int Id
        {
            get => id;
            set => id = value;
        }
        public string Name
        {
            get => name;
            set => name = value;
        }
        public override string ToString()
        {
            return "Id = " + id + " Name = " + name;
        }
    }
}
