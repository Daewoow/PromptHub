using System.ComponentModel.DataAnnotations;

namespace BlazorApp1.Models;

public class UIUser
{
    public int UserID { get; set; }
    [Required]
    public string Name { get; set; } 
    public List<UIPrompt> Prompts { get; set; }
}


public interface IUser<out T>{}

public interface IReader<out T> : IUser<T>;

public interface IWriter<out T> : IReader<T>;