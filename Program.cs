using CsvHelper;
using HtmlAgilityPack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebUserProvisioning
{
    class Program
    {
        
        static void Main(string[] args)
        {
            List<String> failedUsers = new List<string>();

            try
            {
                //Trust all certificates - enable this if using over SSL and debugging with fiddler
                //System.Net.ServicePointManager.ServerCertificateValidationCallback =
                //    ((sender, certificate, chain, sslPolicyErrors) => true);

                List<UserAccount> data = GetUserData();

                if (data.Count() == 0)
                {
                    Console.WriteLine("No users found in csv!");
                }
                else {
                    ProvisionUsers(data, failedUsers);
                }

                Console.WriteLine("Process Complete. Processed {0} users with {1} failures. See log.txt for details", data.Count(), failedUsers.Count());
            }
            catch (Exception ex)
            {
                failedUsers.Add(ex.Message);
                Console.WriteLine("Process Failed" + ex.Message);
            }

            WriteLogFile(failedUsers);

            Console.ReadLine();

        }

        private static void WriteLogFile(List<string> failedUsers)
        {
            File.WriteAllLines(SessionConfig.Setup.LogFile, failedUsers.ConvertAll(Convert.ToString));
        }

        private static void ProvisionUsers(List<UserAccount> data, List<String> failedUsers)
        {
            int delayForSeconds = SessionConfig.Setup.Delay;

            BrowserSession session = new BrowserSession();
            ConsoleSpinner spin = new ConsoleSpinner();

            var response = Login(session);

            foreach (var user in data)
            {
                response = NavigateToManageUsers(session);
                response = AddUserAccount(session, response, user);

                var failed = CheckIfFailed(response, failedUsers, user);
                if (String.IsNullOrEmpty(failed))
                {
                    Console.WriteLine("Added User {0}", user.Email);
                }
                else
                {
                    failedUsers.Add(failed);
                    Console.WriteLine("User Failed {0}", user.Email);
                }

                OutputDelayToConsole(delayForSeconds, spin);
            }

        }

        private static void OutputDelayToConsole(int delayForSeconds, ConsoleSpinner spin)
        {
            Console.WriteLine("Waiting to process next user ");
            Stopwatch s = new Stopwatch();
            s.Start();
            while (s.Elapsed < TimeSpan.FromSeconds((delayForSeconds)))
            {
                Thread.Sleep(1);
                spin.Turn();
            };
            s.Stop();
        }

        private static string CheckIfFailed(HtmlDocument response, List<string> failedUsers, UserAccount user)
        {
            var validationErrors = response
                        .DocumentNode
                        .SelectNodes(SessionConfig.FormElements.ValidationTag);

            if (validationErrors != null)
            {
                return String.Format("User Failed: {0}. Reason: {1}", user.Email, validationErrors.FirstOrDefault().InnerText);
            }

            //check for the oops error page
            if(response.DocumentNode.SelectSingleNode("//head/title") != null && response.DocumentNode.SelectSingleNode("//head/title").InnerText == SessionConfig.FormElements.BrowserTitle)
            {
                return String.Format("User Failed: {0}. Redirected to error page", user.Email);
            }

            return "";
        }

        private static List<UserAccount> GetUserData()
        {
            // Values are available here
            var sourcePath = SessionConfig.Setup.CsvFile;

            using (var sr = new StreamReader(sourcePath))
            {
                Console.WriteLine("Get users from csv file");
                return ReadUsersFromFile(sr);
            }
        }

        private static List<UserAccount> ReadUsersFromFile(StreamReader sr)
        {
            var reader = new CsvReader(sr);

            var importedData = reader.GetRecords<UserAccount>().ToList();
           
            return importedData;
        }

        private static HtmlDocument AddUserAccount(BrowserSession session, HtmlDocument response, UserAccount user)
        {
            var token = GetRequestVerificationToken(response);
            if (!String.IsNullOrEmpty(token))
            {
                session.FormElements[SessionConfig.Headers.RequestVerificationToken] = token;
                session.FormElements[SessionConfig.FormElements.Email] = user.Email;
                session.FormElements[SessionConfig.FormElements.Password] = user.Password;
                session.FormElements[SessionConfig.FormElements.ConfirmPassword] = user.Password;

                return session.Post(SessionConfig.Urls.CreateUser);
            }
            return null;
        }

        private static string GetRequestVerificationToken(HtmlDocument response)
        {
            var reqVerTokenElement = response
                        .DocumentNode
                        .Descendants("input")
                        .Where(n => n.Attributes["name"] != null
                                    && n.Attributes["name"].Value
                                        == "__RequestVerificationToken")
                        .FirstOrDefault();
            if (reqVerTokenElement != null)
            {
                return reqVerTokenElement.Attributes["value"].Value;
            }
            return "";
        }

        private static HtmlDocument NavigateToManageUsers(BrowserSession session)
        {
            return session.NavigateTo(SessionConfig.Urls.ManageUsers);
        }

        private static HtmlDocument Login(BrowserSession session)
        {
            session.Get(SessionConfig.Urls.Login);
            session.FormElements[SessionConfig.FormElements.LoginEmail] = SessionConfig.FormInput.AdminUsername;
            session.FormElements[SessionConfig.FormElements.LoginPassword] = SessionConfig.FormInput.AdminPassword;
            return session.Post(SessionConfig.Urls.Login);
        }
    }
}
