﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SPHS.AppWindow.models;
using SPHS.AppWindow.parameters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPHS.AppWindow
{
    public class Utils
    {
        public static bool compareStringLike(string str1, string str2)
        {
            return str1.ToUpper() == str2.ToUpper();
        }

        // return -1: data input error
        // return seconds
        public static double subDateTime(string dt1, string dt2)
        {
            try
            {
                TimeSpan result = DateTime.Parse(dt1) - DateTime.Parse(dt2);
                return result.TotalSeconds;
            }
            catch
            {
                return -1;
            }
        }

        public static int getMoneyByDate(int _time, object _type)
        {
            int _m = (int)(_time / (60 * 60 * 24)) + 1;
            if (_type.ToString() == VEHICLETYPES.car.ToString())
                return _m * 50000;
            else
                return _m * 5000;
        }

        public static string createLinkAPI(object _collection, string _query)
        {
            string _base = Parameter_Special.ADDRESS_BASE_API + "/" + _collection.ToString() + $"?accessToken={Parameter_Special.USER_PRESENT.accessToken}";
            if (_query != null)
                return _base + $"&{_query}";

            return _base;
        }

        public static string ClassToJsonString<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        public static T JsonStringToClass<T>(string json)
        {
            T obj = Activator.CreateInstance<T>();
            MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            obj = (T)serializer.ReadObject(ms);
            ms.Close();
            return obj;
        }

        // data properties obj_in -> obj_out
        public static void MappingMessageToMessage(object obj_in, object obj_out)
        {
            PropertyInfo[] props_out = obj_out.GetType().GetProperties();
            foreach (var item in props_out)
            {
                if (obj_in.GetType().GetProperty(item.Name) == null)
                    continue;

                object value = obj_in.GetType().GetProperty(item.Name).GetValue(obj_in);
                if (value != null)
                {
                    obj_out.GetType().GetProperty(item.Name).SetValue(obj_out, value);
                }
            }
        }

        private static string ClassToJsonString(object _type, object obj)
        {
            if (_type.ToString() == COLLECTIONS.roles.ToString())
                return ClassToJsonString<roles>((roles)obj);

            if (_type.ToString() == COLLECTIONS.users.ToString())
                return ClassToJsonString<users>((users)obj);

            if (_type.ToString() == COLLECTIONS.transactions.ToString())
                return ClassToJsonString<transactions>((transactions)obj);

            if (_type.ToString() == COLLECTIONS.companies.ToString())
                return ClassToJsonString<companies>((companies)obj);

            if (_type.ToString() == COLLECTIONS.parkingtickets.ToString())
                return ClassToJsonString<parkingTickets>((parkingTickets)obj);

            if (_type.ToString() == COLLECTIONS.vehicletypes.ToString())
                return ClassToJsonString<vehicleTypes>((vehicleTypes)obj);

            return "";
        }

        private static List<object> getObjectByJObject(object _collection, JObject stuff)
        {
            List<object> results = new List<object>();
            int total = int.Parse(stuff[DATARESPONSE.total.ToString()].ToString());
            if (_collection.ToString() == COLLECTIONS.roles.ToString())
            {
                for (int i = 0; i < total; i++)
                {
                    roles _role = Utils.JsonStringToClass<roles>(stuff[DATARESPONSE.data.ToString()][i].ToString());
                    results.Add(_role);
                }
                return results;
            }

            if (_collection.ToString() == COLLECTIONS.users.ToString())
            {
                for (int i = 0; i < total; i++)
                {
                    users _user = Utils.JsonStringToClass<users>(stuff[DATARESPONSE.data.ToString()][i].ToString());
                    results.Add(_user);
                }
                return results;
            }

            if (_collection.ToString() == COLLECTIONS.transactions.ToString())
            {
                for (int i = 0; i < total; i++)
                {
                    transactions _transaction = Utils.JsonStringToClass<transactions>(stuff[DATARESPONSE.data.ToString()][i].ToString());
                    results.Add(_transaction);
                }
                return results;
            }

            if (_collection.ToString() == COLLECTIONS.companies.ToString())
            {
                for (int i = 0; i < total; i++)
                {
                    companies _company = Utils.JsonStringToClass<companies>(stuff[DATARESPONSE.data.ToString()][i].ToString());
                    results.Add(_company);
                }
                return results;
            }

            if (_collection.ToString() == COLLECTIONS.parkingtickets.ToString())
            {
                for (int i = 0; i < total; i++)
                {
                    parkingTickets _parkingTicket = Utils.JsonStringToClass<parkingTickets>(stuff[DATARESPONSE.data.ToString()][i].ToString());
                    results.Add(_parkingTicket);
                }
                return results;
            }

            if (_collection.ToString() == COLLECTIONS.vehicletypes.ToString())
            {
                for (int i = 0; i < total; i++)
                {
                    vehicleTypes _vehicleType = Utils.JsonStringToClass<vehicleTypes>(stuff[DATARESPONSE.data.ToString()][i].ToString());
                    results.Add(_vehicleType);
                }
                return results;
            }

            return results;
        }

        public static List<object> getAPI(object _collection, string _query)
        {
            string url = Utils.createLinkAPI(_collection, _query);
            List<object> results = new List<object>();
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
                        results = getObjectByJObject(_collection, stuff);
                    }
                }
            }
            return results;
        }

        public static bool postAPI(object _collection, object obj)
        {
            bool result = false;
            string url = Utils.createLinkAPI(_collection, "");
            using (var httpClient = new HttpClient())
            {
                var _json = ClassToJsonString(_collection, obj);
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
                        result = true;
                    }
                }
            }

            return result;
        }

        public static bool putAPI(object _collection, object obj)
        {
            bool result = false;
            string url = Utils.createLinkAPI(_collection, "");
            using (var httpClient = new HttpClient())
            {
                var _json = ClassToJsonString(_collection, obj);
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

        public static bool deteleAPI(object _collection, string _id)
        {
            bool result = false;
            string url = Utils.createLinkAPI(_collection, $"_id={_id}");
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

        public static string convertNumberPlate(string _numberPlate)
        {
            _numberPlate = _numberPlate.Replace("\n", "");
            _numberPlate = _numberPlate.Replace("\r", "");
            _numberPlate = _numberPlate.Replace(".", "");
            _numberPlate = _numberPlate.Replace("-", "");

            return _numberPlate;
        }

        public static string convertTimeToString(int _second)
        {
            string s = "";
            int _temp = _second;
            int _day = _second / (24 * 60 * 60);
            if (_day > 0)
            {
                s += $"{_day}d ";
                _temp = _temp - _day * 24 * 60 * 60;
            }
            int _hour = _temp / (60 * 60);
            if (s.Length > 0 || _hour > 0)
            {
                s += $"{_hour}h ";
                _temp = _temp - 60 * 60;
            }
            int _m = _temp / 60;
            int _s = _temp - _m * 60;
            s += $"{_m}m {_s}s";
            return s.Trim();
        }

        public static byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                return ms.ToArray();
            }
        }

        public static void setupFolder()
        {
            if (!Directory.Exists(Parameter_Special.FOLDER_IMAGE))
            {
                Directory.CreateDirectory(Parameter_Special.FOLDER_IMAGE);
            }
        }
    }
}
