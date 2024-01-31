namespace DSUGrupp1.Models.DTO
{
    public class OptionsDto
    {
        public OptionsDto()
        {
            plugins = new PluginsDto();
        }
        public PluginsDto plugins { get; set; }
    }
}
