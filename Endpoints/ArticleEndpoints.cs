using BlogPlatformApi.Data;
using BlogPlatformApi.Dtos;
using BlogPlatformApi.Entities;
using BlogPlatformApi.Mapping;
using Microsoft.EntityFrameworkCore;

namespace BlogPlatformApi.Endpoints;

public static class ArticleEndpoints
{
	public static RouteGroupBuilder MapArticleEndpoints(this WebApplication app)
	{
		var group = app.MapGroup("articles");

		group.MapGet("/", async (BlogPlatformContext dbContext) =>
		{
			var articles = await dbContext.Articles.Include(a => a.Tags).ToListAsync();
			var articleDtos = articles.Select(a => a.ToDto());
			return Results.Ok(articleDtos);
		});

		group.MapGet("/tag-id/{id}", async (int id, BlogPlatformContext dbContext) =>
		{
			var articles = await dbContext.Articles.Include(a => a.Tags).Where(a => a.Tags.Any(t => t.Id == id)).ToListAsync();
			var articleDtos = articles.Select(a => a.ToDto());
			return Results.Ok(articleDtos);
		});

		group.MapGet("/id/{id}", async (int id, BlogPlatformContext dbContext) =>
		{
			var article = await dbContext.Articles.FindAsync(id);
			if (article is null) return Results.NotFound();
			return Results.Ok(article.ToDto());
		});

		group.MapPost("/", async (CreateArticleDto newArticle, BlogPlatformContext dbContext) =>
		{
			var tags = newArticle.TagIds is not null ? await dbContext.Tags.Where(t => newArticle.TagIds.Contains(t.Id)).ToListAsync() : [];
			var article = newArticle.ToEntity(tags);
			await dbContext.Articles.AddAsync(article);
			await dbContext.SaveChangesAsync();

			var articleDto = article.ToDto();
			return Results.Created($"/article/{article.Id}", articleDto); 
		});

		group.MapDelete("/{id}", async (int id, BlogPlatformContext dbContext) =>
		{
			var article = await dbContext.Articles.FindAsync(id);
			if (article is null) return Results.NotFound();

			dbContext.Articles.Remove(article);
			await dbContext.SaveChangesAsync();
			return Results.NoContent();
		});

		group.MapPut("/{id}", async (int id, UpdateArticleDto updatedArticle, BlogPlatformContext dbContext) =>
		{
			var article = await dbContext.Articles.Include(a => a.Tags).FirstOrDefaultAsync(a => a.Id == id);
			if (article is null) return Results.NotFound();

			article.Title = updatedArticle.Title ?? article.Title;
			if (updatedArticle.TagIds is not null)
			{
				var tags = await dbContext.Tags.Where(t => updatedArticle.TagIds.Contains(t.Id)).ToListAsync();
				var missingTagIds = updatedArticle.TagIds.Except(tags.Select(t => t.Id)).ToList();
				if (missingTagIds.Any())
				{
					return Results.BadRequest($"The following tag IDs do not exist: {string.Join(", ", missingTagIds)}");
				}
				article.Tags = tags;
			}
			article.UpdatedAt = DateTime.UtcNow;
			article.Body = updatedArticle.Body ?? article.Body;

			await dbContext.SaveChangesAsync();
			return Results.Ok(article.ToDto());
		});

		return group;
	}
}