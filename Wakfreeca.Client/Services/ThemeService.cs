using Wakfreeca.Client.Models;

namespace Wakfreeca.Client.Services;

public interface IThemeService
{
    public event EventHandler? UpdatedHandler;
    bool IsDarkMode { get; }
    Task SetDarkMode(bool isDarkMode);
    Task ApplyUserPreferences(bool isDarkModeDefaultTheme);
}

public class ThemeService() : IThemeService
{
    public event EventHandler? UpdatedHandler;
    public bool IsDarkMode { get; private set; }

    private readonly IPreferencesService _preferencesService = default!;
    private Preferences? _userPreferences;

    public ThemeService(IPreferencesService preferencesService) : this()
    {
        _preferencesService = preferencesService;
    }

    public async Task SetDarkMode(bool isDarkMode)
    {
        IsDarkMode = isDarkMode;
        await _preferencesService.SavePreferences(_userPreferences!);
        OnUpdateHandler();
    }

    public async Task ApplyUserPreferences(bool isDarkModeDefaultTheme)
    {
        _userPreferences = await _preferencesService.LoadPreferences();
        if (_userPreferences != null)
        {
            IsDarkMode = _userPreferences.IsDarkMode;
        }
        else
        {
            IsDarkMode = isDarkModeDefaultTheme;
            _userPreferences = new() { IsDarkMode = IsDarkMode };
            await _preferencesService.SavePreferences(_userPreferences);
        }
    }

    private void OnUpdateHandler() => UpdatedHandler?.Invoke(this, EventArgs.Empty);
}