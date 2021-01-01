# CheapSteam
Cheap Steam 为您提供 STEAM 与 BUFF 的饰品价格对比数据，您可以从此快速找出价格合适的饰品，在 BUFF 购买然后在 Steam 市场出售，以此赚取大额的差价。  
这世上本没有倒狗，亦或者人人都是倒狗。  
[我的 Steam 主页](https://steamcommunity.com/id/KurisuMoe)

## Feature
+ 使用简单
+ 多线程高速爬取数据
+ 多数据过滤条件，快速找到合适的饰品

## Usage
[Guide](https://github.com/YukiCoco/CheapSteam/blob/master/Guide.md)

## Download
非常不建议从其他地方下载，因为要读取 Steam 和 BUFF Cookie，从其他地方下载的程序有可能会包含恶意代码导致财产损失。  
[Release](https://github.com/YukiCoco/CheapSteam/releases)

## Demo
![Peek-2020-12-14-16-32.gif](https://cdnimg.kurisu.moe/images/2020/12/14/Peek-2020-12-14-16-32.gif)  
![2020-12-13_00-57_1.png](https://cdnimg.kurisu.moe/images/2020/12/13/2020-12-13_00-57_1.png)

## 已知问题  
1. 错误：找不到 MarketID，可能是爬取速度过快  
速度太快了，steam市场做了查询限制，建议调低线程数  
2. 长时间（一小时以上）爬取后出现错误  
IP被 steam 市场暂时 ban 了，请不要长时间开爬虫  
3. **被 BUFF 封号**  
**频繁访问会被 BUFF 封号（这个需要给 BUFF 发工单解封），请不要长时间开爬虫（大于半小时），或者调低线程数（默认为4线程）.**

## LICENSE
LICENSE under the GNU General Public License v3.0.
