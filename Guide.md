# Quick Start
## 获得 BUFF 和 STEAM 的信息

### BUFF Session

以 Chrome 为例，登录 BUFF 后点左上角的小锁，再点 Cookie 就能找到了  
>buff不能直接双击复制，会不完整，显示框显示的也不完整，需要手动拉到左边后从头到尾复制 [issue #1](https://github.com/YukiCoco/CheapSteam/issues/1).
![2020-12-13_18-08.png](https://cdnimg.kurisu.moe/images/2020/12/13/2020-12-13_18-08.png)
### STEAM Cookie

登录 STEAM 市场后，随便打开一个物品，如 [这个](https://steamcommunity.com/market/listings/730/AUG%20%7C%20Tom%20Cat%20%28Field-Tested%29) 然后按 F12 调出开发者模式.

选择网络→点下面任意一个新建立的连接→Request Headers→全部复制 Cookie 这个 Header.

![Untitled.png](https://cdnimg.kurisu.moe/images/2020/12/13/Untitled.png)

## 修改配置文件

1. 修改 `appsettings.json` 文件，找到 `BuffSession` 与 `SteamCookies` 字段，分别修改为 BUFF Cookie 里的 `Session` 和 Steam 市场的 `Cookie` Header.
2. 如果你在中国大陆无法访问 steam 社区，可以考虑使用 [steamcommunity 302](https://keylol.com/t339527-1-1).或者使用配置文件里的 `ProxyUrl` 字段，填写为你科学上网软件提供的本地 HTTP 代理地址，比如 [`http://127.0.0.1:7890`](http://127.0.0.1:7890/) 
3. 打开 `ChpStmScraper.WebApi` 即可
4. 更详细的配置文件详解请查看 [这里](https://github.com/YukiCoco/CheapSteam/blob/master/Config.md)
