---
layout: default
title: Serilog日誌紀錄
parent: 日誌紀錄
nav_order: 3
---


# MDP.Logging.Serilog

MDP.Logging.Serilog擴充.NET Core既有的日誌紀錄，加入Serilog日誌寫入功能。開發人員可以透過Config設定，掛載在執行階段使用的Serilog日誌紀錄。

- 說明文件：[https://clark159.github.io/MDP.Net/](https://clark159.github.io/MDP.Net/)

- 程式源碼：[https://github.com/Clark159/MDP.Net/](https://github.com/Clark159/MDP.Net/)


## 模組使用

### 加入專案

MDP.Logging.Serilog預設獨立在MDP.Net專案範本外，依照下列操作步驟，即可建立加入MDP.Logging.Serilog模組的專案。

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
MDP.Logging.Serilog
```

### 設定參數

建立包含MDP.Logging.Serilog模組的專案之後，在專案裡可以透過Config設定，掛載在執行階段使用的Serilog日誌紀錄。

```
// Config設定
{
  "Logging": {
    "SerilogLogger": { "ConfigFile" : "serilog.json"}
  }
}
- 命名空間：Logging
- 掛載的日誌類別：SerilogLogger
- Serilog的設定檔路徑：ConfigFile="serilog.json"。(serilog.json是預設值，可省略)
```


## 模組範例

專案開發過程，需要將日誌訊息輸出，方便開發人員觀測系統執行狀況。本篇範例協助開發人員使用MDP.Logging.Serilog，逐步完成必要的設計和實作。

- 範例下載：[WebApplication1.zip](https://clark159.github.io/MDP.Net/日誌紀錄/Serilog日誌紀錄/WebApplication1.zip)

### 操作步驟

1.開啟命令提示字元，輸入下列指令。用以安裝MDP.WebApp範本、並且建立一個名為WebApplication1的Web站台。

```
dotnet new install MDP.WebApp
dotnet new MDP.WebApp -n WebApplication1
```

2.使用Visual Studio開啟WebApplication1專案，在專案裡用NuGet套件管理員新增下列NuGet套件。

```
MDP.Logging.Serilog
```

3.於專案內改寫appsettings.json及加入serilog.json，用以掛載SerilogLogger並依照serilog.json設定提供日誌服務。(serilog.json是預設值)

```
{
  "Logging": {
    "SerilogLogger": { }
  }
}
```

```
{
  "Serilog": {
    "MinimumLevel": "Verbose",
    "WriteTo": [
      {
        "Name": "File",
        "Args": {
          "path": ".\\log\\log-.log",
          "rollingInterval": "Day",
          "outputTemplate": "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{ThreadId}] {Level} {SourceContext} - {Message}{NewLine}{Exception}"
        }
      }
    ]
  }
}
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

5.執行專案，於專案的log資料夾裡可以看到日誌檔案，日誌檔案中包含透過ILogger所寫入的日誌訊息 Hello World。(透過SerilogLogger輸出)

![01.執行結果01.png](https://clark159.github.io/MDP.Net/日誌紀錄/Serilog日誌紀錄/01.執行結果01.png)
