using Newtonsoft.Json.Linq;

namespace VkServer.Services;

public class Post
{
	public Post(IEnumerable<string> texts, string hash) =>
		(Texts, Hash) = (texts, hash);

	public IEnumerable<string> Texts { get; }
	public string Hash { get; }
}

public class Wall
{
	private const string ApiUri = "https://api.vk.com/method/wall";

	private readonly HttpClient httpClient;
	private readonly string token;
	private readonly string vkApiVersion;
	
	public Wall(HttpClient httpClient, string token, string vkApiVersion)
	{
		this.httpClient = httpClient;
		this.token = token;
		this.vkApiVersion = vkApiVersion;
	}
	
	public async IAsyncEnumerable<Post> Get(int count)
	{
		var uri = ApiUri + $".get?count={count}&access_token={token}&v={vkApiVersion}";
		
		var httpRequestMsg = new HttpRequestMessage(HttpMethod.Get, uri);
		var httpResponse = await httpClient.SendAsync(httpRequestMsg);

		if (!httpResponse.IsSuccessStatusCode)
			throw new HttpRequestException("Can not take wall posts from vk api");

		var content = await httpResponse.Content.ReadAsStringAsync();

		if (JObject.Parse(content)["error"] != null)
			throw new HttpRequestException((string?) JObject.Parse(content)["error"]?["error_msg"] ?? "");
		
		var contentItems = JObject.Parse(content)["response"]?["items"];

		foreach (var item in contentItems ?? Enumerable.Empty<JToken>())
		{
			var hash = item["hash"] == null ? "" : (string)item["hash"]!;
			var texts = GetPostTexts(item);

			yield return new Post(texts, hash);
		}
	}
	
	private static IEnumerable<string> GetPostTexts(JToken jToken)
	{
		if (jToken["text"] == null)
			yield break;

		yield return (string) jToken["text"]!;
		
		foreach (var copyHistory in jToken["copy_history"] ?? Enumerable.Empty<JToken>())
		{
			foreach (var innerText in GetPostTexts(copyHistory))
				yield return innerText;
		}
	}
}