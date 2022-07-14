using VkServer.Data;
using VkServer.Models;

namespace VkServer.Repositories;

public interface IUserResultRepository
{
	public UserResult? Get(int id);
	public void Add(UserResult userResult);
	public void Update(UserResult userResult);
}

public class UserResultRepository : IUserResultRepository
{
	private readonly UserResultContext context;

	public UserResultRepository(UserResultContext context) =>
		this.context = context;

	public UserResult? Get(int id)
	{
		return context.UserResults.Find(id);
	}

	public void Add(UserResult userResult)
	{
		context.UserResults.Add(userResult);
		context.SaveChanges();
	}

	public void Update(UserResult userResult)
	{
		context.UserResults.Update(userResult);
		context.SaveChanges();
	}
}