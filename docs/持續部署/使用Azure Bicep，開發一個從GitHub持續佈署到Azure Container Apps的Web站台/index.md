---
layout: default
title: 使用Azure Bicep，開發一個從GitHub持續佈署到Azure Container Apps的Web站台
parent: 持續部署
nav_order: 2
---


# 使用Azure Bicep，開發一個從GitHub持續佈署到Azure Container Apps的Web站台

程式碼簽入GitHub之後，啟動GitHub Action流程，編譯並部署程式到Azure Container Apps，是開發系統時常見的功能需求。本篇範例協助開發人員使用GitHub與Azure Bicep，逐步完成必要的設計和實作。

- 範例下載：[SleepZone.zip](https://clark159.github.io/MDP.Net/持續部署/使用Azure Bicep，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/SleepZone.zip)


## 操作步驟

1.註冊並登入[Azure Portal](https://portal.azure.com/)。於首頁左上角的選單裡，點擊資源群組後，進入資源群組頁面。於資源群組頁面，點擊建立按鈕，依照頁面提示建立一個ResourceGroup，並命名為sleep-zone-group。

![01.建立ResourceGroup01.png](https://clark159.github.io/MDP.Net/持續部署/使用Azure Bicep，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/01.建立ResourceGroup01.png)

![01.建立ResourceGroup02.png](https://clark159.github.io/MDP.Net/持續部署/使用Azure Bicep，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/01.建立ResourceGroup02.png)

![01.建立ResourceGroup03.png](https://clark159.github.io/MDP.Net/持續部署/使用Azure Bicep，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/01.建立ResourceGroup03.png)

2.建立完畢後，於ResourceGroup頁面的屬性頁籤，取得「資源識別碼」。

![01.建立ResourceGroup04.png](https://clark159.github.io/MDP.Net/持續部署/使用Azure Bicep，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/01.建立ResourceGroup04.png)

![01.建立ResourceGroup05.png](https://clark159.github.io/MDP.Net/持續部署/使用Azure Bicep，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/01.建立ResourceGroup05.png)

3.回到[Azure Portal](https://portal.azure.com/)。於右上角的選單裡，點擊Cloud Shell按鈕後，開啟Cloud Shell視窗。於Cloud Shell視窗，切換至Bash並執行下列指令，用來取得部署使用的「服務主體憑證」。該指令會建立名為sleep-zone-app-contributor的應用程式註冊，並授權它為sleep-zone-group資源群組的參與者(Contributor)角色。

```
az ad sp create-for-rbac \
    --name "sleep-zone-app-contributor" \
    --role "Contributor" \
    --scopes xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx \
    --sdk-auth
    
- 服務主體名稱：--name "sleep-zone-app-contributor"，可自訂，限制使用英文小寫與「-」。(sleep-zone-app-contributor為持續部署服務主體)
- 服務主體授權角色：--role "Contributor"。
- 服務主體授權範圍：--scopes xxxxxxxxxxxxxxx。(xxxxx填入先前取得的資源識別碼)
```

![02.建立Application01.png](https://clark159.github.io/MDP.Net/持續部署/使用Azure Bicep，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/02.建立Application01.png)

![02.建立Application02.png](https://clark159.github.io/MDP.Net/持續部署/使用Azure Bicep，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/02.建立Application02.png)

4.註冊並登入[GitHub Dashboard](https://github.com/)。點擊首頁左上角的New按鈕，依照頁面提示建立一個Repository，並命名為SleepZone。

![03.建立Repository01.png](https://clark159.github.io/MDP.Net/持續部署/使用Azure Bicep，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/03.建立Repository01.png)

![03.建立Repository02.png](https://clark159.github.io/MDP.Net/持續部署/使用Azure Bicep，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/03.建立Repository02.png)

5.建立完畢後，於Repository頁面，點擊右上角的Setting按鈕進入Setting頁面，並選擇左側選單裡Secrets and variables的Action頁籤。於Action頁籤，點選New repository secret按鈕，依照頁面提示建立一個名為AZURE_CREDENTIALS的Actions Secret，Secret內容則是填入先前取得的「服務主體憑證」。

![03.建立Repository03.png](https://clark159.github.io/MDP.Net/持續部署/使用Azure Bicep，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/03.建立Repository03.png)

![03.建立Repository04.png](https://clark159.github.io/MDP.Net/持續部署/使用Azure Bicep，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/03.建立Repository04.png)

![03.建立Repository05.png](https://clark159.github.io/MDP.Net/持續部署/使用Azure Bicep，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/03.建立Repository05.png)

6.Clone SleepZone Repository到本機Repository資料夾，於本機Repository資料夾裡建立src資料夾。開啟命令提示字元進入src資料夾，輸入下列指令，用以安裝MDP.WebApp範本、並且建立一個名為WebApplication1的Web站台。

```
// 建立Web站台
dotnet new install MDP.WebApp
dotnet new MDP.WebApp -n WebApplication1
```

![04.建立WebApplication01.png](https://clark159.github.io/MDP.Net/持續部署/使用Azure Bicep，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/04.建立WebApplication01.png)

![04.建立WebApplication02.png](https://clark159.github.io/MDP.Net/持續部署/使用Azure Bicep，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/04.建立WebApplication02.png)

7.於本機Repository資料夾裡，進入src/WebApplication1資料夾，並加入Dockerfile。

```
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /repo
COPY . .
RUN dotnet publish "src/WebApplication1/WebApplication1.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WebApplication1.dll"]
```

![04.建立WebApplication03.png](https://clark159.github.io/MDP.Net/持續部署/使用Azure Bicep，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/04.建立WebApplication03.png)

8.於本機Repository資料夾裡，建立.github\workflows資料夾，並加入azure-build-deployment.yml、azure-build-deployment.bicep。

```
// azure-build-deployment.yml
{% raw %}
name: azure-build-deployment

on:
  push:
    branches:
      - main

env:
  RESOURCE_GROUP_NAME: sleep-zone-group
  CONTAINER_APPS_NAME: sleep-zone-app 
  DOCKER_FILE_PATH: ./src/WebApplication1/Dockerfile

jobs:
  deploy:
    runs-on: ubuntu-latest
    steps:  

      - name: Login Azure CLI
        uses: azure/login@v1
        with:
          creds: ${{ secrets.AZURE_CREDENTIALS }}

      - name: Checkout GitHub Repository  
        uses: actions/checkout@v4

      - name: Deploy AzureContainerRegistry by MDP
        uses: MDPOps/azure-container-registry-deploy@v1
        with:
          resourceGroupName: ${{ env.RESOURCE_GROUP_NAME }}
          dockerFilePath: ${{ env.DOCKER_FILE_PATH }}
          dockerImageName: ${{ env.CONTAINER_APPS_NAME }}
        id: azure-container-registry-deploy

      - name: Deploy AzureResourceManager by MDP
        uses: MDPOps/azure-resource-manager-deploy@v1
        with:
          resourceGroupName: ${{ env.RESOURCE_GROUP_NAME }}
          parameters: >-
            containerAppName=${{ env.CONTAINER_APPS_NAME }}
            containerRegistryName=${{ steps.azure-container-registry-deploy.outputs.containerRegistryName }}
            containerRegistryCredentials=${{ toJson(steps.azure-container-registry-deploy.outputs.containerRegistryCredentials) }}
        id: azure-resource-manager-deploy           

- Git分支名稱：main，要特別注意Repository裡的分支是 master or main。
- 資源群組名稱：RESOURCE_GROUP_NAME: sleep-zone-group，可自訂，限制使用英文小寫與「-」。
- 容器應用名稱：CONTAINER_APPS_NAME: sleep-zone-app，可自訂，限制使用英文小寫與「-」。
- Dockerfile路徑：DOCKER_FILE_PATH: ./src/WebApplication1/Dockerfile，路徑區分大小寫，相對於Repository資料夾。
{% endraw %}
```

```
// azure-build-deployment.bicep
{% raw %}
// Inputs
@description('ContainerApp Name')
param containerAppName string

@description('ContainerRegistry Name')
param containerRegistryName string 

@description('ContainerRegistry Credentials')
@secure()
param containerRegistryCredentials object 


// LogAnalyticsWorkspace
module logAnalyticsWorkspace 'modules/logAnalyticsWorkspace.bicep' =  {
  name: 'logAnalyticsWorkspace'
  params: {
    
  }
}

// ContainerAppsEnvironment
module containerAppsEnvironment 'modules/containerAppsEnvironment.bicep' =  {
  name: 'containerAppsEnvironment'
  params: {
    logAnalyticsWorkspaceName: logAnalyticsWorkspace.outputs.name
  }
}

// ContainerApp
module containerApp 'modules/containerApp.bicep' =  {
  name: 'containerApp'
  params: {
    containerAppName: containerAppName
    containerAppsEnvironmentName: containerAppsEnvironment.outputs.name
    containerRegistryName: containerRegistryName
    containerRegistryCredentials: containerRegistryCredentials 
  }
}      
{% endraw %}
```

![04.建立WebApplication04.png](https://clark159.github.io/MDP.Net/持續部署/使用Azure Bicep，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/04.建立WebApplication04.png)

9.簽入並推送本機Repository的變更，到遠端GitHub主機之後。回到[GitHub Dashboard](https://github.com/)，進入SleepZone Repository頁面。可以於Actions頁籤裡，可以看到GitHub Action流程已經觸發執行並成功完成部署。(需時約五分鐘)

![05.執行結果01.png](https://clark159.github.io/MDP.Net/持續部署/使用Azure Bicep，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/05.執行結果01.png)

10.回到[Azure Portal](https://portal.azure.com/)。於首頁左上角的選單裡，點擊容器應用程式後，進入容器應用程式頁面，可以看到剛剛建立的sleep-zone-app容器應用程式。

![05.執行結果02.png](https://clark159.github.io/MDP.Net/持續部署/使用Azure Bicep，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/05.執行結果02.png)

![05.執行結果03.png](https://clark159.github.io/MDP.Net/持續部署/使用Azure Bicep，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/05.執行結果03.png)

11.於sleep-zone-app容器應用程式的概觀頁面，點擊「應用程式 URL」鏈結，在開啟的Browser視窗內，可以看到由WebApplication1程式碼所提供的Hello World。

![05.執行結果04.png](https://clark159.github.io/MDP.Net/持續部署/使用Azure Bicep，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/05.執行結果04.png)

![05.執行結果05.png](https://clark159.github.io/MDP.Net/持續部署/使用Azure Bicep，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/05.執行結果05.png)

12.完成上述步驟之後，每次推送程式碼到GitHub，都會啟動GitHub Action流程，編譯並部署程式到Azure Container Apps。

![05.執行結果06.png](https://clark159.github.io/MDP.Net/持續部署/使用Azure Bicep，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/05.執行結果06.png)
