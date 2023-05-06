---
layout: default
title: 服務註冊(FactoryAttribute)
parent: 核心模組(Kernel)
nav_order: 3
---

# 服務註冊(FactoryAttribute)

在MDP.Net核心模組中，「服務註冊模組」提供FactoryAttribute進行服務註冊。依照下列的類別宣告及參數設定，就可以在系統裡使用APS.NET Core的WebApplicationBuilder進行服務註冊。

```csharp
using Microsoft.AspNetCore.Builder;
using Serilog;
using System;
using System.IO;
using System.Reflection;

namespace WebApplication1
{
    [MDP.Registration.Factory<WebApplicationBuilder, Setting>("Logging", "Serilog")]
    public class SerilogFactory
    {
        // Methods
        public void ConfigureService(WebApplicationBuilder webApplicationBuilder, Setting setting)
        {
            // ...
        }
    }
}
```

```json
{
  "Logging": {   
    "Serilog": {
      "LogFile":"logs/log.txt" 
    }
  }
}
```

「服務註冊模組」提供的FactoryAttribute，參考Configuration參數設定，依照下列規則啟動服務註冊：

```
1. 取得標註FactoryAttribute的FactoryClass。
2. 檢查FactoryAttribute所設定的Namespace、ServiceName是否存在Configuration。
3. 檢查FactoryClass，是否存在ConfigureService方法。
4. 檢查FactoryAttribute<TBuilder, TSetting>與FactoryClass.ConfigureService(TBuilder, TSetting)是否吻合。
5. 上述步驟通過後，執行ConfigureService進行服務註冊。
```

本篇文件介紹，如何使用MDP.Net核心模組中，「服務註冊模組」所提供的FactoryAttribute進行服務註冊。

## 操作步驟

### 1. 建立新專案

依照「[建立MvcPage專案](../建立MvcPage專案/建立MvcPage專案.html)」的操作步驟，建立新的MvcPage專案「WebApplication1」。


### 2. 新增NuGet套件

在專案裡使用NuGet套件管理員，新增下列NuGet套件。

```
Serilog.AspNetCore
```

### 3. 新增SerilogFactory

在專案裡新增Hosting資料夾，並加入SerilogFactory.cs，用以掛載Serilog將Log寫出到檔案系統。

```csharp
using Microsoft.AspNetCore.Builder;
using Serilog;
using System;
using System.IO;
using System.Reflection;

namespace WebApplication1
{
    [MDP.Registration.Factory<WebApplicationBuilder, Setting>("Logging", "Serilog")]
    public class SerilogFactory
    {
        // Methods
        public void ConfigureService(WebApplicationBuilder webApplicationBuilder, Setting setting)
        {
            // UseSerilog
            webApplicationBuilder.Host.UseSerilog((hostBuilderContext, loggerConfiguration) =>
            {
                // MinimumLevel
                loggerConfiguration.MinimumLevel.Error();

                // WriteToFile
                var entryDirectoryPath = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!;
                var LogFilePah = Path.Combine(entryDirectoryPath, setting.LogFile);
                loggerConfiguration.WriteTo.File(LogFilePah);
            });
        }


        // Class
        public class Setting
        {
            // Properties
            public string LogFile { get; set; } = string.Empty;
        }
    }
}
```

### 4. 修改HomeController

在專案裡修改HomeController.cs，於建構子注入ILogger備用，並於Index()使用ILogger把Message寫入Log。

```csharp
using MDP.Logging;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1
{
    public class HomeController : Controller
    {
        // Fields
        private readonly ILogger<HomeController> _logger;


        // Constructors
        public HomeController(ILogger<HomeController> logger)
        {
            // Default
            _logger = logger;
        }


        // Methods
        public ActionResult Index()
        {
            // Message
            this.ViewBag.Message = "Hello World";

            // Log
            _logger.LogError(this.ViewBag.Message);

            // Return
            return View();
        }
    }
}
```

### 5. 修改appsettings.json

在專案裡修改appsettings.json，加入SerilogFactory的註冊參數設定。

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.*": "Warning",
      "System.*": "Warning"
    },
    "Serilog": {
      "LogFile":"logs/log.txt" 
    }
  }
}
```

### 6. 執行專案

完成以上操作步驟後，就已成功在MvcPage專案中使用服務註冊(FactoryAttribute)。按F5執行專案，使用Browser開啟Page：/Home/Index，可以在結果視窗中看到"Hello World"的訊息。而在專案bin目錄底下的logs\log.txt裡，也可以看到"Hello World"的Log訊息。

```
2023-05-06 13:13:42.813 +08:00 [ERR] Hello World
```

## 範例檔案

[https://github.com/Clark159/MDP.Net/tree/master/demo/02.Kernel/服務註冊(FactoryAttribute)](https://github.com/Clark159/MDP.Net/tree/master/demo/02.Kernel/服務註冊(FactoryAttribute))
