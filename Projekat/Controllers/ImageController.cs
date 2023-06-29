using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
        public Task<HttpResponseMessage> UploadImage()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            string rootpath = HttpContext.Current.Server.MapPath("~/Images");
            var provider = new MultipartFileStreamProvider(rootpath);
            var task = Request.Content.ReadAsMultipartAsync(provider).
            ContinueWith<HttpResponseMessage>(t =>
            {
                if (t.IsCanceled || t.IsFaulted)
                {
                    Request.CreateErrorResponse(HttpStatusCode.InternalServerError, t.Exception);
                }
                string newfilename;
                var item = provider.FileData[0];
                try
                {
                    string name = item.Headers.ContentDisposition.FileName.Replace("\"", "");
                    newfilename = Guid.NewGuid() + Path.GetExtension(name);
                    File.Move(item.LocalFileName, Path.Combine(rootpath, newfilename));
                    Uri baseuri = new Uri(Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.PathAndQuery, string.Empty));
                    string fileRelativePath = "~/Images/" + newfilename;
                    Uri filefullpath = new Uri(baseuri, VirtualPathUtility.ToAbsolute(fileRelativePath));
                }
                catch (Exception)
                {
                    throw;
                }

                return Request.CreateResponse(HttpStatusCode.Created,newfilename);
            });
            return task;
        }
    }
}
