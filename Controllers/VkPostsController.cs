using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VkServer.Models;
using VkServer.Repositories;
using VkServer.Services;
using VkServer.Utils;

namespace VkServer.Controllers;

[ApiController]
[Route("[controller]")]
public class VkPostsController : ControllerBase
{
	private readonly IVkApiService vkApiService;
	private readonly IUserResultRepository userResultRepository;

	public VkPostsController(IVkApiService vkApiService, IUserResultRepository userResultRepository)
	{
		this.vkApiService = vkApiService;
		this.userResultRepository = userResultRepository;
	}

	[HttpGet("GetNumberOccurrencesLetters")]
	[Produces("application/json")]
	public async Task<IActionResult> Get(string code)
	{
		await vkApiService.UpdateToken(code);

		var hash = new StringBuilder();
		var texts = new List<string>();
		await foreach (var post in vkApiService.Wall.Get(count: 5))
		{
			hash.Append(post.Hash);
			texts.AddRange(post.Texts);
		}

		var userId = vkApiService.UserId;
		var userResult = userResultRepository.Get(userId);
		if (hash.ToString() == userResult?.Hash)
			return Content(userResult.LastResult, "application/json");

		var result = JsonConvert.SerializeObject(texts.GetLetterOccurrences());
		var newUserResult = new UserResult { Id = userId, Hash = hash.ToString(), LastResult = result };
		
		if (userResult == null)
			userResultRepository.Add(newUserResult);
		else
			userResultRepository.Update(newUserResult);
		
		return Content(result, "application/json");
	}
}