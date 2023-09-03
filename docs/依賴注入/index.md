---
layout: default
title: 依賴注入
parent: null
nav_order: 4
has_children: false
---

# MDP.Hosting

(施工中)

MDP.Hosting是一個.NET開發模組，協助開發人員快速建立具有依賴注入的應用系統。提供標籤註冊、具名實例、具名注入等功能服務，用以簡化開發流程並滿足多變的商業需求。

- 說明文件：[https://clark159.github.io/MDP.Net/](https://clark159.github.io/MDP.Net/)

- 程式源碼：[https://github.com/Clark159/MDP.Net/](https://github.com/Clark159/MDP.Net/)


## 模組架構

![MDP.Hosting-模組架構.png](https://clark159.github.io/MDP.Net/依賴注入/MDP.Hosting-模組架構.png)

### 標籤註冊

MDP.Hosting擴充.NET Core既有的參數管理，加入ServiceAttribute標籤，開發人員只要使用標籤宣告就可以註冊類別(Class)。

```
[Service<MessageRepository>(singleton:false)]
public class SqlMessageRepository : MessageRepository
{
  //...
}

註冊的類別(Class)：SqlMessageRepository
註冊為甚麼服務(Service)：MessageRepository
生成為唯一實例(Instance)：singleton=false(否:預設值，可省略)
```

ServiceAttribute標籤，用來宣告註冊的類別(Class)、這個類別註冊為甚麼服務(Service)、以及類別生成的實例(Instance)是否全域唯一。

```
命名空間：MDP.Registration

類別定義：
[AttributeUsage(AttributeTargets.Class)]
public sealed class ServiceAttribute<TService> : ServiceAttribute where TService : class
```

- TService：類別註冊為甚麼服務(Service)。

- singleton：類別生成的實例(Instance)是否全域唯一。

### 具名實例

MDP.Hosting裡完成註冊的類別(Class)，在執行階段會參考Config參數生成實例(Instance)。開發人員可以透過設定Config參數，生成多個實例；而每個實例除了被標記為服務(Service)的Type類型之外，還會被標註實例(Instance)本身的Name名稱。
	
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

// Config參數
{
  "MyLab.Module": {
    "SqlMessageRepository": {}
  }
}

生成的實例：SqlMessageRepository
實例的Type類型：MessageRepository
實例的Name名稱：SqlMessageRepository
```

### 具名注入

被標註Type類型及Name名稱的實例(Instance)，在系統裡就可以被注入使用。預設.NET Core內建的依賴注入，會使用Type類型做為條件取得實例，來提供Typed注入；而MDP.Hosting的依賴注入，則是可以額外使用Name名稱做為條件取得實例，來提供Named注入。(註：.NET8將會支援Named注入)

- Typed注入範例：ASP.NET Core生成HomeController的時候，取得Type類型被標註為MessageRepository的實例來注入。

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

// Config參數
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

注入的實例：SqlMessageRepository
注入的Type類型：MessageRepository
```

- Named注入範例：MDP.Hosting生成MessageContext的時候參考Config，取得Name名稱被標註為SqlMessageRepository的實例來注入。

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

// Config參數
{
  "MyLab.Module": {
    "MessageContext": {
	  "messageRepository": "SqlMessageRepository"
	},
    "SqlMessageRepository": {}
  }
}

注入的實例：SqlMessageRepository
注入的Name名稱：SqlMessageRepository
```


## 模組使用

### 加入套件

MDP.Hosting預設內建在MDP.Net專案範本內。依照下列操作步驟，即可使用MDP.Hosting所提供的依賴注入功能。

- 在命令提示字元輸入下列指令，使用MDP.Net專案範本建立專案。
 
```
// 建立API服務、Web站台
dotnet new install MDP.WebApp
dotnet new MDP.WebApp -n WebApplication1

// 建立Console程式
dotnet new install MDP.ConsoleApp
dotnet new MDP.ConsoleApp -n ConsoleApp1
```

### 宣告標籤

完成加入套件後，


### 配置參數

完成加入套件後，XXXXXXXXX

```
// JSON格式的Config設定檔
{
  "property1": {
    "property2": "value"
    "property3": [value, value]
  }
}
```

## 模組範例

XXXXXXXXXXXXXXXXXXXX

- 範例下載：[WebApplication1.zip](https://clark159.github.io/MDP.Net/依賴注入/WebApplication1.zip)

### 操作步驟

1.開啟命令提示字元，輸入下列指令。用以安裝MDP.WebApp範本、並且建立一個名為WebApplication1的Web站台。

```
dotnet new install MDP.WebApp
dotnet new MDP.WebApp -n WebApplication1
```


