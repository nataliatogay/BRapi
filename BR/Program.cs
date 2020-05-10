using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BR
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            //.ConfigureLogging(logging =>
            //{
            //    logging.ClearProviders();
            //    logging.AddConsole();
            //})
                .UseStartup<Startup>();
    }
}


// client default photo https://rb2020storage.blob.core.windows.net/photos/default_restaurant.jpg
// event default https://rb2020storage.blob.core.windows.net/photos/default-event.png   
// user default https://rb2020storage.blob.core.windows.net/photos/default-profile.png