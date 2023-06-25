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
        protected void Application_Start()
        {
            Load_Data();
            GlobalConfiguration.Configure(WebApiConfig.Register);

        }
        private void Load_Data()
        {
            try
            {
                // Read admins from file
                string adminPath = HttpContext.Current.Server.MapPath("~/App_Data/RepositoryFiles/Admins.json");
                DB.UsersList = ReadFromFile<User>(adminPath);
                if (DB.UsersList == null)
                {
                    DB.UsersList = new List<User>();
                }

                // Read users from file
                string userPath = HttpContext.Current.Server.MapPath("~/App_Data/RepositoryFiles/Users.json");
                var users = ReadFromFile<User>(userPath);
                if (users != null)
                {
                    DB.UsersList.AddRange(users);
                }

                // Read products from file
                string productPath = HttpContext.Current.Server.MapPath("~/App_Data/RepositoryFiles/Products.json");
                DB.ProductsList = ReadFromFile<Product>(productPath);
                if (DB.ProductsList == null)
                {
                    DB.ProductsList = new List<Product>();
                }

                // Read reviews from file
                string reviewPath = HttpContext.Current.Server.MapPath("~/App_Data/RepositoryFiles/Reviews.json");
                DB.ReviewsList = ReadFromFile<Review>(reviewPath);
                if (DB.ReviewsList == null)
                {
                    DB.ReviewsList = new List<Review>();
                }


                // Read orders from file
                string orderPath = HttpContext.Current.Server.MapPath("~/App_Data/RepositoryFiles/Orders.json");
                DB.OrdersList = ReadFromFile<Order>(orderPath);
                if (DB.OrdersList == null)
                {
                    DB.OrdersList = new List<Order>();
                }
            }
            catch (Exception ex)
            {
                string logPath = HttpContext.Current.Server.MapPath("~/App_Data/logs.txt");
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
