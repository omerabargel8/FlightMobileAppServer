﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Printing;
using System.Threading;

namespace FlightMobileApp.Models
{
    public class AppManager
    {
        ITelnetClient telnetClient;
        private Mutex mutex = new Mutex();
        public AppManager()
        {
            this.telnetClient = new TelnetClient();
            try
            {
                this.telnetClient.connect("localhost", 5002);
                telnetClient.write("data\n");
            }
            catch
            {
                throw new Exception("Unable to connect to server");
            }
        }
        public Byte[] getScreenshot()
        {
            string url = "http://localhost:5000/screenshot";
            //string url = String.Format("http" + "://localhost:5000/screenshot");
            WebRequest requset = WebRequest.Create(url);
            HttpWebResponse respond = null;
            respond = (HttpWebResponse)requset.GetResponse();
            Stream str;
            using (str = respond.GetResponseStream())
            {
                MemoryStream ms = new MemoryStream();
                str.CopyTo(ms);
                byte[] b = ms.ToArray();
                return b;
            }
        }
        public void sendCommand(Command command)
        {
            string returnedVal;
            try
            {
                telnetClient.write("set /controls/flight/aileron " + command.Aileron + "\r\n");
                telnetClient.write("get /controls/flight/aileron\r\n");
                returnedVal = telnetClient.read();
                if (Double.Parse(returnedVal) != command.Aileron)
                    Console.WriteLine("Problem");
                ////
                telnetClient.write("set /controls/flight/rudder " + command.Rudder + "\r\n");
                telnetClient.write("get /controls/flight/rudder\r\n");
                returnedVal = telnetClient.read();
                if (Double.Parse(returnedVal) != command.Rudder)
                    Console.WriteLine("Problem");
                /////
                telnetClient.write("set /controls/flight/elevator " + command.Elevator + "\r\n");
                telnetClient.write("get /controls/flight/elevator\r\n");
                returnedVal = telnetClient.read();
                if (Double.Parse(returnedVal) != command.Elevator)
                    Console.WriteLine("Problem");
                /////
                telnetClient.write("set /controls/engines/current-engine/throttle " + command.Throttle + "\r\n");
                telnetClient.write("get /controls/engines/current-engine/throttle\r\n");
                returnedVal = telnetClient.read();
                if (Double.Parse(returnedVal) != command.Throttle)
                    Console.WriteLine("Problem");
            }
            catch (Exception e)
            {
                throw new Exception("Unable to write to server");
            }
        }
    }
}