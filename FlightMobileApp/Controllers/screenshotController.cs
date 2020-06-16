using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FlightMobileApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightMobileApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class screenshotController : ControllerBase
    {
        private AppManager appManager;
        public screenshotController(AppManager appManager)
        {
            this.appManager = appManager;
        }
        // GET: api/screenshot
        [HttpGet]
        public IActionResult GetScreenshot()
        {

            byte []b = appManager.getScreenshot();
            return File(b, "image/png");
        }

    }
}
