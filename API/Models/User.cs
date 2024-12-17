namespace WebApplication1;

public class User
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string UserPassword { get; set; }
}

public interface IUser<out T>{}

public interface IReader<out T> : IUser<T>;

public interface IWriter<out T> : IReader<T>;