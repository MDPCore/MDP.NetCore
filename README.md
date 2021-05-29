# MDP.Net

MDP.Net是以領域驅動設計為核心的.NET Core開發平台，協助開發人員快速建立Web AP、Console AP。


## MDP.Net-軟體架構

![MDP.Net-軟體架構](https://raw.githubusercontent.com/Clark159/MDP.Net/master/doc/MDP.Net-%E8%BB%9F%E9%AB%94%E6%9E%B6%E6%A7%8B.png)

## MDP.Net-需求分析

![MDP.Net-需求分析](https://raw.githubusercontent.com/Clark159/MDP.Net/master/doc/MDP.Net-%E9%9C%80%E6%B1%82%E5%88%86%E6%9E%90.png)


## 建置步驟

1. 從Firebase後台，下載MDP.Firebase.Admin.json

2. 將MDP.Firebase.Admin.json放到SleepZone.WebApp專案資料夾根目錄

3. 將SleepZone.WebApp專案設為起始專案。

4. 建置並執行專案。


## 測試項目

### 工作管理模組(SleepZone-Todos)

- 測試網址：https://localhost:44318/SleepZone-Todos/Home/Index

- 點擊「AddTodo按鈕」新增工作項目。看到{ "statusCode": 200 }代表工作項目新增成功。

- 點擊「FindAllTodo按鈕」查詢所有工作項目。看到{ "statusCode": 200 }代表工作項目查詢成功。

- 點擊「FindLastSnapshot按鈕」查詢最後一筆工作快照。看到{ "statusCode": 200 }代表工作快照查詢成功。


### 推播發送模組(MDP-Messaging-Notifications)

- 測試網址：https://localhost:44376/MDP-Messaging-Notifications/index.html

- 點擊「Register按鈕」進行Client端註冊。看到{ "statusCode": 200, "content": {} }代表跟Firebase伺服器註冊成功，開始接收推播訊息。

- 點擊「Send按鈕」，進行推播訊息發送。看到{ "statusCode": 200, "content": {} }推播訊息發送成功。

- 「OnMessage區塊」出現訊息，代表從Firebase伺服器收到推播訊息。

- 點擊「Unregister按鈕」進行Client端移除註冊。看到Success: token=xxxxxxxxx代表跟Firebase伺服器移除註冊成功，停止接收推播訊息。

### 測試範例模組(MDP-Notifications)

- 測試網址：https://localhost:44376/MDP-Module01/Home/Index

- 看到「MDP-Module01/Home/Index」藍色標題內容，代表CSS檔案動態掛載成功。

- 看到「Hello World From MDP.ModuleLab.SettingContext!」訊息內容，代表SettingContext動態註冊成功