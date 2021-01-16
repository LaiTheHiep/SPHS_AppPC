using Newtonsoft.Json;
using SPHS.AppWindow.models;
using SPHS.AppWindow.parameters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SPHS.AppWindow.actions
{
    public class CompanyAPI
    {
        public static List<companies> Gets()
        {
            var textJson = File.ReadAllText($"{Parameter_Special.FOLDER_DATA}\\companies.json");
            var companies = string.IsNullOrEmpty(textJson)
                ? new List<companies>()
                : JsonConvert.DeserializeObject<List<companies>>(textJson);

            return companies;
        }

        public static List<devices> GetDevicesByCompanyId(string companyId)
        {
            var result = new List<devices>();
            Regex reg = new Regex($"device");
            var files = Directory.GetFiles(Parameter_Special.FOLDER_DATA, "*.json")
                .Where(p => reg.IsMatch(p)).ToList();

            foreach (var file in files)
            {
                var textJson = File.ReadAllText(file);
                if (!string.IsNullOrEmpty(textJson))
                {
                    var device = JsonConvert.DeserializeObject<devices>(textJson);
                    if(device.companyId == companyId)
                    {
                        result.Add(device);
                    }
                }
            }

            return result;
        }
    }
}
