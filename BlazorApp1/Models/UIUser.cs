using System.ComponentModel.DataAnnotations;

namespace BlazorApp1.Models;

public class UIUser
{
    public int UserID { get; set; }
    [Required]
    public string Name { get; set; } 
    public List<UIPrompt> Prompts { get; set; }
}