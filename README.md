# MDP.Net

MDP.Net是以領域驅動設計為核心的.NET Core開發平台，協助開發人員快速建立Web AP、Console AP。


## MDP.Net-軟體架構

![MDP.Net-軟體架構](https://raw.githubusercontent.com/Clark159/MDP.Net/master/doc/MDP.Net-%E8%BB%9F%E9%AB%94%E6%9E%B6%E6%A7%8B.png)

## MDP.Net-需求分析

![MDP.Net-需求分析](https://raw.githubusercontent.com/Clark159/MDP.Net/master/doc/MDP.Net-%E9%9C%80%E6%B1%82%E5%88%86%E6%9E%90.png)


## 測試項目

### 工作管理模組(SleepZone-Todos)

- 測試網址：https://localhost:44318/SleepZone-Todos/Home/Index

- 點擊「AddTodo按鈕」新增工作項目。看到{ "statusCode": 200 }代表工作項目新增成功。

- 點擊「FindAllTodo按鈕」查詢所有工作項目。看到{ "statusCode": 200 }代表工作項目查詢成功。

- 點擊「FindLastSnapshot按鈕」查詢最後一筆工作快照。看到{ "statusCode": 200 }代表工作快照查詢成功。
