using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ChpStmScraper.Models;
using Newtonsoft.Json.Linq;
using System.Linq;
using ChpStmScraper.Services;

namespace ChpStmScraper
{
    public class Program
    {
        public Program()
        {

        }
        static void Main(string[] args)
        {
            Console.WriteLine("开始抓取内容");
            Init();
            while (true)
            {
                string input = Console.ReadLine();
                if (input == "exit")
                {
                    Process.GetCurrentProcess().Kill();
                }
            }
        }

        public static void Init()
        {
            Start();
        }

        #region 线程锁
        private static object o = new object();
        private static object o2 = new object();
        private static object o3 = new object();
        private static object o4 = new object();
        private static object o5 = new object();
        #endregion
        private static int pageNum = 0;
        private static int maxPageNum = 2333;
        private static int currentSyncThread = 0;
        private static string lastScraperName = "";

        private static void CheckIfSteamCommunity()
        {

            HttpService httpService = new HttpService();
            if (!string.IsNullOrEmpty(Configuration.ProxyUrl))
            {
                httpService = new HttpService(Configuration.ProxyUrl);
            }
            try
            {
                Console.WriteLine("正在检测 steam 社区连通性...");
                httpService.GetWithCookie("https://steamcommunity.com/market/", Configuration.SteamCookies, result =>
                {
                    //判断是否可以访问 steam 社区
                    Regex regex = new Regex(@"(?<=<span id=""market_buynow_dialog_myaccountname"">).+(?=<\/span>)");
                    var match = regex.Matches(result.Content.ReadAsStringAsync().Result);
                    if (match.Count() == 0)
                    {
                        Console.WriteLine($"Steam Cookie 无效或已失效，退出程序");
                        Process.GetCurrentProcess().Kill();
                    }
                    else
                    {
                        Console.WriteLine($"当前 steam 账户为 {match[0].Value}");
                    }
                });
            }
            catch (System.Exception ex)
            {
                 Console.WriteLine("访问 steam 社区出现错误，是否已开启代理？");
                 Console.WriteLine(ex.StackTrace);
                 Process.GetCurrentProcess().Kill();
            }
            
        }
        public static void Start()
        {
            CheckIfSteamCommunity();
            var baseAddress = new Uri(Helper.GetBaseUrl(Configuration.BuffUrl));
            Timer timer = new Timer(state =>
            {
                while (currentSyncThread < Configuration.MaxThread)
                {
                    int currentPageNum = 0;
                    lock (o)
                    {
                        if (pageNum < maxPageNum)
                        {
                            pageNum++;
                            currentPageNum = pageNum;
                        }
                        if (pageNum == maxPageNum)
                        {
                            //达到查询页面上限
                            pageNum = 1;
                        }
                    }
                    currentSyncThread++;
                    HttpService httpService = new HttpService();
                    if (!string.IsNullOrEmpty(Configuration.ProxyUrl))
                    {
                        httpService = new HttpService(Configuration.ProxyUrl);
                    }
                    httpService.GetWithCookie(Configuration.BuffUrl + $"&page_num={pageNum}", new Cookie("session", Configuration.BuffSession), result =>
                      {
                          try
                          {
                              result.EnsureSuccessStatusCode();
                          }
                          catch (HttpRequestException ex)
                          {
                              // TODO:这里会导致 Response status code does not indicate success: 429 (Too Many Requests).
                              Console.WriteLine(ex.Message);
                              Thread.Sleep(TimeSpan.FromSeconds(10));
                              return;
                          }
                          ThreadPool.QueueUserWorkItem(state =>
                          {
                              HttpService httpService = new HttpService();
                              var jsonObj = JObject.Parse(result.Content.ReadAsStringAsync().Result);
                              ScraperDbContext context = new ScraperDbContext();
                              //页面数修改
                              lock (o4)
                              {
                                  maxPageNum = int.Parse(jsonObj["data"]["total_page"].ToString());
                              }
                              foreach (var jItem in jsonObj["data"]["items"])
                              {
                                  var name = Helper.UnicodeToString(jItem["name"].ToString());
                                  lock (o5)
                                  {
                                      if (lastScraperName == name)
                                      {
                                          continue;
                                      }
                                      else
                                      {
                                          lastScraperName = name;
                                      }
                                  }
                                  var buffSellNum = int.Parse(jItem["sell_num"].ToString());
                                  switch (jItem["game"].ToString() == "csgo" ? Goods.GameKind.CSGO : Goods.GameKind.DOTA2)
                                  {
                                      case Goods.GameKind.CSGO:
                                          if (buffSellNum < Configuration.MinSellCount)
                                          {
                                              continue;
                                          }
                                          break;
                                      case Goods.GameKind.DOTA2:
                                          if (buffSellNum < Configuration.MinSellCount)
                                          {
                                              continue;
                                          }
                                          break;
                                  }
                                  //开始在 STEAM 查询
                                  string steamMarketUrl = jItem["steam_market_url"].ToString();
                                  try
                                  {
                                      httpService.GetWithCookie(steamMarketUrl, Configuration.SteamCookies, result =>
                              {
                                  if(!string.IsNullOrEmpty(Configuration.ProxyUrl))
                                      HttpClient.DefaultProxy = new WebProxy(Configuration.ProxyUrl);
                                  string marketResult = result.Content.ReadAsStringAsync().Result;
                                  //Debug.WriteLine(result.RequestMessage.RequestUri.AbsoluteUri);
                                  //重复直到获取的到值
                                  //   while (getResult.StatusCode != HttpStatusCode.OK)
                                  //   {
                                  //       try
                                  //       {
                                  //           //marketResult = await client2.GetStringAsync(steamMarketUrl);
                                  //           getResult = await client2.SendAsync(message);
                                  //           marketResult = await getResult.Content.ReadAsStringAsync();
                                  //       }
                                  //       catch (HttpRequestException ex)
                                  //       {
                                  //           client2.CancelPendingRequests();
                                  //           Console.WriteLine("出现错误：" + ex.Message);
                                  //           //这里会出现 429，等待即可
                                  //           //Thread.Sleep(TimeSpan.FromSeconds(120));
                                  //       }
                                  //   }
                                  //Debug.WriteLine(marketResult);
                                  //获取 steamID 正则
                                  Regex regex = new Regex(@"(?<=Market_LoadOrderSpread\(\s).*(?=\s\);)", RegexOptions.Multiline);
                                  var marketId = regex.Match(marketResult).Value;
                                  if (string.IsNullOrEmpty(marketId))
                                  {
                                      Thread.Sleep(TimeSpan.FromSeconds(10));
                                      throw new Exception("错误：找不到MarketID，可能是爬取速度过快");
                                  }
                                  string steamApiUrl = $"https://steamcommunity.com/market/itemordershistogram?country=CN&language=schinese&currency=23&item_nameid={marketId}&two_factor=0";

                                  var apiResult = httpService.Get(steamApiUrl);
                                  JObject steamJsonObj = JObject.Parse(apiResult.Content.ReadAsStringAsync().Result);
                                  //获取 steam 价格正则
                                  regex = new Regex(@"(?<=数量<\/th><\/tr><tr><td align=""right\"" class="""">¥ ).*?(?=<\/td>)", RegexOptions.Multiline);
                                  var marketSellPrice = regex.Match(steamJsonObj["sell_order_table"].ToString()).Value;
                                  regex = new Regex(@"(?<=数量<\/th><\/tr><tr><td align=""right"" class="""">¥ ).*?(?=<)", RegexOptions.Multiline);
                                  var marketBuyPrice = regex.Match(steamJsonObj["buy_order_table"].ToString()).Value;
                                  regex = new Regex(@"(?<=>)\d*(?=<\/span>\s个出售中)", RegexOptions.Multiline);
                                  var SteamSellNum = regex.Match(steamJsonObj["sell_order_summary"].ToString()).Value;
                                  Goods item = new Goods()
                                  {
                                      Kind = jItem["game"].ToString() == "csgo" ? Goods.GameKind.CSGO : Goods.GameKind.DOTA2,
                                      Name = name,
                                      BuffBuyPrice = Helper.String2Double(jItem["buy_max_price"].ToString()),
                                      BuffSellPrice = Helper.String2Double(jItem["sell_min_price"].ToString()),
                                      SteamSellPrice = Helper.String2Double(marketSellPrice),
                                      SteamBuyPrice = Helper.String2Double(marketBuyPrice),
                                      SteamSellNum = Helper.String2Int(SteamSellNum),
                                      BuffSellNum = buffSellNum
                                  };
                                  if (item.BuffSellPrice != 0 && item.SteamBuyPrice != 0)
                                      item.SteamBuyRadio = Math.Round(item.BuffSellPrice / (item.SteamBuyPrice / 1.15), 2);
                                  else
                                      item.SteamBuyRadio = 0;
                                  if (item.BuffSellPrice != 0 && item.SteamSellPrice != 0)
                                      item.SteamSellRadio = Math.Round(item.BuffSellPrice / (item.SteamSellPrice / 1.15), 2);
                                  else
                                      item.SteamSellRadio = 0;
                                  lock (o2)
                                  {
                                      var querry = context.Goods.Find(item.Name);
                                      if (querry == null)
                                      {
                                          context.Goods.Add(item);
                                      }
                                      else
                                      {
                                          //中国时间
                                          item.UpdateTime = DateTime.UtcNow.AddHours(8);
                                          context.Entry<Goods>(querry).CurrentValues.SetValues(item);
                                      }
                                      context.SaveChanges();
                                      Console.WriteLine($"正在查询第{currentPageNum}页，物品名：{item.Name}");
                                  }
                              });
                                  }
                                  catch (System.Exception e)
                                  {
                                      Console.WriteLine(e.Message);
                                      Console.WriteLine(e.StackTrace);
                                      continue;
                                  }

                              }
                              //保存数据库
                              lock (o2)
                              {
                                  context.SaveChanges();
                              }
                              lock (o3)
                              {
                                  currentSyncThread--;
                              }
                          });
                      });
                }
            });
            //定时检查线程数
            timer.Change(0, 5000);
        }
    }
}
