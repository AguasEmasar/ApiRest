namespace LOGIN.Services.Interfaces
{
    public interface IAPiSubscriberServices
    {
        Task<string> GetCommentAsync(string clave);
        Task<string> GetHistoryAsync(string clave);
        Task<string> GetUserAsync(string clave);
    }
}