# MDP.NetCore

MDP.NetCore是開源的.NET開發平台，協助開發人員快速建立Web站台、API服務和Console程式。提供參數管理、依賴注入、身分驗證等多種功能模組，用以簡化開發流程並滿足多變的商業需求。

- 說明文件：[https://mdpcore.github.io/MDP.NetCore/](https://mdpcore.github.io/MDP.NetCore/)

- 程式源碼：[https://github.com/MDPCore/MDP.NetCore](https://github.com/MDPCore/MDP.NetCore)

 
## 快速開始

- [開發一個依賴注入+參數管理的API服務](https://mdpcore.github.io/MDP.NetCore/快速開始/開發一個依賴注入+參數管理的API服務/)

- [開發一個依賴注入+參數管理的Web站台](https://mdpcore.github.io/MDP.NetCore/快速開始/開發一個依賴注入+參數管理的Web站台/)

- [開發一個依賴注入+參數管理的Console程式](https://mdpcore.github.io/MDP.NetCore/快速開始/開發一個依賴注入+參數管理的Console程式/)


## 平台架構

![MDP.NetCore-平台架構](https://mdpcore.github.io/MDP.NetCore/功能說明/MDP.NetCore-平台架構.png)

MDP.NetCore將應用系統切割為：模組、隔離、平台三個分層，透過架構設計提供模組重用、參數調整、環境建置...等等面向的快速開發能力。

- 模組：企業的商業知識、共用的功能邏輯，在MDP.NetCore裡會被開發成為一個一個的「模組」，方便開發人員依照商業需求，快速組合出應用系統。

- 隔離：MDP.NetCore加入「隔離」的設計，並且模組開發遵循三層式架構設計， 以減少模組對於元件、平台、框架的直接依賴，方便開發人員依照技術需求，快速抽換相依元件。

- 平台：MDP.NetCore透過「平台」的設計，提供一個開箱即用的執行平台，將參數調整、模組整合、環境調適...等等環境建設作業簡化封裝，方便開發人員依照專案需求，快速搭建執行環境。


## 程式架構

![MDP.NetCore-程式架構](https://mdpcore.github.io/MDP.NetCore/功能說明/MDP.NetCore-程式架構.png)

MDP.NetCore遵循三層式架構，將模組開發切割為：系統展示、領域邏輯、資料存取三個分層，減少模組對於元件、平台、框架的直接依賴，提高模組自身的內聚力。

- 系統展示(Presentation)：與目標客戶互動、與遠端系統通訊...等等的功能邏輯，會被歸類在系統展示。例如，使用MessageBox通知使用者處理結果、提供API給遠端系統使用。

- 領域邏輯(Domain)：封裝商業知識的物件、流程、演算法...等等的功能邏輯，會被歸類在領域邏輯。例如，出勤系統的刷卡記錄物件、購物商城的折購計算規則。

- 資料存取(Accesses)：資料庫的新增修改、遠端服務的呼叫調用...等等的功能邏輯，會被歸類在資料存取。例如，將資料存放到SQL Server、或者是從遠端API取得資料。

MDP.NetCore的模組程式遵循此分層，將每個模組拆解為五種類型的專案，依序命名為：

- Module001.csproj：領域邏輯專案。

- Module001.Web.csproj：系統展示專案。(前台&API)

- Module001.Admin.csproj：系統展示專案。(後台)

- Module001.Mocks.csproj：資料存取專案。(Mocks)

- Module001.Accesses.csproj：資料存取專案。(SQL、REST、Cache)

而在MDP.NetCore的領域邏輯(Domain)裡，也加入了下列設計，來進一步提升程式開發速度。

- Entity：DDD領域模型的實例物件(Class)，用以定義資料物件的物件屬性，並可以封裝商業邏輯成為物件方法。

- Repository：資料庫存取的邊界介面(Interface)，用來定義實例物件(Entity)進出資料庫的介面方法。

- Provider：遠端系統調用的邊界介面(Interface)，用來定義實例物件(Entity)進出遠端系統的介面方法。

- Service：商業知識的流程即演算法的封裝物件(Class)，封裝商業邏輯成為物件方法。也可以設計為邊界介面(Interface)，將商業邏輯用實做注入的方式使用。

- Context：做為模組入口的根物件(Class)，遵循Facade Pattern設計的原則，將上述四種物件與介面進行收整。除了做為模組被註冊、注入、使用的根物件之外，也可以封裝商業邏輯成為物件方法。


## 模組架構

![MDP.NetCore-模組架構](https://mdpcore.github.io/MDP.NetCore/功能說明/MDP.NetCore-模組架構.png)


## 版本更新

### MDP.NetCore 8.0.6

- MDP.Registration：TryResolve加入對於強型別的支援。

### MDP.NetCore 8.0.3

- MDP.Security.Tokens.Jwt：加入多組Token同時使用功能。

- MDP.BlazorCore：整併至[MDPCore/MDP.BlazorCore](https://github.com/MDPCore/MDP.BlazorCore) 

- MDP.AspNetCore：整併至[MDPCore/MDP.AspNetCore](https://github.com/MDPCore/MDP.AspNetCore) 

### MDP.NetCore 8.0.2

- MDP.BlazorCore：加入Web、App跨平台執行能力。

### MDP.NetCore 8.0.1

- MDP.NetCore：升級至.NET8.0

### MDP.NetCore 6.1.17

- MDP.Security.Tokens.Jwt：加入MDP.Security.Tokens.Jwt，提供JwtToken生成功能。

### MDP.NetCore 6.1.16

- MDP.Hosting：加入AutoRegister，削減Config設定內容。

### MDP.NetCore 6.1.13

- MDP.Registration：加入ServiceBuilder，削減Domain層對於DI框架的相依。

### MDP.NetCore 6.1.12

- MDP.AspNetCore：加入ForwardedHeaders，提供302轉址使用HTTPS。

### MDP.NetCore 6.1.11

- MDP.Registration：加入ServiceRegistration，簡化Factory註冊服務的步驟。

### MDP.NetCore 6.1.8

- MDP.Logging.Serilog：加入此模組，提供Serilog日誌服務。

### MDP.NetCore 6.1.5

- MDP.NetCore：加入AddMdp()，提升平台識別。

- MDP.AspNetCore：加入AddMdp()，提升平台識別。

### MDP.NetCore 6.1.4

- MDP.DevKit.LineMessaging：遷移此模組，遷移至獨立Repo進行維護。

### MDP.NetCore 6.1.2

- MDP.Hosting：移除Autofac參考。

### MDP.NetCore 6.1.0

- MDP.Registration：加入此模組，提供Attribute註冊功能，用以移除所有.Hosting註冊用專案。

### MDP.NetCore 6.0.16

- MDP.AspNetCore.Authentication.Liff.Services：加入此模組，提供Liff頁面客製化功能。

### MDP.NetCore 6.0.15

- MDP.AspNetCore.Authentication.Liff：調整Liff登入效能、調整Liff登入頁面提示文字。

### MDP.NetCore 6.0.14

- MDP.AspNetCore.Authentication.Liff：調整Liff登入效能、調整Liff登入頁面提示文字。

### MDP.NetCore 6.0.13

- MDP.AspNetCore.WebApp：WebApp專案設定加入GenerateDocumentationFile，讓系統產生API說明檔。

- MDP.AspNetCore：加入Swagger預設參數，讓SwaggerUI顯示API說明檔。

- MDP.IdentityModel.Tokens.Jwt：加入RSA非對稱金鑰支援。

- MDP.AspNetCore.Authentication.Jwt：加入RSA非對稱金鑰支援。

### MDP.NetCore 6.0.12

- MDP.AspNetCore：加入Swagger預設參數，讓SwaggerUI顯示API。

- MDP.IdentityModel.Tokens.Jwt：SecurityTokenFactory加入自訂ExpireMinutes功能。

- MDP.AspNetCore.Authentication.Jwt：加入多個JWT來源驗證功能，提供自訂HeaderName、TokenPrefix設定。

- MDP.AspNetCore.Authentication：加入PolicySchemeSelector，提供依照HttpContex選擇AuthenticationScheme功能。