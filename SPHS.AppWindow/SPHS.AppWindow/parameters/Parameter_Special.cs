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
        public static string ADDRESS_BASE_API = "http://localhost:8080";
        public static users USER_PRESENT = new users();
        public static companies COMPANY_PRESENT = new companies();
    }

    public enum ROLES
    {
        admin,
        manager,
        employee,
        user
    }

    public enum VEHICLETYPES
    {
        car,
        motobike
    }

    public enum COLLECTIONS
    {
        roles,
        users,
        transactions,
        companies,
        parkingtickets,
        vehicletypes,
    }

    public enum LINK_SPECIALS
    {
        authentication // authentication, not collections, using to login
    }

    public enum DATARESPONSE
    {
        total,
        data,
        errorName,
        errorMessage
    }
}
