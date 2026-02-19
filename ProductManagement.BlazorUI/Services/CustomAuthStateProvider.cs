using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace ProductManagement.BlazorUI.Services;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly HttpClient _httpClient;

    public CustomAuthStateProvider(
        ILocalStorageService localStorage,
        HttpClient httpClient)
    {
        _localStorage = localStorage;
        _httpClient = httpClient;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _localStorage.GetItemAsync<string>("authToken");

        if (string.IsNullOrWhiteSpace(token))
            return new AuthenticationState(
                new ClaimsPrincipal(new ClaimsIdentity()));

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        var identity = new ClaimsIdentity(jwt.Claims, "jwt");

        return new AuthenticationState(
            new ClaimsPrincipal(identity));
    }

    public async Task MarkUserAsAuthenticated(string token)
    {
        await _localStorage.SetItemAsync("authToken", token);

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        var identity = new ClaimsIdentity(jwt.Claims, "jwt");

        NotifyAuthenticationStateChanged(
            Task.FromResult(
                new AuthenticationState(
                    new ClaimsPrincipal(identity))));
    }

    public async Task MarkUserAsLoggedOut()
    {
        await _localStorage.RemoveItemAsync("authToken");

        _httpClient.DefaultRequestHeaders.Authorization = null;

        NotifyAuthenticationStateChanged(
            Task.FromResult(
                new AuthenticationState(
                    new ClaimsPrincipal(
                        new ClaimsIdentity()))));
    }
}
