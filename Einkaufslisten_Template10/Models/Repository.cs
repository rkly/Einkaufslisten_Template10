using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Einkaufslisten_Template10.Models
{
    class Repository<T>
    {
        List<T> repo;
        public List<T> Repo
        {
            get { return repo; }
            set { repo = value; }
        }
    }
}
