namespace DSUGrupp1.Models.DTO
{
    public class TitleDto
    {
        public TitleDto() 
        {
            Display = true;
            Text = "Vad som helst";
        }
        public bool Display {  get; set; }
        public string Text { get; set; }
    }
}
