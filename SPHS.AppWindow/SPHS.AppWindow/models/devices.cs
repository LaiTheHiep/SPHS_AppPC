using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPHS.AppWindow.models
{
    public class devices
    {
        public string _id { get; set; }
        public string name { get; set; }
        public List<string> cardIds { get; set; }
        public string companyId { get; set; }
    }
}
