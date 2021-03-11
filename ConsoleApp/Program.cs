using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using Utils.UtilLogger;
using Utils.Options;
using Utils.Word;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();
            using IServiceScope serviceScope = host.Services.CreateScope();
            IServiceProvider provider = serviceScope.ServiceProvider;

            Console.WriteLine("Enter a word to reverse it: ");
            string word = Console.ReadLine();

            WordUtils wordUtils = provider.GetRequiredService<WordUtils>();
            string reverseWord = wordUtils.Reverse(word);

            Console.WriteLine($"The word in reverse is:\n{reverseWord}");

            Console.ReadLine();
        }

        static IHostBuilder CreateHostBuilder(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                    services
                        .Configure<LogOptions>(configuration.GetSection("LogOptions"))
                        .AddSingleton(configuration)
                        .AddSingleton<IUtilLogger, UtilLogger<WordUtils>>()
                        .AddSingleton<WordUtils>());
        }
    }
}