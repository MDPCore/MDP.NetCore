---
layout: default
title: 功能說明
parent: 日誌紀錄
nav_order: 1
---


# MDP.Logging

MDP.Logging是開源的.NET開發套件，協助開發人員快速建立具有日誌紀錄的應用系統。提供NLog、Log4net、Serilog等功能模組，用以簡化開發流程並滿足多變的商業需求。

- 說明文件：[https://mdpnetcore.github.io/MDP.NetCore/](https://mdpnetcore.github.io/MDP.NetCore/)

- 程式源碼：[https://github.com/MDPNetCore/MDP.NetCore/](https://github.com/MDPNetCore/MDP.NetCore/)


## 模組功能

![MDP.Logging-模組功能.png](https://mdpnetcore.github.io/MDP.NetCore/日誌紀錄/功能說明/MDP.Logging-模組功能.png)

### 模組掛載

MDP.Logging擴充.NET Core既有的日誌紀錄，加入NLog、Log4net、Serilog等功能模組的掛載功能。開發人員可以透過Config設定，掛載在執行階段使用的日誌紀錄。

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

### 日誌寫入

MDP.Logging擴充.NET Core既有的日誌紀錄，加入ILogger介面來提供日誌寫入功能，並做為抽象層以減少應用程式對於元件、平台、框架的直接依賴。而在系統底層ILogger介面則是由LoggerAdapter物件實作並轉接.NET Core框架內建的日誌寫入功能。
   
```
// 命名空間：
MDP.Logging

// 類別定義：
public interface ILogger
public interface ILogger<TCategory> : ILogger
- TCategory：寫入日誌的類別(Class)

// 類別方法：
void LogDebug(string message, params object[] args);
void LogDebug(Exception exception, string message, params object[] args);

void LogTrace(string message, params object[] args);
void LogTrace(Exception exception, string message, params object[] args);

void LogInformation(string message, params object[] args);
void LogInformation(Exception exception, string message, params object[] args);

void LogWarning(string message, params object[] args);
void LogWarning(Exception exception, string message, params object[] args);

void LogError(string message, params object[] args);
void LogError(Exception exception, string message, params object[] args);

void LogCritical(string message, params object[] args);
void LogCritical(Exception exception, string message, params object[] args);

- message：寫入日誌的訊息內容。
- args：寫入日誌的物件。
- exception：寫入日誌的例外。
```


## 模組使用

### 加入專案

MDP.Logging預設內建在MDP.Net專案範本內，依照下列操作步驟，即可建立加入MDP.Logging模組的專案。

- 在命令提示字元輸入下列指令，使用MDP.Net專案範本建立專案。
 
```
// 建立API服務、Web站台
dotnet new install MDP.WebApp
dotnet new MDP.WebApp -n WebApplication1

// 建立Console程式
dotnet new install MDP.ConsoleApp
dotnet new MDP.ConsoleApp -n ConsoleApp1
```

### 取得ILogger

建立包含MDP.Logging模組的專案之後，就可以注入ILogger介面來使用日誌紀錄。

```
using MDP.Logging;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1
{
    public class HomeController : Controller
    {
        // Constructors
        public HomeController(ILogger<HomeController> logger)
        {
            // Log
            logger.LogError("Hello World");
        }
    }
}
```


## 模組範例

將日誌訊息輸出到Console視窗，方便開發人員觀測系統執行狀況，是開發系統時常見的功能需求。本篇範例協助開發人員使用MDP.Logging，逐步完成必要的設計和實作。

- 範例下載：[WebApplication1.zip](https://mdpnetcore.github.io/MDP.NetCore/日誌紀錄/功能說明/WebApplication1.zip)

### 操作步驟

1.開啟命令提示字元，輸入下列指令。用以安裝MDP.WebApp範本、並且建立一個名為WebApplication1的Web站台。

```
dotnet new install MDP.WebApp
dotnet new MDP.WebApp -n WebApplication1
```

2.使用Visual Studio開啟WebApplication1專案。改寫專案內的Controllers\HomeController.cs、Views\Home\Index.cshtml，注入並使用ILogger。

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

3.執行專案，於開啟的Console視窗內，可以看到透過ILogger所寫入的日誌訊息 Hello World。(透過.NET Core底層的ConsoleLogger輸出)

![01.執行結果01.png](https://mdpnetcore.github.io/MDP.NetCore/日誌紀錄/功能說明/01.執行結果01.png)
