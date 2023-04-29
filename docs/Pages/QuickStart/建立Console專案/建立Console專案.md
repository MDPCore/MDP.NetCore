---
layout: default
title: 建立Console專案
parent: 快速開始(QuickStart)
nav_order: 3
---

# 建立Console專案

本篇文件介紹，如何建立使用MDP.Net的Console專案。

## 操作步驟

### 1. 建立新專案

開啟Visual Studio，選擇使用範本「主控台應用程式」，建立新的專案「ConsoleApp1」。

### 2. 新增NuGet套件

在專案裡使用NuGet套件管理員，新增下列NuGet套件：

```
MDP.NetCore
```

### 3. 修改Program.cs

在專案裡修改Program.cs：

```csharp
using Microsoft.Extensions.Hosting;
using System;

namespace ConsoleApp1
{
    public class Program
    {
        // Methods
        public static void Run()
        {
            // Message
            var message = "Hello World";

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

在專案裡加入appsettings.json，並設置為永遠複製到輸出目錄：

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

### 5. 執行專案

完成以上操作步驟後，就已成功建立使用MDP.Net的Console專案。按F5執行專案，可以在執行視窗中看到"Hello World"的訊息。

## 範例檔案

[https://github.com/Clark159/MDP.Net/tree/master/demo/01.QuickStart/建立Console專案](https://github.com/Clark159/MDP.Net/tree/master/demo/01.QuickStart/建立Console專案)
