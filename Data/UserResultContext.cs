using Microsoft.EntityFrameworkCore;
using VkServer.Models;

namespace VkServer.Data;

public class UserResultContext : DbContext
{
	public UserResultContext (DbContextOptions<UserResultContext> options) : base(options)
	{}

	public DbSet<UserResult> UserResults => Set<UserResult>();
}