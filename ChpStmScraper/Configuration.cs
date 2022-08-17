using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.Extensions.Configuration.Json;
using Newtonsoft.Json;
using System.Net.Http;

namespace ChpStmScraper
{
    //重写配置保存类
    public class WritableJsonConfigurationProvider : JsonConfigurationProvider
    {
        public WritableJsonConfigurationProvider(JsonConfigurationSource source) : base(source)
        {
        }

        public override void Set(string key, string value)
        {
            base.Set(key, value);

            //Get Whole json file and change only passed key with passed value. It requires modification if you need to support change multi level json structure
            var fileFullPath = base.Source.FileProvider.GetFileInfo(base.Source.Path).PhysicalPath;
            string json = File.ReadAllText(fileFullPath);
            dynamic jsonObj = JsonConvert.DeserializeObject(json);
            jsonObj[key] = value;
            string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(fileFullPath, output);
        }
    }
    public class WritableJsonConfigurationSource : JsonConfigurationSource
    {
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            this.EnsureDefaults(builder);
            return (IConfigurationProvider)new WritableJsonConfigurationProvider(this);
        }
    }

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
            IConfigurationBuilder builder;
            bool isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
            if(!isDevelopment){
                builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).Add<WritableJsonConfigurationSource>(
                (Action<WritableJsonConfigurationSource>)(s =>
                {
                    s.FileProvider = null;
                    s.Path = "appsettings.json";
                    s.Optional = false;
                    s.ReloadOnChange = true;
                    s.ResolveFileProvider();
                }));
            }
            else{
                builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).Add<WritableJsonConfigurationSource>(
                (Action<WritableJsonConfigurationSource>)(s =>
                {
                    s.FileProvider = null;
                    s.Path = "appsettings.Development.json";
                    s.Optional = false;
                    s.ReloadOnChange = true;
                    s.ResolveFileProvider();
                }));
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
        public static string ProxyUrl
        {
            get => configurationRoot["ProxyUrl"];
            set {
                configurationRoot["ProxyUrl"] = value;
                if (!String.IsNullOrEmpty(value))
                {
                    if (Uri.IsWellFormedUriString(value, UriKind.Absolute)) HttpClient.DefaultProxy = new System.Net.WebProxy(value);
                }
                else HttpClient.DefaultProxy = new System.Net.WebProxy();
            }
        }

        /// <summary>
        /// Buff URL
        /// </summary>
        /// <value></value>
        public static string BuffUrl
        {
            get => configurationRoot["BuffUrl"];
            set => configurationRoot["BuffUrl"] = value;
        }
        /// <summary>
        /// BuffSession
        /// </summary>
        /// <value></value>
        public static string BuffSession
        {
            get => configurationRoot["BuffSession"];
            set => configurationRoot["BuffSession"] = value;
        }

        /// <summary>
        /// CSGO 物品最小在售数
        /// </summary>
        /// <value></value>
        public static int MinSellCount
        {
            get => int.Parse(configurationRoot["MinSellCount"]);
            set => configurationRoot["MinSellCount"] = value.ToString();
        }

        /// <summary>
        /// DOTA2 物品最小在售数
        /// </summary>
        /// <value></value>
        //public static int Dota2MinSellCount => int.Parse(configurationRoot["Dota2MinSellCount"]);

        /// <summary>
        /// 最小售价
        /// </summary>
        /// <value></value>
        //public static int MinSellPrice => int.Parse(configurationRoot["MinSellPrice"]);

        /// <summary>
        /// Steam Cookies
        /// </summary>
        /// <value></value>
        public static string SteamCookies
        {
            get => configurationRoot["SteamCookies"];
            set => configurationRoot["SteamCookies"] = value;
        }
        /// <summary>
        /// Steam Cookies
        /// </summary>
        /// <value></value>
        public static string ConnectionString
        {
            get => configurationRoot["ConnectionString"];
            set => configurationRoot["ConnectionString"] = value;
        }
        /// <summary>
        /// Steam Cookies
        /// </summary>
        /// <value></value>
        public static string MysqlVersion
        {
            get => configurationRoot["MysqlVersion"];
            set => configurationRoot["MysqlVersion"] = value;
        }

        /// <summary>
        /// 最大线程
        /// </summary>
        /// <value></value>
        public static int MaxThread
        {
            get => int.Parse(configurationRoot["MaxThread"]);
            set => configurationRoot["MaxThread"] = value.ToString();
        }

        /// <summary>
        /// 是否开启爬虫
        /// </summary>
        /// <value></value>
        public static bool IsEnableScraper
        {
            get => bool.Parse(configurationRoot["IsEnableScraper"]);
            set => configurationRoot["IsEnableScraper"] = value.ToString();
        }

        /// <summary>
        /// 监听端口
        /// </summary>
        /// <value></value>
        public static int ListenPort => int.TryParse(configurationRoot["ListenPort"], out _) ? int.Parse(configurationRoot["ListenPort"]) : 1272;

        public static string GameKind
        {
            get => configurationRoot["GameKind"];
            set => configurationRoot["GameKind"] = value;
        }

        public static int MinSellPrice
        {
            get => int.Parse(configurationRoot["MinSellPrice"]);
            set => configurationRoot["MinSellPrice"] = value.ToString();
        }

        public static int MaxSellPrice
        {
            get => int.Parse(configurationRoot["MaxSellPrice"]);
            set => configurationRoot["MaxSellPrice"] = value.ToString();
        }

        public static bool Auth
        {
            get => bool.Parse(configurationRoot["Auth"]);
            set => configurationRoot["Auth"] = value.ToString();
        }

        public static string ListenIP
        {
            get => configurationRoot["ListenIP"];
            set => configurationRoot["ListenIP"] = value.ToString();
        }
    }
}