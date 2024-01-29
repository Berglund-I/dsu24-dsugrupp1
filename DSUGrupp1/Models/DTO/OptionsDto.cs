namespace DSUGrupp1.Models.DTO
{
    public class OptionsDto
    {
        public OptionsDto()
        {
            Plugins = new PluginsDto();
        }
        public PluginsDto Plugins { get; set; }
    }
}
