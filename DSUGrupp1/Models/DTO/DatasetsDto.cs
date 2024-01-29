namespace DSUGrupp1.Models.DTO
{
    public class DatasetsDto
    {
        public DatasetsDto() 
        {
            Label = "test";
            Data = new List<int> { 1, 2, 3 };
            BackgroundColor = new List<string> { "rgb(220, 174, 198)" };
            BorderWidth = 10;   
        }
        public string Label { get; set; }
        public List<int> Data { get; set; }
        public List<string> BackgroundColor { get; set;}
        public int BorderWidth { get; set;}

    }
}
