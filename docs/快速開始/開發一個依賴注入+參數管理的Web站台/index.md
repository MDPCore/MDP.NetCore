---
layout: default
title: 開發一個依賴注入+參數管理的Web站台
parent: 快速開始
nav_order: 2
has_children: false
---

# 開發一個依賴注入+參數管理的Web站台

從零開始，開發一個依賴注入+參數管理的Web站台，是難度不高但繁瑣的工作項目。本篇內容協助開發人員使用MDP.Net，逐步完成必要的設計和實作。


## 範例程式

- 範例下載：[WebApplication1.zip](https://clark159.github.io/MDP.Net/快速開始/開發一個依賴注入+參數管理的Web站台/WebApplication1.zip)


## 開發步驟

1. 開啟命令提示字元，輸入下列指令。用以安裝MDP.WebApp範本、並且建立一個名為WebApplication1的Web站台。
```
dotnet new install MDP.WebApp
dotnet new MDP.WebApp -n WebApplication1
```

2. 使用Visual Studio開啟WebApplication1專案。並於專案內加入Modules\MessaeRepository.cs，做為注入的Interface。
```
namespace WebApplication1
{
    public interface MessaeRepository
    {
        // Methods
        string GetValue();
    }
}
```

3. 於專案內加入Modules\ConfigMessaeRepository.cs，做為注入的Implement。程式碼中的Service<MessaeRepository>()，將ConfigMessaeRepository註冊為MessaeRepository。
```
using MDP.Registration;

namespace WebApplication1
{
    [Service<MessaeRepository>()]
    public class ConfigMessaeRepository : MessaeRepository
    {
        // Fields
        private readonly string _message;


        // Constructors
        public ConfigMessaeRepository(string message)
        {
            // Default
            _message = message;
        }


        // Methods
        public string GetValue()
        {
            // Return
            return _message;
        }
    }
}
```

4. 改寫專案內的appsettings.json，加入ConfigMessaeRepository的參數設定。參數檔中的"ConfigMessaeRepository": { "Message": "Hello World" }，設定生成ConfigMessaeRepository的時候，將"Hello World"帶入建構子的"Message"參數。
```
{
  "WebApplication1": {
    "ConfigMessaeRepository": { "Message": "Hello World" }
  }
}
```

5. 改寫專案內的Controllers\HomeController、Views\Home\Index.cshtml，注入並使用MessaeRepository。
```
using System;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1
{
    public class HomeController : Controller
    {
        // Fields
        private readonly MessaeRepository _messaeRepository = null;


        // Constructors
        public HomeController(MessaeRepository messaeRepository)
        {
            // Default
            _messaeRepository = messaeRepository;
        }


        // Methods
        public ActionResult Index()
        {
            // ViewBag
            this.ViewBag.Message = _messaeRepository.GetValue();

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

    <!--Message-->
    <h3>@ViewBag.Message</h3>

</body>
</html>
```

6. 執行專案，於執行開啟的Browser視窗內，可以看到由MessaeRepository所提供的"Hello World"。
![01.執行結果01.png](https://clark159.github.io/MDP.Net/快速開始/開發一個依賴注入+參數管理的Web站台/01.執行結果01.png)
