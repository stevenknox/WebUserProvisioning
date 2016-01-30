using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUserProvisioning
{
    static class SessionConfig
    {
        public static class Headers
        {
            public static string ContentType { get { return ConfigurationManager.AppSettings["Headers.ContentType"]; } }
            public static string RequestVerificationToken { get { return ConfigurationManager.AppSettings["Headers.RequestVerificationToken"]; } }
        }

        public static class Urls
        {
            public static string Base { get { return ConfigurationManager.AppSettings["Urls.Base"]; } }
            public static string Login { get { return Base + ConfigurationManager.AppSettings["Urls.Login"]; } }
            public static string ManageUsers { get { return Base + ConfigurationManager.AppSettings["Urls.ManageUsers"]; } }
            public static string CreateUser { get { return Base + ConfigurationManager.AppSettings["Urls.CreateUser"]; } }

        }

        public static class FormElements
        {
            public static string LoginEmail { get { return ConfigurationManager.AppSettings["FormElements.LoginEmail"]; } }
            public static string LoginPassword { get { return ConfigurationManager.AppSettings["FormElements.LoginPassword"]; } }
            public static string Email { get { return ConfigurationManager.AppSettings["FormElements.Email"]; } }
            public static string Password { get { return ConfigurationManager.AppSettings["FormElements.Password"]; } }
            public static string ConfirmPassword { get { return ConfigurationManager.AppSettings["FormElements.ConfirmPassword"]; } }
            public static string BrowserTitle { get { return ConfigurationManager.AppSettings["FormElements.BrowserTitle"]; } }
            public static string ValidationTag { get { return ConfigurationManager.AppSettings["FormElements.ValidationTag"]; } }
        }

        public static class FormInput
        {

            public static string AdminUsername { get { return ConfigurationManager.AppSettings["FormInput.AdminUsername"]; } }
            public static string AdminPassword { get { return ConfigurationManager.AppSettings["FormInput.AdminPassword"]; } }
        }

        public static class Setup
        {
            public static string CsvFile { get { return ConfigurationManager.AppSettings["Setup.CsvFile"]; } }
            public static string LogFile { get { return ConfigurationManager.AppSettings["Setup.LogFile"]; } }
            public static int Delay { get { return Convert.ToInt16(ConfigurationManager.AppSettings["Setup.Delay"]); } }
        }
    }

   
}
