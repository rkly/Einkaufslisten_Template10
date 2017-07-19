using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows.Input;
using Template10.Services.NavigationService;
using Einkaufslisten_Template10.ViewModels;

namespace Einkaufslisten_Template10.Models.Objects
{
    public class Einkaufsliste:BaseObject
    {
        private String _id_user;
        public Einkaufsliste() : base() //leere Klasse für JSON Deserializer in RefreshEinkaufslisten()
        {
        }
        public Einkaufsliste(string name) : base(name)
        {
        }
        public Einkaufsliste(string name, String id_user) : this(name)
        {
            _id_user = id_user;
        }
        public String id_user
        {
            get => _id_user;
            set => _id_user = value;
        }
    }
}