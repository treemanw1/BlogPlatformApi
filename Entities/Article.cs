namespace BlogPlatformApi.Entities;
public class Article
{
	public int Id { get; set; }
	public required string Title { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime UpdatedAt { get; set; }
	public List<Tag> Tags { get; set; } = []; 
	public required string Body { get; set; }
}
