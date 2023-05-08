---
layout: default
title: 服務註冊(ServiceAttribute)
parent: 核心模組(Kernel)
nav_order: 2
---

# 服務註冊(ServiceAttribute)

在MDP.Net核心模組中，「服務註冊模組」提供ServiceAttribute進行服務註冊。依照下列的類別宣告及參數設定，就可以在系統裡註冊Type(FixedDemoService)為Service(DemoService)的Instance。

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

```json
{
  "WebApplication1": {
    "FixedDemoService": {
      "Message": "Hello World 123"
    },
    "FixedDemoService[A]": {
      "Message": "Hello World AAA"
    }
  }
}
```

「服務註冊模組」所提供的ServiceAttribute，參考Configuration參數設定，依照下列規則在系統裡註冊Type為Service的Instance：

```
1. 註冊Type為Service的NamedInstance：Named={Type.Namespace}.{Type.ClassNameType.ClassName}[*]
2. 註冊Type為Service的NamedInstance：Named={Type.ClassName}[*]
3. 註冊Type為Service的TypedInstance：Typed={Type}
4. 上述2、3，最終都是回傳1的NamedInstance
5. 具有[*]別名的Type，不會註冊3的TypedInstance
6. 範例[*]別名的Type："FixedDemoService[A]"、"FixedDemoService[1]"
7. Configuration裡Namespace的每個Type，都會依照上列步驟註冊Type為Service的Instance。
```

本篇文件介紹，如何使用MDP.Net核心模組中，「服務註冊模組」所提供的ServiceAttribute進行服務註冊。

## 操作步驟

### 1. 建立新專案

依照「[建立MvcPage專案](../建立MvcPage專案/建立MvcPage專案.html)」的操作步驟，建立新的MvcPage專案「WebApplication1」。

### 2. 新增DemoService

在專案裡新增Modules資料夾，並加入DemoService.cs。

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

在專案裡的Modules資料夾，加入FixedDemoService.cs，註冊FixedDemoService為DemoService的Instance。

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

### 5. 修改appsettings.json

在專案裡修改appsettings.json，加入FixedDemoService的註冊參數設定。

```json
{
  "WebApplication1": {
    "FixedDemoService": {
      "Message": "Hello World"
    }
  }
}
```

### 6. 執行專案

完成以上操作步驟後，就已成功在MvcPage專案中使用服務註冊(ServiceAttribute)。按F5執行專案，使用Browser開啟Page：/Home/Index，可以在網頁內容看到"Hello World"的訊息，這個由FixedDemoService所提供的訊息內容。

## 範例檔案

[https://github.com/Clark159/MDP.Net/tree/master/demo/02.Kernel/服務註冊(ServiceAttribute)](https://github.com/Clark159/MDP.Net/tree/master/demo/02.Kernel/服務註冊(ServiceAttribute))
