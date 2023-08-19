# MDP.Net 

MDP.Net是.NET版本的開發平台，協助開發人員快速建立：Web站台、API服務、Console程式。

- 說明文件：[https://clark159.github.io/MDP.Net/](https://clark159.github.io/MDP.Net/)

- 程式源碼：[https://github.com/Clark159/MDP.Net/](https://github.com/Clark159/MDP.Net/)


## MDP.Net-平台架構

![MDP.Net-平台架構](https://raw.githubusercontent.com/Clark159/MDP.Net/master/docs/MDP.Net-平台架構.png)

MDP.Net將應用系統切割為：模組、隔離、平台三個分層，透過架構設計提供模組重用、參數調整、環境建置...等等面向的快速開發能力。

- 模組：企業的商業知識、共用的功能邏輯，在MDP.Net裡會被開發成為一個一個的「模組」，方便開發人員依照商業需求，快速組合出應用系統。

- 隔離：MDP.Net加入「隔離」的設計，並且模組開發遵循三層式架構設計， 以減少模組對於元件、平台、框架的直接依賴，方便開發人員依照技術需求，快速抽換相依元件。

- 平台：MDP.Net透過「平台」的設計，提供一個開箱即用的執行平台，將參數調整、模組整合、環境調適...等等環境建設作業簡化封裝，方便開發人員依照專案需求，快速搭建執行環境。


## MDP.Net-模組清單

- 開發工具(MDP.DevKit)
  
  - MDP.DevKit.LineMessaging

  - MDP.DevKit.OpenAI 
 
- 身分驗證(MDP.AspNetCore.Authentication)

  - MDP.AspNetCore.Authentication
  
  - MDP.AspNetCore.Authentication.AzureAD
  
  - MDP.AspNetCore.Authentication.Facebook
  
  - MDP.AspNetCore.Authentication.GitHub
  
  - MDP.AspNetCore.Authentication.Google
  
  - MDP.AspNetCore.Authentication.Jwt
  
  - MDP.AspNetCore.Authentication.Liff
  
  - MDP.AspNetCore.Authentication.Line

- 網路通訊(MDP.Network)
  
  - MDP.Network.Http
  
  - MDP.Network.Rest
  
- 日誌服務(MDP.Logging)
  
  - MDP.Logging.Log4net
  
  - MDP.Logging.NLog
  
- 資料存取(MDP.Data)

  - MDP.Data.MSSql  
  

## MDP.Net-模組架構

![MDP.Net-模組架構](https://raw.githubusercontent.com/Clark159/MDP.Net/master/docs/MDP.Net-模組架構.png)

MDP.Net遵循三層式架構，將模組開發切割為：系統展示、領域邏輯、資料存取三個分層，減少模組對於元件、平台、框架的直接依賴，提高模組自身的內聚力。

- 系統展示(Presentation)：與目標客戶互動、與遠端系統通訊...等等的功能邏輯，會被歸類在系統展示。例如，使用MessageBox通知使用者處理結果、提供API給遠端系統使用。

- 領域邏輯(Domain)：封裝商業知識的物件、流程、演算法...等等的功能邏輯，會被歸類在領域邏輯。例如，出勤系統的刷卡記錄物件、購物商城的折購計算規則。

- 資料存取(Accesses)：資料庫的新增修改、遠端服務的呼叫調用...等等的功能邏輯，會被歸類在資料存取。例如，將資料存放到SQL Server、或者是從遠端API取得資料。

MDP.Net的模組程式遵循此分層，將每個模組拆解為三個專案，依序命名為：

- Module001.csproj：領域邏輯專案。

- Module001.WebAPI.csproj：系統展示專案。

- Module001.Accesses.csproj：資料存取專案。

而在MDP.Net的領域邏輯(Domain)裡，也加入了下列設計，來進一步提升程式開發速度。

- Entity：DDD領域模型的實例物件(Class)，用以定義資料物件的物件屬性，並可以封裝商業邏輯成為物件方法。

- Repository：資料庫存取的邊界介面(Interface)，用來定義實例物件(Entity)進出資料庫的介面方法。

- Provider：遠端系統調用的邊界介面(Interface)，用來定義實例物件(Entity)進出遠端系統的介面方法。

- Service：商業知識的流程即演算法的封裝物件(Class)，封裝商業邏輯成為物件方法。也可以設計為邊界介面(Interface)，將商業邏輯用實做注入的方式使用。

- Context：做為模組入口的根物件(Class)，遵循Facade Pattern設計的原則，將上述四種物件與介面進行收整。除了做為模組被註冊、注入、使用的根物件之外，也可以封裝商業邏輯成為物件方法。
  

## 範例展示(SleepZone.Todos)

- 範例檔案：

  - [https://github.com/Clark159/MDP.Net/tree/master/demo/SleepZone.Todos](https://github.com/Clark159/MDP.Net/tree/master/demo/SleepZone.Todos)

- 操作步驟：

  - 使用Visual Studio 2022開啟方案：SleepZone.Todos.sln
  
  - 設定起始專案為：01.Application/SleepZone.Todos.WebApp
  
  - 按下F5執行專案後，於瀏覽器的網址列：輸入「[https://localhost:44392/Home/Index](https://localhost:44392/Home/Index)」，進入SleepZone.Todos頁面。
  
  - SleepZone.Todos頁面：點擊「AddTodo按鈕」新增工作項目，看到{ "statusCode": 200 }代表工作項目新增成功。

  - SleepZone.Todos頁面：點擊「FindAllTodo按鈕」查詢所有工作項目，看到{ "statusCode": 200 }代表工作項目查詢成功。
  
  - 上述步驟內容，展示使用DI機制，進行Context注入組裝，並且設定為Singleton模式。所以在沒有資料庫的狀態下，FindAll才能讀取到之前寫入的Todo。


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