namespace DSUGrupp1.Models.DTO
{
    public class PluginsDto
    {
        public PluginsDto() 
        {
            Title = new TitleDto();
        } 
        public TitleDto Title { get; set; }
    }
}
