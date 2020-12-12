using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace ChpStmScraper
{
    /// <summary>
    /// 返回程序配置
    /// </summary>
    public partial class Configuration
    {
        private static IConfigurationRoot configurationRoot;
        static Configuration()
        {
            //throw new Exception(Directory.GetCurrentDirectory());
            //File.WriteAllText("debug.log",Directory.GetCurrentDirectory());
            IConfigurationBuilder builder = new ConfigurationBuilder();
            bool isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
            if(!isDevelopment){
                builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", true, reloadOnChange: true);
            }
            else{
                builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.Development.json", true, reloadOnChange: true);
            }
            try
            {
                configurationRoot = builder.Build();
            }
            catch (Exception ex)
            {
                Console.WriteLine("appsettings.json 格式错误");
                Process.GetCurrentProcess().Kill();
            }
        }
        /// <summary>
        /// 代理地址
        /// </summary>
        /// <value></value>
        public static string ProxyUrl => configurationRoot["ProxyUrl"];

        /// <summary>
        /// Buff URL
        /// </summary>
        /// <value></value>
        public static string BuffUrl => configurationRoot["BuffUrl"];
        /// <summary>
        /// BuffSession
        /// </summary>
        /// <value></value>
        public static string BuffSession => configurationRoot["BuffSession"];

        /// <summary>
        /// CSGO 物品最小在售数
        /// </summary>
        /// <value></value>
        public static int CsgoMinSellCount => int.Parse(configurationRoot["CsgoMinSellCount"]);

        /// <summary>
        /// DOTA2 物品最小在售数
        /// </summary>
        /// <value></value>
        public static int Dota2MinSellCount => int.Parse(configurationRoot["Dota2MinSellCount"]);

        /// <summary>
        /// 最小售价
        /// </summary>
        /// <value></value>
        public static int MinSellPrice => int.Parse(configurationRoot["MinSellPrice"]);

        /// <summary>
        /// Steam Cookies
        /// </summary>
        /// <value></value>
        public static string SteamCookies => configurationRoot["SteamCookies"];
        /// <summary>
        /// Steam Cookies
        /// </summary>
        /// <value></value>
        public static string ConnectionString => configurationRoot["ConnectionString"];
        /// <summary>
        /// Steam Cookies
        /// </summary>
        /// <value></value>
        public static string MysqlVersion => configurationRoot["MysqlVersion"];

        /// <summary>
        /// 最大线程
        /// </summary>
        /// <value></value>
        public static int MaxThread => int.Parse(configurationRoot["MaxThread"]);

        /// <summary>
        /// 是否开启爬虫
        /// </summary>
        /// <value></value>
        public static bool IsEnableScraper => bool.Parse(configurationRoot["IsEnableScraper"]);

        /// <summary>
        /// 监听 URL
        /// </summary>
        /// <value></value>
        public static string[] ListeningUrls => configurationRoot.GetSection("ListeningUrls").GetChildren().Select(x => x.Value).ToArray();
    }
}