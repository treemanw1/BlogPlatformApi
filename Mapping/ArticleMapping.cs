using BlogPlatformApi.Dtos;
using BlogPlatformApi.Entities;

namespace BlogPlatformApi.Mapping;
public static class ArticleMapping
{
	public static Article ToEntity(this CreateArticleDto dto, List<Tag> tags)
	{
		return new Article
		{
			Title = dto.Title,
			CreatedAt = DateTime.UtcNow,
			UpdatedAt = DateTime.UtcNow,
			Tags = tags,
			Body = dto.Body
		};
	}

	public static ArticleDto ToDto(this Article article)
	{
		return new ArticleDto(
			article.Id,
			article.Title,
			article.CreatedAt,
			article.UpdatedAt,
			article.Tags?.Select(t => t.Id).ToList(),
			article.Body
		);
	}
}
