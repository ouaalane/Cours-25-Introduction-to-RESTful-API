using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.Reflection.Metadata.Ecma335;

namespace UploadRetrieveVidioAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly string _UploadDirectroty = @"C:\VideoUploads";
        private readonly long _maxFileSize = 500 * 1024 * 1024; //500 MB
        private readonly  string[] _allowedExtensions =  { ".mp4", ".avi", ".mov", "wmv" };



        // post method : api/video/upload

        [HttpPost("upload")]
        [RequestSizeLimit(524_288_000)]
        [RequestFormLimits(MultipartBodyLengthLimit =524_288_000)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<VidioUploadResponse>> UploadVideo(IFormFile VidioFile)
        {
            // validation
            if(VidioFile == null || VidioFile.Length == 0)
            {
                return BadRequest(new {message = "no video file uploaded"}); 
            }

            // validation file extention
            var extension  = Path.GetExtension(VidioFile.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(extension))
            {
                return BadRequest(new { message = $"Invalid file type . allowed  {string.Join(", ", _allowedExtensions)} " });
            }


            // validate file size
            if (VidioFile.Length>_maxFileSize)
            {
                return this.BadRequest($"File size exceeds   : {_maxFileSize / (1024 * 1024)} MB limit");
            }



            // validate MIME Type 
            if (!VidioFile.ContentType.StartsWith("video/"))
            {
                return BadRequest("File most be a video");
            }

            try
            {
                //  create directorty if it doesn't exit

                if (!Directory.Exists(_UploadDirectroty))
                {
                    Directory.CreateDirectory(_UploadDirectroty);

                }


                var uniquefilename = $"{Guid.NewGuid()}{extension}";
                var filepath  =  Path.Combine(_UploadDirectroty,uniquefilename);



                await using (var stream = new FileStream(filepath,FileMode.Create))
                {
                    await VidioFile.CopyToAsync(stream);
                }

                return Ok(new VidioUploadResponse
                {
                    FileName = uniquefilename,
                    OriginalFileName = VidioFile.FileName,
                    FileSize = VidioFile.Length,
                    ContentType = VidioFile.ContentType,
                    UploadAt = DateTime.Now,
                    VideoUrl = $"{Request.Scheme}://{Request.Host}/api/video/{uniquefilename}"

                }
                );

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error Uploading vidoo", error = ex.Message });
            }

           



        }


        // GET: api/Video/{filename}
        [HttpGet("{filename}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetVideo(string filename)
        {
            var filePath = Path.Combine(_UploadDirectroty, filename);

            if (!System.IO.File.Exists(filePath))
                return NotFound(new { message = "Video not found" });

            var mimeType = GetMimeType(filePath);
            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

            // Enable range requests for video seeking
            return File(stream, mimeType, enableRangeProcessing: true);
        }

        [ApiExplorerSettings(IgnoreApi = false)]
        // GET: api/Video/stream/{filename}
        [HttpGet("stream/{filename}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status416RangeNotSatisfiable)]
        public IActionResult StreamVideo(string filename)
        {
            var filePath = Path.Combine(_UploadDirectroty, filename);

            if (!System.IO.File.Exists(filePath))
                return NotFound(new { message = "Video not found" });

            var fileInfo = new FileInfo(filePath);
            var mimeType = GetMimeType(filePath);

            // Support Range requests for video player seeking
            return PhysicalFile(filePath, mimeType, enableRangeProcessing: true);
        }



        [HttpGet("info/{filename}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VideoInfo> GetVideoInfo(string filename)
        {
            var filePath = Path.Combine(_UploadDirectroty, filename);

            if (!System.IO.File.Exists(filePath))
                return NotFound(new { message = "Video not found" });

            var fileInfo = new FileInfo(filePath);

            return Ok(new VideoInfo
            {
                FileName = fileInfo.Name,
                FileSize = fileInfo.Length,
                CreatedAt = fileInfo.CreationTimeUtc,
                VideoUrl = $"{Request.Scheme}://{Request.Host}/api/Video/{fileInfo.Name}",
                MimeType = GetMimeType(filePath)
            });
        }


        public class VideoInfo
        {
            public string FileName { get; set; }
            public long FileSize { get; set; }
            public DateTime CreatedAt { get; set; }
            public string VideoUrl { get; set; }
            public string MimeType { get; set; }
        }


        private string GetMimeType(string FilePath)
        {

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(FilePath, out var mimeType))
            {
                var extension = Path.GetExtension(FilePath).ToLowerInvariant();
                mimeType = extension switch
                {
                    ".mp4" => "video/mp4",
                    ".avi" => "video/x-msvideo",
                    ".mov" => "video/quicktime",
                    ".wmv" => "video/x-ms-wmv",
                    ".flv" => "video/x-flv",
                    ".mkv" => "video/x-matroska",
                    ".webm" => "video/webm",
                    _ => "application/octet-stream"
                };
            }
            return mimeType;
        }
    }



    // response models (DTOs)
    public   class VidioUploadResponse
    {
        public string FileName { get; set; }
        public string OriginalFileName { get; set; }    
        public long FileSize { get; set; }    

        public string ContentType { get; set; }

        public DateTime UploadAt { get; set; }

        public string VideoUrl { get; set; }
    }
}
