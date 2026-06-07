export interface TodoResponse{
    id: number;
    title: string;
    description?: string;
    isCompleted: boolean;
    createdAt: string; // APIではDateTime型で扱っているが、フロントエンドでは文字列として受け取るためstring型にしている
    updatedAt: string;
}

export interface TodoCreateRequest{
    title: string;
    description?: string;
}

export interface TodoUpdateRequest{
    title :string;
    description?: string;
    isCompleted: boolean;
}