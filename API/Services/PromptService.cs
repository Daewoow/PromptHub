using WebApplication1.Cache;

namespace WebApplication1.Services;

public class PromptService(CrudRepository repository, CacheService cacheService)
{
    public async Task CreateAsync(Prompt prompt)
    {
        await repository.Create(prompt);
        await cacheService.Invalidate($"User:all");
    }

    public async Task<IEnumerable<Prompt>> ReadAllAsync() => 
        await cacheService.GetOrAdd("User:all", async () => await repository.ReadAll(), 30);

    public async Task<Prompt> ReadByIdAsync(string id) 
        => await cacheService.GetOrAdd($"User:{id}", 
            async () => await repository.ReadById(id), 30);

    public async Task UpdateAsync(Prompt prompt)
    {
        await repository.Update(prompt);
        await cacheService.Invalidate($"User:{prompt.Id}");
        await cacheService.Invalidate($"User:all");
    }

    public async Task DeleteAsync(string id)
    {
        await repository.Delete(id);
        await cacheService.Invalidate($"User:{id}");
        await cacheService.Invalidate("User:all");
    }
}