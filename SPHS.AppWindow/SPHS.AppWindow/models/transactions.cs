using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPHS.AppWindow.models
{
    public class transactions
    {
        public string _id { get; set; }
        public string userId { get; set; } // user
        public string author { get; set; } // employee transaction user
        public string money { get; set; }
        public string content { get; set; }
        public string description { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }
    }
}
