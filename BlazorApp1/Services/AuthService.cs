using System.Net.Http.Headers;
using BlazorApp1.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.JSInterop;

namespace BlazorApp1.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;
    private const string TokenKey = "jwtToken";

    public AuthService(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
        _httpClient.BaseAddress = new Uri("http://localhost:5160/");
    }

    public async Task<bool> LoginAsync(string username, string password)
    {
        var credentials = new RegisterModel { Username = username, Password = password };

        var response = await _httpClient.PostAsJsonAsync("api/auth/login", credentials);
        
        if (!response.IsSuccessStatusCode)
        {
            var er = await response.Content.ReadAsStringAsync();
            return false;
        }
        
        var result = await response.Content.ReadFromJsonAsync<JwtResponse>();

        if (result?.Token == null) 
            return false;
        
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenKey, result.Token);
        
        Console.WriteLine(result.Token);

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, result.Token);
        
        Console.WriteLine($"Authorization Header Set: {_httpClient.DefaultRequestHeaders.Authorization}");

        return true;
    }

    public async Task LogoutAsync()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    public async Task<string?> GetTokenAsync() 
        => await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", TokenKey);

    private class JwtResponse
    {
        public string Token { get; set; }
    }
}