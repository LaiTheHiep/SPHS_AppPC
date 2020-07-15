using Newtonsoft.Json.Linq;
using SPHS.AppWindow.models;
using SPHS.AppWindow.parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SPHS.AppWindow.actions
{
    public class UserAPI
    {
        public static users login(string _account, string _password)
        {
            users result = new users();
            string url = Parameter_Special.ADDRESS_BASE_API + "/" + LINK_SPECIALS.authentication.ToString();
            using (var httpClient = new HttpClient())
            {
                var _json = Utils.ClassToJsonString<users>(new users() { account = _account, password = _password });
                var _data = new StringContent(_json, Encoding.UTF8, "application/json");
                var response = httpClient.PostAsync(url, _data);
                response.Wait();
                var _result = response.Result;
                if (_result.IsSuccessStatusCode)
                {
                    var readTask = _result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    var data = readTask.Result;
                    JObject stuff = JObject.Parse(data);
                    if (stuff[DATARESPONSE.errorMessage.ToString()] == null)
                    {
                        result = Utils.JsonStringToClass<users>(stuff[DATARESPONSE.data.ToString()].ToString());
                    }
                }
            }

            return result;
        }
    }
}
