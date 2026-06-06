# JWT認証機能付きフルスタックTodoアプリ

PostgreSQLをDocker,ASP.NET Core APIとVite Vanilla TSはローカルで開発する  

## 技術選定理由
PostgreSQLのみをdockerで構成しているのは、DBをDockerで作ることで簡単にDBを作り直すことができるため、練習段階の自分に合っていると思った  
まだASP.NET CoreやTSの開発は練習段階のためDockerでフロントエンドやバックエンドも立ち上げると、手詰まりが起きそうなため、ローカル開発で慣れてきたらDockerでやってみようと思う  


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