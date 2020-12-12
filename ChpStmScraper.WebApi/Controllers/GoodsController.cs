using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ChpStmScraper.Models;
using System.Diagnostics;

namespace ChpStmScraper.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GoodsController : ControllerBase
    {

        private ScraperDbContext _dbContext { get; set; }
        private readonly ILogger<GoodsController> _logger;

        public GoodsController(ILogger<GoodsController> logger, ScraperDbContext dbContext)
        {
            _logger = logger;
            this._dbContext = dbContext;
        }

        public enum OrderBy
        {
            SteamBuyRadio,
            SteamSellRadio
        }

        /// <summary>
        /// 获取 Goods
        /// </summary>
        /// <param name="page">Good 数量</param>
        /// <param name="min_buff_price"></param>
        /// <param name="max_buff_price"></param>
        /// <returns></returns>
        public ActionResult Get(int page = 1, double min_buff_price = 10, double max_buff_price = 200, double min_steam_price = 12.5, double max_steam_price = 250, int min_buff_sell_num = 20, int min_steam_sell_num = 25, OrderBy order_by = OrderBy.SteamBuyRadio,Goods.GameKind kind = Goods.GameKind.CSGO)
        {
            try
            {

                var goodsQuerry = _dbContext.Goods.Where(g => g.BuffSellPrice <= max_buff_price
            && g.SteamSellNum >= min_steam_sell_num
            && g.BuffSellNum >= min_buff_sell_num
            && g.BuffSellPrice >= min_buff_price
            && g.SteamSellPrice <= max_steam_price
            && g.SteamSellPrice >= min_steam_price
            && g.Kind == kind)
            //.OrderBy(g => g.SteamBuyRadio).Take(num);
            .OrderBy(g => order_by == OrderBy.SteamBuyRadio ? g.SteamBuyRadio : g.SteamSellRadio);
                var goods = goodsQuerry.ToArray().SkipWhile((g, i) => i < 30 * (page - 1)).Take(30);
                decimal pagesCount = Math.Ceiling(Convert.ToDecimal(goodsQuerry.Count() / 30));
                return Ok(new
                {
                    status = 200,
                    data = new
                    {
                        result = goods,
                        pages_count = pagesCount
                    }
                });
            }
            catch (System.Exception ex)
            {
                return Ok(new
                {
                    status = 503,
                    message = ex.Message
                });
            }

        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <returns></returns>
        [HttpGet("search")]
        public ActionResult Search(string name)
        {
            var goods = _dbContext.Goods.Where(g => g.Name.Contains(name));
            return Ok(new
            {
                status = 200,
                data = new
                {
                    result = goods,
                    pages_count = Math.Ceiling(Convert.ToDouble(goods.Count()) / 30)
                }
            });
        }
    }
}
