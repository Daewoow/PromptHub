using System.ComponentModel.DataAnnotations;

namespace BlazorApp1.Models;

public class User
{
    public int UserID { get; set; }
    [Required]
    public string Name { get; set; } 
    public List<Prompt> Prompts { get; set; }
}