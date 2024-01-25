using Newtonsoft.Json;

namespace DSUGrupp1.Models.DTO
{
	public class Areas
	{
		[JsonProperty("deso")]
		string? Deso { get; set; }
		[JsonProperty("deso-name")]
		string? DesoName { get; set; }
	}
}
