# MDP.Net 

MDP.Net是.NET版本的開發平台，協助開發人員快速建立Web App、Console App。


## 說明文件

- [clark159.github.io/MDP.Net](https://clark159.github.io/MDP.Net/)


## 範例展示

### SleepZone.Todos

- 範例檔案：

  - [https://github.com/Clark159/MDP.Net/tree/master/demo/SleepZone.Todos](https://github.com/Clark159/MDP.Net/tree/master/demo/SleepZone.Todos)

- 操作步驟：

  - 使用Visual Studio 2022開啟方案：SleepZone.Todos.sln
  
  - 設定起始專案為：01.Application/SleepZone.Todos.WebApp
  
  - 按下F5執行專案後，於瀏覽器的網址列：輸入「[https://localhost:44392/Home/Index](https://localhost:44392/Home/Index)」，進入SleepZone.Todos頁面。
  
  - SleepZone.Todos頁面：點擊「AddTodo按鈕」新增工作項目，看到{ "statusCode": 200 }代表工作項目新增成功。

  - SleepZone.Todos頁面：點擊「FindAllTodo按鈕」查詢所有工作項目，看到{ "statusCode": 200 }代表工作項目查詢成功。
  
  - 上述步驟內容，展示使用DI機制，進行Context注入組裝，並且設定為Singleton模式。所以在沒有資料庫的狀態下，FindAll才能讀取到之前寫入的Todo。

     
## 架構設計

### MDP.Net-平台架構

![MDP.Net-平台架構](https://raw.githubusercontent.com/Clark159/MDP.Net/master/docs/MDP.Net-平台架構.png)

### MDP.Net-模組架構

![MDP.Net-專案架構](https://raw.githubusercontent.com/Clark159/MDP.Net/master/docs/MDP.Net-專案架構.png)

### MDP.Net-模組清單

- MDP.DevKit
  
  - MDP.DevKit.LineMessaging

  - MDP.DevKit.OpenAI 
 
- MDP.AspNetCore.Authentication

  - MDP.AspNetCore.Authentication
  
  - MDP.AspNetCore.Authentication.AzureAD
  
  - MDP.AspNetCore.Authentication.Facebook
  
  - MDP.AspNetCore.Authentication.GitHub
  
  - MDP.AspNetCore.Authentication.Google
  
  - MDP.AspNetCore.Authentication.Jwt
  
  - MDP.AspNetCore.Authentication.Liff
  
  - MDP.AspNetCore.Authentication.Line

- MDP.Network
  
  - MDP.Network.Http
  
  - MDP.Network.Rest
  
- MDP.Data

  - MDP.Data.MSSql


## 版本更新

### MDP.Net 6.1.5

- MDP.NetCore：加入AddMdp()，提升平台識別。

- MDP.AspNetCore：加入AddMdp()，提升平台識別。

### MDP.Net 6.1.4

- MDP.DevKit.LineMessaging：遷移此模組，遷移至獨立Repo進行維護。

### MDP.Net 6.1.2

- MDP.Hosting：移除Autofac參考。

### MDP.Net 6.1.0

- MDP.Registration：加入此模組，提供Attribute註冊功能，用以移除所有.Hosting註冊用專案。

### MDP.Net 6.0.16

- MDP.AspNetCore.Authentication.Liff.Services：加入此模組，提供Liff頁面客製化功能。

### MDP.Net 6.0.15

- MDP.AspNetCore.Authentication.Liff：調整Liff登入效能、調整Liff登入頁面提示文字。

### MDP.Net 6.0.14

- MDP.AspNetCore.Authentication.Liff：調整Liff登入效能、調整Liff登入頁面提示文字。

### MDP.Net 6.0.13

- MDP.AspNetCore.WebApp：WebApp專案設定加入GenerateDocumentationFile，讓系統產生API說明檔。

- MDP.AspNetCore：加入Swagger預設參數，讓SwaggerUI顯示API說明檔。

- MDP.IdentityModel.Tokens.Jwt：加入RSA非對稱金鑰支援。

- MDP.AspNetCore.Authentication.Jwt：加入RSA非對稱金鑰支援。

### MDP.Net 6.0.12

- MDP.AspNetCore：加入Swagger預設參數，讓SwaggerUI顯示API。

- MDP.IdentityModel.Tokens.Jwt：SecurityTokenFactory加入自訂ExpireMinutes功能。

- MDP.AspNetCore.Authentication.Jwt：加入多個JWT來源驗證功能，提供自訂HeaderName、TokenPrefix設定。

- MDP.AspNetCore.Authentication：加入PolicySchemeSelector，提供依照HttpContex選擇AuthenticationScheme功能。