using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OpenSilverEmbeddingData.Browser
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            var host = builder.Build();
            await host.RunAsync();
        }

        private static string GetAssemblyBuildInfo()
        {
            var listOfAttributeKeys = new HashSet<string> { "SourceRevisionId", "BuildDate", "MachineName" };

            var sb = new StringBuilder();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var attributes = Attribute.GetCustomAttributes(assembly, typeof(AssemblyMetadataAttribute))
                    .OfType<AssemblyMetadataAttribute>().Where(a => listOfAttributeKeys.Contains(a.Key)).ToList();
                if (attributes.Any())
                {
                    sb.AppendLine(assembly.FullName + ":");
                }
                foreach (var att in attributes)
                {
                    sb.AppendLine(att.Key + " - " + att.Value);
                }
            }


            return sb.ToString();
        }

        public static void RunApplication()
        {
            Application.RunApplication(() =>
            {
                Console.WriteLine(GetAssemblyBuildInfo());

                var app = new OpenSilverEmbeddingData.App();
            });
        }
    }
}
