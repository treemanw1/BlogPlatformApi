namespace BlogPlatformApi.Dtos;

public record UpdateArticleDto(
	string? Title,
	List<int>? TagIds,
	string? Body
);
