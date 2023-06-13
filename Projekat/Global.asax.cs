using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Newtonsoft.Json;
using Projekat.Models;
using Projekat.Repository;

namespace Projekat
{
    public class WebApiApplication : System.Web.HttpApplication
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
                string adminsJson = File.ReadAllText(adminPath);
                DB.AdminsList = JsonConvert.DeserializeObject<List<User>>(adminsJson);

                // Read users from file
                string userPath = HttpContext.Current.Server.MapPath("~/App_Data/RepositoryFiles/Users.json");
                string usersJson = File.ReadAllText(userPath);
                DB.UsersList = JsonConvert.DeserializeObject<List<User>>(usersJson);
                if(DB.UsersList == null)
                {
                    DB.UsersList = new List<User>();
                }

                // Read products from file
                string productPath = HttpContext.Current.Server.MapPath("~/App_Data/RepositoryFiles/Products.json");
                string productsJson = File.ReadAllText(productPath);
                DB.ProductsList = JsonConvert.DeserializeObject<List<Product>>(productsJson);
                if(DB.ProductsList == null)
                {
                    DB.ProductsList = new List<Product>();
                }

                // Read reviews from file
                string reviewPath = HttpContext.Current.Server.MapPath("~/App_Data/RepositoryFiles/Reviews.json");
                string reviewsJson = File.ReadAllText(reviewPath);
                DB.ReviewsList = JsonConvert.DeserializeObject<List<Review>>(reviewsJson);
                if(DB.ReviewsList == null)
                {
                    DB.ReviewsList = new List<Review>();
                }


                // Read orders from file
                string orderPath = HttpContext.Current.Server.MapPath("~/App_Data/RepositoryFiles/Orders.json");
                string ordersJson = File.ReadAllText(orderPath);
                DB.OrdersList = JsonConvert.DeserializeObject<List<Order>>(ordersJson);
                if(DB.OrdersList == null)
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
    }
}
