using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SuperDuper
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class BuildDateTimeAttribute : Attribute
    {
        public string Date { get; set; }
        public string MachineName { get; set; }
        public string CommitHash { get; set; }

        public BuildDateTimeAttribute(string date, string machineName, string commitHash)
        {
            Date = date;
            MachineName = machineName;
            CommitHash = commitHash;
        }
    }
}

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
            var sb = new StringBuilder();
            var assembly = Assembly.GetExecutingAssembly();
            var attr = Attribute.GetCustomAttribute(assembly, typeof(SuperDuper.BuildDateTimeAttribute)) as SuperDuper.BuildDateTimeAttribute;
            if (attr == null)
            {
                return sb.ToString();
            }

            sb.AppendLine("Build date - " + attr.Date);
            sb.AppendLine("Machine name - " + attr.MachineName);
            sb.AppendLine("Commit Hash - " + attr.CommitHash);

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
