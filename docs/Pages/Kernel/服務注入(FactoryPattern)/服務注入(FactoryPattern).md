---
layout: default
title: 服務注入(FactoryPattern)
parent: 核心模組(Kernel)
nav_order: 4
---

# 服務注入(FactoryPattern)

在MDP.Net核心模組中，「服務注入模組」參考Configuration參數設定，依照下列規則注入Instance：

- Service注入至「使用ServiceAttribute註冊的Type」的建構子。
```
1. 使用Type裡參數(Parameter)數量最多的建構子，進行反射生成。
2. 建構子的參數類型(Parameter.Type)為基本數據，使用參數名稱(Parameter.Name)取得Configuration參數內容，生成基本數據，再綁定(Bind)Configuration參數內容後注入。
3. 建構子的參數類型(Parameter.Type)為物件類別，使用參數名稱(Parameter.Name)取得Configuration參數內容，做為InstanceName，用以取得NamedInstance後注入。
4. 建構子的參數類型(Parameter.Type)為物件類別，使用參數名稱(Parameter.Name)取得Configuration參數內容，為Null或沒有設定，預設取得TypedInstance後注入。
5. 建構子的參數類型(Parameter.Type)為物件類別，使用參數名稱(Parameter.Name)取得Configuration參數內容，無法取得Instance時，使用無參數建構子生成Instance，再綁定(Bind)Configuration參數內容後注入。
```

- Service注入至「Controller」的建構子。
```
1. 建構子的參數類型(Parameter.Type)為物件類別，取得TypedInstance後注入。
2. 依照ASP.NET Core的規則注入。
```

本篇文件介紹，如何使用MDP.Net核心模組中「服務注入模組」，參考Configuration參數設定，進行服務注入。



