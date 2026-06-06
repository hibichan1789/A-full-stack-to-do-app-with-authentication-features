namespace TodoAuthApi.Services.UserService
{
    public interface ILoginService
    {
        // ログインに失敗したらステータスコード401を返す
        Task<string> LoginAsync(string email, string password);
    }
}
