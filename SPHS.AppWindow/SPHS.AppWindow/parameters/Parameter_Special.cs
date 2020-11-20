﻿using SPHS.AppWindow.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPHS.AppWindow.parameters
{
    public class Parameter_Special
    {
        public static string UNKNOWN_STRING = "Unknown";
        //public static string ADDRESS_BASE_API = "http://localhost:8080";
        public static string ADDRESS_BASE_API = "https://sphs-lth.herokuapp.com";
        public static string ADDRESS_URL_IMAGE = "https://api.imgbb.com/1/upload?key=bbda9cabea0a8e3852cbe26df54d959c";
        public static string FOLDER_IMAGE = @"C:\SPHS_images";
        public static string FOLDER_DATA = @"C:\SPHS_data";
        public static users USER_PRESENT = new users();
        public static companies COMPANY_PRESENT = new companies();
        public static int TIME_ASYNC_DEVICE = 5000 * 60; // 5 minutes

        public static List<string> STATUS_ACCESS_VERIFY = new List<string>() 
        { 
            "NO ACCESS",
            "NOT PERMISSION",
            "NOT ROLE ACCESS",
            "ACCESS SUCCESS"
        };
        public static string ACCOUNT_DEFAULT = "admin";
        public static string PASSWORD_DEFAULT = "123456a@";
    }

    public enum ROLES
    {
        admin,
        security,
        manager,
        user
    }

    public enum VEHICLETYPES
    {
        car,
        motorbike
    }

    public enum COLLECTIONS
    {
        roles,
        users,
        transactions,
        companies,
        parkingtickets,
        vehicletypes,
        devices,
        cards
    }
    public enum LINK_SPECIALS
    {
        authentication, // authentication, not collections, using to login
        uploads
    }

    public enum DATARESPONSE
    {
        total,
        data,
        errorName,
        errorMessage
    }

    public enum CAMERASWITCH
    {
        CameraQRCode,
        CameraVehicleIn,
        CameraVehicleOut
    }
}
