using BlazorApp1.Models;

namespace BlazorApp1.Services;

public class PromptServices(HttpClient httpClient)
{
    public async Task<List<Prompt>> ReadAllAsync() 
        => await httpClient.GetFromJsonAsync<List<Prompt>>("todo");
    
    public async Task<Prompt> ReadPromptAsync(string id)
        => await httpClient.GetFromJsonAsync<Prompt>($"todo/{id}");
 
    public async Task CreatePromptAsync(Prompt item)
        => await httpClient.PostAsJsonAsync("todo", item);

 
    public async Task UpdatePromptAsync(Prompt item)
        => await httpClient.PutAsJsonAsync($"todo/{item.Id}", item);
 
    public async Task DeletePromptAsync(string id)
        =>  await httpClient.DeleteAsync($"todo/{id}");
}