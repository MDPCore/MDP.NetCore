---
layout: default
title: 開發一個依賴注入+參數管理的Console程式
parent: 快速開始
nav_order: 3
has_children: false
---

# 開發一個依賴注入+參數管理的Console程式

從零開始，開發一個依賴注入+參數管理的Console程式，是難度不高但繁瑣的工作項目。本篇內容協助開發人員使用MDP.Net，逐步完成必要的設計和實作。

- 範例下載：[ConsoleApp1.zip](https://clark159.github.io/MDP.Net/快速開始/開發一個依賴注入+參數管理的Console程式/ConsoleApp1.zip)


## 開發步驟

1.開啟命令提示字元，輸入下列指令。用以安裝MDP.WebApp範本、並且建立一個名為ConsoleApp1的Console程式。

```
dotnet new install MDP.ConsoleApp
dotnet new MDP.ConsoleApp -n ConsoleApp1
```

2.使用Visual Studio開啟ConsoleApp1專案。並於專案內加入Modules\MessaeRepository.cs，做為注入的Interface。

```csharp
namespace ConsoleApp1
{
    public interface MessaeRepository
    {
        // Methods
        string GetValue();
    }
}
```

3.於專案內加入Modules\ConfigMessaeRepository.cs，做為注入的Implement。程式碼中的``` Service<MessaeRepository>() ```，將ConfigMessaeRepository註冊為MessaeRepository。

```csharp
using MDP.Registration;

namespace ConsoleApp1
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

4.改寫專案內的appsettings.json，加入ConfigMessaeRepository的參數設定。參數檔中的``` "ConfigMessaeRepository": { "Message": "Hello World" } ```，設定生成ConfigMessaeRepository的時候，將Hello World帶入建構子的Message參數。

```json
{
  "ConsoleApp1": {
    "ConfigMessaeRepository": { "Message": "Hello World" }
  }
}
```

5.改寫專案內的Program.cs，注入並使用MessaeRepository。

```csharp
using System;

namespace ConsoleApp1
{
    public class Program
    {
        // Methods
        public static void Run(MessaeRepository messaeRepository)
        {
            // Message
            var message = messaeRepository.GetValue();

            // Display
            Console.WriteLine(message);
        }

        public static void Main(string[] args)
        {
            // Host
            MDP.NetCore.Host.Run<Program>(args);
        }
    }
}
```

6.執行專案，於開啟的Console視窗內，可以看到由MessaeRepository所提供的Hello World。
![01.執行結果01.png](https://clark159.github.io/MDP.Net/快速開始/開發一個依賴注入+參數管理的Console程式/01.執行結果01.png)
