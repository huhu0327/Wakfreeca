using Blazored.LocalStorage;
using Wakfreeca.Client.Models;

namespace Wakfreeca.Client.Services;

public interface IPreferencesService
{
    Task SavePreferences(Preferences preferences);
    Task<Preferences?> LoadPreferences();
}

public class PreferencesService(ILocalStorageService localStorageService) : IPreferencesService
{
    private const string _key = "userPreferences";

    public async Task SavePreferences(Preferences preferences)
    {
        await localStorageService.SetItemAsync(_key, preferences);
    }

    public async Task<Preferences?> LoadPreferences()
    {
        return await localStorageService.GetItemAsync<Preferences>(_key);
    }
}