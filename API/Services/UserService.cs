using WebApplication1.Cache;

namespace WebApplication1.Services;

public class UserService(UsersCrudRepository repository, CacheService cacheService)
{
    public async Task CreateAsync(User user)
    {
        await repository.CreateUser(user);
        await cacheService.Invalidate($"User:all");
    }

    public async Task<IEnumerable<User>> ReadAllAsync() => 
        await cacheService.GetOrAdd("User:all", async () => await repository.ReadAllUsers(), 30);

    public async Task<User> ReadByIdAsync(int id) 
        => await cacheService.GetOrAdd($"User:{id}", 
            async () => await repository.ReadUserById(id), 30);

    public async Task<User> ReadByNameAsync(string userName) => await cacheService.GetOrAdd($"User: {userName}",
        async () => await repository.ReadUserByName(userName), 30);

    public async Task UpdateAsync(User user)
    {
        await repository.UpdateUser(user);
        await cacheService.Invalidate($"User:{user.UserId}");
        await cacheService.Invalidate($"User:all");
    }

    public async Task DeleteAsync(int id)
    {
        await repository.DeleteUser(id);
        await cacheService.Invalidate($"User:{id}");
        await cacheService.Invalidate("User:all");
    }
}