---
layout: default
title: 功能說明
parent: 日誌紀錄
nav_order: 1
---

# MDP.Logging (施工中)

MDP.Logging是一個.NET開發模組，協助開發人員快速建立具有日誌紀錄的應用系統。提供NLog、Log4net、Serilog等功能模組，用以簡化開發流程並滿足多變的商業需求。

- 說明文件：[https://clark159.github.io/MDP.Net/](https://clark159.github.io/MDP.Net/)

- 程式源碼：[https://github.com/Clark159/MDP.Net/](https://github.com/Clark159/MDP.Net/)


## 模組架構

![MDP.Logging-模組架構.png](https://clark159.github.io/MDP.Net/日誌紀錄/功能說明/MDP.Logging-模組架構.png)

### 模組掛載

MDP.Logging擴充.NET Core既有的日誌紀錄，加入NLog、Log4net、Serilog等功能模組的掛載功能。開發人員可以透過設定Config設定，掛載在執行階段使用的功能模組。

```
// Config設定
{
  "Logging": {
    "NLog": { "ConfigFile" : "nlog.config"}
  }
}

- 命名空間：Logging
- 模組名稱：NLog
- 模組參數：ConfigFile="nlog.config"。(nlog.config是預設值，可省略)
```

### 抽象隔離

MDP.Logging擴充.NET Core既有的日誌紀錄，加入ILogger介面、LoggerAdapter物件。ILogger介面作為抽象層，提供給應用程式使用，以減少應用程式對於元件、平台、框架的直接依賴。並且ILogger介面透過LoggerAdapter物件，轉接提供.NET Core框架內建的日誌紀錄功能。

ILogger介面：用來提供日誌服務的介面。

- 命名空間：

```
MDP.Logging
```

- 類別定義：

```
public interface ILogger

public interface ILogger<TCategory> : ILogger

- TCategory：寫入日誌的類別(Class)
```

- 類別方法：

```
// Debug
void LogDebug(string message, params object[] args);

void LogDebug(Exception exception, string message, params object[] args);

// Trace
void LogTrace(string message, params object[] args);

void LogTrace(Exception exception, string message, params object[] args);

// Information
void LogInformation(string message, params object[] args);

void LogInformation(Exception exception, string message, params object[] args);

// Warning
void LogWarning(string message, params object[] args);

void LogWarning(Exception exception, string message, params object[] args);

// Error
void LogError(string message, params object[] args);

void LogError(Exception exception, string message, params object[] args);

// Critical
void LogCritical(string message, params object[] args);

void LogCritical(Exception exception, string message, params object[] args);

- message：寫入日誌的訊息內容。
- args：寫入日誌的物件。
- exception：寫入日誌的例外。
```


## 模組使用

### 加入模組

MDP.Logging預設內建在MDP.Net專案範本內，依照下列操作步驟，即可建立包含MDP.Logging模組的專案。

- 在命令提示字元輸入下列指令，使用MDP.Net專案範本建立專案。
 
```
// 建立API服務、Web站台
dotnet new install MDP.WebApp
dotnet new MDP.WebApp -n WebApplication1

// 建立Console程式
dotnet new install MDP.ConsoleApp
dotnet new MDP.ConsoleApp -n ConsoleApp1
```

### 寫入日誌

建立包含MDP.Logging模組的專案之後，就可以注入並使用ILogger介面來寫入日誌。

```
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
            
             // Log
    logger.LogError("Hello World");
        }
    }
}
```

## 模組範例

專案開發過程，需要將日誌訊息輸出，方便開發人員觀測系統執行狀況。本篇範例協助開發人員使用MDP.Logging，逐步完成必要的設計和實作。

- 範例下載：[WebApplication1.zip](https://clark159.github.io/MDP.Net/日誌紀錄/快速開始/WebApplication1.zip)

### 操作步驟

1.開啟命令提示字元，輸入下列指令。用以安裝MDP.WebApp範本、並且建立一個名為WebApplication1的Web站台。

```
dotnet new install MDP.WebApp
dotnet new MDP.WebApp -n WebApplication1
```