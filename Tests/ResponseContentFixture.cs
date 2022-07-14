using System.Collections.Generic;
using System.Linq;

namespace Tests;

// ReSharper disable InconsistentNaming
public class ResponseContentFixture
{
	public Response response;
	
	public struct Response
	{
		public List<PostResponse> items;
	}
	
	public struct PostResponse
	{
		public string text;
	}

	public ResponseContentFixture() =>
		response.items = new List<PostResponse> { new PostResponse { text = "" } };

	public ResponseContentFixture(params string[] items) =>
		response.items = new List<PostResponse>(items.Select(item => new PostResponse { text = item }));
}