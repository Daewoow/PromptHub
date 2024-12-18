using WebApplication1.Cache;

namespace WebApplication1.Services;

public class PromptService(PromptsCrudRepository repository, CacheService cacheService)
{
    public async Task CreateAsync(Prompt prompt)
    {
        await repository.CreatePrompt(prompt);
        await cacheService.Invalidate($"Prompt:all");
    }

    public async Task<IEnumerable<Prompt>> ReadAllAsync() => 
        await cacheService.GetOrAdd("Prompt:all", async () => await repository.ReadAllPrompts(), 30);

    public async Task<Prompt> ReadByIdAsync(int id) 
        => await cacheService.GetOrAdd($"Prompt:{id}", 
            async () => await repository.ReadPromptById(id), 30);

    public async Task UpdateAsync(Prompt prompt)
    {
        await repository.UpdatePrompt(prompt);
        await cacheService.Invalidate($"Prompt:{prompt.PromptId}");
        await cacheService.Invalidate($"Prompt:all");
    }

    public async Task DeleteAsync(int id)
    {
        await repository.DeletePrompt(id);
        await cacheService.Invalidate($"Prompt:{id}");
        await cacheService.Invalidate("Prompt:all");
    }

    public async Task<string> ReadDescriptionByIdAsync(int id)
    {
        return await repository.ReadPromptDescriptionById(id);
    }
}