namespace DSUGrupp1.Models.DTO
{
    public class PluginsDto
    {
        public PluginsDto() 
        {
            title = new TitleDto();
        } 
        public TitleDto title { get; set; }
    }
}
