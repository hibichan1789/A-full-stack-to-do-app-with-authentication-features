import {api} from "./axiosClient";
import type {
    LoginRequest,
    LoginResponse,
    RegisterRequest,
    RegisterResponse
} from "../types/auth";

// axiosClient.tsからAPIクライアントを持ってくることで、baseURLやJWTを書かなくてもよくなる
export async function login(data: LoginRequest): Promise<LoginResponse> {
    const response = api.post<LoginResponse>("/user/login", data); //ジェネリクスで型を指定することでdataに型が付く 第一引数のURLはbaseURLからの相対パスを指定するだけでよい
    return await response.then(res => res.data);
}

export async function register(data: RegisterRequest): Promise<RegisterResponse> {
    const response = api.post<RegisterResponse>("/user/register", data); //ジェネリクスで型を指定することでdataに型が付く 第一引数のURLはbaseURLからの相対パスを指定するだけでよい
    return await response.then(res => res.data);
}