using BlogPlatformApi.Data;
using BlogPlatformApi.Dtos;
using BlogPlatformApi.Mapping;
using Microsoft.EntityFrameworkCore;

namespace BlogPlatformApi.Endpoints;

public static class TagEndpoints
{
	public static RouteGroupBuilder MapTagEndpoints(this WebApplication app)
	{
		var group = app.MapGroup("tags");

		group.MapGet("/", async (BlogPlatformContext dbContext) =>
		{
			var tags = await dbContext.Tags.ToListAsync();
			return Results.Ok(tags);
		});

		group.MapPost("/", async (CreateTagDto newTag, BlogPlatformContext dbContext) =>
		{
			if (await dbContext.Tags.AnyAsync(t => t.Name == newTag.Name)) return Results.BadRequest("Tag already exists");
			var tag = newTag.ToEntity();

			await dbContext.Tags.AddAsync(tag);
			await dbContext.SaveChangesAsync();
			return Results.Created($"/tag/{tag.Id}", tag);;
		});

		group.MapDelete("/{id}", async (int id, BlogPlatformContext dbContext) =>
		{
			var tag = await dbContext.Tags.FindAsync(id);
			if (tag is null) return Results.NotFound();

			dbContext.Tags.Remove(tag);
			await dbContext.SaveChangesAsync();
			return Results.NoContent();
		});

		return group;
	}
}