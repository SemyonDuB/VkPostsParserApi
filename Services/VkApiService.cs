using Newtonsoft.Json.Linq;

namespace VkServer.Services;

public interface IVkApiService
{
	public int UserId { get; }
	public Wall Wall { get; }
	
	public Task UpdateToken(string code, string redirectUri);
}

public class VkApiService : IVkApiService
{
	private readonly IConfiguration config;
	private readonly IHttpClientFactory httpClientFactory;

	private string token = "";
	private readonly string version;

	public int UserId { get; private set; } = 0;
	public Wall Wall => new Wall(httpClientFactory.CreateClient(), token, version);

	public VkApiService(IConfiguration config, IHttpClientFactory httpClientFactory)
	{
		this.config = config;
		this.httpClientFactory = httpClientFactory;
		
		version = this.config["VkApi:Version"];
	}

	public async Task UpdateToken(string code, string redirectUri)
	{
		const string oauth = "https://oauth.vk.com/access_token";
		var appId = config["VkApi:AppId"];
		var secretKey = config["VkApi:SecretKey"];
		var url = $"{oauth}?client_id={appId}&client_secret={secretKey}&redirect_uri={redirectUri}&code={code}";

		var httpRequestMsg = new HttpRequestMessage(HttpMethod.Get, url);
		var httpClient = httpClientFactory.CreateClient();
		var httpResponse = await httpClient.SendAsync(httpRequestMsg);

		if (!httpResponse.IsSuccessStatusCode)
			throw new HttpRequestException("Vk api is not available, code or redirectUri is wrong");

		var content = await httpResponse.Content.ReadAsStringAsync();

		token = JObject.Parse(content)["access_token"]?.Value<string>()
		        ?? throw new HttpRequestException("Error while requesting vk api access token");
		UserId = JObject.Parse(content)["user_id"]?.Value<int>() ?? 0;
	}
}