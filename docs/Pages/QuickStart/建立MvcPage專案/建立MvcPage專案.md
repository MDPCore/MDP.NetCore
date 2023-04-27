---
layout: default
title: 建立MvcPage專案
parent: 快速開始(QuickStart)
nav_order: 1
---

# 建立MvcPage專案

在這個快速開始文件中，將示範如何使用MDP.Net建立MvcPage專案。

## 操作步驟

### 1. 建立新專案

開啟Visual Studio，建立一個新的專案「WebApplication1」，並選擇使用範本「空的ASP.NET Core」。

### 2. 新增NuGet套件

在專案裡使用NuGet套件管理員，新增下列NuGet套件：

```
MDP.AspNetCore
```

### 3. 修改Program.cs

在專案裡改寫Program.cs為下列程式碼：

```csharp
using Microsoft.Extensions.Hosting;

namespace WebApplication1
{
    public class Program
    {
        // Methods
        public static void Main(string[] args)
        {
            // Host
            MDP.AspNetCore.Host.Create(args).Run();
        }
    }
}
```

### 4. 修改appsettings.json

在專案裡改寫appsettings.json為下列內容，並移除appsettings.Development.json：

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

### 5. 新增HomeController

在專案裡新增Controllers資料夾，並加入HomeController.cs：

```csharp
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1
{
    public class HomeController : Controller
    {
        // Methods
        public ActionResult Index()
        {
            return View();
        }
    }
}
```

### 6. 新增Index.cshtml

在專案裡新增Views\Home資料夾，並加入Index.cshtml：

```html
<!DOCTYPE html>

<html>
<head>
    <meta charset="utf-8" />
    <title></title>
</head>
<body>

    <!--Title-->
    <h2>Hello World</h2>
    <hr />

</body>
</html>
```

完成以上操作後，建立了一個使用MDP.Net的MvcPage專案。按F5執行專案，可以在瀏覽器中看到"Hello World"的訊息。

## 範例檔案

[https://github.com/Clark159/MDP.Net/tree/master/demo/01.QuickStart/建立MvcPage專案](https://github.com/Clark159/MDP.Net/tree/master/demo/01.QuickStart/建立MvcPage專案)
