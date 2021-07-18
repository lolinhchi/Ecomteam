using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Twilio.Rest.Numbers.V2.RegulatoryCompliance;

namespace TeamBee.Helper
{
    public class BaseURL
    {
        public static string GetURL()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            return configuration["GetURL:URL"];
            
        }
    }
}
