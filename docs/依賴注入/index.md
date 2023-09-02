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

MDP.Hosting擴充.NET Core既有的參數管理，加入ServiceAttribute標籤，開發人員只要使用標籤參數就可以註冊類別(Clss)。

- TService：ServiceAttribute的泛型參數，用來定義類別生成的實例(Instance)屬於甚麼服務(Service)。

- singleton：ServiceAttribute的建構子參數，用來定義類別生成的實例(Instance)是否為全域唯一。

```
[Service<MessageRepository>(singleton:false)]
public class MockMessageRepository : MessageRepository
{
  //...
}

註冊的類別：MockMessageRepository
實例屬於甚麼服務：MessageRepository
實例是否全域唯一：singleton=false(否:預設值)
```

### 具名實例

MDP.Hosting裡完成註冊的類別(Class)，在執行階段會參考Config參數，進行實例(Instance)的生成。開發人員可以透過設定Config參數，生成多個具名實例；而每個具名實例，除了被標記為服務(Service)的Type類型之外，還會被標註實例(Instance)本身的Name名稱。
	
```
[Service<MessageRepository>()]
public class MockMessageRepository : MessageRepository
{
  //...
}
```

```
{
  "MyLab.Module": {
    "MockMessageRepository": {}
  }
}
```

```
生成的實例：MockMessageRepository
實例的Type類型：MessageRepository
實例的Name名稱：MockMessageRepository
```

### 具名注入

被標註Type類型及Name名稱的實例(Instance)，在系統裡就可以被注入使用。預設.NET Core內建的依賴注入，會使用Type類型做為條件取得實例，來提供Typed注入；而MDP.Hosting的依賴注入，則是可以額外使用Name名稱做為條件取得實例，來提供Named注入。(註：.NET8將會支援Named注入)

```
public class HomeController : Controller
{
	public HomeController(MessageContext messageContext)
	{
	    // ...
	}
}

[Service<MessageContext>(singleton: true)]
public class MessageContext
{
	public MessageContext(MessageRepository messageRepository)
	{
		// ...
	}
}

[Service<MessageRepository>()]
public class MockMessageRepository : MessageRepository
{
    //...
}
```

```
{
  "MyLab.Module": {
    "MessageContext": {
	  "messageRepository": "MockMessageRepository"
	},
    "MockMessageRepository": {}
  }
}
```

```
注入範例1：public HomeController(MessageContext messageContext)
注入類型：Typed注入
注入邏輯：ASP.NET Core生成HomeController的時候，取得Type類型被標註為MessageContext的實例來注入。
```

```
注入範例2：public MessageContext(MessageRepository messageRepository)
注入類型：Named注入
注入邏輯：MDP.Hosting生成MessageContext的時候，參考Config，取得Name名稱被標註為MockMessageRepository的實例來注入。
```


## 模組使用

### 套用專案範本

MDP.Hosting預設內建在MDP.Net專案範本內。依照下列操作步驟，即可使用MDP.Hosting所提供的依賴注入功能。

1.在命令提示字元輸入下列指令，使用MDP.Net專案範本建立專案。
 
```
// 建立API服務、Web站台
dotnet new install MDP.WebApp
dotnet new MDP.WebApp -n WebApplication1

// 建立Console程式
dotnet new install MDP.ConsoleApp
dotnet new MDP.ConsoleApp -n ConsoleApp1
```

2.XXXXXXXXXXX

### 做為獨立套件

另外，MDP.Hosting也可做為獨立套件，掛載至既有.NET專案。依照下列操作步驟，即可使用MDP.Hosting所提供的依賴注入功能。

1.於專案內，使用CLI指令、NuGet套件管理員，加入MDP.Hosting套件參考。

```
// 新增NuGet套件參考
dotnet add package MDP.Hosting
```

2.XXXXXXXXXXX


## 模組範例

XXXXXXXXXXXXXXXXXXXX

- 範例下載：[WebApplication1.zip](https://clark159.github.io/MDP.Net/依賴注入/WebApplication1.zip)

### 操作步驟

1.開啟命令提示字元，輸入下列指令。用以安裝MDP.WebApp範本、並且建立一個名為WebApplication1的Web站台。

```
dotnet new install MDP.WebApp
dotnet new MDP.WebApp -n WebApplication1
```


