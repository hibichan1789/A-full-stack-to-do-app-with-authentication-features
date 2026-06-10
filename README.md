# JWT認証機能付きフルスタックTodoアプリ、要約機能付き

PostgreSQLをDocker,ASP.NET Core APIとVite Vanilla TSはローカルで開発する 
StyleはTailwind CSSを採用 
ASP.NET Core APIからAzure Functions経由してpythonからOpenAI APIを呼び出しキーワード抽出機能の実装  
ASP.NET Core APIから直接OpenAI APIを呼び出して、azure functions経由とASP.NET Core APIとの実行速度検証用エンドポイントの実装  

## 技術選定理由
- PostgreSQL
DB を簡単に作り直せるため、学習段階で扱いやすい  
アプリ本体はローカルで動かし、DB だけ Docker 化することで開発の複雑さを抑えた  
- ASP.NET Core API + Vite + TypeScript
API とフロントを完全分離し、開発効率を向上  
Vite により高速なホットリロードが可能  
Tailwind CSS により HTML/TS 内で直接スタイル調整ができる  
- JWT認証
セッション管理不要でステートレス  
- tailwind css
HTML,Typescriptファイルに直接スタイルを書き込めるためUI調整がしやさと開発効率の向上  
- Azure Functions(Python)
要約処理は負荷が変動しやすいため、API 本体から切り離してサーバーレスで実行  
Python で柔軟に AI ロジックを記述できる  
Pydantic により ASP.NET Core と型安全に連携  
curl で即テストできるため開発が高速  
- ASP.NET Core APIから直接OpenAI APIを叩く方式
Functions 経由との速度比較のため  
C# の OpenAI SDK を利用し、最適な方式を検証できるようにした  

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

### azure functions
- 拡張機能のazure functions,azuriteをインストールする  
- cd azureFunctions
- cp local.settings.sample.json local.settings.json  
local.setting.jsonに必要な環境変数の設定を行う
- python -m venv .venv
- .venv\Scripts\activate
仮想環境の起動  
- pip install -r requirements.txt
モジュールのインストール  
- ctrl + shift + p → azurite: Start
azuriteの起動
- func start  
azure functionsの起動  
- curl -X POST "http://localhost:7071/api/summary" -H "Content-Type: application/json" -d @requestSample.json  
curl コマンドで動作確認

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
- npm run dev  
フロントエンドの起動  
- http://localhost:5173/src/pages/login/login.html  
ログイン画面に移動  


## Azure Functions経由とASP.NET Core APIから直接OpenAI APIを叩いたときの実行速度比較
### 条件
入力文: "今日はTodoアプリにAI要約機能を追加するためにAzure FunctionsとOpenAI APIの連携を実装した。Pythonで要約処理を書き、ASP.NET Core APIから呼び出せるようにした。今後はAPI直叩き版との比較も行う予定。"   
システムプロンプト:  
"以下の文章から重要なキーワードを3〜5個抽出したリストを作成してください  
出力例:  
キーワード: A, B, C "  
temperature: 0.3 (正確性を重視)  
max tokens: 120(AIがテキストを処理する最小の単位)  
繰り返し回数: 10回
- 結果
### azure functions
```json
{
  "result": {  
    "averageMs": 1331.82674,  
    "minMs": 833.4219,  
    "maxMs": 4433.4166,  
    "allResults": [  
      4433.4166,  
      1004.2188,  
      909.7792,  
      897.5485,  
      854.6527,  
      833.4219,  
      1109.1574,  
      1016.6179,  
      919.263,  
      1340.1914  
    ]  
  }  
}
```
### ASP.NET Core APIから直接
```json
{  
  "result": {  
    "averageMs": 964.9940200000001,  
    "minMs": 724.3702,  
    "maxMs": 1142.7229,  
    "allResults": [  
      848.5827,  
      1142.7229,  
      1107.6956,  
      921.1013,  
      1127.0905,  
      825.2128,  
      724.3702,  
      1113.2062,  
      1022.3147,  
      817.6433  
    ]  
  }  
}  
```
### 結果まとめ
|項目|Azure Functions(Python)経由|ASP.NET Core APIから直接|
|---|---|---|
|平均(ms)|1331.8|965.0|
|最大(ms)|4433.4|1142.7|
|最小(ms)|833.4|724.3|
### 結果考察
Azure Functions は コールドスタート により初回実行が約4秒と遅い  
しかし 2 回目以降は ASP.NET Core API とほぼ同等の速度  
Direct 方式は常に安定して速いが、API 側の負荷が増える  
Functions 方式は API の負荷を分散でき、AI 処理の責務分離ができるため、スケール性・保守性の観点で有用  

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

Azure FunctionsとAPIの通信ときにJSONが空で送られてしまう問題が発生した、原因はわからないがPostAsJsonAsync()メソッドを使うと空のJSONになってしまったため、ヘッダとContentを一から入れるHttpRequestMessageを用いることでJSONが空になる問題を解消した  
しかしJSONを送れたはいいもののJSONのプロパティの大文字小文字がAPIとAzure Functions内のpythonで差異があり処理ができなかった,そのためAPIのDTOクラスのプロパティにJsonPropertyName属性を付与して、APIとAzure Functions内のpythonでプロパティの大文字小文字の差異を吸収して処理を行うことができるように修正をした　　