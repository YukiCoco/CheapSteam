# 配置文件详解
## 配置文件示例
````json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "ProxyUrl": "",
  "BuffUrl": "https://buff.163.com/api/market/goods?game=csgo&min_price=20",
  "BuffSession": "",
  "SteamCookies": "",
  "MaxThread": 4,
  "MinSellCount": 30,
  "ConnectionString": "Data Source=ChpStmScraper.db",
  "IsEnableScraper" : true,
  "ListeningUrls": [
    "http://127.0.0.1:1272"
  ]
}
````
## 字段解释
`ProxyUrl`: HTTP代理地址，留空则不使用  
`BuffUrl`: 爬取 BUFF 的起始地址，可以填入自定义条件，如 `https://buff.163.com/api/market/goods?game=dota2&min_price=20` 爬取 DOTA2 的饰品，最小价格为20  
`MaxThread`: 最大爬取线程，调高可以加快速度，但太快可能会被 Steam Ban IP  
`MinSellCount`: 在 BUFF 的最少销售数，低于此值不会进入数据库  
`ConnectionString`: 数据库连接字符串，你可以把数据库放在其他地方，然后来修改此值  
`IsEnableScraper`: 是否开启爬虫，可以关闭此值，如果你想看之前数据里存的数据的话  
`ListeningUrls`: 本地监听地址，如果要开多个爬虫需要修改此值，以防止端口冲突  
