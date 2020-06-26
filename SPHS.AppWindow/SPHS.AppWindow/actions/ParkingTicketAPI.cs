﻿using Newtonsoft.Json.Linq;
using SPHS.AppWindow.models;
using SPHS.AppWindow.parameters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SPHS.AppWindow.actions
{
    public class ParkingTicketAPI
    {
        public static parkingTickets post(parkingTickets _ticket)
        {
            parkingTickets result = new parkingTickets();
            string url = Utils.createLinkAPI(COLLECTIONS.parkingtickets, "");
            using (var httpClient = new HttpClient())
            {
                var _json = Utils.ClassToJsonString<parkingTickets>(_ticket);
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
                        return Utils.JsonStringToClass<parkingTickets>(stuff[DATARESPONSE.data.ToString()][0].ToString());
                    }
                }
            }
            return result;
        }

        public static bool upload(parkingTickets _ticket, byte[] image, bool isIn)
        {
            bool result = false;
            string url = Utils.createLinkAPI("uploads-parkingtickets", "");
            using (var client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent("images"))
                {
                    content.Add(new StringContent(_ticket.userId), "userId");
                    content.Add(new StringContent(_ticket._id), "_id");
                    if (isIn)
                        content.Add(new StringContent("true"), "uploadIn");
                    content.Add(new StreamContent(new MemoryStream(image)), "images", "upload.jpg");

                    var response = client.PostAsync(url, content);
                    response.Wait();
                    var _result = response.Result;
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