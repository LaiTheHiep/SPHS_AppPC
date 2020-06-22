using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SPHS.AppWindow.models
{
    public class dataResponse
    {
        public int total { get; set; }
        public JsonElement[] data { get; set; }
        public string errorName { get; set; }
        public string errorMessage { get; set; }
    }
}
