using System.ComponentModel.DataAnnotations;

namespace BlazorApp1.Models;

public class Prompt
{
    public int Id { get; set; }
    [Required]
    public string NameOfPrompt { get; set; }
    [Required]
    public string NameOfUser { get; set; }
    [Required]
    public string Description { get; set; }
    public DateTime TimeOfUpdate { get; set; }
}