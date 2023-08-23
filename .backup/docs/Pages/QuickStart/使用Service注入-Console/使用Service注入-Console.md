---
layout: default
title: 使用Service注入-Console
parent: 快速開始(QuickStart)
nav_order: 6
---

# 使用Service注入-Console

本篇文件介紹，如何在Console專案中使用Service注入。

## 操作步驟

### 1. 建立新專案

依照「[建立Console專案](../../QuickStart/建立Console專案/建立Console專案.html)」的操作步驟，建立新的Console專案「ConsoleApp1」。

### 2. 新增DemoService

在專案裡新增Modules資料夾，並加入DemoService.cs。

```csharp
using MDP.Registration;

namespace ConsoleApp1
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

### 3. 修改Program.cs

在專案裡修改Program.cs。

```csharp
using Microsoft.Extensions.Hosting;
using System;

namespace ConsoleApp1
{
    public class Program
    {
        // Methods
        public static void Run(DemoService demoService)
        {
            // Message
            var message = demoService.GetMessage();

            // Display
            Console.WriteLine(message);
        }

        public static void Main(string[] args)
        {
            // Host
            MDP.NetCore.Host.Create<Program>(args).Run();
        }
    }
}
```

### 4. 修改appsettings.json

在專案裡修改appsettings.json。

```json
{
  "ConsoleApp1": {
    "DemoService": {
      "Message": "Hello World"
    }
  }
}
```

### 5. 執行專案

完成以上操作步驟後，就已成功在Console專案中使用Service注入。按F5執行專案，可以在執行視窗看到"Hello World"的訊息。

## 範例檔案

[https://github.com/Clark159/MDP.Net/tree/master/demo/QuickStart/使用Service注入-Console](https://github.com/Clark159/MDP.Net/tree/master/demo/QuickStart/使用Service注入-Console)
