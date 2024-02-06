namespace DSUGrupp1.Models.DTO
{
    public class DatasetsDto
    {
        public DatasetsDto() 
        {
            Label = "test";
            Data = new List<double> { 1, 2, 3 };
            BackgroundColor = new List<string> {};
            BorderWidth = 10;   
        }
        public string Label { get; set; }
        public List<double> Data { get; set; }
        public List<string> BackgroundColor { get; set;}
        public int BorderWidth { get; set;}
        public bool Fill { get; set; }
        public string BorderColor { get; set; }

    }
}
