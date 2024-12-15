using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace WebApplication1;

public class UsersCrudRepository
{
    public async Task CreateUser(User user)
    {
        await using var db = new ApplicationContext();

        db.Users.Add(user);
        await db.SaveChangesAsync();
    }

    public async Task<IEnumerable<User>> ReadAllUsers()
    {
        await using var db = new ApplicationContext();
        return await db.Users.ToListAsync();
    }

    public async Task<User> ReadUserById(int id)
    {
        await using var db = new ApplicationContext();
        return await db.Users.FirstOrDefaultAsync(user => user.UserId == id);
    }

    public async Task<User> ReadUserByName(string userName)
    {
        await using var db = new ApplicationContext();
        return await db.Users.FirstOrDefaultAsync(user => user.UserName == userName);
    }

    public async Task UpdateUser(User user)
    {
        await using var db = new ApplicationContext();
        var dbUser = db.Users.FirstOrDefault(p => p.UserId == user.UserId);
        dbUser.UserName = user.UserName;
        dbUser.UserPassword = user.UserPassword;
        await db.SaveChangesAsync();
    }

    public async Task DeleteUser(int id)
    {
        await using var db = new ApplicationContext();
        var dbUser = db.Users.FirstOrDefault(u => u.UserId == id);
        db.Users.Remove(dbUser);
        await db.SaveChangesAsync();
    }
    
    public bool UserExists(string userName)
    {
        using var db = new ApplicationContext();
        var user = db.Users.FirstOrDefault(user => user.UserName == userName);
        return user is not null;
    }

    public void SaveUser(string username, string password)
    {
        using var db = new ApplicationContext();
        var newUser = new User { UserName = username, UserPassword = password };
        db.Users.Add(newUser);
        db.SaveChangesAsync();
    }
}