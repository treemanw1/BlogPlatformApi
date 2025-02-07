namespace BlogPlatformApi.Dtos;
public record TagDto(
	int Id,
	string Name,
	List<int> ArticleIds
);
