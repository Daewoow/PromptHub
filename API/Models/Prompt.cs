namespace WebApplication1;

public class Prompt
{
    public int PromptId { get; set; }
    public string NameOfUser { get; set; }
    public string NameOfPrompt { get; set; }
    public string Description { get; set; }
    public bool IsPublic { get; set; } = true;
    public DateTimeOffset TimeOfUpdate { get; set; }
}