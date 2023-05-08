---
layout: default
title: 使用Service注入(WebAPI專案)
parent: 快速開始(QuickStart)
nav_order: 5
---

# 使用Service注入(WebAPI專案)

本篇文件介紹，如何在WebAPI專案中使用Service注入。

## 操作步驟

### 1. 建立新專案

依照「[建立WebAPI專案](../建立WebAPI專案/建立WebAPI專案.html)」的操作步驟，建立新的WebAPI專案「WebApplication1」。

### 2. 新增DemoService

在專案裡新增Modules資料夾，並加入DemoService.cs。

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

在專案裡修改HomeController.cs。

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
        public ActionResult<object> Index()
        {
            // Message
            var message = _demoService.GetMessage();

            // Return
            return new { message = message };
        }
    }
}
```

### 4. 修改appsettings.json

在專案裡修改appsettings.json。

```json
{
  "WebApplication1": {
    "DemoService": {
      "Message": "Hello World"
    }
  }
}
```

### 5. 執行專案

完成以上操作步驟後，就已成功在WebAPI專案中使用Service注入。按F5執行專案，使用Postman呼叫API：/Home/Index，可以在結果視窗看到{"message":"Hello World"}的訊息。

## 範例檔案

[https://github.com/Clark159/MDP.Net/tree/master/demo/01.QuickStart/使用Service注入(WebAPI專案)](https://github.com/Clark159/MDP.Net/tree/master/demo/01.QuickStart/使用Service注入(WebAPI專案))
