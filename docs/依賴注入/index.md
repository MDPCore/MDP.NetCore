---
layout: default
title: 依賴注入
parent: null
nav_order: 4
has_children: false
---

# MDP.Hosting

MDP.Hosting是一個.NET開發模組，協助開發人員快速建立具有依賴注入的應用系統。提供標籤註冊、具名實例、具名注入等功能服務，用以簡化開發流程並滿足多變的商業需求。

- 說明文件：[https://clark159.github.io/MDP.Net/](https://clark159.github.io/MDP.Net/)

- 程式源碼：[https://github.com/Clark159/MDP.Net/](https://github.com/Clark159/MDP.Net/)


## 模組架構

![MDP.Hosting-模組架構.png](https://clark159.github.io/MDP.Net/依賴注入/MDP.Hosting-模組架構.png)

### 標籤註冊

MDP.Hosting擴充.NET Core既有的參數管理，加入ServiceAttribute標籤，只要使用標籤宣告就可以註冊類別(Class)。

```
// 註冊類別
[Service<MessageRepository>(singleton:false)]
public class SqlMessageRepository : MessageRepository
{
  //...
}

- 註冊的類別(Class)：SqlMessageRepository
- 註冊為甚麼服務(Service)：MessageRepository
- 生成為唯一實例(Instance)：singleton=false(否:預設值，可省略)
```

ServiceAttribute標籤：用來宣告註冊的類別(Class)、這個類別註冊為甚麼服務(Service)、以及類別生成的實例(Instance)是否全域唯一。

- 命名空間：

```
MDP.Registration
```

- 類別定義：

```
[AttributeUsage(AttributeTargets.Class)]
public sealed class ServiceAttribute<TService> : ServiceAttribute where TService : class

- TService：類別註冊為甚麼服務(Service)。
```

- 建構函式：

```
public ServiceAttribute(bool singleton = false)

- singleton：類別生成的實例(Instance)是否全域唯一。singleton=false(否:預設值，可省略)
```

### 具名實例

MDP.Hosting裡完成註冊的類別(Class)，在執行階段會參考Config設定生成實例(Instance)。開發人員可以透過設定Config設定，生成多個實例；而每個實例除了被標記為服務(Service)的Type類型之外，還會被標記為實例(Instance)本身的Name名稱。
	
```
// 註冊類別
namespace MyLab.Module
{
    [Service<MessageRepository>()]
    public class SqlMessageRepository : MessageRepository
    {
        public SqlMessageRepository(string connectionString)
        {
            // ...
        }
    }
}

// Config設定
{
  "MyLab.Module": {
    "SqlMessageRepository": { "ConnectionString" : "Database Connection String"}
  }
}

- 命名空間：MyLab.Module
- 生成實例：SqlMessageRepository
- 生成參數：ConnectionString。(value="Database Connection String";
- 實例Type類型：MessageRepository
- 實例Name名稱：SqlMessageRepository
```

### 具名注入

被標註Type類型及Name名稱的實例(Instance)，在系統裡就可以被注入使用。預設.NET Core內建的依賴注入，會使用Type類型做為條件取得實例，來提供Typed注入；而MDP.Hosting的依賴注入，則是可以額外使用Name名稱做為條件取得實例，來提供Named注入。(註：.NET8將會支援Named注入)

Typed注入範例：ASP.NET Core生成HomeController的時候，預設取得Type類型被標註為MessageRepository的實例來注入。

```
// 註冊類別
namespace MyLab.Module
{
    [Service<MessageRepository>()]
    public class SqlMessageRepository : MessageRepository
    {
        //...
    }
}

// Config設定
{
  "MyLab.Module": {
    "SqlMessageRepository": {}
  }
}

// 服務注入
public class HomeController : Controller
{
	public HomeController(MessageRepository messageRepository)
	{
	    // ...
	}
}

- 命名空間：MyLab.Module
- 生成實例：SqlMessageRepository
- 實例Type類型：MessageRepository
- 實例Name名稱：SqlMessageRepository
```

Named注入範例：MDP.Hosting生成MessageContext的時候，參考Config設定(``` "messageRepository": "SqlMessageRepository" ```)，取得Name名稱被標註為SqlMessageRepository的實例來注入。

```
// 註冊類別
namespace MyLab.Module
{
	[Service<MessageContext>(singleton: true)]
	public class MessageContext
	{
		public MessageContext(MessageRepository messageRepository)
		{
			// ...
		}
	}

	[Service<MessageRepository>()]
	public class SqlMessageRepository : MessageRepository
	{
 	   //...
	}
}

// Config設定
{
  "MyLab.Module": {
    "MessageContext": {
	  "messageRepository": "SqlMessageRepository"
	},
    "SqlMessageRepository": {}
  }
}

- 命名空間：MyLab.Module
- 生成實例：SqlMessageRepository
- 實例Type類型：MessageRepository
- 實例Name名稱：SqlMessageRepository
```


## 模組使用

### 加入模組

MDP.Hosting預設內建在MDP.Net專案範本內，依照下列操作步驟，即可建立包含MDP.Hosting模組的專案。

- 在命令提示字元輸入下列指令，使用MDP.Net專案範本建立專案。
 
```
// 建立API服務、Web站台
dotnet new install MDP.WebApp
dotnet new MDP.WebApp -n WebApplication1

// 建立Console程式
dotnet new install MDP.ConsoleApp
dotnet new MDP.ConsoleApp -n ConsoleApp1
```

### 使用標籤

建立包含MDP.Hosting模組的專案之後，使用ServiceAttribute標籤，就可以透過標籤宣告來註冊類別(Class)。

```
// 註冊類別
namespace MyLab.Module
{
    [Service<MessageRepository>()]
    public class SqlMessageRepository : MessageRepository
    {
        public SqlMessageRepository(string connectionString)
        {
            // ...
        }
    }
}
```

### 設定參數

MDP.Hosting裡完成註冊的類別(Class)，開發人員可以透過設定Config設定生成實例。

```
// Config設定
{
  "MyLab.Module": {
    "SqlMessageRepository": { "ConnectionString" : "Database Connection String"}
  }
}

- 命名空間：MyLab.Module
- 生成實例：SqlMessageRepository
- 生成參數：ConnectionString(value="Database Connection String")
- 實例Type類型：MessageRepository
- 實例Name名稱：SqlMessageRepository
```

## 模組範例

專案開發過程，在開發/測試/正式三個執行環境，常常需要使用不同的連線字串、甚至使用不同資料來源。例如：開發環境使用Mock資料來源(假資料)、測試環境使用SQL資料來源(連線至測試資料庫)、正式環境使用SQL資料來源(連線至正式資料庫)。本篇範例協助開發人員使用MDP.Hosting，逐步完成必要的設計和實作。


- 範例下載：[WebApplication1.zip](https://clark159.github.io/MDP.Net/依賴注入/WebApplication1.zip)

### 操作步驟

1.開啟命令提示字元，輸入下列指令。用以安裝MDP.WebApp範本、並且建立一個名為WebApplication1的Web站台。

```
dotnet new install MDP.WebApp
dotnet new MDP.WebApp -n WebApplication1
```

2.使用Visual Studio開啟WebApplication1專案。於專案內加入Modules\MessageContext.cs、Modules\MessageRepository.cs，並使用標籤宣告來註冊MessageContext。

```
using MDP.Registration;

namespace WebApplication1
{
    [Service<MessageContext>(singleton: true)]
    public class MessageContext
    {
        // Fields
        private readonly MessageRepository _messageRepository = null;


        // Constructors
        public MessageContext(MessageRepository messageRepository)
        {
            // Default
            _messageRepository = messageRepository;
        }


        // Methods
        public string GetValue()
        {
            // Return
            return _messageRepository.GetValue();
        }
    }
}
```

```
namespace WebApplication1
{
    public interface MessageRepository
    {
        // Methods
        string GetValue();
    }
}
```

3.於專案內加入Modules\SqlMessageRepository.cs、Modules\MockMessageRepository.cs，並使用標籤宣告來註冊類別為MessageRepository。

```
using MDP.Registration;

namespace WebApplication1
{
    [Service<MessageRepository>()]
    public class SqlMessageRepository : MessageRepository
    {
        // Fields
        private readonly string _connectionString;


        // Constructors
        public SqlMessageRepository(string connectionString)
        {
            // Default
            _connectionString = connectionString;
        }


        // Methods
        public string GetValue()
        {
            // Return
            return "Hello World By " + _connectionString;
        }
    }
}
```

```
using MDP.Registration;

namespace WebApplication1
{
    [Service<MessageRepository>()]
    public class MockMessageRepository : MessageRepository
    {
        // Methods
        public string GetValue()
        {
            // Return
            return "Hello World By Mock Source";
        }
    }
}
```

4.於專案內加入下列三個Config設定檔，作為開發/測試/正式三個執行環境，各自讀取的Config設定檔。

- 開發環境：\config\Development\appsettings.json，設定為MessageContext具名注入MockMessageRepository。

```
{
  "WebApplication1": {
    "MessageContext": {
      "MessageRepository": "MockMessageRepository"
    },
    "MockMessageRepository": {}
  }
}
```

- 測試環境：\config\Staging\appsettings.json，設定為MessageContext具名注入SqlMessageRepository，並且設定連線至Staging Database。

```
{
  "WebApplication1": {
    "MessageContext": {
      "MessageRepository": "SqlMessageRepository"
    },
    "SqlMessageRepository": { "connectionString": "Staging Database" }
  }
}
```

- 正式環境：\config\Production\appsettings.json，設定為MessageContext具名注入SqlMessageRepository，並且設定連線至Production Database。

```
{
  "WebApplication1": {
    "MessageContext": {
      "MessageRepository": "SqlMessageRepository"
    },
    "SqlMessageRepository": { "connectionString": "Production Database" }
  }
}
```

5.執行專案，於開啟的Browser視窗內，可以看到系統依照``` 開發環境：\config\Development\appsettings.json ```的設定執行，於畫面顯示MockMessageRepository回傳的Hello World By Mock Source。

![01.執行結果01.png](https://clark159.github.io/MDP.Net/依賴注入/01.執行結果01.png)

6.改寫專案內的啟動檔 \Properties\launchSettings.json，將ASPNETCORE_ENVIRONMENT的內容改為Staging。

```json
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

7.重建並執行專案，於開啟的Browser視窗內，可以看到系統依照``` 測試環境：\config\Staging\appsettings.json ```的設定執行，於畫面顯示SqlMessageRepository回傳的Hello World By Staging Database。

![01.執行結果02.png](https://clark159.github.io/MDP.Net/依賴注入/01.執行結果02.png)