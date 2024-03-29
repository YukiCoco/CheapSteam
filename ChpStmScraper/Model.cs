using System;
using System.ComponentModel.DataAnnotations;

namespace ChpStmScraper.Models
{
    public class Goods
    {
        public enum GameKind
        {
            DOTA2 = 1,
            CSGO = 2,
        }
        public GameKind Kind { get; set; }
        [Key]
        [MaxLength(100)]
        public string Name { get; set; }
        /// <summary>
        /// Steam市场ID
        /// </summary>
        /// <value></value>
        [MaxLength(255)]
        public string SteamMarketID { get; set; }
        /// <summary>
        /// Buff市场ID
        /// </summary>
        /// <value></value>
        [MaxLength(100)]
        public string BuffID { get; set; }
        /// <summary>
        /// Steam出售数
        /// </summary>
        /// <value></value>
        public int SteamSellNum { get; set; }
        /// <summary>
        /// Buff出售数
        /// </summary>
        /// <value></value>
        public int BuffSellNum { get; set; }
        public double BuffSellPrice { get; set; }
        /// <summary>
        /// BUFF 求购价
        /// </summary>
        /// <value></value>
        public double BuffBuyPrice { get; set; }
        public double SteamSellPrice { get; set; }
        /// <summary>
        /// STEAM 求购价
        /// </summary>
        /// <value></value>
        public double SteamBuyPrice { get; set; }
        /// <summary>
        /// Steam 出售价税后比例 BuffSellPrice / SteamSellRadio
        /// </summary>
        /// <value></value>
        public double SteamSellRadio { get; set; }
        /// <summary>
        /// Steam 求购价税后比例 BuffSellPrice / SteamBuyPrice
        /// </summary>
        /// <value></value>
        public double SteamBuyRadio { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        /// <value></value>
        public DateTime UpdateTime{get; set;}  = DateTime.UtcNow.AddHours(8);
    }
}
