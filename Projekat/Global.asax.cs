using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Newtonsoft.Json;

namespace Projekat
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Load_Data();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        
        }

        protected void Application_End()
        {
            Save_Data();
        }

        private void Load_Data()
        {

        }

        private void Save_Data()
        {

        }


    }
}
