using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using Projekat.Models;
using Projekat.Repository;

namespace Projekat
{
    public class WebApiApplication : HttpApplication
    {
        string data_path { get; set; }
        JsonSerializerSettings settings = new JsonSerializerSettings();
        Timer timer;
        CancellationTokenSource cancellationTokenSource;
        protected void Application_Start()
        {
            // Get server path for data files
            data_path = HttpContext.Current.Server.MapPath("~");

            // Set json formating
            settings.Formatting = Formatting.Indented;
            settings.DateFormatString = "dd/MM/yyyy";
            // Read data from json files
            Load_Data();

            try
            {
                // Prepare auto save task
                
                cancellationTokenSource = new CancellationTokenSource();
                int period = int.Parse(ConfigurationManager.AppSettings["AutoSavePeriod"]);
                TimeSpan interval = TimeSpan.FromMinutes(period);
                timer = new Timer(Save_Data, null, TimeSpan.Zero, interval);
            }
            catch (Exception ex)
            {
                // Error logging
                string logPath = Path.Combine(data_path, "App_Data/logs.txt");
                File.AppendAllText(logPath, $"[{DateTime.Now.ToString("dd.MM.yyyy - HH:mm:ss")}] {ex.Message}\n");
            }
            GlobalConfiguration.Configure(WebApiConfig.Register);

        }

        protected void Application_End()
        {
            // Save data to appropriate json files
            Save_Data(null);
            cancellationTokenSource.Cancel();
            timer.Dispose();
            cancellationTokenSource.Dispose();
        }

        private void Save_Data(object state)
        {
            if (cancellationTokenSource.Token.IsCancellationRequested)
            {
                return;
            }
            try
            {
                // Write admins to file
                string adminPath = Path.Combine(data_path, "App_Data/RepositoryFiles/Admins.json");
                if (DB.UsersList != null)
                {
                    // Find all admins
                    List<User> admins = DB.UsersList.FindAll(x => x.Role == UserType.Administrator);

                    // Save admins to Admin.json
                    File.WriteAllText(adminPath, JsonConvert.SerializeObject(admins, settings));

                }

                // Write users to file
                string userPath = Path.Combine(data_path, "App_Data/RepositoryFiles/Users.json");
                if (DB.UsersList != null)
                {
                    List<User> users = DB.UsersList.FindAll(x => x.Role != UserType.Administrator);
                    File.WriteAllText(userPath, JsonConvert.SerializeObject(users, settings));
                }

                // Write products to file
                string productPath = Path.Combine(data_path, "App_Data/RepositoryFiles/Products.json");
                if (DB.ProductsList != null)
                {
                    var products = DB.ProductsList.ToArray();
                    File.WriteAllText(productPath, JsonConvert.SerializeObject(products, settings));
                }

                // Write reviews to file
                string reviewPath = Path.Combine(data_path, "App_Data/RepositoryFiles/Reviews.json");
                if (DB.ReviewsList != null)
                {
                    var reviews = DB.ReviewsList.ToArray();
                    File.WriteAllText(reviewPath, JsonConvert.SerializeObject(reviews, settings));
                }


                // Write orders to file
                string orderPath = Path.Combine(data_path, "App_Data/RepositoryFiles/Orders.json");
                if (DB.OrdersList != null)
                {
                    var orders = DB.OrdersList.ToArray();
                    File.WriteAllText(orderPath, JsonConvert.SerializeObject(orders, settings));
                }
                WriteLog("Auto save complete");
            }
            catch (Exception ex)
            {
                // Error logging
                WriteLog(ex.Message);
            }
        }

        private void Load_Data()
        {
            try
            {
                // Read admins from file
                string adminPath = Path.Combine(data_path, "App_Data/RepositoryFiles/Admins.json");
                DB.UsersList = ReadFromFile<User>(adminPath);
                if (DB.UsersList == null)
                {
                    DB.UsersList = new List<User>();
                }

                // Read users from file
                string userPath = Path.Combine(data_path, "App_Data/RepositoryFiles/Users.json");
                var users = ReadFromFile<User>(userPath);
                if (users != null)
                {
                    DB.UsersList.AddRange(users);
                }

                // Read products from file
                string productPath = Path.Combine(data_path, "App_Data/RepositoryFiles/Products.json");
                DB.ProductsList = ReadFromFile<Product>(productPath);
                if (DB.ProductsList == null)
                {
                    DB.ProductsList = new List<Product>();
                }

                // Read reviews from file
                string reviewPath = Path.Combine(data_path, "App_Data/RepositoryFiles/Reviews.json");
                DB.ReviewsList = ReadFromFile<Review>(reviewPath);
                if (DB.ReviewsList == null)
                {
                    DB.ReviewsList = new List<Review>();
                }


                // Read orders from file
                string orderPath = Path.Combine(data_path, "App_Data/RepositoryFiles/Orders.json");
                DB.OrdersList = ReadFromFile<Order>(orderPath);
                if (DB.OrdersList == null)
                {
                    DB.OrdersList = new List<Order>();
                }
            }
            catch (Exception ex)
            {
                // Error logging
                string logPath = Path.Combine(data_path, "App_Data/logs.txt");
                File.WriteAllText(logPath, ex.Message);
            }
        }


        private void WriteLog(string message)
        {
            // Error logging
            string logPath = Path.Combine(data_path, "App_Data/logs.txt");
            File.AppendAllText(logPath, $"[{DateTime.Now.ToString("dd.MM.yyyy - HH:mm:ss")}] - {message}\n");
        }
        private List<T> ReadFromFile<T>(string path)
        {
            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<List<T>>(json, settings);
        }
    }
}
