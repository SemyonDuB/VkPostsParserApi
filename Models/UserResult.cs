using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VkServer.Models;

public class UserResult
{
	public int Id { get; set; }
	
	[Required]
	public string Hash { get; set; }
	
	[Required]
	[Column(TypeName = "jsonb")]
	public string LastResult { get; set; }
}