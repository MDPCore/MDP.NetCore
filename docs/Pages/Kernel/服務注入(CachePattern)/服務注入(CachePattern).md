---
layout: default
title: 服務注入(CachePattern)
parent: 核心模組(Kernel)
nav_order: 7
---

# 服務注入(CachePattern)

在MDP.Net核心模組中，「服務注入模組」注入使用ServiceAttribute註冊的Instance時，可以在建構子注入另外一個Instance，進行逐層的服務注入；搭配微軟提供的IMemoryCache類別，就可以提供資料快取功能。依照下列的類別宣告，就可以在系統裡註冊Type(CacheUserRepository)為Service(UserRepository)的Instance，並為注入Service(UserRepository)提供資料快取功能。

```csharp
using MDP.Registration;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace WebApplication1
{
    [Service<UserRepository>()]
    public class CacheUserRepository : UserRepository
    {
        // Fields
        private readonly IMemoryCache _userCache = new MemoryCache(new MemoryCacheOptions());

        private readonly UserRepository _userRepository;


        // Constructors
        public CacheUserRepository(UserRepository userRepository)
        {
            // Default
            _userRepository = userRepository;
        }


        // Methods
        public User? Find()
        {
            // UserCache-Hit
            var user = _userCache.Get<User>("Test");
            if (user != null) return user;

            // UserRepository-Find
            user = _userRepository.Find();
            if (user == null) return user;

            // UserCache-Set
            _userCache.Set<User>("Test", user, new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10)
            });

            // Return
            return user;
        }
    }
}
```

本篇文件介紹，如何使用MDP.Net核心模組中「服務注入模組」，注入使用ServiceAttribute註冊的Instance時，在建構子注入另外一個Instance，進行逐層的服務注入；並搭配微軟提供的IMemoryCache類別，用以提供資料快取功能

## 操作步驟

### 1. 建立新專案

依照「[建立MvcPage專案](../../QuickStart/建立MvcPage專案/建立MvcPage專案.html)」的操作步驟，建立新的MvcPage專案「WebApplication1」。

### 2. 新增UserContext、UserRepository、User

在專案裡新增Modules資料夾，加入UserContext.cs、UserRepository.cs、User.cs。其中：UserContext做為注入根節點，且設定singleton參數為true，讓注入的Instance為全局同一個。

```csharp
using MDP.Registration;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace WebApplication1
{
    [Service<UserContext>(singleton: true)]
    public class UserContext
    {
        // Fields
        private readonly UserRepository _userRepository;


        // Constructors
        public UserContext(UserRepository userRepository)
        {
            #region Contracts

            if (userRepository == null) throw new ArgumentException($"{nameof(userRepository)}=null");

            #endregion

            // Default
            _userRepository = userRepository;
        }


        // Methods
        public User? Find()
        {
            // User
            var user = _userRepository.Find();

            // Return
            return user;
        }
    }
    
    public interface UserRepository
    {
        // Methods
        User? Find();
    }
    
    public class User
    {
        // Properties
        public string Name { get; set; } = string.Empty;
    }
}
```

### 3. 新增CacheUserRepository

在專案裡的Modules資料夾，加入CacheUserRepository.cs。除使用ServiceAttribute註冊CacheUserRepository為UserRepository的Instance，也由建構子接受UserRepository userRepository參數，接收逐層注入的UserRepository，並且為其提供資料快取功能。

```csharp
using MDP.Registration;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace WebApplication1
{
    [Service<UserRepository>()]
    public class CacheUserRepository : UserRepository
    {
        // Fields
        private readonly IMemoryCache _userCache = new MemoryCache(new MemoryCacheOptions());

        private readonly UserRepository _userRepository;


        // Constructors
        public CacheUserRepository(UserRepository userRepository)
        {
            // Default
            _userRepository = userRepository;
        }


        // Methods
        public User? Find()
        {
            // Cache-Hit
            var user = _userCache.Get<User>("Test");
            if (user != null) return user;

            // Repository-Find
            user = _userRepository.Find();
            if (user == null) return user;

            // Cache-Set
            _userCache.Set<User>("Test", user, new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10)
            });

            // Return
            return user;
        }
    }
}
```

### 4. 新增SqlUserRepository

在專案裡的Modules資料夾，加入SqlUserRepository.cs。同樣使用ServiceAttribute註冊SqlUserRepository為UserRepository的Instance，但是建構子只接受模擬的string connectionString參數，做為逐層注入UserRepository的最後一個注入。

```csharp
using MDP.Registration;
using System;

namespace WebApplication1
{
    [Service<UserRepository>()]
    public class SqlUserRepository : UserRepository
    {
        // Fields
        private readonly string _connectionString;


        // Constructors
        public SqlUserRepository(string connectionString)
        {
            // Default
            _connectionString = connectionString;
        }


        // Methods
        public User? Find()
        {
            // Return
            return new User()
            {
                Name= "Clark-->" + DateTime.Now.ToString()
            };
        }
    }
}
```

### 5. 修改appsettings.json

在專案裡修改appsettings.json，加入逐層注入UserRepository的註冊參數設定。其中UserContext注入CacheUserRepository、CacheUserRepository注入SqlUserRepository。

```json
{
  "WebApplication1": {

    // UserContext
    "UserContext": {
      "userRepository": "CacheUserRepository"
    },

    // UserRepository
    "CacheUserRepository": {
      "userRepository": "SqlUserRepository"
    },
    "SqlUserRepository": {
      "connectionString": "Database=LabDB;Integrated Security=True;"
    }
  }
}
```

### 6. 修改HomeController

在專案裡修改HomeController.cs，注入UserContext的TypedInstance。

```csharp
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1
{
    public class HomeController : Controller
    {
        // Fields
        private readonly UserContext _userContext;


        // Constructors
        public HomeController(UserContext userContext)
        {
            // Default
            _userContext = userContext;
        }


        // Methods
        public ActionResult Index()
        {
            // Message
            this.ViewBag.Message = _userContext.Find()?.Name;

            // Return
            return View();
        }
    }
}
```

### 6. 執行專案

完成以上操作步驟後，就已成功在MvcPage專案中使用服務注入(CachePattern)。按F5執行專案，使用Browser開啟Page：/Home/Index，可以在網頁內容看到下列，由UserContext所提供的訊息內容。

```
Clark-->2023/05/11 21:20:00
```

此時UserContext的Configuration參數設定，userRepository設定為CacheUserRepository，所以UserContext注入的UserRepository是CacheUserRepository。按F5重新整理網頁，會發現因為有Cache的關係，網頁內容上的時間訊息，每10秒才會跳動一次。

```json
{
  "WebApplication1": {

    // UserContext
    "UserContext": {
      "userRepository": "CacheUserRepository"
    }
  }
}
```

```
Clark-->2023/05/11 21:20:00
10s
Clark-->2023/05/11 21:30:00
10s
Clark-->2023/05/11 21:40:00
```

接著修改UserContext的Configuration參數設定，userRepository設定為SqlUserRepository，所以UserContext注入的UserRepository是SqlUserRepository。按F5重新整理網頁，會發現因為沒有Cache的關係，網頁內容上的時間訊息，每秒都會跳動一次。

```json
{
  "WebApplication1": {

    // UserContext
    "UserContext": {
      "userRepository": "SqlUserRepository"
    }
  }
}
```

```
Clark-->2023/05/11 21:20:00
1s
Clark-->2023/05/11 21:30:01
1s
Clark-->2023/05/11 21:40:02
```

## 範例檔案

[https://github.com/Clark159/MDP.Net/tree/master/demo/02.Kernel/服務注入(CachePattern)](https://github.com/Clark159/MDP.Net/tree/master/demo/02.Kernel/服務注入(CachePattern))