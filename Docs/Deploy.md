# 在服务器部署
## 前言  
`v2.1.0` 版本后添加了验证模块，您可以在服务器部署 CheapSteam。  
此种方式的优点在于你无需通过特殊手段访问 Steam 市场（假定您部署在海外服务器上），而且对市场的爬取速度较为理想。
## 下载
通过 [发布页](https://github.com/YukiCoco/CheapSteam/releases) 下载适用于 Linux 的程序，然后解压到任意位置。
## 部署
### 反向代理
将下列配置文件添加到 nginx 的配置中，一般路径为 `/etc/nginx/sites-enabled/`  
**注意需要将下面的域名修改为你的域名**
````conf
server {
  listen 80;
  listen [::]:80;
  server_name your.domain.name;

location / {
        proxy_pass http://127.0.0.1:1272/;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection 'upgrade';
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
    }
  
}
````
执行 `nginx -t` 验证通过后再执行 `systemctl reload nginx`
### 程序守护
将下列配置文件添加到 systemed 的配置中，一般编辑并保存的文件为 `/etc/systemd/system/cheapsteam.service`  
**注意需要将下面的路径修改为你解压的路径**
````
[Unit]
Description=CheapSteam Application

[Service]
WorkingDirectory=/path/to/CheapSteam
ExecStart=/path/to/CheapSteam.UI
Restart=always
# Restart service after 10 seconds if the dotnet service crashes:
RestartSec=10
KillSignal=SIGINT
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

[Install]
WantedBy=multi-user.target
````
`systemctl start cheapsteam` 开启运行 CheapSteam
## 完成
此时可以访问你的域名进入程序