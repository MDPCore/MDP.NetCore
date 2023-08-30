---
layout: default
title: 依賴注入
parent: null
nav_order: 4
has_children: false
---

# MDP.Hosting

(施工中)

MDP.Hosting是一個.NET開發套件，協助開發人員快速建立具有依賴注入的應用系統。提供標籤註冊、具名實例等多種功能服務，用以簡化開發流程並滿足多變的商業需求。

- 說明文件：[https://clark159.github.io/MDP.Net/](https://clark159.github.io/MDP.Net/)

- 程式源碼：[https://github.com/Clark159/MDP.Net/](https://github.com/Clark159/MDP.Net/)


# 模組架構

![MDP.Hosting-模組架構.png](https://clark159.github.io/MDP.Lab/依賴注入/MDP.Hosting-模組架構.png)

MDP.Hosting擴充.NET Core既有的依賴注入，加入標籤註冊/具名實例/具名注入三個功能服務，提供給開發人員依據業務場景進行組合使用。

- 標籤註冊：MDP.Hosting提供ServiceAttribute、FactoryAttribute兩種標籤註冊模式。ServiceAttribute比較簡便，設定類別(Class)要註冊為甚麼服務(Service)就好；FactoryAttribute比較靈活，可以用程式碼設定類別(Class)要註冊為甚麼服務(Service)。

```csharp
// ServiceAttribute

// FactoryAttribute
```

- 具名實例：MDP.Hosting裡完成註冊的類別(Class)，在執行階段會參考Config參數，進行實例(Instance)的生成。開發人員可以透過設定Config參數，生成多個具名實例；而每個具名實例，除了被標記為服務(Service)的Type類型之外，還會被標註實例(Instance)本身的Name名稱。

```json
宣告兩個具名物件
```

- 具名注入：被標註Type類型及Name名稱的實例(Instance)，在系統裡就可以被注入使用。預設.NET Core內建的依賴注入，會使用Type類型做為條件取得實例，來提供Typed注入；而MDP.Hosting的依賴注入，則是可以額外使用Name名稱做為條件取得實例，來提供Named注入。(註：.NET8將會支援Named注入)

```
//範例1：Typed注入

```

```
//範例2：Named注入

```


## 模組使用

### 套用專案範本使用

MDP.Configuration預設內建在MDP.Net專案範本內。依照下列操作步驟，即可使用MDP.Configuration所提供的參數管理功能。

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

### 做為獨立套件使用

另外，MDP.Configuration也可做為獨立套件，掛載至既有.NET專案。依照下列操作步驟，即可使用MDP.Configuration所提供的參數管理功能。

1.於專案內，使用CLI指令、NuGet套件管理員，加入MDP.Configuration套件參考。

```
// 新增NuGet套件參考
dotnet add package MDP.Configuration
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