---
layout: default
title: 參數管理
parent: null
nav_order: 3
has_children: false
---

# MDP.Configuration

MDP.Configuration是.NET版本的開發套件，協助開發人員快速建立具有參數管理的應用系統。

- 說明文件：[https://clark159.github.io/MDP.Net/](https://clark159.github.io/MDP.Net/)

- 程式源碼：[https://github.com/Clark159/MDP.Net/](https://github.com/Clark159/MDP.Net/)


## 模組架構

![MDP.Configuration-模組架構]()

MDP.Configuration擴充.Net Core既有的參數管理，加入了依照：開發/測試/正式三個執行環境，讀取不同設定檔的功能。

1.執行環境(Environment)名稱：

```
- 開發環境：Development
- 測試環境：Staging
- 正式環境：Production
```

2.執行資料夾(<EntryDir>)讀取設定檔：

```
- <EntryDir>\appsettings.json
- <EntryDir>\*.{Environment}.json
```

3.參數資料夾(<EntryDir>\config)讀取設定檔：

```
- <EntryDir>\config\appsettings.json
- <EntryDir>\config\*.{Environment}.json
```

4.環境資料夾(<EntryDir>\config\{Environment})讀取設定檔：

```
- <EntryDir>\config\{Environment}\*.json
```


## 模組使用


## 模組範例

