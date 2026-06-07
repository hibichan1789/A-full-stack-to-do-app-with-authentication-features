import {login} from "../../api/authApi";
import type {LoginRequest} from "../../types/auth";

const form = document.getElementById("loginForm") as HTMLFormElement;
const emailInput = document.getElementById("email") as HTMLInputElement;
const passwordInput = document.getElementById("password") as HTMLInputElement;

form.addEventListener("submit", async (e)=>{
    e.preventDefault(); // これをすることで、ページのリロードを防ぎ、JavaScriptでフォームの送信処理を行うことができる

    const data: LoginRequest = {
        email: emailInput.value,
        password: passwordInput.value,
    };

    try{
        const response = await login(data);

        // JWTを保存,実務でもlocalstorageに保存することがあるが、XSS攻撃のリスクがあるため、今後はセキュリティを考慮して保存方法を選択する必要がある
        localStorage.setItem("token", response.token);

        // ログイン成功後にTODOリストのページに遷移
        window.location.href = "/src/pages/todo/todo.html"; 
    }
    catch(error){
        alert("ログインに失敗しました。メールアドレスとパスワードを確認してください。");
        console.error(error);
    }
});