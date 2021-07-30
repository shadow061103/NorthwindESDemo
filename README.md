# NorthwindESDemo
### 前置作業
1.在專案跟目錄執行powershell或cmd
```
//建elasticsearch
docker-compose up -d

docker run --name redis-lab -p 6379:6379 -d redis
```
2.打開SSMS使用localDB新增查詢，執行Database\script.sql 建立Northwind資料庫
3.新增Hangfire資料庫
## 專案說明
### Hangfire
- 建立一個定時啟動的排程工作
- 會用ED core把資料從DB撈回來
- 上傳到elasticsearch
```
本機ES網址
http://127.0.0.1:9200/
本機kibana網址
http://127.0.0.1:5601/app/kibana#/home?_g=()
```
### API
- 有做兩個api分別是用編號取Orders跟用條件取得
- 有做快取裝飾者設定，需安裝scrutor套件並在startup.cs設定
- 快取使用記憶體快取跟redis快取
- 依序會使用Memory > Redis > ES
- Redis狀態可以用[Another Desktop Manager](https://github.com/qishibo/AnotherRedisDesktopManager)查看
