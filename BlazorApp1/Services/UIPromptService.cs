using BlazorApp1.Models;

namespace BlazorApp1.Services;

public class UIPromptService(HttpClient httpClient)
{
    public async Task<List<UIPrompt>> ReadAllAsync() 
        => await httpClient.GetFromJsonAsync<List<UIPrompt>>("todo");
    
    public async Task<UIPrompt> ReadPromptAsync(int id)
        => await httpClient.GetFromJsonAsync<UIPrompt>($"todo/{id}");
 
    public async Task CreatePromptAsync(UIPrompt item)
        => await httpClient.PostAsJsonAsync("todo", item);
 
    public async Task UpdatePromptAsync(UIPrompt item)
        => await httpClient.PutAsJsonAsync($"todo/{item.PromptId}", item);
 
    public async Task DeletePromptAsync(int id)
        =>  await httpClient.DeleteAsync($"todo/{id}");
}