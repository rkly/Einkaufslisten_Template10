using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Einkaufslisten_Template10.Models.Source
{
    interface ISource <T>
    {
        Task<T> Create(T t);
        Task<Collection<T>> GetRepo();
        void Update(T einkaufsliste);
        void Delete(T einkaufsliste);
    }
}
