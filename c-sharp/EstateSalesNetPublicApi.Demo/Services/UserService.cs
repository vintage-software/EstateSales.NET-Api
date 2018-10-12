using System;
using System.IO;
using System.Linq;

namespace EstateSalesNetPublicApi.Demo.Services
{
    public class UserService
    {
        private static string userDataPath = "./user-data.txt";

        public static UserAction GetUserAction()
        {
            Console.WriteLine("Available User Actions");
            Console.WriteLine("  1 - View active sales");
            Console.WriteLine("  2 - Create a new sale");
            Console.WriteLine("  3 - Delete an existing sale");
            Console.WriteLine("  4 - Publish an existing sale");
            Console.WriteLine("  5 - Unpublish an existing sale");
            Console.WriteLine("  6 - Quit");
            return ConsoleHelpers.Prompt<UserAction>("Enter the action you want to perform");
        }

        public static User GetUser(string apiBaseUrl)
        {
            return new User(GetUserData(), apiBaseUrl);
        }

        private static UserData GetUserData()
        {
            if (File.Exists(userDataPath) == false)
            {
                string apiKey = ConsoleHelpers.Prompt<string>("Enter your API Key");
                int orgId = ConsoleHelpers.Prompt<int>("Enter your Org ID");

                Console.WriteLine("We will remember this information next time you use this application.");
                Console.WriteLine();

                WriteUserData(new UserData(apiKey, orgId));
            }
            else
            {
                Console.WriteLine("Reading user data from file...");
                Console.WriteLine();
            }

            return ReadUserData();
        }

        private static void WriteUserData(UserData data)
        {
            try
            {
                string[] lines = { data.ApiKey, data.OrgId.ToString() };
                File.WriteAllLines(userDataPath, lines);
            }
            catch (Exception exception)
            {
                throw new Exception("Unable to write user data", exception);
            }
        }

        private static UserData ReadUserData()
        {
            try
            {
                string[] lines = File.ReadLines(userDataPath).ToArray();
                return new UserData(lines[0], int.Parse(lines[1]));
            }
            catch (Exception exception)
            {
                throw new Exception("Unable to read user data", exception);
            }
        }
    }
}
