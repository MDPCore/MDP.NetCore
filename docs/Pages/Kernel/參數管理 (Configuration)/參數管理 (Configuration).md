---
layout: default
title: 參數管理(Configuration)
parent: 核心模組(Kernel)
nav_order: 1
---

# 參數管理(Configuration)

在MDP.Net核心模組中的「參數管理模組」，參考執行環境(Environment)，依序載入下列Configuration設定檔：

- <EntryDir>\appsettings.json
- <EntryDir>\*.{Environment}.json
- <EntryDir>\config\appsettings.json
- <EntryDir>\config\*.{Environment}.json
- <EntryDir>\config\{Environment}\*.json
- {Environment}：Development\Staging\Production

本篇文件介紹，如何使用MDP.Net核心模組中的「參數管理模組」，依照執行環境進行參數設定切換。

## 操作步驟

### 1. 建立新專案

依照「[建立MvcPage專案](../建立MvcPage專案/建立MvcPage專案.html)」的操作步驟，建立新的MvcPage專案「WebApplication1」。

### 2. 新增DemoService

在專案裡新增Modules資料夾，並加入DemoService.cs。註冊DemoService為自己的Instance，且接收從Configuration參數設定注入的message。

```csharp
using MDP.Registration;

namespace WebApplication1
{
    [Service<DemoService>()]
    public class DemoService
    {
        // Fields
        private readonly string _message;


        // Constructors
        public DemoService(string message)
        {
            // Default
            _message = message;
        }


        // Methods
        public string GetMessage()
        {
            // Return
            return _message;
        }
    }
}
```

### 3. 修改HomeController

在專案裡修改HomeController.cs，注入DemoService的TypedInstance。

```csharp
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1
{
    public class HomeController : Controller
    {
        // Fields
        private readonly DemoService _demoService;


        // Constructors
        public HomeController(DemoService demoService)
        {
            // Default
            _demoService = demoService;
        }


        // Methods
        public ActionResult Index()
        {
            // Message
            this.ViewBag.Message = _demoService.GetMessage();

            // Return
            return View();
        }
    }
}
```

### 4. 修改appsettings.json

在專案裡修改appsettings.json。

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.*": "Warning",
      "System.*": "Warning"
    }
  }
}
```

在專案裡新增config\Development資料夾，並加入appsettings.json。

```json
{
  "WebApplication1": {
    "DemoService": {
      "Message": "Hello World 1 - Development"
    }
  }
}
```

在專案裡新增config\Staging資料夾，並加入appsettings.json。

```json
{
  "WebApplication1": {
    "DemoService": {
      "Message": "Hello World 2 - Staging"
    }
  }
}
```

在專案裡新增config\Production資料夾，並加入appsettings.json。

```json
{
  "WebApplication1": {
    "DemoService": {
      "Message": "Hello World 3 - Production"
    }
  }
}
```

### 5. 執行專案

修改Properties\launchSettings.json，並設定執行環境為"Development"。按F5執行專案，使用Browser開啟Page：/Home/Index，可以在網頁內容看到"Hello World 1 - Development"，這個來自config\Development\appsettings.json設定檔的訊息。

```
"ASPNETCORE_ENVIRONMENT": "Development"
```

修改Properties\launchSettings.json，並設定執行環境為"Staging"。按F5執行專案，使用Browser開啟Page：/Home/Index，可以在網頁內容看到"Hello World 2 - Staging"，這個來自config\Staging\appsettings.json設定檔的訊息。

```
"ASPNETCORE_ENVIRONMENT": "Staging"
```

修改Properties\launchSettings.json，並設定執行環境為"Production"。按F5執行專案，使用Browser開啟Page：/Home/Index，可以在網頁內容看到"Hello World 3 - Production"，這個來自config\Production\appsettings.json設定檔的訊息。

```
"ASPNETCORE_ENVIRONMENT": "Production"
```

## 範例檔案

[https://github.com/Clark159/MDP.Net/tree/master/demo/02.Kernel/參數管理(Configuration)](https://github.com/Clark159/MDP.Net/tree/master/demo/02.Kernel/參數管理(Configuration))
