---
layout: default
title: 使用Service注入(WebAPI專案)
parent: 快速開始(QuickStart)
nav_order: 4
---

# 使用Service注入(WebAPI專案)

在這個文件中將示範，在WebAPI專案中使用Service注入。

## 操作步驟

### 1. 建立新專案

依照「[建立WebAPI專案](../建立WebAPI專案/建立WebAPI專案.html)」示範的操作步驟，建立新的專案「WebApplication1」。

### 2. 新增MessageService

在專案裡新增Modules資料夾，並加入MessageService.cs：

```csharp
using MDP.Registration;

namespace WebApplication1
{
    [Service<MessageService>()]
    public class MessageService
    {
        // Fields
        private readonly string _message;


        // Constructors
        public MessageService(string message)
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

### 3. 修改HomeController

在專案裡修改HomeController.cs：

```csharp
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1
{
    public class HomeController : Controller
    {
        // Fields
        private readonly MessageService _messageService;


        // Constructors
        public HomeController(MessageService messageService)
        {
            // Default
            _messageService = messageService;
        }


        // Methods
        public ActionResult<object> Index()
        {
            // Message
            var message = _messageService.GetValue();

            // Return
            return new { message= message };
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
  },

  "WebApplication1": {
    "MessageService": {
      "Message": "Hello World"
    }
  }
}
```

完成以上操作步驟後，就已成功在WebAPI專案中使用Service注入。按F5執行專案，使用Postman呼叫API：/Home/Index，可以在結果視窗中看到{"message":"Hello World"}的訊息。

## 範例檔案

範例檔案：[https://github.com/Clark159/MDP.Net/tree/master/demo/01.QuickStart/使用Service注入(WebAPI專案)](https://github.com/Clark159/MDP.Net/tree/master/demo/01.QuickStart/使用Service注入(WebAPI專案))
