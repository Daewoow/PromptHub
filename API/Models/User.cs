namespace WebApplication1;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Prompt> Prompts { get; set; }
}