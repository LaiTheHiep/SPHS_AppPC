using Newtonsoft.Json.Linq;
using SPHS.AppWindow.models;
using SPHS.AppWindow.parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPHS.AppWindow.actions
{
    public class RoleAPI
    {
        public static List<roles> get(string _query)
        {
            string url = Utils.createLinkAPI(COLLECTIONS.roles, _query);
            List<roles> results = new List<roles>();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml+json");
                var response = httpClient.GetAsync(url);
                response.Wait();
                var result = response.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    var data = readTask.Result;
                    JObject stuff = JObject.Parse(data);
                    if (stuff[DATARESPONSE.errorMessage.ToString()] == null)
                    {
                        int total = int.Parse(stuff[DATARESPONSE.total.ToString()].ToString());
                        for (int i = 0; i < total; i++)
                        {
                            roles _role = Utils.JsonStringToClass<roles>(stuff[DATARESPONSE.data.ToString()][i].ToString());
                            results.Add(_role);
                        }
                    }
                }
            }
            return results;
        }

        public static bool post(roles _role)
        {
            bool result = false;
            string url = Utils.createLinkAPI(COLLECTIONS.roles, "");
            using(var httpClient = new HttpClient())
            {
                var _json = Utils.ClassToJsonString<roles>(_role);
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
                    if(stuff[DATARESPONSE.errorMessage.ToString()] == null)
                    {
                        result = true;
                    }
                }
            }

            return result;
        }

        public static bool put(roles _role)
        {
            bool result = false;
            string url = Utils.createLinkAPI(COLLECTIONS.roles, "");
            using (var httpClient = new HttpClient())
            {
                var _json = Utils.ClassToJsonString<roles>(_role);
                var _data = new StringContent(_json, Encoding.UTF8, "application/json");
                var response = httpClient.PutAsync(url, _data);
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
                        result = true;
                    }
                }
            }

            return result;
        }

        public static bool detele(string _id)
        {
            bool result = false;
            string url = Utils.createLinkAPI(COLLECTIONS.roles, $"_id={_id}");
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml+json");
                var response = httpClient.DeleteAsync(url);
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
                        result = true;
                    }
                }
            }

            return result;
        }
    }
}
