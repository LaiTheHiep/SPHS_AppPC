using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPHS.AppWindow.models
{
    public class cards
    {
        public string _id { get; set; }
        public DateTime expired { get; set; }
        public string description { get; set; }
        public string deviceId { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }
    }
}
