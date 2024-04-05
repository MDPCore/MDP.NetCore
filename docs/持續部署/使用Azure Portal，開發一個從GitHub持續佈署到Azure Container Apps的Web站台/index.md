---
layout: default
title: 使用Azure Portal，開發一個從GitHub持續佈署到Azure Container Apps的Web站台
parent: 持續部署
nav_order: 1
---


# 使用Azure Portal，開發一個從GitHub持續佈署到Azure Container Apps的Web站台

程式碼簽入GitHub之後，啟動GitHub Action流程，編譯並部署程式到Azure Container Apps，是開發系統時常見的功能需求。本篇範例協助開發人員使用GitHub與Azure Portal，逐步完成必要的設計和實作。

- 範例下載：[SleepZone.zip](https://mdpnetcore.github.io/MDP.NetCore/持續部署/使用Azure Portal，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/SleepZone.zip)


## 操作步驟

1.註冊並登入[Azure Portal](https://portal.azure.com/)。於首頁左上角的選單裡，點擊資源群組後，進入資源群組頁面。於資源群組頁面，點擊建立按鈕，依照頁面提示建立一個ResourceGroup，並命名為sleep-zone-group。

![01.建立ResourceGroup01.png](https://mdpnetcore.github.io/MDP.NetCore/持續部署/使用Azure Portal，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/01.建立ResourceGroup01.png)

![01.建立ResourceGroup02.png](https://mdpnetcore.github.io/MDP.NetCore/持續部署/使用Azure Portal，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/01.建立ResourceGroup02.png)

![01.建立ResourceGroup03.png](https://mdpnetcore.github.io/MDP.NetCore/持續部署/使用Azure Portal，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/01.建立ResourceGroup03.png)

2.建立完畢後，於ResourceGroup頁面的屬性頁籤，取得「資源識別碼」。

![01.建立ResourceGroup04.png](https://mdpnetcore.github.io/MDP.NetCore/持續部署/使用Azure Portal，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/01.建立ResourceGroup04.png)

![01.建立ResourceGroup05.png](https://mdpnetcore.github.io/MDP.NetCore/持續部署/使用Azure Portal，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/01.建立ResourceGroup05.png)

3.回到[Azure Portal](https://portal.azure.com/)。於右上角的選單裡，點擊Cloud Shell按鈕後，開啟Cloud Shell視窗。於Cloud Shell視窗，切換至Bash並執行下列指令，用來取得部署使用的「服務主體憑證」。該指令會建立名為sleep-zone-app-contributor的應用程式註冊(服務主體)，並授權它為sleep-zone-group資源群組的參與者(Contributor)角色。

```
az ad sp create-for-rbac \
    --name "sleep-zone-app-contributor" \
    --role "Contributor" \
    --scopes xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx \
    --sdk-auth
    
- 服務主體名稱：--name "sleep-zone-app-contributor"，可自訂。(sleep-zone-app-contributor為持續部署用的服務主體)
- 服務主體授權角色：--role "Contributor"。
- 服務主體授權範圍：--scopes xxxxxxxxxxxxxxx。(xxxxx為先前取得的資源群組-資源識別碼)
```

![02.建立Application01.png](https://mdpnetcore.github.io/MDP.NetCore/持續部署/使用Azure Portal，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/02.建立Application01.png)

![02.建立Application02.png](https://mdpnetcore.github.io/MDP.NetCore/持續部署/使用Azure Portal，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/02.建立Application02.png)

4.回到[Azure Portal](https://portal.azure.com/)。於首頁左上角的選單裡，點擊容器登錄後，進入容器登錄頁面。於容器登錄頁面，點擊建立按鈕，依照頁面提示建立一個ContainerRegistry，並命名為sleepzone。

![03.建立ContainerRegistry01.png](https://mdpnetcore.github.io/MDP.NetCore/持續部署/使用Azure Portal，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/03.建立ContainerRegistry01.png)

![03.建立ContainerRegistry02.png](https://mdpnetcore.github.io/MDP.NetCore/持續部署/使用Azure Portal，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/03.建立ContainerRegistry02.png)

![03.建立ContainerRegistry03.png](https://mdpnetcore.github.io/MDP.NetCore/持續部署/使用Azure Portal，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/03.建立ContainerRegistry03.png)

5.建立完畢後，於ContainerRegistry頁面的存取金鑰頁籤，開啟「管理使用者」功能。

![03.建立ContainerRegistry04.png](https://mdpnetcore.github.io/MDP.NetCore/持續部署/使用Azure Portal，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/03.建立ContainerRegistry04.png)

![03.建立ContainerRegistry05.png](https://mdpnetcore.github.io/MDP.NetCore/持續部署/使用Azure Portal，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/03.建立ContainerRegistry05.png)

6.回到Azure Portal。於右上角的選單裡，點擊Cloud Shell按鈕後，開啟Cloud Shell視窗。於Cloud Shell視窗，切換至Bash並執行下列指令，用來佈署QuickStart映像檔到ContainerRegistry給後續流程使用。

```
az acr import \
    --name sleepzone \
    --source mcr.microsoft.com/k8se/quickstart:latest \
    --image sleep-zone-app:latest
    
- 容器登錄名稱：--name sleepzone。(sleepzone為先前建立的容器登錄名稱)
- 來源映像名稱：--source mcr.microsoft.com/k8se/quickstart:latest，固定值。(QuickStart為微軟官方的映像檔)
- 容器映像名稱：--image sleep-zone-app:latest，可自訂，限制使用英文小寫與「-」，格式為image-name:tag-name。
```

![03.建立ContainerRegistry06.png](https://mdpnetcore.github.io/MDP.NetCore/持續部署/使用Azure Portal，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/03.建立ContainerRegistry06.png)

7.回到[Azure Portal](https://portal.azure.com/)。於首頁左上角的選單裡，點擊容器應用程式後，進入容器應用程式頁面。於容器應用程式頁面，點擊建立按鈕，依照頁面提示建立一個ContainerApp，命名為sleep-zone-app、並於容器頁簽設定使用先前建立的容器登錄、於輸入頁簽設定啟用輸入。

![04.建立ContainerApp01.png](https://mdpnetcore.github.io/MDP.NetCore/持續部署/使用Azure Portal，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/04.建立ContainerApp01.png)

![04.建立ContainerApp02.png](https://mdpnetcore.github.io/MDP.NetCore/持續部署/使用Azure Portal，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/04.建立ContainerApp02.png)

![04.建立ContainerApp03.png](https://mdpnetcore.github.io/MDP.NetCore/持續部署/使用Azure Portal，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/04.建立ContainerApp03.png)

![04.建立ContainerApp04.png](https://mdpnetcore.github.io/MDP.NetCore/持續部署/使用Azure Portal，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/04.建立ContainerApp04.png)

![04.建立ContainerApp05.png](https://mdpnetcore.github.io/MDP.NetCore/持續部署/使用Azure Portal，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/04.建立ContainerApp05.png)

8.註冊並登入[GitHub Dashboard](https://github.com/)。點擊首頁左上角的New按鈕，依照頁面提示建立一個Repository，並命名為SleepZone。

![05.建立Repository01.png](https://mdpnetcore.github.io/MDP.NetCore/持續部署/使用Azure Portal，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/05.建立Repository01.png)

![05.建立Repository02.png](https://mdpnetcore.github.io/MDP.NetCore/持續部署/使用Azure Portal，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/05.建立Repository02.png)

9.建立完畢後，於Repository頁面，點擊右上角的Setting按鈕進入Setting頁面，並選擇左側選單裡Secrets and variables的Action頁籤。於Action頁籤，點選New repository secret按鈕，依照頁面提示建立一個名為AZURE_CREDENTIALS的Actions Secret，Secret內容則是填入先前取得的「服務主體憑證」。

![05.建立Repository03.png](https://mdpnetcore.github.io/MDP.NetCore/持續部署/使用Azure Portal，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/05.建立Repository03.png)

![05.建立Repository04.png](https://mdpnetcore.github.io/MDP.NetCore/持續部署/使用Azure Portal，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/05.建立Repository04.png)

![05.建立Repository05.png](https://mdpnetcore.github.io/MDP.NetCore/持續部署/使用Azure Portal，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/05.建立Repository05.png)

10.Clone SleepZone Repository到本機Repository資料夾，於本機Repository資料夾裡建立src資料夾。開啟命令提示字元進入src資料夾，輸入下列指令，用以安裝MDP.WebApp範本、並且建立一個名為WebApplication1的Web站台。

```
// 建立Web站台
dotnet new install MDP.WebApp
dotnet new MDP.WebApp -n WebApplication1

- 應用程式名稱：WebApplication1，可自訂，限制使用英文大小寫。
```

![06.建立WebApplication01.png](https://mdpnetcore.github.io/MDP.NetCore/持續部署/使用Azure Portal，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/06.建立WebApplication01.png)

![06.建立WebApplication02.png](https://mdpnetcore.github.io/MDP.NetCore/持續部署/使用Azure Portal，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/06.建立WebApplication02.png)

11.於本機Repository資料夾裡，進入src/WebApplication1資料夾，並加入Dockerfile。

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

- 應用程式名稱：WebApplication1，可自訂，限制使用英文大小寫。
```

![06.建立WebApplication03.png](https://mdpnetcore.github.io/MDP.NetCore/持續部署/使用Azure Portal，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/06.建立WebApplication03.png)

12.於本機Repository資料夾裡，建立.github\workflows資料夾，並加入azure-build-deployment.yml。

```
{% raw %}
name: azure-build-deployment

on:
  push:
    branches:
      - main

env:
  RESOURCE_GROUP_NAME: sleep-zone-group
  CONTAINER_APPS_NAME: sleep-zone-app 
  CONTAINER_REGISTRY_NAME: sleepzone
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
      
      - name: Deploy ContainerRegistry
        uses: azure/CLI@v1
        with:
          inlineScript: >-
            az acr build
            --registry ${{ env.CONTAINER_REGISTRY_NAME }} 
            --file ${{ env.DOCKER_FILE_PATH }} 
            --image ${{ env.CONTAINER_APPS_NAME }}:${{ github.sha }} .

      - name: Deploy ContainerApp
        uses: azure/CLI@v1
        with:
          inlineScript: >-
            az containerapp update
            --name ${{ env.CONTAINER_APPS_NAME }}
            --resource-group ${{ env.RESOURCE_GROUP_NAME }}
            --image ${{ env.CONTAINER_REGISTRY_NAME }}.azurecr.io/${{ env.CONTAINER_APPS_NAME }}:${{ github.sha }}
            
- Git分支名稱：main，要特別注意Repository裡的分支是 master or main。
- 資源群組名稱：RESOURCE_GROUP_NAME: sleep-zone-group。(sleep-zone-group為先前建立的資源群組名稱)
- 容器應用名稱：CONTAINER_APPS_NAME: sleep-zone-app，可自訂，限制使用英文小寫與「-」。
- 容器登錄名稱：CONTAINER_REGISTRY_NAME: sleepzone。(sleepzone為先前建立的容器登錄名稱)
- Dockerfile路徑：DOCKER_FILE_PATH: ./src/WebApplication1/Dockerfile，路徑區分大小寫，相對於Repository資料夾。
{% endraw %}
```

![06.建立WebApplication04.png](https://mdpnetcore.github.io/MDP.NetCore/持續部署/使用Azure Portal，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/06.建立WebApplication04.png)

13.簽入並推送本機Repository的變更，到遠端GitHub主機之後。回到[GitHub Dashboard](https://github.com/)，進入SleepZone Repository頁面。可以於Actions頁籤裡，可以看到GitHub Action流程已經觸發執行並成功完成部署。(需時約兩分鐘)

![07.執行結果01.png](https://mdpnetcore.github.io/MDP.NetCore/持續部署/使用Azure Portal，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/07.執行結果01.png)

14.回到[Azure Portal](https://portal.azure.com/)。於首頁左上角的選單裡，點擊容器應用程式後，進入容器應用程式頁面。於ContainerApp頁面的概觀頁籤，點擊「應用程式 URL」鏈結，在開啟的Browser視窗內，可以看到由WebApplication1程式碼所提供的Hello World。

![07.執行結果02.png](https://mdpnetcore.github.io/MDP.NetCore/持續部署/使用Azure Portal，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/07.執行結果02.png)

![07.執行結果03.png](https://mdpnetcore.github.io/MDP.NetCore/持續部署/使用Azure Portal，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/07.執行結果03.png)

![07.執行結果04.png](https://mdpnetcore.github.io/MDP.NetCore/持續部署/使用Azure Portal，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/07.執行結果04.png)

15.完成上述步驟之後，每次推送程式碼到GitHub，都會啟動GitHub Action流程，編譯並部署程式到Azure Container Apps。

![07.執行結果05.png](https://mdpnetcore.github.io/MDP.NetCore/持續部署/使用Azure Portal，開發一個從GitHub持續佈署到Azure Container Apps的Web站台/07.執行結果05.png)