namespace BlogPlatformApi.Dtos;

public record CreateArticleDto(
	string Title,
	List<int>? TagIds,
	string Body
);