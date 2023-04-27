# MDP.Net 

MDP.Net是.NET版本的開發平台，協助開發人員快速建立Web App、Console App。


## 範例展示

### SleepZone.Todos

- 開發工具：Visual Studio 2022 

- 方案路徑：Repo資料夾=>examples/00.SleepZone.Todos/SleepZone.Todos.sln

- 起始專案：方案總管內=>01.Application/SleepZone.Todos.WebApp

- 測試步驟：
  
  - 瀏覽器的網址列：輸入「[https://localhost:44392/Home/Index](https://localhost:44392/Home/Index)」，進入SleepZone.Todos頁面。
  
  - SleepZone.Todos頁面：點擊「AddTodo按鈕」新增工作項目，看到{ "statusCode": 200 }代表工作項目新增成功。

  - SleepZone.Todos頁面：點擊「FindAllTodo按鈕」查詢所有工作項目，看到{ "statusCode": 200 }代表工作項目查詢成功。
  
  - 上述步驟內容，展示使用DI機制，進行Context注入組裝，並且設定為Singleton模式。所以在沒有資料庫的狀態下，FindAll才能讀取到之前寫入的Todo。

- 團隊分工：

  - 團隊依專業分為：平台開發團隊、應用開發團隊。
  
  - 平台開發團隊：建置SleepZone.Todos.WebApp，給應用團隊開箱即用。交付內容包含：專案原始碼、DevOps專案、CICD Pipeline、Container、Network、DNS...等平台工程產出。
  
  - 應用開發團隊：建置SleepZone.Todos Module，滿足應用系統需求。開發規範依循：「[MDP.Net-軟體架構](https://github.com/Clark159/MDP.Net#mdpnet-%E8%BB%9F%E9%AB%94%E6%9E%B6%E6%A7%8B)」、「[MDP.Net-分層架構](https://github.com/Clark159/MDP.Net#mdpnet-%E5%88%86%E5%B1%A4%E6%9E%B6%E6%A7%8B)」進行開發。
  
  
## 架構設計

### MDP.Net-軟體架構

![MDP.Net-軟體架構](https://raw.githubusercontent.com/Clark159/MDP.Net/master/docs/MDP.Net-%E8%BB%9F%E9%AB%94%E6%9E%B6%E6%A7%8B.png)

### MDP.Net-分層架構

![MDP.Net-分層架構](https://raw.githubusercontent.com/Clark159/MDP.Net/master/docs/MDP.Net-%E5%88%86%E5%B1%A4%E6%9E%B6%E6%A7%8B.png)

### MDP.Net-平台架構

![MDP.Net-平台架構](https://raw.githubusercontent.com/Clark159/MDP.Net/master/docs/MDP.Net-%E5%B9%B3%E5%8F%B0%E6%9E%B6%E6%A7%8B.png)

### MDP.Net-需求分析

![MDP.Net-需求分析](https://raw.githubusercontent.com/Clark159/MDP.Net/master/docs/MDP.Net-%E9%9C%80%E6%B1%82%E5%88%86%E6%9E%90.png)


## 版本更新

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
