using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace UploadRetrieveImageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageUploadRetrieveController : ControllerBase
    {


        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile Imagefile)
        {

            if (Imagefile == null || Imagefile.Length==0)
            {
                return this.BadRequest("No file to upload ! ");
            }

            Console.WriteLine(Imagefile.ContentDisposition);
            var UploadDirectory = @"C:\MyUploads";


            var filename  = Guid.NewGuid().ToString() + Path.GetExtension(Imagefile.FileName);
            var filePath  =  Path.Combine(UploadDirectory, filename);


            if (!Directory.Exists(UploadDirectory))
            { 
                Directory.CreateDirectory(UploadDirectory);
            }


            using (var stream =  new FileStream(filePath,FileMode.Create))
            {
                await Imagefile.CopyToAsync(stream);
            }
            return this.Ok(filePath);
        }



        [HttpGet("GetImage/{filename}")]
        public ActionResult GetImage(string filename)
        {
            var UploadDirectory = @"C:\MyUploads";

            var filePath   = Path.Combine(UploadDirectory, filename);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("Image not found !");
            }

            var image  =  System.IO.File.OpenRead(filePath);
            var mimtype = GetMimType(filePath);

            return this.File(image, mimtype);

        }

        private string GetMimType(string filepath)
        {
            var extension = Path.GetExtension(filepath).ToLowerInvariant();

            return extension switch
            {
                "jpg" or "jpge" => "image/jpge",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream"

            };
                
        }
            

        


    }
}
