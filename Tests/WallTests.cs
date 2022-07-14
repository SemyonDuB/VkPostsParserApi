using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework;
using VkServer.Services;

namespace Tests;

public class WallTests
{
	[Test]
	public async Task GetPosts_OnEmptyJsonString_ReturnNothing()
	{
		var handlerMock = GetHttpMessageHandlerMock("{}");
		var httpClient = new HttpClient(handlerMock.Object);

		var wall = new Wall(httpClient, "token", "5.131");

		var actual = new List<string>();
		await foreach (var post in wall.Get(count: 5))
			actual.AddRange(post.Texts);

		Assert.IsEmpty(actual);
	}

	[Test]
	public async Task GetPosts_OnPostsWithoutTexts_ReturnPostsWithoutTexts()
	{
		var expected = new List<string> { "", "" };
		var content = JsonConvert.SerializeObject(new ResponseContentFixture(expected.ToArray()));

		var handlerMock = GetHttpMessageHandlerMock(content);
		var httpClient = new HttpClient(handlerMock.Object);

		var wall = new Wall(httpClient, "token", "5.131");

		var actual = new List<string>();
		await foreach (var post in wall.Get(count: 5))
			actual.AddRange(post.Texts);

		Assert.AreEqual(expected, actual);
	}

	[TestCase("post1")]
	[TestCase("post1", "post2")]
	[TestCase("post1", "post2", "post3")]
	public async Task GetPosts_OnPostsWithText_ReturnCorrectStrings(params string[] expected)
	{
		var content = JsonConvert.SerializeObject(new ResponseContentFixture(expected));

		var handlerMock = GetHttpMessageHandlerMock(content);
		var httpClient = new HttpClient(handlerMock.Object);

		var wall = new Wall(httpClient, "token", "5.131");

		var actual = new List<string>();
		await foreach (var post in wall.Get(count: 5))
			actual.AddRange(post.Texts);

		Assert.AreEqual(expected, actual);
	}

	[Test]
	public void GetWallPostsText_OnErrorResponse_ThrowsException()
	{
		var content = JsonConvert.SerializeObject(new { error = new { error_msg = "Error from vk api" } });

		var handlerMock = GetHttpMessageHandlerMock(content);
		var httpClient = new HttpClient(handlerMock.Object);

		var wall = new Wall(httpClient, "token", "5.131");

		var actual = new List<string>();

		Assert.ThrowsAsync<HttpRequestException>(async () =>
		{
			await foreach (var post in wall.Get(count: 5))
				_ = post.Texts;
		});
	}

	[Test]
	public void GetWallPostsText_OnNotSuccessRequest_ThrowsException()
	{
		var content = JsonConvert.SerializeObject(new ResponseContentFixture("notEmptyItem"));

		var handlerMock = GetHttpMessageHandlerMock(content, HttpStatusCode.BadRequest);
		var httpClient = new HttpClient(handlerMock.Object);

		var wall = new Wall(httpClient, "token", "5.131");

		Assert.ThrowsAsync<HttpRequestException>(async () =>
		{
			await foreach (var post in wall.Get(count: 5))
				_ = post.Texts;
		});
	}

	private static Mock<HttpMessageHandler> GetHttpMessageHandlerMock(string responseContent, HttpStatusCode statusCode = HttpStatusCode.OK)
	{
		var handlerMock = new Mock<HttpMessageHandler>();
		handlerMock.Protected()
			.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
			.ReturnsAsync(new HttpResponseMessage
			{
				StatusCode = statusCode,
				Content = new StringContent(responseContent)
			});

		return handlerMock;
	}
}