export interface LoginRequest {
    email: string;
    password: string;
}

export interface LoginResponse {
    token: string;
}

export interface RegisterRequest {
    email: string;
    password: string;
    userName: string;
}

export interface RegisterResponse{
    id: number;
    email: string;
    userName: string;
}
// Typescriptのinterfaceは型の形を表すだけでAPIのDTOと大文字小文字の違いがあっても差異を吸収してくれるため、APIのDTOと同じ形で定義する必要はない
