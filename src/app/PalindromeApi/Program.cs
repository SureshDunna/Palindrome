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
using PalindromeApi.Filters;
using PalindromeApi.Models;
using Microsoft.Extensions.Logging;
using System;
using PalindromeApi.DataAccess;

namespace PalindromeApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<PalindromeContext>();
                    DbInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, $"An error occurred while seeding the database. {ex}");
                }
            }

            host.Run();
        }

        public static IWebHostBuilder BuildWebHost(string[] args)
        {
           return new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseUrls("http://localhost:5002")
                .UseStartup<Startup>()
                .ConfigureServices((services) =>
                {
                    services.AddAutofac();
                    JsonConvert.DefaultSettings = () => new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        Converters = new List<JsonConverter> { new StringEnumConverter() }
                    };

                    services.AddMvc(
                        options =>
                        {
                            options.Filters.Add(typeof(ValidationFilterAttribute));
                            options.Filters.Add(typeof(ExceptionLoggingFilter));
                        }
                    )
                    .AddJsonOptions(
                        options => { options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver(); });
                    
                    services.AddSwaggerGen(options =>
                    {
                        options.SwaggerDoc("v1",
                        new Info
                        {
                            Title = "Palindrome Api",
                            Version = "1.0"
                        });

                        options.IncludeXmlComments($"{PlatformServices.Default.Application.ApplicationBasePath}\\PalindromeApi.xml");
                    });                
                });
        }
     }
}
