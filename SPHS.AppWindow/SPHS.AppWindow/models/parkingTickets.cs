using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPHS.AppWindow.models
{
    public class parkingTickets
    {
        public string _id { get; set; }
        public string name { get; set; }
        public string port { get; set; }
        public string description { get; set; }
        public string timeIn { get; set; }
        public string timeOut { get; set; }
        public string author { get; set; } // employee
        public string userId { get; set; } // vehicle parked
    }
}
