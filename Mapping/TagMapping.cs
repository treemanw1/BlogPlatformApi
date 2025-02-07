using BlogPlatformApi.Dtos;
using BlogPlatformApi.Entities;

namespace BlogPlatformApi.Mapping;
public static class TagMapping
{
	public static Tag ToEntity(this CreateTagDto dto)
	{
		return new Tag
		{
			Name = dto.Name,
			Articles = []
		};
	}
}
