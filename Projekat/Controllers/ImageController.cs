using System;
using System.Web;
using System.Web.Http;
using Projekat.Repository;

namespace Projekat.Controllers
{
    public class ImageController : ApiController
    {
        [HttpPost]
        [ActionName("add")]
        [Authorize]
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
