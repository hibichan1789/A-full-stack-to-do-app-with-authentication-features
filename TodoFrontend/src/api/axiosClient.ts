import axios from "axios";

// axiosインスタンスの作成
// axios.create()は自分専用にカスタマイズしたインスタンスを作成するメソッド
export const api = axios.create({
    baseURL: "http://localhost:5175/api", // ここにASP.NET Core APIのURLのベースURLのして
    timeout: 10*1000,// タイムアウト時間（ミリ秒）
});

// リクエスト前にJWTを自動で付与
// axios.interceptors()はaxiosの通信の前後に割り込んで処理を追加できる機能
// tokenがあればAuthorizationヘッダーにBearerトークンをセットするようにしている
api.interceptors.request.use(
    (config) => {
        const token = localStorage.getItem("token");
        if(token){
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    }
    , (error) => {
        return Promise.reject(error);
    }
);

// 共通エラーハンドリング
api.interceptors.response.use(
    (response) => response,
    (error) => {
        console.error("API Error:", error.response?.data || error.message);
        return Promise.reject(error);
    }
);