using System.ComponentModel.DataAnnotations;

namespace BlazorApp1.Models;

public class UIPrompt
{
    public int PromptId { get; set; }
    public string NameOfUser { get; set; }
    [Required] public string NameOfPrompt { get; set; }
    [Required] public string Description { get; set; }

    [Required] public bool IsPublic { get; set; } = true;
    public DateTimeOffset TimeOfUpdate { get; set; }
    public List<UIPrompt> History { get; set; }
}
