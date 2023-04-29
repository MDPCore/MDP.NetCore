---
layout: default
title: 建立WebAPI專案
parent: 快速開始(QuickStart)
nav_order: 2
---

# 建立WebAPI專案

本篇文件介紹，如何建立使用MDP.Net的WebAPI專案。

## 操作步驟

### 1. 建立新專案

開啟Visual Studio，選擇使用範本「空的ASP.NET Core」，建立新的專案「WebApplication1」。並且修改WebApplication1.csproj，開啟C# 11.0語言版本支援。

```
<LangVersion>11.0</LangVersion>
```

### 2. 新增NuGet套件

在專案裡使用NuGet套件管理員，新增下列NuGet套件：

```
MDP.AspNetCore
```

### 3. 修改Program.cs

在專案裡修改Program.cs為下列程式碼：

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

在專案裡修改appsettings.json，並移除appsettings.Development.json：

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
        public ActionResult<object> Index()
        {
            // Message
            var message = "Hello World";

            // Return
            return new { message = message };
        }
    }
}
```

### 6. 執行專案

完成以上操作步驟後，就已成功建立使用MDP.Net的WebAPI專案。按F5執行專案，使用Postman呼叫API：/Home/Index，可以在結果視窗中看到{"message":"Hello World"}的訊息。

## 範例檔案

[https://github.com/Clark159/MDP.Net/tree/master/demo/01.QuickStart/建立WebAPI專案](https://github.com/Clark159/MDP.Net/tree/master/demo/01.QuickStart/建立WebAPI專案)
