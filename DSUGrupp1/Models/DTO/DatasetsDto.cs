namespace DSUGrupp1.Models.DTO
{
    public class DatasetsDto
    {
        public DatasetsDto() 
        {
            label = "test";
            data = new List<double> { 1, 2, 3 };
            backgroundColor = new List<string> {};
            borderWidth = 10;   
        }
        public string label { get; set; }
        public List<double> data { get; set; }
        public List<string> backgroundColor { get; set;}
        public int borderWidth { get; set;}

    }
}
