---
layout: default
title: 服務注入(FactoryPattern)
parent: 核心模組(Kernel)
nav_order: 4
---

# 服務注入(FactoryPattern)

在MDP.Net核心模組中，「服務注入模組」參考Configuration參數設定及系統內註冊的服務，依照下列規則進行服務注入：

- 注入至「ServiceAttribute註冊的Type」的建構子。
```
1. 使用Type裡參數(Parameter)數量最多的建構子，進行服務注入。
2. 建構子的參數類型(Parameter.Type)為基本數據，使用參數名稱(Parameter.Name)取得Configuration參數內容，轉型Configuration參數內容為基本數據後注入。
3. 建構子的參數類型(Parameter.Type)為物件類別，使用參數名稱(Parameter.Name)取得Configuration參數內容，為InstanceName，用以取得NamedInstance後注入。
4. 建構子的參數類型(Parameter.Type)為物件類別，使用參數名稱(Parameter.Name)取得Configuration參數內容，為Null或沒有設定，預設取得TypedInstance後注入。
5. 建構子的參數類型(Parameter.Type)為物件類別，使用參數名稱(Parameter.Name)取得Configuration參數內容，無法取得Instance時，使用無參數建構子生成Instance，再綁定(Bind)Configuration參數內容後注入。
```

- 注入至「Controller」的建構子。
```
1. 依照ASP.NET Core的規則注入。
2. 上述規則包含，建構子的參數類型(Parameter.Type)為物件類別，取得TypedInstance後注入。
```

本篇文件介紹，如何使用MDP.Net核心模組中「服務注入模組」，參考Configuration參數設定，進行服務注入。

## 操作步驟

### 1. 建立新專案

依照「[建立MvcPage專案](../建立MvcPage專案/建立MvcPage專案.html)」的操作步驟，建立新的MvcPage專案「WebApplication1」。

### 2. 新增DemoContext、DemoService、DemoSetting

在專案裡新增Modules資料夾，並加入DemoContext.cs、DemoService.cs、DemoSetting.cs。

DemoContext：使用ServiceAttribute註冊自己Type的Instance，由建構子接受下列的服務注入。
- DemoService demoServiceByTyped：注入Type為DemoService的TypedInstance。
- DemoService demoServiceByNamed：注入Name為DemoService的NamedInstance。
- DemoService demoServiceByNamed001：注入Name為DemoService[001]的NamedInstance。
- DemoSetting demoSetting：注入反射生成後，綁定(Bind)Configuration參數內容的Instance。
- string message：轉型Configuration參數內容為基本數據後注入。

```csharp
using MDP.Registration;
using System;

namespace WebApplication1
{
    [Service<DemoContext>()]
    public class DemoContext
    {
        // Fields
        private readonly DemoService _demoServiceByTyped;

        private readonly DemoService _demoServiceByNamed;

        private readonly DemoService _demoServiceByNamed001;

        private readonly DemoSetting _demoSetting;

        private readonly string _message;


        // Constructors
        public DemoContext
        (
            DemoService demoServiceByTyped,
            DemoService demoServiceByNamed,
            DemoService demoServiceByNamed001,
            DemoSetting demoSetting,
            string message
        )
        {
            // Default
            _demoServiceByTyped = demoServiceByTyped;
            _demoServiceByNamed = demoServiceByNamed;
            _demoServiceByNamed001 = demoServiceByNamed001;
            _demoSetting = demoSetting;
            _message = message;
        }


        // Methods
        public string GetMessage()
        {
            // Message
            var message = string.Empty;

            //  DemoService
            message += $"{_demoServiceByTyped.GetMessage()} by DemoServiceByTyped.GetMessage()" + Environment.NewLine;
            message += $"{_demoServiceByNamed.GetMessage()} by DemoServiceByNamed.GetMessage()" + Environment.NewLine;
            message += $"{_demoServiceByNamed001.GetMessage()} by DemoServiceByNamed[001].GetMessage()" + Environment.NewLine;
            message += Environment.NewLine;

            // DemoSetting
            message += $"{_demoSetting.Message} by DemoSetting.Message" + Environment.NewLine;
            message += Environment.NewLine;

            // DemoContext
            message += $"{_message} by DemoContext.Message" + Environment.NewLine;

            // Return
            return message;
        }
    }
}
```

DemoService：使用ServiceAttribute註冊自己Type的Instance，由建構子接受下列的服務注入。
- string message：轉型Configuration參數內容為基本數據後注入。
    
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

DemoSetting：做為服務注入Context時，綁定(Bind)Configuration參數內容的Instance。

```csharp
namespace WebApplication1
{
    public class DemoSetting
    {
        // Properties
        public string Message { get; set; } = string.Empty;
    }
}
```

### 3. 修改appsettings.json

在專案裡修改appsettings.json，調整為上述步驟2對應的Configuration參數內容。

```json
{
  "WebApplication1": {

    // DemoContext
    "DemoContext": {

      // DemoService
      "DemoServiceByTyped": null,
      "DemoServiceByNamed": "DemoService",
      "DemoServiceByNamed001": "DemoService[001]",

      // DemoSetting
      "DemoSetting": {
        "Message": "Hello World (DemoSetting)"
      },

      // DemoContext
      "Message": "Hello World (DemoContext)"
    },

    // DemoService: Named, Typed
    "DemoService": {
      "Message": "Hello World (DemoService)"
    },

    // DemoService: Named
    "DemoService[001]": {
      "Message": "Hello World (DemoService[001])"
    }
  }
}
```

### 4. 修改HomeController

在專案裡修改HomeController.cs，注入DemoContext的TypedInstance。

```csharp
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1
{
    public class HomeController : Controller
    {
        // Fields
        private readonly DemoContext _demoContext;


        // Constructors
        public HomeController(DemoContext demoContext)
        {
            // Default
            _demoContext = demoContext;
        }


        // Methods
        public ActionResult Index()
        {
            // Message
            this.ViewBag.Message = _demoContext.GetMessage();

            // Return
            return View();
        }
    }
}
```

### 5. 修改Index.cshtml

在專案裡修改Index.cshtml，讓內容可以呈現文字換行。

```html
<!DOCTYPE html>

<html>
<head>
    <meta charset="utf-8" />
    <title></title>
</head>
<body>

    <!--Title-->
    <h2>@Html.Raw( @ViewBag.Message.Replace(Environment.NewLine, "<br/>"))</h2>
    <hr />

</body>
</html>
```

### 6. 執行專案

完成以上操作步驟後，就已成功在MvcPage專案中使用服務注入(FactoryPattern)。按F5執行專案，使用Browser開啟Page：/Home/Index，可以在網頁內容看到下列由DemoContext所提供的訊息內容。

```
Hello World (DemoService) by DemoServiceByTyped.GetMessage()
Hello World (DemoService) by DemoServiceByNamed.GetMessage()
Hello World (DemoService[001]) by DemoServiceByNamed[001].GetMessage()
Hello World (DemoSetting) by DemoSetting.Message
Hello World (DemoContext) by DemoContext.Message
```

## 範例檔案

[https://github.com/Clark159/MDP.Net/tree/master/demo/02.Kernel/服務注入(FactoryPattern)](https://github.com/Clark159/MDP.Net/tree/master/demo/02.Kernel/服務注入(FactoryPattern))