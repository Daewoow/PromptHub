using System.Runtime.Intrinsics.Arm;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1;

public class CrudRepository
{
    public async Task Create(Prompt prompt)
    {
        await using var db = new ApplicationContext();

        db.Prompts.Add(prompt);
        await db.SaveChangesAsync();
    }

    public async Task<IEnumerable<Prompt>> ReadAll()
    {
        await using var db = new ApplicationContext();
        return await db.Prompts.ToListAsync();
    }

    public async Task<Prompt> ReadById(int id)
    {
        await using var db = new ApplicationContext();
        return await db.Prompts.FirstOrDefaultAsync(prompt => prompt.Id == id);
    }

    public async Task Update(Prompt prompt)
    {
        await using var db = new ApplicationContext();
        var dbPrompt = db.Prompts.FirstOrDefault(p => p.Id == prompt.Id);
        dbPrompt.NameOfUser = prompt.NameOfUser;
        dbPrompt.NameOfPrompt = prompt.NameOfPrompt;
        dbPrompt.Description = prompt.Description;
        dbPrompt.TimeOfUpdate = DateTime.Now;
        await db.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        await using var db = new ApplicationContext();
        var dbPrompt = db.Prompts.FirstOrDefault(p => p.Id == id);
        db.Prompts.Remove(dbPrompt);
        await db.SaveChangesAsync();
    }
}