# JWT認証機能付きフルスタックTodoアプリ

PostgreSQLをDocker,ASP.NET Core APIとVite Vanilla TSはローカルで開発する  

## 技術選定理由
PostgreSQLのみをdockerで構成しているのは、DBをDockerで作ることで簡単にDBを作り直すことができるため、練習段階の自分に合っていると思った  
まだASP.NET CoreやTSの開発は練習段階のためDockerでフロントエンドやバックエンドも立ち上げると、手詰まりが起きそうなため、ローカル開発で慣れてきたらDockerでやってみようと思う  
認証はJWTを採用しているが、JWTはクライアント側でトークンを管理するため、セッション管理が不要であることや、スケーラビリティが高いことから選定した  


## githubからクローン後動作手順  
### postgreSQL
- cp .env.sample .env  
postgreSQLのための.envの環境変数を設定  
- docker compose config  
docker-compose.ymlに環境変数が反映されているか確認  
- docker compose up -d  
postgreSQLの起動  
- docker compose exec db bash
コンテナに入り込む  
- psql -U ユーザ名 -p ポート番号 -d DB名  
立ち上げたpostgreSQLデータベースに接続確認  

### ASP.NET Core API
- cd TodoAuthApi  
バックエンドのディレクトリに移動  
- dotnet restore
パッケージの復元  
- dotnet watch
APIの起動  
APIが起動したら、http://localhost:<ポート番号>/swagger/index.html"にアクセスして、APIのエンドポイントを確認できることを確認する  




## 苦労したところ
DockerでPostgreSQLを立ち上げてASP.NET Core APIと接続するときにポート番号が競合してしまっていたため環境構築に時間がかかった  
原因としては,おそらくローカル環境でポート番号5432で常駐しているプロセスがあったため、Dockerで立ち上げたPostgreSQLも同じポート番号を使用しようとして競合していたと思われる  
そこで、Dockerで立ち上げるPostgreSQLのポート番号をPostgreSQLで標準的5432から変更することで解決した  

Swaggerで認証の機能を作るときに、HS256で暗号化するが鍵の長さが足りないとエラーが出てしまうため、そこでエラーの原因を探すのに時間がかかった  
エラーが鍵の長さが足りないことを発見できたのは、break pointを使用して、1行ごとにコードを確認していったからである  

ASP.NET Core APIが"sub"をClaimTypes.NameIdentifierにマッピングしていたためFind("sub")がnullを返してしまっていたため、ユーザIDを取得できなかった  
しかしFind(ClaimTypes.NameIdentifier)に変更することで、ユーザIDを取得できるようになった  