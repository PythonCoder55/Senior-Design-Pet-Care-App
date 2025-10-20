namespace Senior_Design_Pet_Care_App.Services
{
    public interface ICustomSessionService
    {
        Task<string> GetItemAsStringAsync(string key);
        Task SetItemAsStringAsync(string key, string value);
        Task RemoveItemAsync(string key);
    }
}
