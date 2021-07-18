using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TeamBee.Helper
{
    public class ConfigurationBuilderExtensions
    {
        private static IConfigurationBuilder builder;
        public static IConfigurationBuilder Builder
        { get
            {
                if (builder == null)
                {
                    builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
                }
                return builder;
            }
        }

        private static IConfigurationRoot configuration;
        public static IConfigurationRoot Configuration
        {
            get
            {
                if (configuration == null)
                {
                    configuration = Builder.Build();
                }
                return configuration;
            }
        }


        public static string GetFromPhone()
        {
            return Configuration["SMS:from"];
        }
        public static string GetToPhone()
        {
            return Configuration["SMS:to"];
        }
        public static string GetAccountSid()
        {
            return Configuration["SMS:accountSid"];
        }
        public static string GetAuthToken()
        {
            return Configuration["SMS:authToken"];
        }
        public static string GetAuthyAPIKey()
        {
            return Configuration["SMS:AuthyAPIKey"];
        }


        public static string GetServiceSid()
        {
            return Configuration["SMS:servicesid"];
        }
    }
}
