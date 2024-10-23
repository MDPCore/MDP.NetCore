---
layout: default
title: NLog日誌紀錄
parent: 日誌紀錄
nav_order: 2
---


# MDP.Logging.NLog

MDP.Logging.NLog擴充.NET Core既有的日誌紀錄，加入NLog日誌寫入功能。開發人員可以透過Config設定，掛載在執行階段使用的NLog日誌紀錄。

- 說明文件：[https://mdpcore.github.io/MDP.NetCore/](https://mdpcore.github.io/MDP.NetCore/)

- 程式源碼：[https://github.com/MDPCore/MDP.NetCore/](https://github.com/MDPCore/MDP.NetCore/)


## 模組使用

### 加入專案

MDP.Logging.NLog預設獨立在MDP.Net專案範本外，依照下列操作步驟，即可建立加入MDP.Logging.NLog模組的專案。

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
MDP.Logging.NLog
```

### 設定參數

建立包含MDP.Logging.NLog模組的專案之後，在專案裡可以透過Config設定，掛載在執行階段使用的NLog日誌紀錄。

```
// Config設定
{
  "Logging": {
    "NLogLogger": { "ConfigFile" : "nlog.config"}
  }
}
- 命名空間：Logging
- 掛載的日誌類別：NLogLogger
- NLog的設定檔路徑：ConfigFile="nlog.config"。(nlog.config是預設值，可省略)
```


## 模組範例

將日誌訊息輸出到Log檔案，方便開發人員觀測系統執行狀況，是開發系統時常見的功能需求。本篇範例協助開發人員使用MDP.Logging.NLog，逐步完成必要的設計和實作。

- 範例下載：[WebApplication1.zip](https://mdpcore.github.io/MDP.NetCore/日誌紀錄/NLog日誌紀錄/WebApplication1.zip)

### 操作步驟

1.開啟命令提示字元，輸入下列指令。用以安裝MDP.WebApp範本、並且建立一個名為WebApplication1的Web站台。

```
dotnet new install MDP.WebApp
dotnet new MDP.WebApp -n WebApplication1
```

2.使用Visual Studio開啟WebApplication1專案，在專案裡用NuGet套件管理員新增下列NuGet套件。

```
MDP.Logging.NLog
```

3.於專案內改寫appsettings.json及加入nlog.config，用以掛載NLogLogger並依照nlog.config設定提供日誌服務。(nlog.config是預設值)

```
{
  "Logging": {
    "NLogLogger": { }
  }
}
```

```
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd" 
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
      autoReload="true"
      throwExceptions="false"
      internalLogFile="nlog-internal.log"
      internalLogLevel="Warn" >
  
  <!-- Targets -->
  <targets async="true">
    <!-- FileLog -->
    <target name="FileLog" xsi:type="File" 
            filename="log/${gdc:item=ApplicationName}-${cached:cached=true:Inner=${date:format=yyyyMMdd}:CacheKey=${shortdate}}.log"
            archiveFileName="log/${gdc:item=ApplicationName}-{#}.log"
            archiveEvery="Day"
            archiveNumbering="Date"
            archiveDateFormat="yyyyMMdd"
            maxArchiveFiles="30"
            layout="${date:format=yyyy-MM-dd HH\:mm\:ss.fff} [${threadid}] ${level:uppercase=true} ${logger} - ${message}${when:when=length('${exception}')>0:Inner=${newline}}${exception:format=tostring}"
    ></target>    
  </targets>
  
  <!-- Rules -->
  <rules>
    <!-- FileLog -->
    <logger writeTo="FileLog" name="*" minlevel="Trace" />
  </rules>  

</nlog>
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

5.執行專案，於專案的log資料夾裡可以看到日誌檔案，日誌檔案中包含透過ILogger所寫入的日誌訊息 Hello World。(透過NLogLogger輸出)

![01.執行結果01.png](https://mdpcore.github.io/MDP.NetCore/日誌紀錄/NLog日誌紀錄/01.執行結果01.png)
