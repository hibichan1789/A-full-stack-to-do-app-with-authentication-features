import {api} from "./axiosClient";
import type {
    TodoResponse,
    TodoCreateRequest,
    TodoUpdateRequest
} from "../types/todo";

// Todo一覧取得
export async function getTodos():Promise<TodoResponse[]>{
    const response = await api.get<TodoResponse[]>("/todo");
    return response.data;
};

// Todo作成
export async function createTodo(data: TodoCreateRequest):Promise<TodoResponse>{
    const response = await api.post<TodoResponse>("/todo", data);
    return response.data;
};

// Todo更新
export async function updateTodo(id: number, data: TodoUpdateRequest):Promise<TodoResponse>{
    const response = await api.put<TodoResponse>(`/todo/${id}`, data);
    return response.data;
}

// Todo削除
export async function deleteTodo(id: number):Promise<void>{
    await api.delete(`/todo/${id}`);
}