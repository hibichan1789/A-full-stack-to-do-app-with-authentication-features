import {register} from "../../api/authApi";
import type {RegisterRequest} from "../../types/auth";

const form = document.getElementById("registerForm") as HTMLFormElement;
const userNameInput = document.getElementById("userName") as HTMLInputElement;
const emailInput = document.getElementById("email") as HTMLInputElement;
const passwordInput = document.getElementById("password") as HTMLInputElement;

form.addEventListener("submit", async (e)=> {
    e.preventDefault(); // これをすることで、ページのリロードを防ぎ、JavaScriptでフォームの送信処理を行うことができる

    const data: RegisterRequest = {
        userName: userNameInput.value,
        email: emailInput.value,
        password: passwordInput.value,
    };

    try{
        const response = await register(data);

        window.alert("登録が成功しました。ログインページに遷移します。");

        // 登録成功後にログインページに遷移
        window.location.href = "/src/pages/login/login.html"; 
    }
    catch(error){
        alert("登録に失敗しました。入力内容を確認してください。");
        console.error(error);
    }
});