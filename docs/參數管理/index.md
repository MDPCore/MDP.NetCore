---
layout: default
title: 參數管理
parent: null
nav_order: 3
has_children: false
---


# MDP.Configuration

MDP.Configuration是開源的.NET開發套件，協助開發人員快速建立具有參數管理的應用系統。提供參數掛載等功能服務，用以簡化開發流程並滿足多變的商業需求。

- 說明文件：[https://mdpnetcore.github.io/MDP.NetCore/](https://mdpnetcore.github.io/MDP.NetCore/)

- 程式源碼：[https://github.com/MDPNetCore/MDP.NetCore/](https://github.com/MDPNetCore/MDP.NetCore/)


## 模組功能

![MDP.Configuration-模組功能.png](https://mdpnetcore.github.io/MDP.NetCore/參數管理/MDP.Configuration-模組功能.png)

### 參數掛載

MDP.Configuration擴充.NET Core既有的參數管理，加入在開發/測試/正式三個執行環境，依據執行環境名稱(EnvironmentName)讀取不同Config設定檔的功能服務。

- 執行環境設定：

```
// 執行環境名稱(EnvironmentName)
開發環境：Development
測試環境：Staging
正式環境：Production

// 環境變數名稱
Web專案：ASPNETCORE_ENVIRONMENT
Console專案：NETCORE_ENVIRONMENT

// 環境變數範例
ASPNETCORE_ENVIRONMENT=Staging
NETCORE_ENVIRONMENT=Production
```

- 依執行環境名稱，讀取執行資料夾(``` <EntryDir> ```)內的Config設定檔：

```
<EntryDir>\appsettings.json
EntryDir>\*.{EnvironmentName}.json
```

- 依執行環境名稱，讀取參數資料夾(``` <EntryDir>\config ```)內的Config設定檔：

```
<EntryDir>\config\appsettings.json
<EntryDir>\config\*.{EnvironmentName}.json
```

- 依執行環境名稱，讀取環境資料夾(``` <EntryDir>\config\{EnvironmentName} ```)內的Config設定檔：

```
<EntryDir>\config\{EnvironmentName}\*.json
```

### 參數讀取

MDP.Configuration擴充.NET Core既有的參數管理，使用.NET Core內建的IConfiguration介面提供參數讀取功能。

```
// 命名空間：
Microsoft.Extensions.Configuration

// 類別定義：
public interface IConfiguration

// 類別方法：
public IConfigurationSection GetSection(string key);
- key：讀取的參數路徑。
- IConfigurationSection：回傳值，讀取的參數內容。

public static T? Get<T> (this IConfiguration configuration);
- T：讀取的參數類型。
- T?，回傳值，讀取的參數內容。
```


## 模組使用

### 加入專案

MDP.Configuration預設內建在MDP.Net專案範本內，依照下列操作步驟，即可建立加入MDP.Configuration模組的專案。

- 在命令提示字元輸入下列指令，使用MDP.Net專案範本建立專案。
 
```
// 建立API服務、Web站台
dotnet new install MDP.WebApp
dotnet new MDP.WebApp -n WebApplication1

// 建立Console程式
dotnet new install MDP.ConsoleApp
dotnet new MDP.ConsoleApp -n ConsoleApp1
```

### 設定參數

建立包含MDP.Configuration模組的專案之後，在專案裡將Config設定檔放到指定的資料夾，系統就會依據執行環境名稱(EnvironmentName)讀取不同Config設定檔。

```
// Config設定
{
  "property1": {
    "property2": "value"
    "property3": [value, value]
  }
}
```

### 取得IConfiguration

完成參數設定之後的專案，就可以注入IConfiguration介面來使用參數管理。

```
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace WebApplication1
{
    public class HomeController : Controller
    {
        // Constructors
        public HomeController(IConfiguration configuration)
        {            
             // Setting
             var setting = configuration.GetSection("Setting").Get<string>();
        }
    }
}
```


## 模組範例

在開發/測試/正式三個執行環境，讀取不同Config設定檔，用以提供連線字串、功能參數...等等的參數管理，是開發系統時常見的功能需求。本篇範例協助開發人員使用MDP.Configuration，逐步完成必要的設計和實作。

- 範例下載：[WebApplication1.zip](https://mdpnetcore.github.io/MDP.NetCore/參數管理/WebApplication1.zip)

### 操作步驟

1.開啟命令提示字元，輸入下列指令。用以安裝MDP.WebApp範本、並且建立一個名為WebApplication1的Web站台。

```
dotnet new install MDP.WebApp
dotnet new MDP.WebApp -n WebApplication1
```

2.使用Visual Studio開啟WebApplication1專案。於專案內加入下列三個Config設定檔，作為開發/測試/正式三個執行環境，各自讀取的Config設定檔。

- 開發環境：\config\Development\appsettings.json

```
{
  "WebApplication1": {
    "Message": "Hello World By Development"
  }
}
```

- 測試環境：\config\Staging\appsettings.json

```
{
  "WebApplication1": {
    "Message": "Hello World By Staging"
  }
}
```

- 正式環境：\config\Production\appsettings.json

```
{
  "WebApplication1": {
    "Message": "Hello World By Production"
  }
}
```

5.改寫專案內的Controllers\HomeController.cs、Views\Home\Index.cshtml，注入並使用.Net Core內建的IConfiguration。

```
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace WebApplication1
{
    public class HomeController : Controller
    {
        // Fields
        private readonly IConfiguration _configuration = null;


        // Constructors
        public HomeController(IConfiguration configuration)
        {
            // Default
            _configuration = configuration;
        }


        // Methods
        public ActionResult Index()
        {
            // ViewBag
            this.ViewBag.Message = _configuration.GetSection("WebApplication1:Message").Get<string>();

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

6.執行專案，於開啟的Browser視窗內，可以看到由``` 開發環境：\config\Development\appsettings.json ```所提供的 Hello World By Development。

![01.執行結果01.png](https://mdpnetcore.github.io/MDP.NetCore/參數管理/01.執行結果01.png)

7.改寫專案內的啟動檔 \Properties\launchSettings.json，將ASPNETCORE_ENVIRONMENT的內容改為Staging。

```
{
  "profiles": {
    "WebApplication1": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "applicationUrl": "https://localhost:7146;http://localhost:5257",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Staging"
      }
    }
  }
}
```

6.重建並執行專案，於開啟的Browser視窗內，可以看到由``` 測試環境：\config\Staging\appsettings.json ```所提供的 Hello World By Staging。

![01.執行結果02.png](https://mdpnetcore.github.io/MDP.NetCore/參數管理/01.執行結果02.png)
