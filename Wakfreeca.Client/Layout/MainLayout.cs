using Microsoft.AspNetCore.Components;
using MudBlazor;
using Wakfreeca.Client.Services;

namespace Wakfreeca.Client.Layout;

public partial class MainLayout : LayoutComponentBase, IDisposable
{
    [Inject] private IThemeService _themeService { get; set; } = default!;

    private MudThemeProvider _themeProvider = default!;
    private string _icon = Icons.Material.Rounded.AutoMode;

    private bool _systemThemeMode;

    protected override void OnInitialized()
    {
        _themeService.UpdatedHandler += ThemeServiceOnUpdatedHandler;

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

    private async Task SetLightMode()
    {
        _icon = Icons.Material.Rounded.LightMode;
        await _themeService.SetDarkMode(false);
    }

    private async Task SetDarkMode()
    {
        _icon = Icons.Material.Rounded.DarkMode;
        await _themeService.SetDarkMode(true);
    }

    private async Task SetSystemMode()
    {
        _icon = Icons.Material.Rounded.AutoMode;
        await _themeService.SetDarkMode(_systemThemeMode);
    }

    private async Task ApplyUserPreferences()
    {
        _systemThemeMode = await _themeProvider.GetSystemPreference();
        await _themeService.ApplyUserPreferences(_systemThemeMode);
        await _themeService.SetDarkMode(_themeService.IsDarkMode);
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