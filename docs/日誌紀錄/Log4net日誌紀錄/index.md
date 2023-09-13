---
layout: default
title: Log4net日誌紀錄
parent: 日誌紀錄
nav_order: 4
---

# MDP.Logging.Log4net

MDP.Logging.Log4net擴充.NET Core既有的日誌紀錄，加入Log4net日誌寫入功能。開發人員可以透過Config設定，掛載在執行階段使用的Log4net日誌紀錄。
    

## 模組使用

### 建立專案

MDP.Logging.Log4net預設獨立在MDP.Net專案範本外，依照下列操作步驟，即可建立加入MDP.Logging.Log4net模組的專案。

- 在命令提示字元輸入下列指令，使用MDP.Net專案範本建立專案。
 
```
// 建立API服務、Web站台
dotnet new install MDP.WebApp
dotnet new MDP.WebApp -n WebApplication1

// 建立Console程式
dotnet new install MDP.ConsoleApp
dotnet new MDP.ConsoleApp -n ConsoleApp1
```

- 使用Visual Studio開啟專案。在專案裡使用NuGet套件管理員，新增下列NuGet套件。

```
MDP.Logging.Log4net
```

### 設定參數

建立包含MDP.Logging.Log4net模組的專案之後，在專案裡可以透過Config設定，掛載在執行階段使用的Log4net日誌紀錄。

```
// Config設定
{
  "Logging": {
    "Log4netLogger": { "ConfigFile" : "log4net.config"}
  }
}

- 命名空間：Logging
- 掛載的日誌類別：Log4netLogger
- Log4net的設定檔路徑：ConfigFile="log4net.config"。(log4net.config是預設值，可省略)
```


## 模組範例

專案開發過程，需要將日誌訊息輸出，方便開發人員觀測系統執行狀況。本篇範例協助開發人員使用MDP.Logging.Log4net，逐步完成必要的設計和實作。

- 範例下載：[WebApplication1.zip](https://clark159.github.io/MDP.Net/日誌紀錄/Log4net日誌紀錄/WebApplication1.zip)

### 操作步驟

1.開啟命令提示字元，輸入下列指令。用以安裝MDP.WebApp範本、並且建立一個名為WebApplication1的Web站台。

```
dotnet new install MDP.WebApp
dotnet new MDP.WebApp -n WebApplication1
```

2.使用Visual Studio開啟WebApplication1專案，在專案裡用NuGet套件管理員新增下列NuGet套件。

```
MDP.Logging.Log4net
```

3.於專案內改寫appsettings.json及加入log4net.config，用以掛載Log4netLogger並依照log4net.config設定提供日誌服務。(log4net.config是預設值)

```
{
  "Logging": {
    "Log4netLogger": { }
  }
}
```

```
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <log4net>
    <appender name="FileLog" type="log4net.Appender.RollingFileAppender">           
      <file type="log4net.Util.PatternString" value="log/%property{ApplicationName}-" />
      <rollingStyle value="Date" />
      <datePattern value="yyyyMMdd.lo\g" />
      <staticLogFileName value="false" />
      <appendToFile value="true" />        
      <layout type="log4net.Layout.PatternLayout">
        <IgnoresException value="false" />
        <conversionPattern value="%date{yyyy-MM-dd HH:mm:ss.fff} [%thread] %level %logger - %message%newline%exception" />
      </layout>
    </appender>
    <root>
      <appender-ref ref="FileLog" />
    </root>
  </log4net>
</configuration>
```

4.改寫專案內的Controllers\HomeController.cs、Views\Home\Index.cshtml，注入並使用ILogger。

```
using MDP.Logging;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1
{
    public class HomeController : Controller
    {
        // Fields
        private readonly ILogger _logger = null;


        // Constructors
        public HomeController(ILogger<HomeController> logger)
        {
            // Default
            _logger = logger;
        }


        // Methods
        public ActionResult Index()
        {
            // Log
            _logger.LogError("Hello World");

            // Return
            return View();
        }
    }
}
```

```
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>WebApplication1</title>
</head>
<body>

    <!--Title-->
    <h2>WebApplication1</h2>
    <hr />

</body>
</html>
```

5.執行專案，於專案bin\log資料夾內可以看到日誌檔案，日誌檔案中包含透過ILogger所寫入的日誌訊息 Hello World。(透過Log4netLogger輸出)

![01.執行結果01.png](https://clark159.github.io/MDP.Net/日誌紀錄/Log4net日誌紀錄/01.執行結果01.png)
