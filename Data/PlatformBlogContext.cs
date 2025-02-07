using Microsoft.EntityFrameworkCore;
using BlogPlatformApi.Entities;

namespace BlogPlatformApi.Data;

public class BlogPlatformContext : DbContext
{
	public BlogPlatformContext(DbContextOptions<BlogPlatformContext> options): base(options) { }

	public DbSet<Article> Articles { get; set; }
	public DbSet<Tag> Tags { get; set; }
}