namespace DSUGrupp1.Models.DTO
{
    public class TitleDto
    {
        public TitleDto() 
        {
            display = true;
            text = "Vad som helst";
        }
        public bool display {  get; set; }
        public string text { get; set; }
    }
}
