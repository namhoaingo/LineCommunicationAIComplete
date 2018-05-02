using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Bot_LineMessagingApi.SDK.Controllers
{
    [Route("api/[controller]")]
    public class ImageController : Controller
    {

        private readonly IHostingEnvironment _hostingEnvironment;

        public ImageController(IHostingEnvironment hostingEnvironment)
        {
            this._hostingEnvironment = hostingEnvironment;
        }

        [HttpGet("{imageName}/{imageSize}")]        
        public async Task<IActionResult> Image(string imageName, int imageSize)
        {
            imageName = "DeathStar2.jpg";

            string full_imageName = String.Empty;
            string imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "Assets");

            //Image width: 240px, 300px, 460px, 700px, 1040px
            switch (imageSize)
            {
                case 240:
                    full_imageName = Path.Combine(imagePath, "240", imageName);
                    break;
                case 300:
                    full_imageName = Path.Combine(imagePath, "300", imageName);
                    break;
                case 460:
                    full_imageName = Path.Combine(imagePath, "460", imageName);
                    break;
                case 700:
                    full_imageName = Path.Combine(imagePath, "700", imageName);
                    break;
                case 1040:
                    full_imageName = Path.Combine(imagePath, "1040", imageName);
                    break;
            }

            return File(System.IO.File.OpenRead(full_imageName), "image/jpeg");
        }
    }
}