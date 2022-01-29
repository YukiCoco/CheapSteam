# Quick Start
## 获得 BUFF 和 STEAM 的信息

### BUFF Session

以 Chrome 为例，登录 BUFF 后点左上角的小锁，再点 Cookie 就能找到了  
>buff不能直接双击复制，会不完整，显示框显示的也不完整，需要手动拉到左边后从头到尾复制 [issue #1](https://github.com/YukiCoco/CheapSteam/issues/1).  

![2020-12-13_18-08.png](https://blob.keylol.com/forum/202012/14/172856vjfuswziw26e0e5w.png)
### STEAM Cookies

登录 STEAM 市场后，随便打开一个物品，如 [这个](https://steamcommunity.com/market/listings/730/AUG%20%7C%20Tom%20Cat%20%28Field-Tested%29) 然后按 F12 调出开发者模式.

选择网络→点下面任意一个新建立的连接→Request Headers→全部复制 Cookie 这个 Header.

![Untitled.png](https://blob.keylol.com/forum/202012/14/172921uc65v3vii3rpp536.png)

## 修改设置

1. 打开 `CheapSteam.UI.exe`
2. 访问设置页面 `http://127.0.0.1:1272/settings`，将 `BUFF Seesion` 和 `Steam Cookies` 修改为上面获得的字段
3. 如果你在中国大陆无法访问 steam 社区，可以考虑使用 [steamcommunity 302](https://keylol.com/t339527-1-1). 或者将 `代理地址` 填写为你科学上网软件提供的本地 HTTP 代理地址，比如 [`http://127.0.0.1:7890`](http://127.0.0.1:7890/) 
4. 对爬虫配置做你需要的修改

## 开始使用

在 `Steam 市场` 及 `网易 BUFF` 连通性测试都通过的情况下，点启动即可。爬取的数据将在 `http://127.0.0.1:1272/fetchdata` 页面显示。