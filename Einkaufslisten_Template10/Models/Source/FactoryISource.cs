using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Einkaufslisten_Template10.Models.Objects;

namespace Einkaufslisten_Template10.Models.Source
{
    class FactoryISource
    {
        public ISource<Einkaufsliste> CreateEinkaufsListenSource()
        {
            return new FileReaderEinkaufslisten();
        }
        public ISource<Produkt> CreateProduktSource(int id)
        {
            return new FileReaderProdukt(id);
        }
    }
}
