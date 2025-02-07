namespace BlogPlatformApi.Dtos;

public record ArticleDto(
	int Id,
	string Title,
	DateTime CreatedAt,
	DateTime UpdatedAt,
	List<int>? TagIds,
	string Body
);