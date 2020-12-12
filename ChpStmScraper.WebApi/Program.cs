using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ChpStmScraper.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            CreateHostBuilder(args).Build().Run();
        }

        public static void Init()
        {
            Console.WriteLine("CheapSteam 正在启动");
            //初始化数据库
            if(!File.Exists("ChpStmScraper.db"))
                File.Copy("ChpStmScraper.Template.db","ChpStmScraper.db");
            if(string.IsNullOrEmpty(Configuration.ProxyUrl))
                Console.WriteLine("检测到未设置代理，是否已能够访问 https://steamcommunity.com/ ？您可能需要开启加速器加速 steam 社区");
            if(string.IsNullOrEmpty(Configuration.SteamCookies)){
                Console.WriteLine("Steam Cookie 未设置，程序关闭");
                Process.GetCurrentProcess().Kill();
            }
            if(string.IsNullOrEmpty(Configuration.BuffSession)){
                Console.WriteLine("Buff Session 未设置，程序关闭");
                Process.GetCurrentProcess().Kill();
            }
            Console.WriteLine($"本地监听地址为 {string.Join(';',Configuration.ListeningUrls)}，请在浏览器打开");
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls(Configuration.ListeningUrls);
                    webBuilder.UseStartup<Startup>();
                });
    }
}
