---
layout: default
title: 服務註冊(ServiceAttribute)
parent: 核心模組(Kernel)
nav_order: 2
---

# 服務註冊(ServiceAttribute)

在MDP.Net的核心模組中，「服務註冊模組」提供ServiceAttribute進行服務註冊。依照下列的類別宣告內容，就可以在系統裡將Type(FixedDemoService)，宣告為Service(DemoService)。

```csharp
using MDP.Registration;

namespace WebApplication1
{
    [Service<DemoService>()]
    public class FixedDemoService : DemoService
    {
      // ...
    }
}
```

完成類別宣告之後，還需要在Configuration裡進行實例註冊。依照下列的參數設定內容，就可以在系統裡註冊一個Instance(FixedDemoService)，提供給服務注入模組使用。

```json
{
  "WebApplication1": {
    "FixedDemoService": {
      "Message": "Hello World"
    }
  }
}
```

本篇文件介紹，如何使用MDP.Net的服務註冊(ServiceAttribute)進行服務註冊。

## 操作步驟

### 1. 建立新專案

依照「[建立MvcPage專案](../建立MvcPage專案/建立MvcPage專案.html)」的操作步驟，建立新的MvcPage專案「WebApplication1」。

### 2. 新增DemoService

在專案裡新增Modules資料夾，並加入DemoService.cs：

```csharp
using MDP.Registration;

namespace WebApplication1
{
    public interface DemoService
    {
        // Methods
        string GetMessage();
    }
}

```

### 3. 新增FixedDemoService

在專案裡的Modules資料夾，加入FixedDemoService.cs：

```csharp
using MDP.Registration;

namespace WebApplication1
{
    [Service<DemoService>()]
    public class FixedDemoService : DemoService
    {
        // Fields
        private readonly string _message;


        // Constructors
        public FixedDemoService(string message)
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

### 4. 修改HomeController

在專案裡修改HomeController.cs：

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

### 5. 修改appsettings.json

在專案裡修改appsettings.json：

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.*": "Warning",
      "System.*": "Warning"
    }
  },

  "WebApplication1": {
    "FixedDemoService": {
      "Message": "Hello World"
    }
  }
}
```

### 6. 執行專案

完成以上操作步驟後，就已成功在MvcPage專案中使用服務註冊(ServiceAttribute)。按F5執行專案，使用Browser開啟Page：/Home/Index，可以在結果視窗中看到"Hello World"的訊息。

## 範例檔案

[https://github.com/Clark159/MDP.Net/tree/master/demo/02.Kernel/服務註冊(ServiceAttribute)](https://github.com/Clark159/MDP.Net/tree/master/demo/02.Kernel/服務註冊(ServiceAttribute))
