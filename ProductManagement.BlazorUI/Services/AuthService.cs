using Blazored.LocalStorage;
using ProductManagement.Application.Features.Auth.DTOs;
using System.Net.Http.Json;

namespace ProductManagement.BlazorUI.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly CustomAuthStateProvider _authStateProvider;

    public AuthService(
        HttpClient httpClient,
        ILocalStorageService localStorage,
        CustomAuthStateProvider authStateProvider)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
        _authStateProvider = authStateProvider;
    }

    public async Task<bool> LoginAsync(LoginRequestDto model)
    {
        var response = await _httpClient
            .PostAsJsonAsync("api/auth/login", model);

        if (!response.IsSuccessStatusCode)
            return false;

        var result = await response
            .Content.ReadFromJsonAsync<AuthResponseDto>();

        if (result == null)
            return false;

        await _localStorage.SetItemAsync("authToken", result.AccessToken);
        await _localStorage.SetItemAsync("refreshToken", result.RefreshToken);

        await _authStateProvider
            .MarkUserAsAuthenticated(result.AccessToken);

        return true;
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync("authToken");
        await _localStorage.RemoveItemAsync("refreshToken");

        await _authStateProvider.MarkUserAsLoggedOut();
    }

    public async Task<bool> RegisterAsync(RegisterRequestDto model)
    {
        var response = await _httpClient
            .PostAsJsonAsync("api/auth/register", model);

        return response.IsSuccessStatusCode;
    }

}
