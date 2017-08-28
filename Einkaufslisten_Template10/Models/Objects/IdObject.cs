using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Einkaufslisten_Template10.Models.Objects
{
    public abstract class IdObject
    {
        public IdObject()
        {
        }
        public IdObject(String id)
        {
            _id = id;
        }
        protected String _id;
        public String id
        {
            get => _id;
            set => _id = value;
        }
    }
}
