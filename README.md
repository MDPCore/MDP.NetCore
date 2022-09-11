# MDP.Net 

MDP.Net是以領域驅動設計為核心的.NET Core開發平台，協助開發人員快速建立Web APP、Console APP。


## 範例展示

### Todos-工作管理

- 開發工具：Visual Studio 2022 

- 方案路徑：src/MDP.Net.sln

- 起始專案：方案總管/01.Application/01.SleepZone.Todos/SleepZone.Todos.WebApp

- 起始網址：https://localhost:44318/SleepZone-Todos/Home/Index

- 測試步驟：

  - 點擊「AddTodo按鈕」新增工作項目。看到{ "statusCode": 200 }代表工作項目新增成功。

  - 點擊「FindAllTodo按鈕」查詢所有工作項目。看到{ "statusCode": 200 }代表工作項目查詢成功。


## 架構設計

### MDP.Net-軟體架構

![MDP.Net-軟體架構](https://raw.githubusercontent.com/Clark159/MDP.Net/master/doc/MDP.Net-%E8%BB%9F%E9%AB%94%E6%9E%B6%E6%A7%8B.png)

### MDP.Net-平台架構

![MDP.Net-平台架構](https://github.com/Clark159/MDP.Net/raw/master/doc/MDP.Net-%E5%B9%B3%E5%8F%B0%E6%9E%B6%E6%A7%8B.png)

### MDP.Net-需求分析

![MDP.Net-需求分析](https://raw.githubusercontent.com/Clark159/MDP.Net/master/doc/MDP.Net-%E9%9C%80%E6%B1%82%E5%88%86%E6%9E%90.png)


## 版本更新

### MDP.Net 6.0.11

- MDP.AspNetCore：加入Swagger預設參數，讓API顯示在SwaggerUI。

- MDP.IdentityModel.Tokens.Jwt：SecurityTokenFactory加入自訂ExpireMinutes功能。

- MDP.AspNetCore.Authentication.Jwt：加入多個JWT來源驗證功能。

- MDP.AspNetCore.Authentication：加入PolicySchemeSelector，提供依照HttpContex選擇AuthenticationScheme功能
