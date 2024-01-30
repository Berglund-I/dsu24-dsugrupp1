using Newtonsoft.Json;

namespace DSUGrupp1.Models.DTO
{
	public class Areas
	{
		[JsonProperty("deso")]
		public string? Deso { get; set; }
		[JsonProperty("deso-name")]
		public string? DesoName { get; set; }
	}
}
