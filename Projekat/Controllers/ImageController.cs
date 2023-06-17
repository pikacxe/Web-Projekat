using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Projekat.Repository;

namespace Projekat.Controllers
{
    public class ImageController : ApiController
    {
        [HttpPost]
        [ActionName("add")]
        public IHttpActionResult UploadImage()
        {
            var filename = string.Empty;
            try
            {
                var image = Request.Content.ReadAsByteArrayAsync().Result;
                filename = $"uploaded_{DB.GenerateId()}.png";
                var path = HttpContext.Current.Server.MapPath($"~/App_Data/Images/{filename}");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(filename);
        }
    }
}
