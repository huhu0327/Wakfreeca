using Microsoft.AspNetCore.Components;
using MudBlazor;
using Wakfreeca.Client.Services;

namespace Wakfreeca.Client.Layout;

public partial class MainLayout : LayoutComponentBase, IDisposable
{
    public enum ThemeMode
    {
        LightMode,
        DarkMode,
        SystemMode
    }
    
    [Inject] private IThemeService _themeService { get; set; } = default!;
    
    private MudThemeProvider _themeProvider = default!;
    private string _icon = default!;
    private string _toolTip = default!;
    
    private ThemeMode _mode;
    private bool _systemThemeMode;
    private IDictionary<ThemeMode, Func<Task>> _modeActions = default!;

    protected override void OnInitialized()
    {
        _themeService.UpdatedHandler += ThemeServiceOnUpdatedHandler;
        _modeActions = new Dictionary<ThemeMode, Func<Task>>()
        {
            { ThemeMode.LightMode , async () => await SetLightMode() },
            { ThemeMode.DarkMode , async () => await SetDarkMode() },
            { ThemeMode.SystemMode , async () => await SetSystemMode() },
        };
        
        base.OnInitialized();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            await ApplyUserPreferences();
        }
    }

    private async Task ToggleThemeMode()
    {
        _mode = (ThemeMode)((int)(_mode + 1) % 3);
        await _modeActions[_mode].Invoke();
    }

    private async Task SetLightMode()
    {
        _icon = Icons.Material.Rounded.DarkMode;
        await _themeService.SetDarkMode(false);
        _toolTip = "Dark Mode로 변경";
    }
    private async Task  SetDarkMode()
    {
        _icon = Icons.Material.Rounded.AutoMode;
        await _themeService.SetDarkMode(true);
        _toolTip = "Sytem Mode로 변경";
    }
    private async Task  SetSystemMode()
    {
        _icon = Icons.Material.Rounded.LightMode;
        await _themeService.SetDarkMode(_systemThemeMode);
        _toolTip = "Light Mode로 변경";
    }

    private async Task ApplyUserPreferences()
    {
        _systemThemeMode = await _themeProvider.GetSystemPreference();
        await _themeService.ApplyUserPreferences(_systemThemeMode);
        
        _mode = (ThemeMode)Convert.ToInt32(_themeService.IsDarkMode);
        await _modeActions[_mode].Invoke();
    }

    private void ThemeServiceOnUpdatedHandler(object? sender, EventArgs e)
    {
        StateHasChanged();
    }

    public void Dispose()
    {
        _themeService.UpdatedHandler -= ThemeServiceOnUpdatedHandler;
    }
}