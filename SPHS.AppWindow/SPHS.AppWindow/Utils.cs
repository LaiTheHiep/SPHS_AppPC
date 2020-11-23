using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SPHS.AppWindow.actions;
using SPHS.AppWindow.models;
using SPHS.AppWindow.parameters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;

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
                return result.TotalSeconds + 7 * 3600;
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

            if (_collection.ToString() == COLLECTIONS.devices.ToString())
            {
                for (int i = 0; i < total; i++)
                {
                    devices _device = Utils.JsonStringToClass<devices>(stuff[DATARESPONSE.data.ToString()][i].ToString());
                    results.Add(_device);
                }
                return results;
            }

            if (_collection.ToString() == COLLECTIONS.cards.ToString())
            {
                for (int i = 0; i < total; i++)
                {
                    cards _card = Utils.JsonStringToClass<cards>(stuff[DATARESPONSE.data.ToString()][i].ToString());
                    results.Add(_card);
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

        public static string ScanQRCodeByBitMap(Bitmap bm)
        {
            IBarcodeReader reader = new BarcodeReader();
            var result = reader.Decode(bm);
            if (result != null)
            {
                return result.Text;
            }

            return null;
        }

        public static void setupFolder()
        {
            if (!Directory.Exists(Parameter_Special.FOLDER_IMAGE))
            {
                Directory.CreateDirectory(Parameter_Special.FOLDER_IMAGE);
            }
            if (!Directory.Exists(Parameter_Special.FOLDER_DATA))
            {
                Directory.CreateDirectory(Parameter_Special.FOLDER_DATA);
            }
        }

        public static int verifyQRCode(string qrCode, string deviceId)
        {
            int resultVerify = 1;
            JObject stuffQRCode = JObject.Parse(qrCode);
            if (stuffQRCode["_id"] == null) return resultVerify;

            string _id = stuffQRCode["_id"].ToString();
            string url = Utils.createLinkAPI(COLLECTIONS.users, $"_id={_id}");
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
                        try
                        {
                            if (stuff["data"][0]["devicesAccess"] != null && stuff["data"][0]["devicesAccess"][deviceId] != null)
                            {
                                DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                DateTime now = start.AddMilliseconds(long.Parse(stuffQRCode["createdTime"].ToString())).ToLocalTime();
                                string dataDeviceAccess = stuff["data"][0]["devicesAccess"][deviceId][(int)now.DayOfWeek].ToString();
                                var deviceAccess = JsonStringToClass<deviceAccess>(dataDeviceAccess);
                                string[] froms = deviceAccess.dateTimeFrom.Split('/');
                                string[] tos = deviceAccess.dateTimeTo.Split('/');
                                if (int.Parse(froms[0]) <= now.Hour && int.Parse(tos[0]) >= now.Hour
                                    && int.Parse(froms[1]) <= now.Minute && int.Parse(tos[1]) >= now.Minute
                                    && int.Parse(froms[2]) <= now.Second && int.Parse(tos[2]) >= now.Second)
                                {
                                    resultVerify = 3;
                                    Utils.postAPI(COLLECTIONS.parkingtickets, new parkingTickets()
                                    {
                                        author = _id,
                                        userId = _id,
                                        companyId = deviceId,
                                        port = "event",
                                        description = "event",
                                        timeIn = stuffQRCode["createdTime"].ToString(),
                                        timeOut = stuffQRCode["createdTime"].ToString()
                                    });
                                }
                                else
                                {
                                    resultVerify = 2;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            return resultVerify;
                        }
                    }
                }
            }

            return resultVerify;
        }

        public static int verifyCard(string cardId, string deviceId)
        {
            int resultVerify = 1;

            string url = Utils.createLinkAPI(COLLECTIONS.users, $"cardIds={cardId}");
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
                        try
                        {
                            if (stuff["data"][0]["devicesAccess"] != null && stuff["data"][0]["devicesAccess"][deviceId] != null)
                            {
                                DateTime now = DateTime.Now;
                                string dataDeviceAccess = stuff["data"][0]["devicesAccess"][deviceId][(int)now.DayOfWeek].ToString();
                                var deviceAccess = JsonStringToClass<deviceAccess>(dataDeviceAccess);
                                string[] froms = deviceAccess.dateTimeFrom.Split('/');
                                string[] tos = deviceAccess.dateTimeTo.Split('/');
                                if (int.Parse(froms[0]) <= now.Hour && int.Parse(tos[0]) >= now.Hour
                                    && int.Parse(froms[1]) <= now.Minute && int.Parse(tos[1]) >= now.Minute
                                    && int.Parse(froms[2]) <= now.Second && int.Parse(tos[2]) >= now.Second)
                                {
                                    resultVerify = 3;
                                    Utils.postAPI(COLLECTIONS.parkingtickets, new parkingTickets()
                                    {
                                        author = stuff["data"][0]["_id"].ToString(),
                                        userId = stuff["data"][0]["_id"].ToString(),
                                        companyId = deviceId,
                                        port = "event",
                                        description = "event",
                                        timeIn = now.ToString(),
                                        timeOut = now.ToString()
                                    });
                                }
                                else
                                {
                                    resultVerify = 2;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            return resultVerify;
                        }
                    }
                }
            }

            return resultVerify;
        }

        public static string get(object _collection, string _query)
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
                        return data;
                    }
                }
            }
            return null;
        }


        public static void AsyncDataDevice()
        {
            try
            {
                if (string.IsNullOrEmpty(Parameter_Special.USER_PRESENT.accessToken))
                {
                    Parameter_Special.USER_PRESENT = UserAPI.login(Parameter_Special.ACCOUNT_DEFAULT, Parameter_Special.PASSWORD_DEFAULT);
                }
                // get all device
                var devices = getAPI(COLLECTIONS.devices, "");
                foreach (devices device in devices)
                {
                    // data device
                    if (!File.Exists($"{Parameter_Special.FOLDER_DATA}\\device.{device._id}.json"))
                    {
                        using (var fileStream = new FileStream($"{Parameter_Special.FOLDER_DATA}\\device.{device._id}.json", FileMode.OpenOrCreate, FileAccess.ReadWrite))
                        {
                            using (StreamWriter sw = new StreamWriter(fileStream))
                            {
                                sw.WriteLine(ClassToJsonString<devices>(device));
                            }
                        }
                    }
                    else
                    {
                        using (var fileStream = new FileStream($"{Parameter_Special.FOLDER_DATA}\\device.{device._id}.json", FileMode.OpenOrCreate, FileAccess.ReadWrite))
                        {
                            byte[] bytes = new byte[fileStream.Length];
                            int numBytesToRead = (int)fileStream.Length;
                            int numBytesRead = 0;
                            while (numBytesToRead > 0)
                            {
                                int n = fileStream.Read(bytes, numBytesRead, numBytesToRead);
                                if (n == 0)
                                    break;

                                numBytesRead += n;
                                numBytesToRead -= n;
                            }

                            string textJson = Encoding.UTF8.GetString(bytes);
                            textJson = textJson.Trim();
                            if (textJson != ClassToJsonString<devices>(device))
                            {
                                using (StreamWriter sw = new StreamWriter(fileStream))
                                {
                                    sw.WriteLine(ClassToJsonString<devices>(device));
                                }
                            }
                        }
                    }

                    // data user by company
                    string userCompany = get(COLLECTIONS.users, $"companyId={device.companyId}");
                    dataResponse responseUser = JsonStringToClass<dataResponse>(userCompany);
                    if (!string.IsNullOrEmpty(responseUser.errorMessage))
                        continue;
                    if (!File.Exists($"{Parameter_Special.FOLDER_DATA}\\user.{device.companyId}.json"))
                    {
                        using (var fileStream = new FileStream($"{Parameter_Special.FOLDER_DATA}\\user.{device.companyId}.json", FileMode.OpenOrCreate, FileAccess.ReadWrite))
                        {
                            using (StreamWriter sw = new StreamWriter(fileStream))
                            {
                                sw.WriteLine(userCompany);
                            }
                        }
                    }
                    else
                    {
                        using (var fileStream = new FileStream($"{Parameter_Special.FOLDER_DATA}\\user.{device.companyId}.json", FileMode.OpenOrCreate, FileAccess.ReadWrite))
                        {
                            byte[] bytes = new byte[fileStream.Length];
                            int numBytesToRead = (int)fileStream.Length;
                            int numBytesRead = 0;
                            while (numBytesToRead > 0)
                            {
                                int n = fileStream.Read(bytes, numBytesRead, numBytesToRead);
                                if (n == 0)
                                    break;

                                numBytesRead += n;
                                numBytesToRead -= n;
                            }

                            string textJson = Encoding.UTF8.GetString(bytes);
                            textJson = textJson.Trim();
                            if (textJson != userCompany)
                            {
                                using (StreamWriter sw = new StreamWriter(fileStream))
                                {
                                    sw.WriteLine(userCompany);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Parameter_Special.USER_PRESENT = UserAPI.login(Parameter_Special.ACCOUNT_DEFAULT, Parameter_Special.PASSWORD_DEFAULT);
            }
        }

        // string json
        // format: {total: number, data: []}
        public static string readDataInFile(string pathFile)
        {
            if (pathFile.IndexOf(Parameter_Special.FOLDER_DATA) < 0)
                pathFile = $"{Parameter_Special.FOLDER_DATA}\\{pathFile}";
            if (File.Exists(pathFile))
            {
                using (var fileStream = new FileStream(pathFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    byte[] bytes = new byte[fileStream.Length];
                    int numBytesToRead = (int)fileStream.Length;
                    int numBytesRead = 0;
                    while (numBytesToRead > 0)
                    {
                        int n = fileStream.Read(bytes, numBytesRead, numBytesToRead);
                        if (n == 0)
                            break;

                        numBytesRead += n;
                        numBytesToRead -= n;
                    }

                    string textJson = Encoding.UTF8.GetString(bytes);
                    return textJson;
                }
            }

            return null;
        }

        #region Excute local

        public static int verfyQRCodeInLocal(string qrCode, devices device)
        {
            int resultVerify = 1;
            JObject stuffQRCode = JObject.Parse(qrCode);
            if (stuffQRCode["_id"] == null) return resultVerify;
            var deviceId = device._id;

            string _id = stuffQRCode["_id"].ToString();
            using (var httpClient = new HttpClient())
            {
                string data = readDataInFile($"{Parameter_Special.FOLDER_DATA}\\user.{device.companyId}.json");
                if (!string.IsNullOrEmpty(data))
                {
                    JObject stuff = JObject.Parse(data);
                    if (stuff[DATARESPONSE.errorMessage.ToString()] == null)
                    {
                        try
                        {
                            if (stuff["data"][0]["devicesAccess"] != null && stuff["data"][0]["devicesAccess"][deviceId] != null)
                            {
                                DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                DateTime now = start.AddMilliseconds(long.Parse(stuffQRCode["createdTime"].ToString())).ToLocalTime();
                                string dataDeviceAccess = stuff["data"][0]["devicesAccess"][deviceId][(int)now.DayOfWeek].ToString();
                                var deviceAccess = JsonStringToClass<deviceAccess>(dataDeviceAccess);
                                string[] froms = deviceAccess.dateTimeFrom.Split('/');
                                string[] tos = deviceAccess.dateTimeTo.Split('/');
                                if (int.Parse(froms[0]) <= now.Hour && int.Parse(tos[0]) >= now.Hour
                                    && int.Parse(froms[1]) <= now.Minute && int.Parse(tos[1]) >= now.Minute
                                    && int.Parse(froms[2]) <= now.Second && int.Parse(tos[2]) >= now.Second)
                                {
                                    resultVerify = 3;
                                    //Utils.postAPI(COLLECTIONS.parkingtickets, new parkingTickets()
                                    //{
                                    //    author = _id,
                                    //    userId = _id,
                                    //    companyId = deviceId,
                                    //    port = "event",
                                    //    description = "event",
                                    //    timeIn = stuffQRCode["createdTime"].ToString(),
                                    //    timeOut = stuffQRCode["createdTime"].ToString()
                                    //});
                                }
                                else
                                {
                                    resultVerify = 2;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            return resultVerify;
                        }
                    }
                }
            }

            return resultVerify;
        }

        public static int verifyCardInLocal(string cardId, devices device)
        {
            int resultVerify = 1;
            var deviceId = device._id;
            var data = readDataInFile($"user.{device.companyId}.json");
            if (!string.IsNullOrEmpty(data) && data.IndexOf($"\"{cardId}\"") > 0)
            {
                JObject stuff = JObject.Parse(data);
                if (stuff[DATARESPONSE.errorMessage.ToString()] == null)
                {
                    try
                    {
                        if (stuff["data"][0]["devicesAccess"] != null && stuff["data"][0]["devicesAccess"][deviceId] != null)
                        {
                            DateTime now = DateTime.Now;
                            string dataDeviceAccess = stuff["data"][0]["devicesAccess"][deviceId][(int)now.DayOfWeek].ToString();
                            var deviceAccess = JsonStringToClass<deviceAccess>(dataDeviceAccess);
                            string[] froms = deviceAccess.dateTimeFrom.Split('/');
                            string[] tos = deviceAccess.dateTimeTo.Split('/');
                            if (int.Parse(froms[0]) <= now.Hour && int.Parse(tos[0]) >= now.Hour
                                && int.Parse(froms[1]) <= now.Minute && int.Parse(tos[1]) >= now.Minute
                                && int.Parse(froms[2]) <= now.Second && int.Parse(tos[2]) >= now.Second)
                            {
                                resultVerify = 3;
                                //Utils.postAPI(COLLECTIONS.parkingtickets, new parkingTickets()
                                //{
                                //    author = stuff["data"][0]["_id"].ToString(),
                                //    userId = stuff["data"][0]["_id"].ToString(),
                                //    companyId = deviceId,
                                //    port = "event",
                                //    description = "event",
                                //    timeIn = now.ToString(),
                                //    timeOut = now.ToString()
                                //});
                            }
                            else
                            {
                                resultVerify = 2;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return resultVerify;
                    }
                }
            }

            return resultVerify;
        }

        #endregion
    }
}
