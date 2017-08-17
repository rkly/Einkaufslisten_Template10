using System;

namespace Einkaufslisten_Template10.Models.Objects
{
    public class MobileServiceUser_Erweitert
    {
        public Message Message { get; set; }    
    }
    public class Message
    {
        public String id { get; set; }
        public String name { get; set; }
        public String email { get; set; }
    }
}