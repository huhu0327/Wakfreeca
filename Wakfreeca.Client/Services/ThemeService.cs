using Wakfreeca.Client.Models;

namespace Wakfreeca.Client.Services;

public interface IThemeService
{
    public event EventHandler? UpdatedHandler;
    bool IsDarkMode { get; }
    Task SetDarkMode(bool isDarkMode);
    Task ApplyUserPreferences(bool isDarkModeDefaultTheme);
    // Task ToggleThemeMode();
}

public class ThemeService() : IThemeService
{
    public event EventHandler? UpdatedHandler;
    public bool IsDarkMode { get; private set; }

    private readonly IUserPreferencesService _userPreferencesService = default!;
    private UserPreferences? _userPreferences;

    public ThemeService(IUserPreferencesService userPreferencesService) : this()
    {
        _userPreferencesService = userPreferencesService;
    }

    public async Task SetDarkMode(bool isDarkMode)
    {
        IsDarkMode = isDarkMode;
        await _userPreferencesService.SaveUserPreferences(_userPreferences!);
        OnUpdateHandler();
    }

    public async Task ApplyUserPreferences(bool isDarkModeDefaultTheme = false)
    {
        _userPreferences = await _userPreferencesService.LoadUserPreferences();
        if (_userPreferences != null)
        {
            IsDarkMode = _userPreferences.IsDarkMode;
        }
        else
        {
            IsDarkMode = isDarkModeDefaultTheme;
            _userPreferences = new() { IsDarkMode = IsDarkMode };
            await _userPreferencesService.SaveUserPreferences(_userPreferences);
        }
    }

    // public async Task ToggleThemeMode()
    // {
    //     IsDarkMode = !IsDarkMode;
    //     _userPreferences!.IsDarkMode = IsDarkMode;
    //     
    // }

    private void OnUpdateHandler() => UpdatedHandler?.Invoke(this, EventArgs.Empty);
}