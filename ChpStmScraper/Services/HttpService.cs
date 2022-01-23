using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChpStmScraper.Services
{
    public class HttpService
    {
        //string _url;

        public HttpService()
        {
            //this._url = url;
        }

        /// <summary>
        /// With Proxy
        /// </summary>
        public HttpService(string url)
        {
            HttpClient.DefaultProxy = new WebProxy(url);
        }

        /// <summary>
        /// 带着 Cookie 的 Get 请求
        /// </summary>
        /// <param name="cookie"></param>
        /// <param name="next">do what fuck you want to do</param>
        /// <returns></returns>
        public async void GetWithCookie(string url,Cookie cookie, Action<HttpResponseMessage> next)
        {
            var cookieContainer = new CookieContainer();
            var baseUrl = new System.Uri(Helper.GetBaseUrl(url));
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = baseUrl })
            {
                cookieContainer.Add(baseUrl, cookie);
                var result = await client.GetAsync(url);
                next(result);
            }
            return;
        }

        /// <summary>
        /// 带着 Cookie 的 Get 请求
        /// </summary>
        /// <param name="cookie"></param>
        /// <param name="next">do what fuck you want to do</param>
        /// <returns></returns>
        public async Task GetWithCookieAsync(string url,Cookie cookie, Func<HttpResponseMessage,Task> next)
        {
            var cookieContainer = new CookieContainer();
            var baseUrl = new System.Uri(Helper.GetBaseUrl(url));
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = baseUrl })
            {
                cookieContainer.Add(baseUrl, cookie);
                var result = await client.GetAsync(url);
                await next(result);
            }
            return;
        }

        private List<Cookie> StrToCookies(string cookiesStr)
        {
            var cookieArray = cookiesStr.Split(";");
            List<Cookie> cookies = new List<Cookie>();
            // xxxx=xxxxx
            foreach (var item in cookieArray)
            {
                var itemArray = item.Split("=");
                //跳过 timezoneOffset
                if(itemArray[0].Trim() == "timezoneOffset")
                    continue;
                var cookie = new Cookie();
                cookie = new Cookie(itemArray[0].Trim(),itemArray[1].Trim());
                cookies.Add(cookie);
            }
            return cookies;
        }
        /// <summary>
        /// 带着 Cookie 的 Get 请求
        /// </summary>
        /// <param name="cookie"></param>
        /// <param name="next">do what fuck you want to do</param>
        /// <returns></returns>
        public void GetWithCookie(string url,string cookie, Action<HttpResponseMessage> next)
        {
            var cookieContainer = new CookieContainer();
            var baseUrl = new System.Uri(Helper.GetBaseUrl(url));
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = baseUrl })
            {
                var cookies =  StrToCookies(cookie);
                foreach (var item in cookies)
                {
                    try
                    {
                        cookieContainer.Add(baseUrl, item);
                    }
                    catch (System.Exception ex)
                    {
                         continue;
                    }
                }
                var result = client.GetAsync(url).Result;
                next(result);
            }
            return;
        }

        /// <summary>
        /// 带着 Cookie 的 Get 请求
        /// </summary>
        /// <param name="cookie"></param>
        /// <param name="next">do what fuck you want to do</param>
        /// <returns></returns>
        public async Task GetWithCookieAsync(string url,string cookie, Func<HttpResponseMessage,Task> next)
        {
            var cookieContainer = new CookieContainer();
            var baseUrl = new System.Uri(Helper.GetBaseUrl(url));
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = baseUrl })
            {
                var cookies =  StrToCookies(cookie);
                foreach (var item in cookies)
                {
                    try
                    {
                        cookieContainer.Add(baseUrl, item);
                    }
                    catch (System.Exception ex)
                    {
                         continue;
                    }
                }
                var result = await client.GetAsync(url);
                await next(result);
            }
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookie"></param>
        /// <param name="timeOut">超时时间</param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task GetWithCookieAsync(string url, string cookie, int timeOut, Func<HttpResponseMessage, Task> next)
        {
            var cookieContainer = new CookieContainer();
            var baseUrl = new System.Uri(Helper.GetBaseUrl(url));
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = baseUrl })
            {
                client.Timeout = TimeSpan.FromSeconds(timeOut);
                var cookies = StrToCookies(cookie);
                foreach (var item in cookies)
                {
                    try
                    {
                        cookieContainer.Add(baseUrl, item);
                    }
                    catch (System.Exception ex)
                    {
                        continue;
                    }
                }
                var result = await client.GetAsync(url);
                await next(result);
            }
            return;
        }

        /// <summary>
        /// GET
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public HttpResponseMessage Get(string url)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var result = httpClient.GetAsync(url).Result;
                return result;
            }
        }

        /// <summary>
        /// GET
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var result = await httpClient.GetAsync(url);
                return result;
            }
        }
        
    }
}