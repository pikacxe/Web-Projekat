using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Http; 
using Newtonsoft.Json;
using Projekat.Models;
using Projekat.Repository;

namespace Projekat
{
    public class WebApiApplication : HttpApplication
    {
        private string data_path { get; set; }
        protected void Application_Start()
        {
            // Get server path for data files
            data_path = HttpContext.Current.Server.MapPath("~");

            // Read data from json files
            Load_Data();

            GlobalConfiguration.Configure(WebApiConfig.Register);

        }

        protected void Application_End()
        {
            // Save data to appropriate json files
            Save_Data();
        }

        private void Save_Data()
        {
            try
            {
                // Write admins to file
                string adminPath = Path.Combine(data_path, "App_Data/RepositoryFiles/Admins.json");
                if (DB.UsersList != null)
                {
                    // Find all admins
                    List<User> admins = DB.UsersList.FindAll(x => x.Role == UserType.Administrator);

                    // Save admins to Admin.json
                    File.WriteAllText(adminPath, JsonConvert.SerializeObject(admins, Formatting.Indented));
                    
                    // Remove admins from existing user list
                    foreach (var admin in admins)
                    {
                        DB.UsersList.Remove(admin);
                    }
                }

                // Write users to file
                string userPath = Path.Combine(data_path, "App_Data/RepositoryFiles/Users.json");
                if (DB.UsersList != null)
                {
                    File.WriteAllText(userPath, JsonConvert.SerializeObject(DB.UsersList, Formatting.Indented));
                }

                // Write products to file
                string productPath = Path.Combine(data_path, "App_Data/RepositoryFiles/Products.json");
                if (DB.ProductsList != null)
                {
                    File.WriteAllText(productPath, JsonConvert.SerializeObject(DB.ProductsList, Formatting.Indented));
                }

                // Write reviews to file
                string reviewPath = Path.Combine(data_path, "App_Data/RepositoryFiles/Reviews.json");
                if (DB.ReviewsList != null)
                {
                    File.WriteAllText(reviewPath, JsonConvert.SerializeObject(DB.ReviewsList, Formatting.Indented));
                }


                // Write orders to file
                string orderPath = Path.Combine(data_path, "App_Data/RepositoryFiles/Orders.json");
                if (DB.OrdersList != null)
                {
                    File.WriteAllText(orderPath, JsonConvert.SerializeObject(DB.OrdersList, Formatting.Indented));
                }
            }
            catch (Exception ex)
            {
                // Error logging
                string logPath = Path.Combine(data_path, "App_Data/logs.txt");
                File.WriteAllText(logPath, ex.Message);
            }
        }

        private void Load_Data()
        {
            try
            {
                // Read admins from file
                string adminPath =Path.Combine(data_path,"App_Data/RepositoryFiles/Admins.json");
                DB.UsersList = ReadFromFile<User>(adminPath);
                if (DB.UsersList == null)
                {
                    DB.UsersList = new List<User>();
                }

                // Read users from file
                string userPath =Path.Combine(data_path,"App_Data/RepositoryFiles/Users.json");
                var users = ReadFromFile<User>(userPath);
                if (users != null)
                {
                    DB.UsersList.AddRange(users);
                }

                // Read products from file
                string productPath =Path.Combine(data_path,"App_Data/RepositoryFiles/Products.json");
                DB.ProductsList = ReadFromFile<Product>(productPath);
                if (DB.ProductsList == null)
                {
                    DB.ProductsList = new List<Product>();
                }

                // Read reviews from file
                string reviewPath =Path.Combine(data_path,"App_Data/RepositoryFiles/Reviews.json");
                DB.ReviewsList = ReadFromFile<Review>(reviewPath);
                if (DB.ReviewsList == null)
                {
                    DB.ReviewsList = new List<Review>();
                }


                // Read orders from file
                string orderPath =Path.Combine(data_path,"App_Data/RepositoryFiles/Orders.json");
                DB.OrdersList = ReadFromFile<Order>(orderPath);
                if (DB.OrdersList == null)
                {
                    DB.OrdersList = new List<Order>();
                }
            }
            catch (Exception ex)
            {
                // Error logging
                string logPath =Path.Combine(data_path,"App_Data/logs.txt");
                File.WriteAllText(logPath, ex.Message);
            }
        }

        private List<T> ReadFromFile<T>(string path)
        {
            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<List<T>>(json);
        }
    }
}
