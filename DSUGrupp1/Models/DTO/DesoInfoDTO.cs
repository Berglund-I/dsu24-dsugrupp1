using Newtonsoft.Json;

namespace DSUGrupp1.Models.DTO
{
	public class DesoInfoDTO
	{
		[JsonProperty("areas")]
		List<Areas> Areas { get; set; }
	}
}
