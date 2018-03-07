using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.PlatformAbstractions;
using Palindrome.Filters;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.AspNetCore;

namespace PalindromeUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Build().Run();
        }

        public static IWebHost BuildWebHost1(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();

        public static IWebHostBuilder BuildWebHost(string[] args)
        {
           return new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseUrls("http://localhost:5001")
                .UseStartup<Startup>()
                .ConfigureServices((services) =>
                {
                    services.AddAutofac();
                    services.AddMemoryCache();
                    JsonConvert.DefaultSettings = () => new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        Converters = new List<JsonConverter> { new StringEnumConverter() }
                    };

                    services.AddSwaggerGen(options =>
                    {
                        options.SwaggerDoc("v1",
                        new Info
                        {
                            Title = "Palindrome Api",
                            Version = "1.0"
                        });

                        options.IncludeXmlComments($"{PlatformServices.Default.Application.ApplicationBasePath}\\Palindrome.xml");
                    });                
                });
        }
     }
}
