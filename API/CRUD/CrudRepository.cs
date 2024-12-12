using Raven.Client.Documents;

namespace WebApplication1;

public class CrudRepository(IDocumentStore store)
{
    public async Task Create(Prompt prompt)
    {
        using var session = store.OpenAsyncSession();

        var existingPrompts = await session.Query<Prompt>().ToListAsync();
        var newId = existingPrompts.Any() 
            ? Enumerable.Range(1, int.MaxValue).Except(existingPrompts
                .Select(p => int.Parse(p.Id)))
                .First()
            : 1;

        prompt.Id = newId.ToString();
        
        Console.WriteLine(newId);

        await session.StoreAsync(prompt);
        await session.SaveChangesAsync();
    }

    public async Task<IEnumerable<Prompt>> ReadAll()
    {
        using var session = store.OpenAsyncSession();
        return await session.Query<Prompt>().ToListAsync();
    }

    public async Task<Prompt> ReadById(string id)
    {
        using var session = store.OpenAsyncSession();
        return await session.LoadAsync<Prompt>(id);
    }

    public async Task Update(Prompt prompt)
    {
        using var session = store.OpenAsyncSession();
        session.Advanced.Patch(prompt, x => x.NameOfUser, prompt.NameOfUser);
        session.Advanced.Patch(prompt, x => x.NameOfPrompt, prompt.NameOfPrompt);
        session.Advanced.Patch(prompt, x => x.TimeOfUpdate, DateTime.Now);
        session.Advanced.Patch(prompt, x => x.Description, prompt.Description);
        await session.SaveChangesAsync();
    }

    public async Task Delete(string id)
    {
        using var session = store.OpenAsyncSession();
        session.Delete(id);
        await session.SaveChangesAsync();
    }
}