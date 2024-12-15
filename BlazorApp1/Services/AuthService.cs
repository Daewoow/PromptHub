using System.Net.Http.Headers;
using BlazorApp1.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.JSInterop;

namespace BlazorApp1.Services;

public class AuthService(HttpClient httpClient, IJSRuntime jsRuntime)
{
    private const string TokenKey = "jwtToken";

    public async Task<bool> LoginAsync(string username, string password)
    {
        var credentials = new RegisterModel { Username = username, Password = password };
        
        httpClient.BaseAddress = new Uri("http://localhost:5160/");

        var response = await httpClient.PostAsJsonAsync("api/auth/login", credentials);
        
        if (!response.IsSuccessStatusCode)
        {
            var er = await response.Content.ReadAsStringAsync();
            return false;
        }
        
        var result = await response.Content.ReadFromJsonAsync<JwtResponse>();

        if (result?.Token == null) 
            return false;
        
        await jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenKey, result.Token);
        
        Console.WriteLine(result.Token);

        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, result.Token);
        
        Console.WriteLine($"Authorization Header Set: {httpClient.DefaultRequestHeaders.Authorization}");

        return true;
    }

    public async Task LogoutAsync()
    {
        await jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);
        httpClient.DefaultRequestHeaders.Authorization = null;
    }

    public async Task<string?> GetTokenAsync() 
        => await jsRuntime.InvokeAsync<string?>("localStorage.getItem", TokenKey);

    private class JwtResponse
    {
        public string Token { get; set; }
    }
}