---
layout: default
title: 服務注入(SingletonPattern)
parent: 核心模組(Kernel)
nav_order: 5
---

# 服務注入(SingletonPattern)

在MDP.Net核心模組中，「服務注入模組」注入使用ServiceAttribute註冊的Instance時，會參考singleton參數，決定注入的Instance為全局同一個、或是每次注入都是一個新的。依照下列的類別宣告，就可以在系統裡註冊Type(DemoService)為自己的Instance，並且全局都是同一個Instance。

```csharp
using MDP.Registration;

namespace WebApplication1
{
    [Service<DemoService>(singleton:true)]
    public class DemoService
    {
        // Fields
        private int _count = 0;


        // Methods
        public string GetMessage()
        {
            // Count
            _count++;

            // Return
            return "Count=" + _count.ToString();
        }
    }
}
```

本篇文件介紹，如何使用MDP.Net核心模組中「服務注入模組」，在注入使用ServiceAttribute註冊的Instance時，參考singleton參數，決定注入的Instance為全局同一個、或是每次注入都是一個新的。

## 操作步驟

### 1. 建立新專案

依照「[建立MvcPage專案](../建立MvcPage專案/建立MvcPage專案.html)」的操作步驟，建立新的MvcPage專案「WebApplication1」。

### 2. 新增DemoService

在專案裡新增Modules資料夾，加入DemoService.cs。並使用ServiceAttribute註冊DemoService為自己的Instance，且設定singleton參數為true，讓注入的Instance為全局同一個。

```csharp
using MDP.Registration;

namespace WebApplication1
{
    [Service<DemoService>(singleton:true)]
    public class DemoService
    {
        // Fields
        private int _count = 0;


        // Methods
        public string GetMessage()
        {
            // Count
            _count++;

            // Return
            return "Count=" + _count.ToString();
        }
    }
}
```

### 3. 修改appsettings.json

在專案裡修改appsettings.json，加入DemoService的註冊參數設定。

```json
{
  "WebApplication1": {
    "DemoService": {
      
    }
  }
}
```

### 4. 修改HomeController

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

### 6. 執行專案

完成以上操作步驟後，就已成功在MvcPage專案中使用服務注入(SingletonPattern)。按F5執行專案，使用Browser開啟Page：/Home/Index，可以在網頁內容看到下列由DemoService所提供的訊息內容。

```
Count=1
```

此時singleton參數為true，注入的DemoService是全局同一個Instance。按F5重新整理網頁，每按一次Count數量就會增加1。

```
Count=2~N
```

接著修改singleton參數為false，讓注入的DemoService每次注入都是一個新的Instance。按F5重新整理網頁，Count數量會固定為1。

```
Count=1
```

- 

## 範例檔案

[https://github.com/Clark159/MDP.Net/tree/master/demo/02.Kernel/服務注入(SingletonPattern)](https://github.com/Clark159/MDP.Net/tree/master/demo/02.Kernel/服務注入(SingletonPattern))