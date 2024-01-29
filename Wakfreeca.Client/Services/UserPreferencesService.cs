using Blazored.LocalStorage;
using Wakfreeca.Client.Models;

namespace Wakfreeca.Client.Services;

public interface IUserPreferencesService
{
    public Task SaveUserPreferences(UserPreferences userPreferences);
    public Task<UserPreferences?> LoadUserPreferences();
}

public class UserPreferencesService(ILocalStorageService localStorageService) : IUserPreferencesService
{
    private const string _key = "userPreferences";

    public async Task SaveUserPreferences(UserPreferences userPreferences)
    {
        await localStorageService.SetItemAsync(_key, userPreferences);
    }

    public async Task<UserPreferences?> LoadUserPreferences()
    {
        return await localStorageService.GetItemAsync<UserPreferences>(_key);
    }
}