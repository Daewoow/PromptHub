﻿using System.Runtime.Intrinsics.Arm;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1;

public class PromptsCrudRepository
{
    public async Task CreatePrompt(Prompt prompt)
    {
        await using var db = new ApplicationContext();

        db.Prompts.Add(prompt);
        await db.SaveChangesAsync();
    }

    public async Task<IEnumerable<Prompt>> ReadAllPrompts()
    {
        await using var db = new ApplicationContext();
        return await db.Prompts.ToListAsync();
    }

    public async Task<Prompt> ReadPromptById(int id)
    {
        await using var db = new ApplicationContext();
        return await db.Prompts.FirstOrDefaultAsync(prompt => prompt.PromptId == id);
    }

    public async Task UpdatePrompt(Prompt prompt)
    {
        await using var db = new ApplicationContext();
        var dbPrompt = db.Prompts.FirstOrDefault(u => u.PromptId == prompt.PromptId);
        dbPrompt.NameOfUser = prompt.NameOfUser;
        dbPrompt.NameOfPrompt = prompt.NameOfPrompt;
        dbPrompt.Description = prompt.Description;
        dbPrompt.TimeOfUpdate = DateTime.Now;
        await db.SaveChangesAsync();
    }

    public async Task DeletePrompt(int id)
    {
        await using var db = new ApplicationContext();
        var dbPrompt = db.Prompts.FirstOrDefault(u => u.PromptId == id);
        db.Prompts.Remove(dbPrompt);
        await db.SaveChangesAsync();
    }
}