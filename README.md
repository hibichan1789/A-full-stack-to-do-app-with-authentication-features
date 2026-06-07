# JWT認証機能付きフルスタックTodoアプリ

PostgreSQLをDocker,ASP.NET Core APIとVite Vanilla TSはローカルで開発する 
StyleはTailwind CSSを採用 

## 技術選定理由
PostgreSQLのみをdockerで構成しているのは、DBをDockerで作ることで簡単にDBを作り直すことができるため、練習段階の自分に合っていると思った  
まだASP.NET CoreやTSの開発は練習段階のためDockerでフロントエンドやバックエンドも立ち上げると、手詰まりが起きそうなため、ローカル開発で慣れてきたらDockerでやってみようと思う  
認証はJWTを採用しているが、JWTはクライアント側でトークンを管理するため、セッション管理が不要であることや、スケーラビリティが高いことから選定した  
API(ASP.NET Core) + フロントエンド(Vite,TS) + 複数HTMLは、MVCよりページ遷移や動作が早く、開発もフロントエンドだけに集中して開発できる　　
Viteを使うことで、ホットリロードが効くため開発がスムーズになる　　
tailwind cssを導入することでcssファイルを開くことなく、HTML,Typescriptファイルに直接スタイルを書き込めるのでUI調整がしやすい  

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

### フロントエンドVite
- cd TodoFrontend
フロントエンドのディレクトリに移動  
- vite run dev  
フロントエンドの起動  
- http://localhost:5173/src/pages/login/login.html  
ログイン画面に移動  



## 苦労したところ
DockerでPostgreSQLを立ち上げてASP.NET Core APIと接続するときにポート番号が競合してしまっていたため環境構築に時間がかかった  
原因としては,おそらくローカル環境でポート番号5432で常駐しているプロセスがあったため、Dockerで立ち上げたPostgreSQLも同じポート番号を使用しようとして競合していたと思われる  
そこで、Dockerで立ち上げるPostgreSQLのポート番号をPostgreSQLで標準的5432から変更することで解決した  

Swaggerで認証の機能を作るときに、HS256で暗号化するが鍵の長さが足りないとエラーが出てしまうため、そこでエラーの原因を探すのに時間がかかった  
エラーが鍵の長さが足りないことを発見できたのは、break pointを使用して、1行ごとにコードを確認していったからである  

ASP.NET Core APIが"sub"をClaimTypes.NameIdentifierにマッピングしていたためFind("sub")がnullを返してしまっていたため、ユーザIDを取得できなかった  
しかしFind(ClaimTypes.NameIdentifier)に変更することで、ユーザIDを取得できるようになった  　

Corsエラーが最初に出たが、ASP.NET Core APIのprogram.csにCorsの設定を追加してフロントエンドのURLからのアクセスを許可することで、Corsエラーの解消ができた  
また最初に何となく5秒で決めていたTimeOutではAPIとの通信がうまくいかなかったが、TimeOut10秒に設定することで通信することができた  