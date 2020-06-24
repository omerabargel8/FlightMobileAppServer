using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Printing;
using System.Threading;
using Microsoft.Extensions.Configuration;

namespace FlightMobileApp.Models
{
    public class AppManager
    {
        ITelnetClient telnetClient;
        private Mutex mutex = new Mutex();
        string url;

        public AppManager(IConfiguration config)
        {
            this.telnetClient = new TelnetClient();
            try
            {
                string ip = config.GetConnectionString("ip");
                int port = int.Parse(config.GetConnectionString("port"));
                url = config["urls"];
                this.telnetClient.connect(ip, port);
                telnetClient.write("data\n");
            }
            catch
            {
                throw new Exception("Unable to connect to server");
            }
        }
        public Byte[] getScreenshot()
        {
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
            new Thread(delegate () { 
                string returnedVal;
                double retVal;
                try
                {
                    mutex.WaitOne();
                    telnetClient.write("set /controls/flight/aileron " + command.Aileron + "\r\n");
                    telnetClient.write("get /controls/flight/aileron\r\n");
                    returnedVal = telnetClient.read();
                    double.TryParse(returnedVal, out retVal);
                    if (Double.Parse(returnedVal) != command.Aileron)
                        Console.WriteLine("Problem");
                    mutex.ReleaseMutex();
                    ////
                    mutex.WaitOne();
                    telnetClient.write("set /controls/flight/rudder " + command.Rudder + "\r\n");
                    telnetClient.write("get /controls/flight/rudder\r\n");
                    returnedVal = telnetClient.read();
                    if (Double.Parse(returnedVal) != command.Rudder)
                        Console.WriteLine("Problem");
                    mutex.ReleaseMutex();
                    /////
                    mutex.WaitOne();
                    telnetClient.write("set /controls/flight/elevator " + command.Elevator + "\r\n");
                    telnetClient.write("get /controls/flight/elevator\r\n");
                    returnedVal = telnetClient.read();
                    if (Double.Parse(returnedVal) != command.Elevator)
                        Console.WriteLine("Problem");
                    mutex.ReleaseMutex();
                    /////
                    mutex.WaitOne();
                    telnetClient.write("set /controls/engines/current-engine/throttle " + command.Throttle + "\r\n");
                    telnetClient.write("get /controls/engines/current-engine/throttle\r\n");
                    returnedVal = telnetClient.read();
                    if (double.Parse(returnedVal) != command.Throttle)
                        Console.WriteLine("Problem");
                    mutex.ReleaseMutex();
                }
                catch (Exception e)
                {
                    throw new Exception("Unable to write to server");
                }
        }).Start();
    }
    }
}
