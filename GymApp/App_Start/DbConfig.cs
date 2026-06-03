using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GymApp.App_Start
{
    public static class DbConfig
    {
        public static string ConnectionString => System.Configuration.ConfigurationManager.ConnectionStrings["GymSys"].ConnectionString;
    }
}