using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightMobileApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightMobileApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class commandController : ControllerBase
    {
        private AppManager appManager;
        public commandController(AppManager appManager)
        {
            this.appManager = appManager;
        }
        // POST: api/command
        [HttpPost]
        public void PostCommand(Command command)
        {
            Console.WriteLine("ddddddd");
            appManager.sendCommand(command);
        }
    }
}
