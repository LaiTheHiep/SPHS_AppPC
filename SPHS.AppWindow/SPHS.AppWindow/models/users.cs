using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPHS.AppWindow.models
{
    public class users
    {
        public string _id { get; set; }
        public string account { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string companyId { get; set; }
        public string cmt { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string role { get; set; }
        public string numberPlate { get; set; }
        public int balance { get; set; }
        public string description { get; set; }
        public string vehicleColor { get; set; }
        public string vehicleBranch { get; set; }
        public string vehicleType { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }
        public string accessToken { get; set; }
    }
}
