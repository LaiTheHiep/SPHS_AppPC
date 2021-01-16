using AuthenticationService.Managers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SPHS.AppWindow.models;
using SPHS.AppWindow.parameters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPHS.AppWindow.actions
{
    public class UserAPI
    {
        public static users login(string _account, string _password)
        {
            users result = new users();
            //string url = Parameter_Special.ADDRESS_BASE_API + "/" + LINK_SPECIALS.authentication.ToString();
            //using (var httpClient = new HttpClient())
            //{
            //    var _json = Utils.ClassToJsonString<users>(new users() { account = _account, password = _password });
            //    var _data = new StringContent(_json, Encoding.UTF8, "application/json");
            //    var response = httpClient.PostAsync(url, _data);
            //    response.Wait();
            //    var _result = response.Result;
            //    if (_result.IsSuccessStatusCode)
            //    {
            //        var readTask = _result.Content.ReadAsStringAsync();
            //        readTask.Wait();
            //        var data = readTask.Result;
            //        JObject stuff = JObject.Parse(data);
            //        if (stuff[DATARESPONSE.errorMessage.ToString()] == null)
            //        {
            //            result = Utils.JsonStringToClass<users>(stuff[DATARESPONSE.data.ToString()].ToString());
            //        }
            //    }
            //}
            try
            {
                Regex reg = new Regex("user.");
                var files = Directory.GetFiles(Parameter_Special.FOLDER_DATA, "*.json")
                    .Where(p => reg.IsMatch(p)).ToList();
                foreach (var file in files)
                {
                    var textJson = File.ReadAllText(file);
                    bool isBreak = false;
                    if (!string.IsNullOrEmpty(textJson))
                    {
                        var stuff = JObject.Parse(textJson);
                        if (stuff["data"] != null)
                        {
                            var users = JsonConvert.DeserializeObject<List<users>>(stuff["data"].ToString());
                            foreach (var user in users)
                            {
                                if (user.account == _account)
                                {
                                    result = user;
                                    var jWTService = new JWTService();
                                    result.accessToken = jWTService.GenerateToken();
                                    isBreak = true;
                                    break;
                                }
                            }
                        }
                        if (isBreak) break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            return result;
        }

        public static users getUserByCompanyIdAndId(string _id, string cardId, string companyId)
        {
            users result = new users();
            try
            {
                Regex reg = new Regex($"user.{companyId}");
                var files = Directory.GetFiles(Parameter_Special.FOLDER_DATA, "*.json")
                    .Where(p => reg.IsMatch(p)).ToList();

                foreach (var file in files)
                {
                    var textJson = File.ReadAllText(file);
                    bool isBreak = false;
                    if (!string.IsNullOrEmpty(textJson))
                    {
                        var stuff = JObject.Parse(textJson);
                        if (stuff["data"] != null)
                        {
                            var users = JsonConvert.DeserializeObject<List<users>>(stuff["data"].ToString());
                            foreach (var user in users)
                            {
                                if (!string.IsNullOrEmpty(_id) && user._id == _id)
                                {
                                    result = user;
                                    isBreak = true;
                                    break;
                                }
                                else if(!string.IsNullOrEmpty(cardId) && user.cardIds.Contains(cardId))
                                {
                                    result = user;
                                    isBreak = true;
                                    break;
                                }
                            }
                        }
                        if (isBreak) break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return result;
        }
    }
}
