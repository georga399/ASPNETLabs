using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WEB_253505_AZAROV.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly string _imagePath;
        public FilesController(IWebHostEnvironment webHost)
        {
            _imagePath = Path.Combine(webHost.WebRootPath, "images");
        }

        [HttpPost]
        public async Task<IActionResult> SaveFile(IFormFile file)
        {
            if (file is null) 
            {
                return BadRequest();
            }

            var filePath = Path.Combine(_imagePath, file.FileName);
            var fileInfo = new FileInfo(filePath);
    
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }

            using var fileStream = fileInfo.Create();
            await file.CopyToAsync(fileStream);

            var host = HttpContext.Request.Host;
            var fileUrl = $"Http://{host}/images/{file.FileName}";
            return Ok(fileUrl);
        }

        [HttpDelete("{fileName}")]
        [Authorize]
        public IActionResult DeleteFile(string fileName) 
        {
            var filePath = Path.Combine(_imagePath, fileName);
            var fileInfo = new FileInfo(filePath);

            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
            return Ok();
        }
    }
}
